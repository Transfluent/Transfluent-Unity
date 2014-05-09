using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NSubstitute.Core;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace transfluent.editor
{
	[ExecuteInEditMode]
	public class AsyncEditorWebRequester
	{
		private GameTimeWWW www;

		//[MenuItem("asink/test asink hijack")]
		public static void MakeRequests()
		{
			var hijack = new AsyncEditorWebRequester();
			hijack.DoThing(new RequestAllLanguages(), gotStatusUpdate);
			hijack.DoThing(new Hello("World"), gotStatusUpdate);
			Debug.Log("DOING THING");
		}

		private static IEnumerator gotStatusUpdate(WebServiceReturnStatus status)
		{
			Debug.Log("Web request got back:" + status);
			yield return null;
		}

		public void DoThing(ITransfluentParameters parameters, GameTimeWWW.GotstatusUpdate statusUpdated)
		{
			www = new GameTimeWWW();
			www.runner = new AsyncRunner();
			www.webRequest(parameters, statusUpdated);
		}
	}

	[ExecuteInEditMode]
	public class AsyncRunner : IRoutineRunner
	{
		private static readonly TimeSpan maxTime = new TimeSpan(0, 0, 10);
		private IEnumerator _routineHandle;
		private Stopwatch sw;

		public void runRoutine(IEnumerator routineToRun)
		{
			_routineHandle = routineToRun;
			sw = new Stopwatch();
			sw.Start();
			Debug.Log("Run routine");
			doCoroutine();
		}

		//[MenuItem("asink/testme2")]
		public static void testMe()
		{
			var runner = new AsyncRunner();
			runner.runRoutine(testRoutine());
		}

		private static IEnumerator testRoutine()
		{
			var sw = new Stopwatch();
			sw.Start();
			int ticks = 0;
			//while(maxticks >0)
			while(ticks < 100) //sw.Elapsed < maxTime)
			{
				ticks++;
				Debug.Log("MAXticks:" + ticks + " time:" + sw.Elapsed);
				yield return null;
			}
			Debug.LogWarning(ticks + "TOTLAL TIME:" + sw.Elapsed);
			yield return null;
			Debug.LogWarning("LAST LINE OF COROUTINE");
		}

		private void doCoroutine()
		{
			Debug.Log("DO COROTUINE");
			if(sw.Elapsed < maxTime)
			{
				//if routineHandl e.Current == waitforseconds... wait for that many seconds before checking or moving forward again
				if(_routineHandle != null)
				{
					//kill the reference if we no longer move forward
					if(!_routineHandle.MoveNext())
					{
						Debug.LogWarning("KILLING SELF as otherCoroutine ended:" + sw.Elapsed);
						_routineHandle = null;
						EditorApplication.update = null;
					}
					else
					{
						Debug.Log("setting up to run again");
						EditorApplication.update = doCoroutine;
					}
				}
				else
				{
					Debug.LogWarning("ENDED COROUTINE BECAUSE routine is over");
					EditorApplication.update = null;
				}
			}
			else
			{
				Debug.LogWarning("waiting for next editor update");
				EditorApplication.update = doCoroutine;
			}
		}

	}

	[ExecuteInEditMode]
	public class AsyncTester : IRoutineRunner
	{
		public static int staticCounter = 1;
		private readonly int counter;
		private readonly TimeSpan maxTime = new TimeSpan(0, 0, 10);
		private readonly Stopwatch sw;

		private IEnumerator routineHandle;

		public AsyncTester()//Func<bool> isDone
		{
			counter = staticCounter++;
			sw = new Stopwatch();
			sw.Start();

			routineHandle = testRoutine();
			EditorApplication.update += doCoroutine;
		}

		public void runRoutine(IEnumerator routineToRun)
		{
			throw new NotImplementedException();
		}

		//[MenuItem("asink/testme")]
		public static void testMe()
		{
			new AsyncTester();
		}

		public IEnumerator testRoutine()
		{
			int maxticks = 100;
			Debug.Log(counter + "MAXticks:" + maxticks);
			//while(maxticks >0)
			
			yield return new WaitForSeconds(5f);
			while(sw.Elapsed < maxTime)
			{
				maxticks--; 
				UnityEngine.Debug.Log("MAXticks:" + maxticks + " time:" + sw.Elapsed);
				yield return null;
			}
			Debug.LogWarning(counter + "TOTLAL TIME:" + sw.Elapsed);
			yield return null;
			Debug.LogWarning("LAST LINE OF COROUTINE");
		}

		private void doCoroutine()
		{
			//Debug.Log(counter + "coroutine:" );
			if(sw.Elapsed < maxTime) //if(true) also works.
			{
				//if routineHandl e.Current == waitforseconds... wait for that many seconds before checking or moving forward again
				if(routineHandle != null)
				{

					//kill the reference if we no longer move forward
					if(!routineHandle.MoveNext())
					{
						Debug.LogWarning(counter + "KILLING SELF as otherCoroutine ended:" + sw.Elapsed);
						routineHandle = null;
					}
				}
			}
			else
			{
				EditorApplication.update = doCoroutine;
			}
		}
	}

	[ExecuteInEditMode]
	public class EditorWWWWaitUntil2
	{
		private WWW _www;
		private Action<WebServiceReturnStatus> _callback;
		WWWFacade _getMyWwwFacade = new WWWFacade();
		Stopwatch _sw = new Stopwatch();
		private ITransfluentParameters _callParams;
		public EditorWWWWaitUntil2(ITransfluentParameters callParams, Action<WebServiceReturnStatus> callback)
		{
			_sw.Start();
			_callParams = callParams;

			_getMyWwwFacade = new WWWFacade();
			//string url = _getMyWwwFacade.encodeGETParams(callParams.getParameters);
			_www = _getMyWwwFacade.request(callParams);
			
			_callback = callback;
			EditorWaitUntil wait = new EditorWaitUntil(() =>
			{ return _www.error != null || _www.isDone; },
				internalCallback
			);

		}

		void internalCallback()
		{
			if(_callback != null)
			{
				_callback(_getMyWwwFacade.getStatusFromFinishedWWW(_www, _sw,_callParams));
			}

		}
		WWW getStatus()
		{
			return _www;
		}
	}
	[ExecuteInEditMode]
	public class EditorWWWWaitUntil
	{
		private WWW _www;
		private Action<WWW> _callback;
		WWWFacade _getMyWwwFacade = new WWWFacade();

		public EditorWWWWaitUntil(WWW www,Action<WWW> callback)
		{
			_www = www;
			_callback = callback;
			EditorWaitUntil wait = new EditorWaitUntil(() =>
			{ return _www.error != null || _www.isDone; },
				internalCallback
			);

		}

		void internalCallback()
		{
			if(_callback != null)
			{
				_callback(_www);
			}
			
		}
		WWW getStatus()
		{
			return _www;
		}
	}

	[ExecuteInEditMode]
	public class EditorWaitUntil 
	{
		private IEnumerator routineHandle;
		private Func<bool> _isDone;
		private Action _onFinished;
		public EditorWaitUntil(Func<bool> isDone,Action onFinished)
		{
			_isDone = isDone;
			_onFinished = onFinished;

			EditorApplication.update += doCoroutine;
		}


		//[MenuItem("asink/test waituntil")]
		public static void testMe()
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();

			var wait = new EditorWaitUntil(() => { return sw.Elapsed.Seconds > 15; }, () => { Debug.Log("Editor thing finished"); });

		} 
		//TODO: can I run multiple of these
		private void doCoroutine()
		{
			if(_isDone() == false)
			{
				EditorApplication.update = doCoroutine;
			} else
			{
				EditorApplication.update = null;
				if(_onFinished != null)
				{
					_onFinished();
				}
			}

		}
	}
}