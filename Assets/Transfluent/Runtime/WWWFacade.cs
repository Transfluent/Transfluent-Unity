using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace transfluent
{
	public class WWWFacade
	{
		public WWW request(string url, Dictionary<string, string> postParams)
		{
			var form = new WWWForm();
			if(postParams != null)
			{
				foreach(var param in postParams)
				{
					if(param.Value == null)
					{
						throw new Exception("NULL PARAMATER PASSED TO WEB REQUEST:" + param.Key);
					}

					form.AddField(param.Key, param.Value);
				}
			}

			return new WWW(url, form);
		}

		public WWW request(ITransfluentParameters call)
		{
			Route route = RestUrl.GetRouteAttribute(call.GetType());
			string url = RestUrl.GetURL(call);

			string urlWithGetParams = url + encodeGETParams(call.getParameters);
			if(route.requestType == RestRequestType.GET)
			{
				return request(urlWithGetParams);
			}
			return request(urlWithGetParams, call.postParameters);
		}

		public WWW request(string url)
		{
			return new WWW(url);
		}

		public string encodeGETParams(Dictionary<string, string> getParams)
		{
			var sb = new StringBuilder();
			sb.Append("?");
			foreach(var kvp in getParams)
			{
				sb.Append(WWW.EscapeURL(kvp.Key) + "=" + WWW.EscapeURL(kvp.Value) + "&");
			}
			string fullUrl = sb.ToString();
			if(fullUrl.EndsWith("&"))
			{
				fullUrl = fullUrl.Substring(0, fullUrl.LastIndexOf("&"));
			}
			return fullUrl;
		}

		//throws TransportException,ApplicatonLevelException,HttpErrorCode
		public WebServiceReturnStatus getStatusFromFinishedWWW(WWW www, Stopwatch sw,
			ITransfluentParameters originalCallParams)
		{
			//Debug.Log("WWW:" + www.url);

			var status = new WebServiceReturnStatus
			{
				serviceParams = originalCallParams
			};
			sw.Stop();
			status.requestTimeTaken = sw.Elapsed;

			if(!www.isDone && www.error == null)
			{
				throw new TransportException("Timeout total time taken:");
			}
			if(www.error == null)
			{
				status.text = www.text;
			}
			else
			{
				string error = www.error;
				if(knownTransportError(error))
				{
					www.Dispose();
					throw new TransportException(error);
				}
				status.httpErrorCode = -1;
				int firstSpaceIndex = error.IndexOf(" ");

				if(firstSpaceIndex > 0)
				{
					www.Dispose();

					int.TryParse(error.Substring(0, firstSpaceIndex), out status.httpErrorCode);
					//there has to be a better way to get error codes.
					if(status.httpErrorCode == 0)
					{
						throw new Exception("UNHANDLED ERROR CODE FORMAT:(" + error + ")");
					}
					if(status.httpErrorCode >= 400 && status.httpErrorCode <= 499)
					{
						throw new ApplicatonLevelException("HTTP Error code, application level:" + status.httpErrorCode,
							status.httpErrorCode);
					}
					throw new HttpErrorCode(status.httpErrorCode);
				}
				throw new Exception("Unknown error:" + error); //can't parse error status
			}
			www.Dispose();
			return status;
		}

		public bool knownTransportError(string input)
		{
			if(input.Contains("Could not resolve host"))
			{
				return true;
			}
			return false;
		}
	}
}