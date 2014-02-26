using System;
using System.Diagnostics;
using NUnit.Framework;
using transfluent;
using UnityEditor;

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
	public void testBackwardsLanguage()
	{
		var language = new RequestAllLanguages();
		language.Execute();

		LanguageList list = language.languagesRetrieved;
		Assert.NotNull(list);
		Assert.IsTrue(list.languages.Count > 0);

		var englishLanguage = list.getLangaugeByCode("en-us");
		var lang = list.getLangaugeByCode(TransfluentLanguage2.BACKWARDS_LANGUAGE_NAME);
		Assert.AreNotEqual(englishLanguage.code,0);
		Assert.NotNull(lang);

		//post text key
		string textToSave = "this is sample text" + UnityEngine.Random.value;
		SaveTextKey saveOp = new SaveTextKey()
		{
			authToken = accessToken,
			language = englishLanguage.id,
			text = textToSave,
			text_id = HELLO_WORLD_TEXT_KEY
		};
		saveOp.Execute();

		UnityEngine.Debug.Log("Saved successfullly:" + saveOp.savedSuccessfully);

		GetTextKey testForExistance = new GetTextKey()
		{
			authToken = accessToken,
			language = englishLanguage.id,
			text_id = HELLO_WORLD_TEXT_KEY
		};
		testForExistance.Execute();
		Assert.IsFalse(string.IsNullOrEmpty(testForExistance.keyValue));
		Assert.AreEqual(textToSave, testForExistance.keyValue);
	}

	[Test]
	public void getKeyThatDoesNotExist()
	{
		var language = new RequestAllLanguages();
		language.Execute();
		var englishLanguage = language.languagesRetrieved.getLangaugeByCode("en-us");

		GetTextKey testForExistance = new GetTextKey()
		{
			authToken = accessToken,
			language = englishLanguage.id,
			text_id = "THIS_DOES_NOT_EXIST" + UnityEngine.Random.value
		};
		Assert.Throws(typeof(Exception), testForExistance.Execute);
	}

	[Test]
	public void testGetAllLanugages()
	{
		var language = new RequestAllLanguages();
		language.Execute();
	}

	[Test]
	public void testHello()
	{
		var hello = new Hello
		{
			name = "world"
		};
		hello.Execute();

		Assert.IsNotNull(hello.helloWorldText);
		Assert.AreEqual(hello.helloWorldText.ToLower(), "hello world");
	}

	[Test]
	public void testLanguages()
	{
		var language = new RequestAllLanguages();
		language.Execute();

		Assert.IsNotNull(language.languagesRetrieved);
		var list = language.languagesRetrieved;
		Assert.IsNotNull(list);
		Assert.IsTrue(list.languages.Count > 0);
	}

	
}