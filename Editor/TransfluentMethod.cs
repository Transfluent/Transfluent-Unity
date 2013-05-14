using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public enum TransfluentDelegateType { GetLanguages }

public delegate void TransfluentDelegate(TransfluentDelegateType type, object[] data);

public enum TransfluentMethodMode { GET, POST };
public enum TransfluentMethodType { Hello, Authenticate, Languages, Text, Texts, TextStatus, TextsTranslate, TextWordCount };

public class TransfluentMethod
{
    public class Parameter
    {
        public string m_name;
    }

    public class ParameterValue : Parameter
    {
        public int m_value;

        public ParameterValue(string name, int value)
        {
            m_name = name;
            m_value = value;
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
    }

    public TransfluentMethodType m_type;
    public TransfluentMethodMode m_mode;

    public DateTime m_timeSent;
    public DateTime m_timeFinished;

    private WWW m_www;
    private WWWForm m_form;
    private List<Parameter> m_parameters;

    private TransfluentDelegate m_onSuccessDelegate;

    private bool m_consumed;
    private string m_parsedResponse;
    private string m_error;

    public bool isDone
    {
        get { return m_www.isDone; }
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
                return m_www.url;
            else
            {
                string parameters = "";

                foreach (Parameter param in m_parameters)
                {
                    if (param is ParameterText)
                        parameters +=((ParameterText)param).m_text;
                    if (param is ParameterValue)
                        parameters += ((ParameterValue)param).m_value;
                }

                return parameters;
            }
        }
    }

    public string response
    {
        get { return m_www.text; }
    }

    public string parsedResponse
    {
        get { return m_parsedResponse; }
    }

    public string error
    {
        get { return m_www.error; }
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

    public TransfluentMethod(TransfluentMethodType type, TransfluentMethodMode mode, TransfluentDelegate onSuccessDelegate)
    {
        m_type = type;
        m_mode = mode;
        m_onSuccessDelegate = onSuccessDelegate;
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

    public void SendTo(string url)
    {
        if (m_parameters != null)
        {
            if (m_mode == TransfluentMethodMode.GET)
            {
                string parameters = "";

                for (int i = 0; i < m_parameters.Count; i++)
                {
                    Parameter item = m_parameters[i];

                    if (i > 0)
                        parameters += "&";

                    if (item is ParameterValue)
                    {
                        ParameterValue value = item as ParameterValue;
                        parameters += value.m_name + "=" + value.m_value;
                    }
                    if (item is ParameterText)
                    {
                        ParameterText text = item as ParameterText;
                        parameters += text.m_name + "=" + WWW.EscapeURL(text.m_text);
                    }
                }
                
                string get = url + "?" + parameters;
                Debug.Log("TransfluentMethod: Sending GET: " + get);
                m_www = new WWW(get);

                m_timeSent = DateTime.Now;
            }
            else
            {
                string values = "";
                m_form = new WWWForm();

                foreach (Parameter item in m_parameters)
                {
                    values += "\n";

                    if (item is ParameterValue)
                    {
                        ParameterValue value = item as ParameterValue;
                        m_form.AddField(value.m_name, value.m_value);
                        values += "Item '" + value.m_name + "' value: " + value.m_value;
                    }
                    if (item is ParameterText)
                    {
                        ParameterText text = item as ParameterText;
                        m_form.AddField(text.m_name, text.m_text);
                        values += "Item '" + text.m_name + "' text: " + text.m_text;
                    }
                }

                Debug.Log("TransfluentMethod: Sending POST to " + url + values);
                m_www = new WWW(url, m_form);

                m_timeSent = DateTime.Now;
            }
        }
        else
        {
            Debug.Log("TransfluentMethod: Sending GET to " + url);
            m_www = new WWW(url);

            m_timeSent = DateTime.Now;
        }
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

    public void InvokeDelegate(TransfluentDelegateType type, object[] data)
    {
        if (m_onSuccessDelegate != null)
            m_onSuccessDelegate(type, data);
    }

    public void Consume()
    {
        m_consumed = true;
        m_timeFinished = DateTime.Now;
    }
}