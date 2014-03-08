using System;
using System.Collections.Generic;
using NUnit.Framework;
using Pathfinding.Serialization.JsonFx;
using transfluent.editor;
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
			var login = new Login(credentials.username, credentials.password);
			var request = new DebugSyncronousEditorWebRequest();

			AuthenticationResponse response = login.Parse(request.request(login).text);
			accessToken = response.token;
			if (string.IsNullOrEmpty(accessToken))
			{
				throw new Exception("was not able to log in!");
			}
			getLanguages();
			SaveRetrieveKey(TRANSLATION_KEY,englishLanguage);
		}

		public void getLanguages()
		{
			var language = new RequestAllLanguages();
			var requester = new SyncronousEditorWebRequest();

			LanguageList list = language.Parse(requester.request(language).text);
			Assert.NotNull(list);

			englishLanguage = list.getLangaugeByCode("en-us");
			backwardsLanguage = list.getLangaugeByCode(TransfluentLanguage.BACKWARDS_LANGUAGE_NAME);
			languageCache = list;
			Assert.AreNotEqual(englishLanguage.code, 0);
			Assert.NotNull(backwardsLanguage);
		}

		public void SaveRetrieveKey(string keyToSaveAndThenGet,TransfluentLanguage language)
		{
			//post text key
			string textToSave = textToSetTestTokenTo + Random.value;
			var saveOp = new SaveTextKey
				(
				language: language.id,
				text: textToSave,
				text_id: keyToSaveAndThenGet
				);
			string callText = justCall(saveOp);
			Assert.IsTrue(saveOp.Parse(callText));
			var testForExistance = new GetTextKey
				(
				languageID: language.id,
				text_id: keyToSaveAndThenGet
				);
			string stringFromServer = testForExistance.Parse(justCall(testForExistance));

			Assert.IsFalse(string.IsNullOrEmpty(stringFromServer));
			Assert.AreEqual(textToSave, stringFromServer);

			//save it back to what we expect it to be
			saveOp = new SaveTextKey
				(
				language: language.id,
				text: textToSetTestTokenTo,
				text_id: keyToSaveAndThenGet
				);
			callText = justCall(saveOp);
			Assert.IsTrue(saveOp.Parse(callText));

			stringFromServer = testForExistance.Parse(justCall(testForExistance));
			Assert.AreEqual(textToSetTestTokenTo, stringFromServer);
		}

		public const string TRANSLATION_KEY = "UNITY_TEST_TRANSLATION_KEY";

		public string justCall(WebServiceParameters call)
		{
			if (call.getParameters.ContainsKey("token"))
				call.getParameters.Remove("token");

			call.getParameters.Add("token", accessToken);
			var requester = new SyncronousEditorWebRequest();
			WebServiceReturnStatus result = requester.request(call);
			if (result.httpErrorCode > 0)
			{
				throw new HttpErrorCode(result.httpErrorCode);
			}
			return result.text;
		}

		/*
		 * This throws a 500, but I'm not sure if this is acceptable or not, so I'm leaving this test as failing
		 */

		[Test]
		public void getBackwardsLanguage()
		{
			var englishKeyGetter = new GetTextKey
				(
				languageID: englishLanguage.id,
				text_id: TRANSLATION_KEY
				);

			string stringToReverse = englishKeyGetter.Parse(justCall(englishKeyGetter));
			Assert.AreEqual(stringToReverse, textToSetTestTokenTo);
			var getText = new GetTextKey
				(
				languageID: backwardsLanguage.id,
				text_id: TRANSLATION_KEY
				);
			string reversedString = getText.Parse(justCall(getText));
			Assert.AreNotEqual(stringToReverse, reversedString);
			var reverser = new WordReverser();
			string manuallyReversedString = reverser.reverseString(stringToReverse);
			Debug.Log(string.Format(" manully reversed:{0} reversed from call{1}", manuallyReversedString, reversedString));
			Assert.AreEqual(manuallyReversedString, reversedString);
		}

		[Test]
		public void THIS_FAILS_testAlreadyInsertedException()
		{
			string textToSave = textToSetTestTokenTo + Random.value;
			var saveOp = new SaveTextKey
				(
				language: englishLanguage.id,
				text: textToSave,
				text_id: TRANSLATION_KEY
				);
			justCall(saveOp);
			justCall(saveOp); //error 500, this looks like a temporary server error...
		}

		[Test]
		public void testListAllOrders()
		{
			var getAllOrders = new GetAllOrders
				(
				);
			List<TransfluentOrder> orders = getAllOrders.Parse(justCall(getAllOrders));
			Assert.IsNotNull(orders);
			//Assert.Greater(orders.Count, 0); //TODO: test making an actual order
		}

		[Test]
		public void testListAllTranslations()
		{
			var getAllKeys = new GetAllExistingTranslationKeys
				(englishLanguage.id
				);

			Dictionary<string, TransfluentTranslation> translations = getAllKeys.Parse(justCall(getAllKeys));
			Assert.IsNotNull(translations);
			Assert.Greater(translations.Count, 0);

			getAllKeys = new GetAllExistingTranslationKeys
				(backwardsLanguage.id
				);
			Dictionary<string, TransfluentTranslation> backwardsTranslations = getAllKeys.Parse(justCall(getAllKeys));
			Assert.IsNotNull(backwardsTranslations);
			Assert.Greater(backwardsTranslations.Count, 0);

			Assert.IsTrue(backwardsTranslations.ContainsKey(TRANSLATION_KEY));
		}

		[Test]
		public void testStatusOfAlreadyInsertedKey()
		{
			var englishTranslationOfEnglishKey = new TextStatus
				(
				text_id: TRANSLATION_KEY,
				language_id: englishLanguage.id
				);
			TextStatusResult rawStatus = englishTranslationOfEnglishKey.Parse(justCall(englishTranslationOfEnglishKey));

			bool status = rawStatus.is_translated;

			Assert.True(status);

			var backwardsTranslationOfExistingKey = new TextStatus
				(
				text_id: TRANSLATION_KEY,
				language_id: backwardsLanguage.id
				);

			status = backwardsTranslationOfExistingKey.Parse(justCall(backwardsTranslationOfExistingKey)).is_translated;
			Assert.IsTrue(status);
		}

		[Test]
		public void testStatusOfNonExistantKey()
		{
			var englishTranslationOfEnglishKey = new TextStatus
				(
				text_id: TRANSLATION_KEY + " THIS IS INVALID" + Random.value,
				language_id: englishLanguage.id
				);


			Assert.Catch<ApplicatonLevelException>(
				() => englishTranslationOfEnglishKey.Parse(justCall(englishTranslationOfEnglishKey)));

			var backwardsTranslationOfExistingKey = new TextStatus
				(
				text_id: TRANSLATION_KEY + " THIS IS INVALID" + Random.value,
				language_id: backwardsLanguage.id
				);
			Assert.Catch<ApplicatonLevelException>(
				() => backwardsTranslationOfExistingKey.Parse(justCall(backwardsTranslationOfExistingKey)));
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
				text_id: TRANSLATION_KEY,
				language_id: nonExistantTranslationKey
				);
			TextStatusResult result = englishTranslationOfEnglishKey.Parse(justCall(englishTranslationOfEnglishKey));

			Assert.False(result.is_translated);
		}

		[Test]
		public void testTranslation()
		{
			var translateRequest = new OrderTranslation
				(englishLanguage.id, new[] {backwardsLanguage.id}, new[] {TRANSLATION_KEY}
				);
			OrderTranslation.TextsTranslateResult translationResult = translateRequest.Parse(justCall(translateRequest));
			var requester = new SyncronousEditorWebRequest();
			WebServiceReturnStatus result = requester.request(translateRequest);
			Debug.Log("Full result from test translation:" + JsonWriter.Serialize(result.text));
			Assert.IsTrue(translationResult.word_count > 0);
		}

		//NOTE: keys are not immediately translated
		[Test]
		public void testGermanKeyBackwards()
		{
			var german = languageCache.getLangaugeByCode("de-de");
			Assert.NotNull(german);
			SaveRetrieveKey("TEST_KEY_2", german);
			var englishTranslationOfEnglishKey = new TextStatus
				(
				text_id: TRANSLATION_KEY,
				language_id: german.id
				);
			TextStatusResult rawStatus = englishTranslationOfEnglishKey.Parse(justCall(englishTranslationOfEnglishKey));

			bool status = rawStatus.is_translated;

			Assert.True(status);

			var backwardsTranslationOfExistingKey = new TextStatus
				(
				text_id: TRANSLATION_KEY,
				language_id: backwardsLanguage.id
				);

			status = backwardsTranslationOfExistingKey.Parse(justCall(backwardsTranslationOfExistingKey)).is_translated;
			Assert.IsTrue(status);
		}
	}
}