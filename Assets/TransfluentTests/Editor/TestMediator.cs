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
		[Test]
		public void testCreation()
		{
			var mediator = new TransfluentEditorWindowMediator();
			Assert.IsNotNullOrEmpty(mediator.getUserNamePassword().Key);
			
			Assert.NotNull(mediator.getUserNamePassword());
			Assert.IsNotNullOrEmpty(mediator.getUserNamePassword().Key);
			Assert.IsNotNullOrEmpty(mediator.getUserNamePassword().Value);

			mediator.doAuth(mediator.getUserNamePassword().Key, mediator.getUserNamePassword().Value);
			Assert.IsTrue(mediator.authIsDone());
		}

	}

}

