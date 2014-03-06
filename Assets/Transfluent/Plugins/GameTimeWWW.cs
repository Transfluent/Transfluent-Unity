using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace transfluent
{
	class GameTimeWWW
	{
		private IRoutineRunner runner = new RoutineRuner();
		
		public void startRoutine(IEnumerator routine)
		{
			runner.runRoutine(routine);
		}

		public void webRequest(ITransfluentParameters call, GotstatusUpdate onStatusDone)
		{
			runner.runRoutine(doWebRequest(call, onStatusDone));
		}
		public delegate IEnumerator GotstatusUpdate(WebServiceReturnStatus status);

		IEnumerator doWebRequest(ITransfluentParameters call, GotstatusUpdate onStatusDone)
		{
			WWWFacade facade = new WWWFacade();
			Stopwatch sw = new Stopwatch();
			sw.Start();
			WWW www = facade.request(call);

			yield return www;
			WebServiceReturnStatus status = new WebServiceReturnStatus() {serviceParams = call};
			try
			{
				status = facade.getStatusFromFinishedWWW(www, sw,call);
			}
			catch (CallException e)
			{
				UnityEngine.Debug.Log("Exception:" + e.Message);
			}
			
			if (onStatusDone != null)
			{
				runner.runRoutine(onStatusDone(status));
			}
		}
	}

}
