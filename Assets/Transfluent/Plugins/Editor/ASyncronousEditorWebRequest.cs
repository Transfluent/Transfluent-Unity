using System;
using System.Collections;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace transfluent.editor
{
	[ExecuteInEditMode]
	public class AsyncEditorWebRequester
	{
		private GameTimeWWW www;

		[MenuItem("asink/test asink hijack")]
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

		[MenuItem("asink/testme2")]
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

		public AsyncTester()
		{
			counter = staticCounter++;
			sw = new Stopwatch();
			routineHandle = testRoutine();
			EditorApplication.update += doCoroutine;
		}

		public void runRoutine(IEnumerator routineToRun)
		{
			throw new NotImplementedException();
		}

		[MenuItem("asink/testme")]
		public static void testMe()
		{
			new AsyncTester();
			//new AsyncTester();
			//new AsyncTester();
		}

		public IEnumerator testRoutine()
		{
			int maxticks = 100;
			Debug.Log(counter + "MAXticks:" + maxticks);
			//while(maxticks >0)
			while(sw.Elapsed < maxTime)
			{
				maxticks--;
				//UnityEngine.Debug.Log("MAXticks:" + maxticks + " time:" + sw.Elapsed);
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
}