using System.Collections.Generic;
using NUnit.Framework;
using transfluent.editor;

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

			List<TransfluentTranslation> textEntries = mediator.knownTextEntries();

			Assert.NotNull(textEntries);


			Assert.Greater(textEntries.Count, 0);
		}
	}
}