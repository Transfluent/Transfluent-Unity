using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MiniJSON;

public enum TransfluentDelegateType { None, TransfluentLanguagesUpdated, FollowUpTranslateTexts, GetLanguages }

public delegate void TransfluentDelegate(TransfluentDelegateType type, object[] data);

public enum TransfluentMethodMode { GET, POST };
public enum TransfluentMethodType { Hello, Authenticate, Register, Languages, Text, Texts, TextStatus, TextsTranslate, TextWordCount, CombinedTexts_Send, CombinedTexts_Translate };

public class TransfluentMethod
{
    public class Parameter
    {
        public string m_name;

        public override string ToString()
        {
            return m_name;
        }
    }

    public class ParameterValue : Parameter
    {
        public int m_value;

        public ParameterValue(string name, int value)
        {
            m_name = name;
            m_value = value;
        }

        public override string ToString()
        {
            return m_name + ": " + m_value;
        }
    }

    public class ParameterText : Parameter
    {
        public string m_text;

        public ParameterText(string name, string text)
        {
            m_name = name;
            m_text = text;
        }

        public override string ToString()
        {
            return m_name + ": '" + m_text + "'";
        }
    }

    public class ParameterGroup : Parameter
    {
        public List<Parameter> m_parameters;

        public ParameterGroup(string name, List<Parameter> parameters)
        {
            m_name = name;
            m_parameters = parameters;
        }
    }

    public class WebRequestState
    {
        public WebRequest m_webRequest;
        public WebResponse m_webResponse;
        public IAsyncResult m_asyncResult;

        public string m_responseMessage;
        public string m_errorMessage;
        public string m_serverErrorMessage;
        public string m_backendErrorType;

        public WebRequestState()
        {
            m_webRequest = null;
            m_webResponse = null;
            m_asyncResult = null;
            m_responseMessage = null;
            m_errorMessage = null;
            m_backendErrorType = null;
        }

        public void Close()
        {
            if (m_webRequest != null)
                m_webRequest.Abort();
            if (m_webResponse != null)
                m_webResponse.Close();
        }
    }

    // Mono security policy doesn't trust anyone so we need our own security policy
    public static bool Validator(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors policyErrors)
    {
        //*** Just accept and move on...
        Debug.LogWarning("TransfluentUtility: DummyX509SecurityPolicy: Ignoring any validation errors!");
        return true;
    }

    public TransfluentMethodType m_type;
    public TransfluentMethodMode m_mode;

    public DateTime m_timeSent;
    public DateTime m_timeFinished;

    private WebRequestState m_webRequestState;
    const int m_timeoutThreshold = 30; // in seconds

    private bool m_isResponseCompleted = false;

    private List<Parameter> m_parameters;

    private TransfluentDelegate m_onSuccessDelegate;
    private TransfluentDelegateType m_delegateType;
    private object[] m_delegateData;

    private bool m_consumed;
    private string m_parsedResponse;
    private string m_error;

    public bool isDone
    {
        get { return m_isResponseCompleted; }
    }

    public bool isConsumed
    {
        get { return m_consumed; }
    }

    public string request
    {
        get
        {
            if (m_mode == TransfluentMethodMode.GET)
                return m_webRequestState.m_webRequest.RequestUri.AbsoluteUri;
            else
            {
                string parameters = "";

                for (int i = 0; i < m_parameters.Count; i++)
                {
                    if (i > 0)
                        parameters += "&";

                    parameters += EncodeParameterItem(m_parameters[i]);
                }

                return m_webRequestState.m_webRequest.RequestUri.AbsoluteUri + "\nParameters: " + parameters;
            }
        }
    }

    public string response
    {
        get { return m_webRequestState.m_responseMessage; }
    }

    public string parsedResponse
    {
        get { return m_parsedResponse; }
    }

    public string error
    {
        get { return m_webRequestState.m_errorMessage; }
    }

    public string serverError
    {
        get { return m_webRequestState.m_serverErrorMessage; }
    }

    public string backendError
    {
        get { return m_webRequestState.m_backendErrorType; }
    }

    public bool hasDelegate
    {
        get { return m_onSuccessDelegate != null; }
    }

    public bool isGet
    {
        get { return m_mode == TransfluentMethodMode.GET; }
    }

    public bool isPost
    {
        get { return m_mode == TransfluentMethodMode.POST; }
    }

