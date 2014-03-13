using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;
using transfluent;
using UnityEngine;

public class DebugSyncronousEditorWebRequest : IWebService
{
	private readonly IWebService realRequest = new SyncronousEditorWebRequest();
	public bool debug = true;

	public DebugSyncronousEditorWebRequest()
	{
		if (debug) Debug.Log("CREATING SYNC REQUESTs");
	}

	public WebServiceReturnStatus request(string url)
	{
		if (debug) Debug.Log("calling url:" + url + "(GET) ");
		WebServiceReturnStatus result = realRequest.request(url);
		if (debug) Debug.Log("GOT BACK WITH RESULT:" + result);
		return result;
	}

	public WebServiceReturnStatus request(string url, Dictionary<string, string> postParams)
	{
		if (postParams != null)
		{
			foreach (var param in postParams)
			{
				if (debug) Debug.Log("Field added:" + param.Key + " with value:" + param.Value);
			}
			if (debug) Debug.Log("ALL params:" + JsonWriter.Serialize(postParams));
		}
		if (debug) Debug.Log("calling url:" + url + "(POST) ");
		WebServiceReturnStatus result = realRequest.request(url, postParams);

		if (debug) Debug.Log("GOT BACK WITH RESULT:" + result);
		return result;
	}

	public WebServiceReturnStatus request(ITransfluentParameters call)
	{
		return realRequest.request(call);
	}
}