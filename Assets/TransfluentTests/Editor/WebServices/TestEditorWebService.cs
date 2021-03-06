﻿using NUnit.Framework;
using System.Diagnostics;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace transfluent.tests
{
	[TestFixture]
	public class TestEditorWebService
	{
		[Test]
		public void testDebugSyncronousWebService()
		{
			IWebService service = new DebugSyncronousEditorWebRequest();
			WebServiceReturnStatus result = service.request("https://transfluent.com/v2/hello/world");
			Assert.AreEqual(result.httpErrorCode, 0);
			Assert.NotNull(result.text);
			Assert.Greater(result.text.Length, 0);
		}

		[Test]
		public void testSyncronousWebService()
		{
			IWebService service = new SyncronousEditorWebRequest();
			WebServiceReturnStatus result = service.request("https://transfluent.com/v2/hello/world");
			Assert.AreEqual(result.httpErrorCode, 0);
			Assert.NotNull(result.text);
			Assert.Greater(result.text.Length, 0);
		}

		[Test]
		public void testWWWCallWithStep()
		{
			var sw = new Stopwatch();
			sw.Start();
			var testWww = new WWW("https://transfluent.com/v2/hello/world");
			while(testWww.isDone == false && sw.Elapsed.Seconds < 20f)
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
			var testWww = new WWW("https://transfluent.com/v2/hello/world");
			while(testWww.isDone == false && sw.Elapsed.Seconds < 20f)
			{
				Thread.Sleep(100);
			}
			Debug.Log("time elapsed running test:" + sw.Elapsed);
			Assert.IsTrue(testWww.isDone);
		}
	}
}