    public TransfluentMethod(TransfluentMethodType type, TransfluentMethodMode mode, TransfluentDelegate onSuccessDelegate, TransfluentDelegateType delegateType, object[] delegateData)
    {
        m_type = type;
        m_mode = mode;
        m_onSuccessDelegate = onSuccessDelegate;
        m_delegateType = delegateType;
        m_delegateData = delegateData;
    }

    public void AddParameter(string name, int value)
    {
        if (m_parameters == null)
            m_parameters = new List<Parameter>();

        m_parameters.Add(new ParameterValue(name, value));
    }

    public void AddParameter(string name, string text)
    {
        if (m_parameters == null)
            m_parameters = new List<Parameter>();

        m_parameters.Add(new ParameterText(name, text));
    }

    public void AddParameterGroup(string name, string[] textIds, string[] texts)
    {
        if (m_parameters == null)
            m_parameters = new List<Parameter>();

        List<Parameter> parameters = new List<Parameter>(texts.Length);

        for (int i = 0; i < texts.Length; i++)
            parameters.Add(new ParameterText(name + "[" + textIds[i] + "]", texts[i]));

        m_parameters.Add(new ParameterGroup(name, parameters));
    }

    public void AddParameterGroup(string name, string[] valueIds, int[] values)
    {
        if (m_parameters == null)
            m_parameters = new List<Parameter>();

        List<Parameter> parameters = new List<Parameter>(values.Length);

        for (int i = 0; i < values.Length; i++)
            parameters.Add(new ParameterValue(name + "[" + valueIds[i] + "]", values[i]));

        m_parameters.Add(new ParameterGroup(name, parameters));
    }

    public List<Parameter> GetParameters()
    {
        return m_parameters;
    }

    public bool TryGetTextParameter(string name, out string value)
    {
        foreach (Parameter param in m_parameters)
        {
            if (param.m_name == name)
            {
                ParameterText pt = param as ParameterText;

                if (pt != null)
                {
                    value = pt.m_text;
                    return true;
                }
            }
        }

        value = null;
        return false;
    }

    public bool TryGetTextParameters(string name, out string[] values)
    {
        List<string> foundValues = new List<string>();

        foreach (Parameter param in m_parameters)
        {
            if (param.m_name == name)
            {
                ParameterText pt = param as ParameterText;

                if (pt != null)
                    foundValues.Add(pt.m_text);
            }
        }

        values = foundValues.ToArray();
        return foundValues.Count > 0;
    }

    public bool TryGetValueParameter(string name, out int value)
    {
        foreach (Parameter param in m_parameters)
        {
            if (param.m_name == name)
            {
                ParameterValue pt = param as ParameterValue;

                if (pt != null)
                {
                    value = pt.m_value;
                    return true;
                }
            }
        }

        value = 0;
        return false;
    }

    public bool TryGetValueParameters(string name, out int[] values)
    {
        List<int> foundValues = new List<int>();

        foreach (Parameter param in m_parameters)
        {
            if (param.m_name == name)
            {
                ParameterValue pt = param as ParameterValue;

                if (pt != null)
                    foundValues.Add(pt.m_value);
            }
        }

        values = foundValues.ToArray();
        return foundValues.Count > 0;
    }

    private void StartRequest(WebRequest webRequest)
    {
        Debug.Log("TransfluentMethod." + m_type + ": Starting WebRequest");

        m_isResponseCompleted = false;

        m_webRequestState = new WebRequestState();
        m_webRequestState.m_webRequest = webRequest;

        m_webRequestState.m_asyncResult = (IAsyncResult)m_webRequestState.m_webRequest.BeginGetResponse(new AsyncCallback(ResponseCallback), m_webRequestState);

        ThreadPool.RegisterWaitForSingleObject(m_webRequestState.m_asyncResult.AsyncWaitHandle,
            new WaitOrTimerCallback(ScanTimeoutCallback),
            m_webRequestState,
            (m_timeoutThreshold * 1000), // obviously because this is in miliseconds
            true
            );
    }

    private void ScanTimeoutCallback(object state, bool timedOut)
    {
        if (timedOut)
        {
            WebRequestState requestState = (WebRequestState)state;
            if (requestState != null)
                requestState.m_webRequest.Abort();

            Debug.LogError("TransfluentMethod." + m_type + ": Request timed out");
        }
        else
        {
            RegisteredWaitHandle registeredWaitHandle = (RegisteredWaitHandle)state;
            if (registeredWaitHandle != null)
                registeredWaitHandle.Unregister(null);
        }
    }

