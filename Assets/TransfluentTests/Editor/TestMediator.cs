using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using transfluent.editor;
using UnityEngine;
using NUnit.Framework;
using Debug = UnityEngine.Debug;

namespace transfluent.tests
{
	[TestFixture]
	public class TestMediator
	{
		private TransfluentEditorWindowMediator mediator;
		[TestFixtureSetUp]
		public void testCreation()
		{
			mediator = new TransfluentEditorWindowMediator();
			Assert.IsNotNullOrEmpty(mediator.getUserNamePassword().Key);
			
			Assert.NotNull(mediator.getUserNamePassword());
			Assert.IsNotNullOrEmpty(mediator.getUserNamePassword().Key);
			Assert.IsNotNullOrEmpty(mediator.getUserNamePassword().Value);

			mediator.doAuth(mediator.getUserNamePassword().Key, mediator.getUserNamePassword().Value);
			Assert.IsTrue(mediator.authIsDone());
		}

		[Test]
		public void setCurrentLanguage()
		{
			
		}

		[Test]
		public void testGetAllKeys()
		{
			string langauge = "fr-fr";
			mediator.setCurrentLanguageFromLanguageCode(langauge);

			mediator.SetText("TEST_TEXT", "HELLO WORLD");

			Assert.NotNull(mediator.GetCurrentLanguage());
			Assert.AreEqual(mediator.GetCurrentLanguage().code, langauge);

			var textEntries = mediator.knownTextEntries();

			Assert.NotNull(textEntries);


			Assert.Greater(textEntries.Count, 0);
		}

	}

}

