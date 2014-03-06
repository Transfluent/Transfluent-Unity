using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;

namespace transfluent
{
	class GameTimeWWW
	{
		private GameTimeWWWMonobehaviour routineRunner;
		public GameTimeWWW()
		{
			GameObject go = new GameObject("serviceRunner");
			routineRunner = go.AddComponent<GameTimeWWWMonobehaviour>();
		}

		public void startRoutine(IEnumerator routine)
		{
			routineRunner.StartCoroutine(routine);
		}

		public void webRequest(WebServiceParameters call,Action<WebServiceReturnStatus> onStatusDone)
		{
			GotstatusUpdate wrappedReturn = status =>
			{
				if (onStatusDone != null)
					onStatusDone(status);
				return null;
			};
			routineRunner.StartCoroutine(doWebRequest(call, wrappedReturn));
		}
		public void webRequest(WebServiceParameters call, GotstatusUpdate onStatusDone)
		{
			routineRunner.StartCoroutine(doWebRequest(call, onStatusDone));
		}
		public delegate IEnumerator GotstatusUpdate(WebServiceReturnStatus status);

		IEnumerator doWebRequest(WebServiceParameters call, GotstatusUpdate onStatusDone)
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
				routineRunner.StartCoroutine(onStatusDone(status));
			}
		}
	}

	class GameTimeWWWMonobehaviour : MonoBehaviour
	{
		
	}
}