    private void ResponseCallback(IAsyncResult asyncResult)
    {
        Debug.Log("TransfluentMethod." + m_type + ": Request responded!");

        try
        {
            m_webRequestState.m_webResponse = m_webRequestState.m_webRequest.EndGetResponse(asyncResult);
        }
        catch (WebException webException)
        {
            Debug.LogError("TransfluentMethod." + m_type + ": WebException was received!\n" + webException.Message);
            m_webRequestState.m_serverErrorMessage = webException.Message;
            m_webRequestState.m_webResponse = webException.Response;
        }

        Debug.Log("TransfluentMethod." + m_type + ": Reading response...");

        StreamReader reader = new StreamReader(m_webRequestState.m_webResponse.GetResponseStream());
        m_webRequestState.m_responseMessage = reader.ReadToEnd();

        Debug.Log("TransfluentMethod." + m_type + ": Response:\n" + m_webRequestState.m_responseMessage);

        m_isResponseCompleted = true;
    }

    public void SendRequest(string uri)
    {
    }

    public void SendTo(string url)
    {
        string parameters = "";

        if (m_parameters != null)
        {
            for (int i = 0; i < m_parameters.Count; i++)
            {
                if (i > 0)
                    parameters += "&";
                else if (m_mode == TransfluentMethodMode.GET)
                    parameters += "?";

                parameters += EncodeParameterItem(m_parameters[i]);
            }
        }

        string uri = url;

        if (m_mode == TransfluentMethodMode.GET)
        {
            uri += parameters;
            Debug.Log("TransfluentMethod: Sending " + m_mode.ToString() + " to: " + uri);
        }

        ServicePointManager.ServerCertificateValidationCallback = Validator;

        WebRequest webRequest = WebRequest.Create(uri);
        webRequest.Method = m_mode.ToString();

        if (m_mode == TransfluentMethodMode.POST)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(parameters);
            webRequest.ContentLength = byteArray.Length;
            webRequest.ContentType = "application/x-www-form-urlencoded";

            Stream dataStream = webRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            Debug.Log("TransfluentMethod: Sending " + m_mode.ToString() + " to: " + uri + " with " + byteArray.Length + " bytes of POST data:\n" + parameters);
        }

        m_timeSent = DateTime.Now;
        StartRequest(webRequest);
    }

    private string EncodeParameterItem(Parameter item)
    {
        string data = "";

        if (item is ParameterGroup)
        {
            if (m_mode == TransfluentMethodMode.GET)
                Debug.LogError("TransfluentUtility: GET methods do not support POST parameter arrays!\nParameter group '" + item.m_name + "' was discarded");
            if (m_mode == TransfluentMethodMode.POST)
            {
                ParameterGroup group = item as ParameterGroup;
                for (int i = 0; i < group.m_parameters.Count; i++)
                {
                    if (i > 0)
                        data += "&";
                    data += EncodeParameterItem(group.m_parameters[i]);
                }
            }
        }
        if (item is ParameterValue)
        {
            ParameterValue value = item as ParameterValue;
            data += value.m_name + "=" + value.m_value;
        }
        if (item is ParameterText)
        {
            ParameterText text = item as ParameterText;
            data += text.m_name + "=" + WWW.EscapeURL(text.m_text);
        }

        return data;
    }

    public TimeSpan GetDuration()
    {
        if (isDone)
            return m_timeFinished.Subtract(m_timeSent);

        return DateTime.Now.Subtract(m_timeSent);
    }

    public IDictionary ParseJSON()
    {
        return (IDictionary)Json.Deserialize(response);
    }

    public void SetParsedResponse(string text)
    {
        m_parsedResponse = text;
    }

    public void SetErrorMessage(string message)
    {
        m_webRequestState.m_errorMessage = message;
    }

    public void SetBackendErrorType(string type)
    {
        m_webRequestState.m_backendErrorType = type;
    }

    public void InvokeDelegate()
    {
        if (m_onSuccessDelegate != null)
        {
            Debug.Log("Delegate method called: " + m_delegateType);
            m_onSuccessDelegate(m_delegateType, m_delegateData);
        }
        else
            Debug.LogError(m_delegateType + ".InvokeDelegate() OnSuccessDelegate is null!");
    }

    public void Consume()
    {
        m_consumed = true;
        m_webRequestState.Close();
        m_timeFinished = DateTime.Now;
    }
}