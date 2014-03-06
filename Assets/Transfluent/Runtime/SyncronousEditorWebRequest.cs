using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using transfluent;
using UnityEngine;

public class SyncronousEditorWebRequest : IWebService
{
	readonly WWWFacade _getMyWwwFacade = new WWWFacade();

	public WebServiceReturnStatus request(string url)
	{
		WWW www = _getMyWwwFacade.request(url);
		return doWWWCall(www);
	}

	public WebServiceReturnStatus request(string url, Dictionary<string, string> postParams)
	{
		WWW www = _getMyWwwFacade.request(url, postParams);
		return doWWWCall(www);
	}

	public WebServiceReturnStatus request(ITransfluentParameters parameters)
	{
		WWW www = _getMyWwwFacade.request(parameters);
		return doWWWCall(www,parameters);
	}

	private WebServiceReturnStatus doWWWCall(WWW www, ITransfluentParameters wsParams = null)
	{
		var sw = new Stopwatch();
		sw.Start();
		while(true)
		{
			if(www.isDone)
				break;
			if(www.error != null)
				break;
			if(sw.Elapsed.TotalSeconds >= 30f)
				break;

			//EditorApplication.Step();
			Thread.Sleep(100);
		}

		return _getMyWwwFacade.getStatusFromFinishedWWW(www, sw, wsParams);
	}

	public bool knownTransportError(string input)
	{
		if(input.Contains("Could not resolve host"))
		{
			return true;
		}
		return false;
	}

	//Could not resolve host: transfluent.com (Could not contact DNS servers)
}