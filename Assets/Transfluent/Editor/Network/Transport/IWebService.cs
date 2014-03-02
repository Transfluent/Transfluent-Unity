using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Pathfinding.Serialization.JsonFx;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace transfluent
{
	public interface IWebService
	{
		WebServiceReturnStatus request(string url);
		WebServiceReturnStatus request(string url, Dictionary<string, string> postParams);
		WebServiceReturnStatus request(ITransfluentCall call);
		string encodeGETParams(Dictionary<string, string> getParams);
	}
	public class DebugSyncronousEditorWebRequest : IWebService
	{
		public bool debug = true;
		private readonly IWebService realRequest = new SyncronousEditorWebRequest();

		public DebugSyncronousEditorWebRequest()
		{
			Debug.Log("CREATING SYNC REQUESTs");
		}

		public WebServiceReturnStatus request(string url)
		{
			if(debug) Debug.Log("calling url:" + url + "(GET) ");
			var result = realRequest.request(url);
			if(debug) Debug.Log("GOT BACK WITH RESULT:" + result);
			return result;
		}

		public WebServiceReturnStatus request(string url, Dictionary<string, string> postParams)
		{
			if (postParams != null)
			{
				foreach (KeyValuePair<string, string> param in postParams)
				{
					if(debug) Debug.Log("Field added:" + param.Key + " with value:" + param.Value);
				}
				Debug.Log("ALL params:"+JsonWriter.Serialize(postParams));
			}
			if(debug) Debug.Log("calling url:" + url + "(POST) ");
			var result = realRequest.request(url, postParams);
			
			if(debug) Debug.Log("GOT BACK WITH RESULT:" + result);
			return result;
		}

		public WebServiceReturnStatus request(ITransfluentCall call)
		{
			return realRequest.request(call);
		}

		public string encodeGETParams(Dictionary<string, string> getParams)
		{
			return realRequest.encodeGETParams(getParams);
		}
	}

	public class SyncronousEditorWebRequest : IWebService
	{
		
		public WebServiceReturnStatus request(string url)
		{
			
			return doWWWCall(new WWW(url));
		}

		public WebServiceReturnStatus request(string url, Dictionary<string, string> postParams)
		{
			var form = new WWWForm();
			if (postParams != null)
			{
				foreach (KeyValuePair<string, string> param in postParams)
				{
					if (param.Value == null)
					{
						throw new Exception("NULL PARAMATER PASSED TO WEB REQUEST:"+param.Key);
					}
					
					form.AddField(param.Key,param.Value);
				}
			}
			
			return doWWWCall(new WWW(url, form));
		}

		public WebServiceReturnStatus request(ITransfluentCall call)
		{
			Route route = RestUrl.GetRouteAttribute(call);
			string url = RestUrl.GetURL(call);
			WebServiceReturnStatus status;
			string urlWithGetParams = url + encodeGETParams(call.getParameters());
			if(route.requestType == RestRequestType.GET)
			{
				status = request(urlWithGetParams);
			}
			else
			{
				status = request(urlWithGetParams, call.postParameters());
			}

			return status;
		}
		public string encodeGETParams(Dictionary<string, string> getParams)
		{
			var sb = new StringBuilder();
			sb.Append("?");
			foreach(KeyValuePair<string, string> kvp in getParams)
			{
				sb.Append(WWW.EscapeURL(kvp.Key) + "=" + WWW.EscapeURL(kvp.Value) + "&");
			}
			return sb.ToString();
		}

		private WebServiceReturnStatus doWWWCall(WWW www)
		{
			var status = new WebServiceReturnStatus();

			var sw = new Stopwatch();
			sw.Start();
			while(www.isDone == false && www.error == null && sw.Elapsed.TotalSeconds < 10f)
			{
				//EditorApplication.Step();
				Thread.Sleep(100);
			}
			
			sw.Stop();
			status.requestTimeTaken = sw.Elapsed;

			if (!www.isDone)
			{
				status.status = ServiceStatus.TIMEOUT;
				www.Dispose();
				return status;
			}
			if (www.error == null)
			{
				status.status = ServiceStatus.SUCCESS;
				status.text = www.text;
				status.bytes = www.bytes;
			}else{
				string error = www.error;
				status.rawErrorCode = -1;
				int firstSpaceIndex = error.IndexOf(" ");
				if (firstSpaceIndex > 0)
				{
					
					int.TryParse(error.Substring(0, firstSpaceIndex), out status.rawErrorCode);
					if (status.rawErrorCode == 0)
					{
						throw new Exception("UNHANDLED ERROR CODE FORMAT:("+ error+")");
					}
					if(status.rawErrorCode >= 400 && status.rawErrorCode <= 499)
					{
						status.status = ServiceStatus.APPLICATION_ERROR;
					}
					else
					{
						status.status = ServiceStatus.TRANSPORT_ERROR;
					}
				}
				else
				{
					status.status = ServiceStatus.UNKNOWN; //can't parse error status
				}
				
			}
			www.Dispose();
			return status;
		}
	}


	
	public enum ServiceStatus
	{
		UNKNOWN,
		SUCCESS,
		TRANSPORT_ERROR,
		APPLICATION_ERROR,
		TIMEOUT,
	}

	public struct WebServiceReturnStatus
	{
		public TimeSpan requestTimeTaken;
		public ServiceStatus status;
		public int rawErrorCode;
		public string text; //if text is the  requested thing
		[JsonIgnore]
		public byte[] bytes;

		//this is here until I figure out how to get a result from status better
		public T Parse<T>()
		{
			var reader = new ResponseReader<T>
			{
				text = text
			};
			reader.deserialize();
			Debug.Log("TOSTRING STUFF " +ToString());
			return reader.response;
		}
		public bool wasSuccessful()
		{
			return status == ServiceStatus.SUCCESS && rawErrorCode == 0;
		}
		public override string ToString()
		{
			return "RETURN STATUS:" + JsonWriter.Serialize(this) + " time in seconds taken:" + requestTimeTaken.TotalSeconds;
		}
	}
}
