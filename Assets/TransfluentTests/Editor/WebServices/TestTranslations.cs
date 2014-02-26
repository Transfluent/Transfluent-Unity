using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pathfinding.Serialization.JsonFx;
using transfluent;

namespace Assets.Editor.Tests
{
	[TestFixture]
	class TestTranslations
	{
		public string accessToken;
		//public RequestAllLanguages languageContainer;
		public TransfluentLanguage2 englishLanguage;
		public TransfluentLanguage2 backwardsLanguage;
		public const string textToSetTestTokenTo = "this is test text";

		[TestFixtureSetUp]
		public void getTestCredentials()
		{
			OneTimeSetup();
		}

		//[Test]
		public void OneTimeSetup()
		{
			var credentials = new TestLoginFlow.TestCredentialProvider();
			Assert.False(string.IsNullOrEmpty(credentials.username));
			Assert.False(string.IsNullOrEmpty(credentials.password));
			var login = new Login
			{
				username = credentials.username,
				password = credentials.password,
			};
			login.Execute();

			accessToken = login.token;
			if(string.IsNullOrEmpty(accessToken))
			{
				throw new Exception("was not able to log in!");
			}
			getLanguages();
			SaveRetrieveKey(TRANSLATION_KEY);
		}

		public void getLanguages()
		{
			var language = new RequestAllLanguages();
			language.Execute();

			LanguageList list =  language.languagesRetrieved;
			Assert.NotNull(list);
			Assert.IsTrue(list.languages.Count > 0);

			englishLanguage = list.getLangaugeByCode("en-us");
			backwardsLanguage = list.getLangaugeByCode(TransfluentLanguage2.BACKWARDS_LANGUAGE_NAME);
			Assert.AreNotEqual(englishLanguage.code, 0);
			Assert.NotNull(backwardsLanguage);
		}

		public void SaveRetrieveKey(string keyToSaveAndThenGet)
		{
			//post text key
			string textToSave = textToSetTestTokenTo + UnityEngine.Random.value;
			SaveTextKey saveOp = new SaveTextKey()
			{
				authToken = accessToken,
				language = englishLanguage.id,
				text = textToSave,
				text_id = keyToSaveAndThenGet
			};
			saveOp.Execute();

			GetTextKey testForExistance = new GetTextKey()
			{
				authToken = accessToken,
				language = englishLanguage.id,
				text_id = keyToSaveAndThenGet
			};
			testForExistance.Execute();
			Assert.IsFalse(string.IsNullOrEmpty(testForExistance.keyValue));
			Assert.AreEqual(textToSave, testForExistance.keyValue);

			//save it back to what we expect it to be
			saveOp.text = textToSetTestTokenTo;
			saveOp.Execute();

			testForExistance.Execute();  //get it again
			Assert.AreEqual(textToSetTestTokenTo, testForExistance.keyValue);
		}
		
		public const string TRANSLATION_KEY = "UNITY_TEST_TRANSLATION_KEY";
		[Test]
		public void testTranslation()
		{
			var translateRequest = new OrderTranslation()
			{
				source_language = englishLanguage.id,
				target_languages = new []{backwardsLanguage.id},
				texts = new []{TRANSLATION_KEY},
				authToken = accessToken,
			};
			translateRequest.Execute();

			UnityEngine.Debug.Log("Full result from test translation:" + JsonWriter.Serialize(translateRequest.fullResult));
			Assert.IsTrue(translateRequest.fullResult.word_count > 0);
		}


		[Test]
		public void getBackwardsLanguage()
		{
			var englishKeyGetter = new GetTextKey()
			{
				authToken = accessToken,
				language = englishLanguage.id,
				text_id = TRANSLATION_KEY,
			};
			englishKeyGetter.Execute();
			string stringToReverse = englishKeyGetter.keyValue;
			Assert.AreEqual(stringToReverse, textToSetTestTokenTo);
			var getText = new GetTextKey()
			{
				authToken = accessToken,
				language = backwardsLanguage.id,
				text_id = TRANSLATION_KEY,
			};
			getText.Execute();
			string reversedString = getText.keyValue;
			Assert.AreNotEqual(stringToReverse, reversedString);

			string manuallyReversedString = reverseString(stringToReverse);
			UnityEngine.Debug.Log(string.Format(" manully reversed:{0} reversed from call{1}",manuallyReversedString,reversedString));
			Assert.AreEqual(manuallyReversedString,reversedString);
		}

		string reverseString(string str)
		{
			string[] words = str.Split(new string[]{" "},StringSplitOptions.None);
			StringBuilder sb = new StringBuilder();
			for(int i=words.Length-1;i>=0;i--)
			{
				string word = words[i];
				
				char[] charArray = word.ToCharArray();
				IEnumerable<char> walker = charArray.Reverse();
				foreach(char t in walker)
				{
					sb.Append(t);
				}
				if(i != 0)
					sb.Append(" ");
			}

			return sb.ToString();
		}
	}
}
