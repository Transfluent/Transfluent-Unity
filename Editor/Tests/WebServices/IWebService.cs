using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Pathfinding.Serialization.JsonFx;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace transfluent
{
	public interface IWebService
	{
		ReturnStatus request(string url);
		ReturnStatus request(string url, Dictionary<string, string> postParams);
		string encodeGETParams(Dictionary<string, string> getParams);
	}
	public class DebugSyncronousEditorWebRequest : IWebService
	{
		private const bool debug = true;
		private IWebService realRequest = new SyncronousEditorWebRequest();

		public DebugSyncronousEditorWebRequest()
		{
			Debug.Log("CREATING SYNC REQUESTs");
		}

		public ReturnStatus request(string url)
		{
			if(debug) Debug.Log("calling url:" + url + "(GET) ");
			var result = realRequest.request(url);
			if(debug) Debug.Log("GOT BACK WITH RESULT:" + result);
			return result;
		}

		public ReturnStatus request(string url, Dictionary<string, string> postParams)
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

		public string encodeGETParams(Dictionary<string, string> getParams)
		{
			return realRequest.encodeGETParams(getParams);
		}
	}
	public class SyncronousEditorWebRequest : IWebService
	{
		
		public ReturnStatus request(string url)
		{
			
			return doWWWCall(new WWW(url));
		}

		public ReturnStatus request(string url, Dictionary<string, string> postParams)
		{
			WWWForm form = new WWWForm();
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

		public string encodeGETParams(Dictionary<string, string> getParams)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("?");
			foreach(KeyValuePair<string, string> kvp in getParams)
			{
				sb.Append(kvp.Key + "=" + kvp.Value + "&");
			}
			return sb.ToString();
		}

		private ReturnStatus doWWWCall(WWW www)
		{
			ReturnStatus status = new ReturnStatus();

			var sw = new Stopwatch();
			sw.Start();
			while(www.isDone == false && www.error == null && sw.Elapsed.Seconds < 10f)
			{
				EditorApplication.Step();
				//Thread.Sleep(100);
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
					
					status.text = Encoding.UTF8.GetString(www.bytes);
				}
				else
				{
					status.status = ServiceStatus.UNKNOWN; //can't parse error status
				}
				
			}
			www.Dispose();
			return status;
			//Debug.Log("time elapsed running test:" + sw.Elapsed);
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

	public struct ReturnStatus
	{
		public TimeSpan requestTimeTaken;
		public ServiceStatus status;
		public int rawErrorCode;
		public string text; //if text is the  requested thing
		[JsonIgnore]
		public byte[] bytes;

		public override string ToString()
		{
			return "RETURN STATUS:"+ JsonWriter.Serialize(this);
		}
	}
}
