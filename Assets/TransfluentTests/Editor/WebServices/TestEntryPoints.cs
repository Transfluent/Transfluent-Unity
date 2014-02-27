using System;
using NUnit.Framework;
using transfluent;
using UnityEngine;
using Random = UnityEngine.Random;

[TestFixture]
public class TestEntryPoints
{
	public string accessToken;

	[TestFixtureSetUp]
	public void getTestCredentials()
	{
		OneTimeSetup();
	}

	//[Test]
	public void OneTimeSetup()
	{
		var credentials = new FileBasedCredentialProvider();
		Assert.False(string.IsNullOrEmpty(credentials.username));
		Assert.False(string.IsNullOrEmpty(credentials.password));
		var login = new Login
		{
			username = credentials.username,
			password = credentials.password,
			service = new SyncronousEditorWebRequest()
		};
		login.Execute();

		accessToken = login.token;
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new Exception("was not able to log in!");
		}
	}

	private string HELLO_WORLD_TEXT_KEY = "HELLO_WORLD_TEXT_KEY";

	[Test]
	public void getKeyThatDoesNotExist()
	{
		var language = new RequestAllLanguages(){service = new SyncronousEditorWebRequest()};
		language.Execute();
		TransfluentLanguage2 englishLanguage = language.languagesRetrieved.getLangaugeByCode("en-us");

		var testForExistance = new GetTextKey
		{
			authToken = accessToken,
			languageID = englishLanguage.id,
			text_id = "THIS_DOES_NOT_EXIST" + Random.value,
			service = new SyncronousEditorWebRequest()
		};
		Assert.Throws(typeof (Exception), testForExistance.Execute);
	}

	[Test]
	public void testBackwardsLanguage()
	{
		var language = new RequestAllLanguages(){service = new SyncronousEditorWebRequest()};
		language.Execute();

		LanguageList list = language.languagesRetrieved;
		Assert.NotNull(list);
		Assert.IsTrue(list.languages.Count > 0);

		TransfluentLanguage2 englishLanguage = list.getLangaugeByCode("en-us");
		TransfluentLanguage2 lang = list.getLangaugeByCode(TransfluentLanguage2.BACKWARDS_LANGUAGE_NAME);
		Assert.AreNotEqual(englishLanguage.code, 0);
		Assert.NotNull(lang);

		//post text key
		string textToSave = "this is sample text" + Random.value;
		var saveOp = new SaveTextKey
		{
			authToken = accessToken,
			language = englishLanguage.id,
			text = textToSave,
			text_id = HELLO_WORLD_TEXT_KEY,
			service = new SyncronousEditorWebRequest()
		};
		saveOp.Execute();

		Debug.Log("Saved successfullly:" + saveOp.savedSuccessfully);

		var testForExistance = new GetTextKey
		{
			authToken = accessToken,
			languageID = englishLanguage.id,
			text_id = HELLO_WORLD_TEXT_KEY,
			service = new SyncronousEditorWebRequest()
		};
		testForExistance.Execute();
		Assert.IsFalse(string.IsNullOrEmpty(testForExistance.resultOfCall));
		Assert.AreEqual(textToSave, testForExistance.resultOfCall);
	}

	[Test]
	public void testGetAllLanugages()
	{
		var language = new RequestAllLanguages() { service = new SyncronousEditorWebRequest() };
		language.Execute();
	}

	[Test]
	public void testHello()
	{
		var hello = new Hello
		{
			name = "world",
			service = new SyncronousEditorWebRequest()
		};
		hello.Execute();

		Assert.IsNotNull(hello.helloWorldText);
		Assert.AreEqual(hello.helloWorldText.ToLower(), "hello world");
	}

	[Test]
	public void testLanguages()
	{
		var language = new RequestAllLanguages() { service = new SyncronousEditorWebRequest() };
		language.Execute();

		Assert.IsNotNull(language.languagesRetrieved);
		LanguageList list = language.languagesRetrieved;
		Assert.IsNotNull(list);
		Assert.IsTrue(list.languages.Count > 0);
	}
}