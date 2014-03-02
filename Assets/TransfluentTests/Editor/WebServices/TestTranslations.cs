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
			var result = request.request(login);

			var response = result.Parse<AuthenticationResponse>();
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
			var language = new RequestAllLanguages {service = new SyncronousEditorWebRequest()};
			var requester = new SyncronousEditorWebRequest();
			WebServiceReturnStatus status = requester.request(language);

			Assert.True(status.wasSuccessful());
			var retrieved = status.Parse<List<Dictionary<string, TransfluentLanguage>>>();
			Assert.NotNull(retrieved);

			LanguageList list = language.GetLanguageListFromRawReturn(retrieved);
			Assert.NotNull(list);
			Assert.IsTrue(list.languages.Count > 0);

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
			justCall<bool>(saveOp);
			var testForExistance = new GetTextKey
			(
				languageID : englishLanguage.id,
				text_id    : keyToSaveAndThenGet
			);
			string stringFromServer = justCall<string>(testForExistance);
			Assert.IsFalse(string.IsNullOrEmpty(stringFromServer));
			Assert.AreEqual(textToSave, stringFromServer);

			//save it back to what we expect it to be
			saveOp = new SaveTextKey
			(
				language: englishLanguage.id,
				text: textToSetTestTokenTo,
				text_id: keyToSaveAndThenGet
			);
			justCall<bool>(saveOp);

			stringFromServer = justCall<string>(testForExistance);
			Assert.AreEqual(textToSetTestTokenTo, stringFromServer);
		}

		public const string TRANSLATION_KEY = "UNITY_TEST_TRANSLATION_KEY";

		public T justCall<T>(ITransfluentCall call)
		{
			if (call.getParameters().ContainsKey("token"))
				call.getParameters().Remove("token");

			call.getParameters().Add("token", accessToken);
			var requester = new SyncronousEditorWebRequest();
			var result = requester.request(call);
			return result.Parse<T>();
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
			justCall<bool>(saveOp);
			justCall<bool>(saveOp); //error 500, this looks like a temporary server error...
		}

		[Test]
		public void getBackwardsLanguage()
		{
			var englishKeyGetter = new GetTextKey
			(
				languageID : englishLanguage.id,
				text_id    : TRANSLATION_KEY
			);
			
			string stringToReverse = justCall<string>(englishKeyGetter);
			Assert.AreEqual(stringToReverse, textToSetTestTokenTo);
			var getText = new GetTextKey
			(
				languageID : backwardsLanguage.id,
				text_id    : TRANSLATION_KEY
			);
			string reversedString = justCall<string>(getText);
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

			var translations = justCall<List<TransfluentTranslation>>(getAllKeys);
			Assert.IsNotNull(translations);
			Assert.Greater(translations.Count, 0);

			getAllKeys = new GetAllExistingTranslationKeys
			(
				language: backwardsLanguage.id
			);
			var backwardsTranslations = justCall<List<TransfluentTranslation>>(getAllKeys);
			Assert.IsNotNull(backwardsTranslations);
			Assert.Greater(backwardsTranslations.Count, 0);
			translations.AddRange(backwardsTranslations);

			Assert.Greater(backwardsTranslations.Count, 0); 
			bool hastargetKey = false;
			translations.ForEach((TransfluentTranslation trans) => { if (trans.text_id == TRANSLATION_KEY) hastargetKey = true; });
			Assert.IsTrue(hastargetKey);
		}

		[Test]
		public void testListAllOrders()
		{
			var getAllKeys = new GetAllOrders
			(
			);
			var orders = justCall<List<TransfluentOrder>>(getAllKeys);
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
			bool status = justCall<TextStatusResult>(englishTranslationOfEnglishKey).is_translated;

			Assert.True(status);

			var backwardsTranslationOfExistingKey = new TextStatus
			(
				text_id     : TRANSLATION_KEY,
				language_id : backwardsLanguage.id
			);
			status = justCall<TextStatusResult>(backwardsTranslationOfExistingKey).is_translated;
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
			bool status = justCall<TextStatusResult>(englishTranslationOfEnglishKey).is_translated;

			Assert.False(status);

			var backwardsTranslationOfExistingKey = new TextStatus
			(
				text_id     : TRANSLATION_KEY + " THIS IS INVALID" + Random.value,
				language_id : backwardsLanguage.id
			);
			status = justCall<TextStatusResult>(englishTranslationOfEnglishKey).is_translated;
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
			var result = justCall<TextStatusResult>(englishTranslationOfEnglishKey);

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
			var translationResult = justCall<OrderTranslation.TextsTranslateResult>(translateRequest);
			var requester = new SyncronousEditorWebRequest();
			var result = requester.request(translateRequest);
			Debug.Log("Full result from test translation:" + JsonWriter.Serialize(result.text));
			Assert.IsTrue(translationResult.word_count > 0);
		}
	}
}