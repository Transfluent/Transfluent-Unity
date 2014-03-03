using System;
using System.Collections.Generic;
using NUnit.Framework;
using Pathfinding.Serialization.JsonFx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace transfluent.tests
{
	[TestFixture]
	internal class TestTranslations
	{
		public string accessToken;
		public TransfluentLanguage englishLanguage;
		public TransfluentLanguage backwardsLanguage;
		private LanguageList languageCache;
		public const string textToSetTestTokenTo = "this is test text";

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
			(
				username : credentials.username,
				password : credentials.password
			);
			var request = new DebugSyncronousEditorWebRequest();

			var response = login.Parse(request.request(login).text);
			accessToken = response.token;
			if (string.IsNullOrEmpty(accessToken))
			{
				throw new Exception("was not able to log in!");
			}
			getLanguages();
			SaveRetrieveKey(TRANSLATION_KEY);
		}

		public void getLanguages()
		{
			var language = new RequestAllLanguages();
			var requester = new SyncronousEditorWebRequest();

			var list = language.Parse(requester.request(language).text);
			Assert.NotNull(list);

			englishLanguage = list.getLangaugeByCode("en-us");
			backwardsLanguage = list.getLangaugeByCode(TransfluentLanguage.BACKWARDS_LANGUAGE_NAME);
			languageCache = list;
			Assert.AreNotEqual(englishLanguage.code, 0);
			Assert.NotNull(backwardsLanguage);
		}

		public void SaveRetrieveKey(string keyToSaveAndThenGet)
		{
			//post text key
			string textToSave = textToSetTestTokenTo + Random.value;
			var saveOp = new SaveTextKey
			(
				language : englishLanguage.id,
				text     : textToSave,
				text_id  : keyToSaveAndThenGet
			);
			justCall(saveOp);
			var testForExistance = new GetTextKey
			(
				languageID : englishLanguage.id,
				text_id    : keyToSaveAndThenGet
			);
			string stringFromServer = testForExistance.Parse(justCall(testForExistance));

			Assert.IsFalse(string.IsNullOrEmpty(stringFromServer));
			Assert.AreEqual(textToSave, stringFromServer);

			//save it back to what we expect it to be
			saveOp = new SaveTextKey
			(
				language: englishLanguage.id,
				text: textToSetTestTokenTo,
				text_id: keyToSaveAndThenGet
			);
			justCall(saveOp);

			stringFromServer = testForExistance.Parse(justCall(testForExistance));
			Assert.AreEqual(textToSetTestTokenTo, stringFromServer);
		}

		public const string TRANSLATION_KEY = "UNITY_TEST_TRANSLATION_KEY";

		public string justCall(ITransfluentParameters call)
		{
			if (call.getParameters.ContainsKey("token"))
				call.getParameters.Remove("token");

			call.getParameters.Add("token", accessToken);
			var requester = new SyncronousEditorWebRequest();
			var result = requester.request(call);
			if (result.httpErrorCode > 0)
			{	
				throw new WebServiceParameters.HttpErrorCode(result.httpErrorCode);
			}
			return result.text;
		}


		[Test]
		public void testAlreadyInsertedException()
		{
			string textToSave = textToSetTestTokenTo + Random.value;
			var saveOp = new SaveTextKey
			(
				language  : englishLanguage.id,
				text      : textToSave,
				text_id   : TRANSLATION_KEY
			);
			justCall(saveOp);
			justCall(saveOp); //error 500, this looks like a temporary server error...
		}

		[Test]
		public void getBackwardsLanguage()
		{
			var englishKeyGetter = new GetTextKey
			(
				languageID : englishLanguage.id,
				text_id    : TRANSLATION_KEY
			);

			string stringToReverse = englishKeyGetter.Parse(justCall(englishKeyGetter));
			Assert.AreEqual(stringToReverse, textToSetTestTokenTo);
			var getText = new GetTextKey
			(
				languageID : backwardsLanguage.id,
				text_id    : TRANSLATION_KEY
			);
			string reversedString = getText.Parse(justCall(getText));
			Assert.AreNotEqual(stringToReverse, reversedString);
			var reverser = new WordReverser();
			string manuallyReversedString = reverser.reverseString(stringToReverse);
			Debug.Log(string.Format(" manully reversed:{0} reversed from call{1}", manuallyReversedString, reversedString));
			Assert.AreEqual(manuallyReversedString, reversedString);
		}

		[Test]
		public void testListAllTranslations()
		{
			var getAllKeys = new GetAllExistingTranslationKeys
			(
				language : englishLanguage.id
			);

			var translations = getAllKeys.Parse(justCall(getAllKeys));
			Assert.IsNotNull(translations);
			Assert.Greater(translations.Count, 0);

			getAllKeys = new GetAllExistingTranslationKeys
			(
				language: backwardsLanguage.id
			);
			var backwardsTranslations = getAllKeys.Parse(justCall(getAllKeys));
			Assert.IsNotNull(backwardsTranslations);
			Assert.Greater(backwardsTranslations.Count, 0);

			Assert.IsTrue(backwardsTranslations.ContainsKey(TRANSLATION_KEY));
		}

		[Test]
		public void testListAllOrders()
		{
			var getAllOrders = new GetAllOrders
			(
			);
			var orders = getAllOrders.Parse(justCall(getAllOrders));
			Assert.IsNotNull(orders);
			//Assert.Greater(orders.Count, 0); //TODO: test making an actual order
		}

		[Test]
		public void testStatusOfAlreadyInsertedKey()
		{
			var englishTranslationOfEnglishKey = new TextStatus
			(
				text_id     : TRANSLATION_KEY,
				language_id : englishLanguage.id
			);
			var rawStatus = englishTranslationOfEnglishKey.Parse(justCall(englishTranslationOfEnglishKey));

			bool status = rawStatus.is_translated;

			Assert.True(status);

			var backwardsTranslationOfExistingKey = new TextStatus
			(
				text_id     : TRANSLATION_KEY,
				language_id : backwardsLanguage.id
			);

			status = backwardsTranslationOfExistingKey.Parse(justCall(backwardsTranslationOfExistingKey)).is_translated;
			Assert.IsTrue(status);
		}

		[Test]
		public void testStatusOfNonExistantKey()
		{
			var englishTranslationOfEnglishKey = new TextStatus
			(
				text_id     : TRANSLATION_KEY + " THIS IS INVALID" + Random.value,
				language_id : englishLanguage.id
			);

			bool status = englishTranslationOfEnglishKey.Parse(justCall(englishTranslationOfEnglishKey)).is_translated;

			Assert.False(status);

			var backwardsTranslationOfExistingKey = new TextStatus
			(
				text_id     : TRANSLATION_KEY + " THIS IS INVALID" + Random.value,
				language_id : backwardsLanguage.id
			);
			status = backwardsTranslationOfExistingKey.Parse(justCall(backwardsTranslationOfExistingKey)).is_translated;
			Assert.False(status);
		}

		[Test]
		public void testStatusOfNotOrderedTranslations()
		{
			int nonExistantTranslationKey = -1;
			foreach (TransfluentLanguage lang in languageCache.languages)
			{
				if (lang.id != englishLanguage.id && lang.id != backwardsLanguage.id)
				{
					nonExistantTranslationKey = lang.id;
					break;
				}
			}

			var englishTranslationOfEnglishKey = new TextStatus
			(
				text_id     : TRANSLATION_KEY,
				language_id : nonExistantTranslationKey
			);
			var result = englishTranslationOfEnglishKey.Parse(justCall(englishTranslationOfEnglishKey));

			Assert.False(result.is_translated);
		}

		[Test]
		public void testTranslation()
		{
			var translateRequest = new OrderTranslation
			(
				source_language  : englishLanguage.id,
				target_languages : new int[] {backwardsLanguage.id},
				texts            : new string[] {TRANSLATION_KEY}
			);
			var translationResult = translateRequest.Parse(justCall(translateRequest));
			var requester = new SyncronousEditorWebRequest();
			var result = requester.request(translateRequest);
			Debug.Log("Full result from test translation:" + JsonWriter.Serialize(result.text));
			Assert.IsTrue(translationResult.word_count > 0);
		}
	}
}