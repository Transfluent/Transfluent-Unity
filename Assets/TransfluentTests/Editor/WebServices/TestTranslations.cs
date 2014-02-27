﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pathfinding.Serialization.JsonFx;
using transfluent;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Editor.Tests
{
	[TestFixture]
	internal class TestTranslations
	{
		public string accessToken;
		public TransfluentLanguage2 englishLanguage;
		public TransfluentLanguage2 backwardsLanguage;
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
			getLanguages();
			SaveRetrieveKey(TRANSLATION_KEY);
		}

		public void getLanguages()
		{
			var language = new RequestAllLanguages {service = new SyncronousEditorWebRequest()};
			language.Execute();

			LanguageList list = language.languagesRetrieved;
			Assert.NotNull(list);
			Assert.IsTrue(list.languages.Count > 0);

			englishLanguage = list.getLangaugeByCode("en-us");
			backwardsLanguage = list.getLangaugeByCode(TransfluentLanguage2.BACKWARDS_LANGUAGE_NAME);
			languageCache = list;
			Assert.AreNotEqual(englishLanguage.code, 0);
			Assert.NotNull(backwardsLanguage);
		}

		public void SaveRetrieveKey(string keyToSaveAndThenGet)
		{
			//post text key
			string textToSave = textToSetTestTokenTo + Random.value;
			var saveOp = new SaveTextKey
			{
				authToken = accessToken,
				language = englishLanguage.id,
				text = textToSave,
				text_id = keyToSaveAndThenGet,
				service = new SyncronousEditorWebRequest()
			};
			saveOp.Execute();

			var testForExistance = new GetTextKey
			{
				authToken = accessToken,
				languageID = englishLanguage.id,
				text_id = keyToSaveAndThenGet,
				service = new SyncronousEditorWebRequest()
			};
			testForExistance.Execute();
			Assert.IsFalse(string.IsNullOrEmpty(testForExistance.resultOfCall));
			Assert.AreEqual(textToSave, testForExistance.resultOfCall);

			//save it back to what we expect it to be
			saveOp.text = textToSetTestTokenTo;
			saveOp.Execute();

			testForExistance.Execute(); //get it again
			Assert.AreEqual(textToSetTestTokenTo, testForExistance.resultOfCall);
		}

		public const string TRANSLATION_KEY = "UNITY_TEST_TRANSLATION_KEY";

		private string reverseString(string str)
		{
			string[] words = str.Split(new[] {" "}, StringSplitOptions.None);
			var sb = new StringBuilder();
			for (int i = words.Length - 1; i >= 0; i--)
			{
				string word = words[i];

				char[] charArray = word.ToCharArray();
				IEnumerable<char> walker = charArray.Reverse();
				foreach (char t in walker)
				{
					sb.Append(t);
				}
				if (i != 0)
					sb.Append(" ");
			}

			return sb.ToString();
		}

		[Test]
		public void getBackwardsLanguage()
		{
			var englishKeyGetter = new GetTextKey
			{
				authToken = accessToken,
				languageID = englishLanguage.id,
				text_id = TRANSLATION_KEY,
				service = new SyncronousEditorWebRequest()
			};
			englishKeyGetter.Execute();
			string stringToReverse = englishKeyGetter.resultOfCall;
			Assert.AreEqual(stringToReverse, textToSetTestTokenTo);
			var getText = new GetTextKey
			{
				authToken = accessToken,
				languageID = backwardsLanguage.id,
				text_id = TRANSLATION_KEY,
				service = new SyncronousEditorWebRequest()
			};
			getText.Execute();
			string reversedString = getText.resultOfCall;
			Assert.AreNotEqual(stringToReverse, reversedString);

			string manuallyReversedString = reverseString(stringToReverse);
			Debug.Log(string.Format(" manully reversed:{0} reversed from call{1}", manuallyReversedString, reversedString));
			Assert.AreEqual(manuallyReversedString, reversedString);
		}
		

		//TODO: verify this fails because the backwards language is not an "Ordered" translation
		[Test]
		public void testListAllTranslations()
		{
			var getAllKeys = new GetAllExistingTranslationKeys
			{
				authToken = accessToken,
				service = new SyncronousEditorWebRequest()
			};
			getAllKeys.Execute();
			List<TransfluentTranslation> translations = getAllKeys.translations;
			Assert.IsNotNull(getAllKeys.translations);
			Assert.Greater(getAllKeys.translations.Count, 0);  /// I don't know why this is 0
			bool hastargetKey = false;
			translations.ForEach((TransfluentTranslation trans) => { if (trans.key == TRANSLATION_KEY) hastargetKey = true; });
			Assert.IsTrue(hastargetKey);
		}

		[Test]
		public void testStatusOfAlreadyInsertedKey()
		{
			var englishTranslationOfEnglishKey = new TextStatus
			{
				authToken = accessToken,
				text_id = TRANSLATION_KEY,
				language_id = englishLanguage.id,
				service = new SyncronousEditorWebRequest()
			};
			englishTranslationOfEnglishKey.Execute();

			Assert.True(englishTranslationOfEnglishKey.wasTranslated);

			var backwardsTranslationOfExistingKey = new TextStatus
			{
				authToken = accessToken,
				text_id = TRANSLATION_KEY,
				language_id = backwardsLanguage.id,
				service = new SyncronousEditorWebRequest()
			};
			backwardsTranslationOfExistingKey.Execute();
			Assert.IsTrue(backwardsTranslationOfExistingKey.wasTranslated);
		}

		[Test]
		public void testStatusOfNonExistantKey()
		{
			var englishTranslationOfEnglishKey = new TextStatus
			{
				authToken = accessToken,
				text_id = TRANSLATION_KEY + " THIS IS INVALID" + Random.value,
				language_id = englishLanguage.id,
				service = new SyncronousEditorWebRequest()
			};
			englishTranslationOfEnglishKey.Execute();

			Assert.False(englishTranslationOfEnglishKey.wasTranslated);

			var backwardsTranslationOfExistingKey = new TextStatus
			{
				authToken = accessToken,
				text_id = TRANSLATION_KEY + " THIS IS INVALID" + Random.value,
				language_id = backwardsLanguage.id,
				service = new SyncronousEditorWebRequest()
			};
			backwardsTranslationOfExistingKey.Execute();
			Assert.False(backwardsTranslationOfExistingKey.wasTranslated);
		}

		[Test]
		public void testStatusOfNotOrderedTranslations()
		{
			int nonExistantTranslationKey = -1;
			foreach (TransfluentLanguage2 lang in languageCache.languages)
			{
				if (lang.id != englishLanguage.id && lang.id != backwardsLanguage.id)
				{
					nonExistantTranslationKey = lang.id;
					break;
				}
			}

			var englishTranslationOfEnglishKey = new TextStatus
			{
				authToken = accessToken,
				text_id = TRANSLATION_KEY,
				language_id = nonExistantTranslationKey,
				service = new SyncronousEditorWebRequest()
			};
			englishTranslationOfEnglishKey.Execute();

			Assert.False(englishTranslationOfEnglishKey.wasTranslated);
		}

		[Test]
		public void testTranslation()
		{
			var translateRequest = new OrderTranslation
			{
				source_language = englishLanguage.id,
				target_languages = new[] {backwardsLanguage.id},
				texts = new[] {TRANSLATION_KEY},
				authToken = accessToken,
				service = new SyncronousEditorWebRequest()
			};
			translateRequest.Execute();

			Debug.Log("Full result from test translation:" + JsonWriter.Serialize(translateRequest.fullResult));
			Assert.IsTrue(translateRequest.fullResult.word_count > 0);
		}
	}
}