using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

namespace transfluent.tests
{
	[TestFixture]
	public class TestEntryPoints
	{
		public string accessToken;

		[TestFixtureSetUp]
		public void getTestCredentials()
		{
			OneTimeSetup();
		}
		
		public string justCall(WebServiceParameters call)
		{
			if(call.getParameters.ContainsKey("token"))
				call.getParameters.Remove("token");

			call.getParameters.Add("token", accessToken);
			var requester = new SyncronousEditorWebRequest();
			var result = requester.request(call);
			if(result.httpErrorCode > 0)
			{
				throw new HttpErrorCode(result.httpErrorCode);
			}
			return result.text;
		}
		//[Test]
		public void OneTimeSetup()
		{
			var credentials = new FileBasedCredentialProvider();
			Assert.False(string.IsNullOrEmpty(credentials.username));
			Assert.False(string.IsNullOrEmpty(credentials.password));
			var login = new Login
			(
				username : credentials.username,
				password : credentials.password
			);
			var responseText = justCall(login);

			accessToken = login.Parse(responseText).token;
			if(string.IsNullOrEmpty(accessToken))
			{
				throw new Exception("was not able to log in!");
			}
		}

		private const string HELLO_WORLD_TEXT_KEY = "HELLO_WORLD_TEXT_KEY";

		LanguageList getLanguageList()
		{
			var language = new RequestAllLanguages();
			var requester = new SyncronousEditorWebRequest();
			WebServiceReturnStatus status = requester.request(language);

			var list = language.Parse(status.text);
			Assert.NotNull(list);

			Assert.IsTrue(list.languages.Count > 0);
			return list;
		}

		[Test]
		[ExpectedException(typeof(ApplicatonLevelException))]
		public void getKeyThatDoesNotExist()
		{
			TransfluentLanguage englishLanguage = getLanguageList().getLangaugeByCode("en-us");

			var testForExistance = new GetTextKey
			(
				languageID : englishLanguage.id,
				text_id    : "THIS_DOES_NOT_EXIST" + Random.value
			);
			string rawOutput = justCall(testForExistance);
			testForExistance.Parse(rawOutput);
		}

		[Test]
		public void testBackwardsLanguage()
		{
			var list = getLanguageList();
			Assert.NotNull(list);
			Assert.IsTrue(list.languages.Count > 0);

			TransfluentLanguage englishLanguage = list.getLangaugeByCode("en-us");
			TransfluentLanguage lang = list.getLangaugeByCode(TransfluentLanguage.BACKWARDS_LANGUAGE_NAME);
			Assert.AreNotEqual(englishLanguage.code, 0);
			Assert.NotNull(lang);

			//post text key
			string textToSave = "this is sample text" + Random.value;
			var saveOp = new SaveTextKey
			(
				language : englishLanguage.id,
				text     : textToSave,
				text_id  : HELLO_WORLD_TEXT_KEY
			);
			bool saved = saveOp.Parse(justCall(saveOp));
			Debug.Log("Saved successfullly:" + saved);

			var testForExistance = new GetTextKey
			(
				languageID : englishLanguage.id,
				text_id    : HELLO_WORLD_TEXT_KEY
			);
			string keyFromDB = testForExistance.Parse(justCall(testForExistance));
			Assert.IsFalse(string.IsNullOrEmpty(keyFromDB));
			Assert.AreEqual(textToSave, keyFromDB);
		}

		[Test]
		public void testGetAllLanugages()
		{
			getLanguageList();
		}

		[Test]
		public void testHello()
		{
			var hello = new Hello
			(
				name : "world"
			);
			var text = hello.Parse(justCall(hello)); 

			Assert.IsNotNull(text);
			Assert.AreEqual(text.ToLower(), "hello world");
		}

		[Test]
		public void testLanguages()
		{
			var language = getLanguageList();

			Assert.IsNotNull(language);
			Assert.IsNotNull(language.languages);
			Assert.IsTrue(language.languages.Count > 0);
		}
	}
}
