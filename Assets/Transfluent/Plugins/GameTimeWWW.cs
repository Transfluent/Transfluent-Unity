using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace transfluent
{
	public class GameTimeWWW
	{
		public delegate IEnumerator GotstatusUpdate(WebServiceReturnStatus status);

		public IRoutineRunner runner = new RoutineRunner();

		public void startRoutine(IEnumerator routine)
		{
			runner.runRoutine(routine);
		}

		public void webRequest(ITransfluentParameters call, GotstatusUpdate onStatusDone)
		{
			runner.runRoutine(doWebRequest(call, onStatusDone));
		}

		private IEnumerator doWebRequest(ITransfluentParameters call, GotstatusUpdate onStatusDone)
		{
			var facade = new WWWFacade();
			var sw = new Stopwatch();
			sw.Start();
			WWW www = facade.request(call);

			yield return www;
			var status = new WebServiceReturnStatus { serviceParams = call };
			try
			{
				status = facade.getStatusFromFinishedWWW(www, sw, call);
			}
			catch(CallException e)
			{
				Debug.Log("Exception:" + e.Message);
			}

			if(onStatusDone != null)
			{
				runner.runRoutine(onStatusDone(status));
			}
		}
	}
}