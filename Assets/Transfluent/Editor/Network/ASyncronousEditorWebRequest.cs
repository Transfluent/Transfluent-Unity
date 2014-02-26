using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace transfluent
{

	public class AsyncTester
	{
		[MenuItem("asink/testme")]
		public static void testMe()
		{
			new AsyncTester();
		}

		private TimeSpan maxTime = new TimeSpan(0, 0, 10);
		private Stopwatch sw;

		private IEnumerator routineHandle;

		public AsyncTester()
		{
			sw = new Stopwatch();
			EditorApplication.update += doCoroutine;

			routineHandle = testRoutine();
		}

		public IEnumerator testRoutine()
		{
			int maxticks = 100;
			
			//while(maxticks >0)
			while(sw.Elapsed < maxTime)
			{
				maxticks--;
				//UnityEngine.Debug.Log("MAXticks:" + maxticks);
				yield return null;
			}

		}

		private void doCoroutine()
		{
			sw.Start();
			if (sw.Elapsed < maxTime) //if(true) also works.
			{
				//if routineHandl e.Current == waitforseconds... wait for that many seconds before checking or moving forward again
				if (routineHandle != null)
				{
					//kill the reference if we no longer move forward
					if(!routineHandle.MoveNext())
					{
						UnityEngine.Debug.Log("KILLING SELF:"+sw.Elapsed);
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