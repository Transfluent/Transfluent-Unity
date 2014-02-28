using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace transfluent.tests
{
	[TestFixture]
	public class TestEditorWebService
	{
		private void manualWWWCall(WWW www)
		{
			var sw = new Stopwatch();
			sw.Start();
			while(www.isDone == false && www.error == null && sw.Elapsed.Seconds < 1f)
			{
				//EditorApplication.Step();
				Thread.Sleep(100);
			}
			Debug.Log("time elapsed running test:" + sw.Elapsed);
			Assert.IsTrue(www.isDone || www.error != null);
		}


		[Test]
		public void testWWWCallWithStep()
		{
			var sw = new Stopwatch();
			sw.Start();
			var testWww = new WWW("https://transfluent.com/v2/");
			int maxTicks = 100;
			while(testWww.isDone == false && maxTicks-- > 0)
			{
				EditorApplication.Step();
				Thread.Sleep(100);
			}
			Debug.Log("time elapsed running test:" + sw.Elapsed);
			Assert.IsTrue(testWww.isDone);
		}

		[Test]
		public void testWWWCallWithThreadSleep()
		{
			var sw = new Stopwatch();
			sw.Start();
			var testWww = new WWW("https://transfluent.com/v2/");
			while(testWww.isDone == false && sw.Elapsed.Seconds < 1f)
			{
				Thread.Sleep(100);
			}
			Debug.Log("time elapsed running test:" + sw.Elapsed);
			Assert.IsTrue(testWww.isDone);
		}
	}

}
