using System;
using System.Collections.Generic;
using System.Text;
using transfluent;
using UnityEditor;
using UnityEngine;

//editor time utility to get ngui serialization into and out of ngui's format
public class ImportExportNGUILocalization
{
	private static readonly List<string> keysThatMustExistFirst = new List<string> {"KEYS", "Language"};

	//write tests.  lots of tests.  This has to work *perfectly* for the keying to map well.  Other people use different language identifiers than transfluent


	//Handle the mapping of language code to a language name in it's own language to a language
	//Not meant to be a complete list, but a start of one
	//NOTE: we lose some information by translating to a common name, as you can no longer pick apart regional variants of a language that uses the same common name
	//NOTE: not handling alterinative names such as pinyin or other alternative forms of the language name
	//doing that complicates (at best) the flow for making sure that we can import/export information with no data loss
	private static readonly Dictionary<string, string> languageCodeToCommonName = new Dictionary<string, string>
	{
		{"en-us", "English"},
		{"fr-fr", "Français"},
		{"de-de","Deutsch"},
		{"zh-cn","中文"}, //简体中文 ? 
		//{"zh-hk","中文"}, //繁體中文? 古文? 文言?
		{"es-es","Español"},
		{"ja-jp","日本語"},
		{"ko-kr","조선말"},
		{"tl-ph","Tagalog"},  //ᜊᜊᜌᜒ?
		{"pt-br","Português"}, //
		{"fi-fi","Suomi"},
		{"it-it","Italiano"},
		{"nl-nl","Nederlands"},
		{"sv-se","Svenska"},
		{"ru-ru","Русский"},
		{"hi-in","हिन्दी"},
		{"ar-sa","اللغة العربية"},
		{"ms-my","بهاس ملايو"},
		{"he-il","Hebrew"},
		{"da-dk","Dansk"},
		{"vi-vn","Tiếng Việt"},
		{"pl-pl","Polski"},
		{"tr-tr","Türkçe"},
		{"no-no","Norsk"},
		{"uk-ua","Українська"},
		{"hu-hu","Hungarian"},
		{"el-gr","Ελληνικά"},
		{"xx-xx","Pseudo Language"},
	};
	 
	
	protected static string takeLanguageNameAndTurnItIntoAKnownLanguageCode(string languageNameInItsOwnLanguage)
	{
		foreach (var kvp in languageCodeToCommonName)
		{
			if (kvp.Value.ToLower() == languageNameInItsOwnLanguage.ToLower())
			{
				return kvp.Key;
			}
		}
		return languageNameInItsOwnLanguage; //we don't know
	}

	protected static string takeLanguageCodeAndTurnItIntoNativeName(string languageCode)
	{
		if (languageCodeToCommonName.ContainsKey(languageCode))
		{
			return languageCodeToCommonName[languageCode];
		}
		return languageCode; //we have no idea
	}

	//NOTE: this belongs elsewhere
	public string getNGUISetLocalizationLanguageName()
	{
		return EditorPrefs.GetString("Language");
	}

	public void setNGUILocalizationLanguage(string languageName)
	{
		EditorPrefs.SetString("Language", languageName);
	}

	public class NGUICSVExporter
	{
		private string csvString;

		//NOTE: it appears as if groupid maps roughly to KEY.  But most of the time KEY is the language in those files... so I'm not sure I should take that for granted
		public NGUICSVExporter(List<TransfluentLanguage> languagesToExportTo, string groupid = "")
		{
			var allTranslationsIndexedByLanguage = new Dictionary<string, Dictionary<string, string>>();
			foreach (TransfluentLanguage lang in languagesToExportTo)
			{
				GameTranslationSet destLangDB = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(lang.code);
				if (destLangDB == null || destLangDB.allTranslations == null)
				{
					Debug.LogWarning("could not find any information for language:" + lang);
					continue;
				}
				Dictionary<string, string> translations = destLangDB.getKeyValuePairs(groupid);
				string languageNameInNativeLanguage = takeLanguageCodeAndTurnItIntoNativeName(lang.code);
				allTranslationsIndexedByLanguage.Add(languageNameInNativeLanguage, translations);
			}
			Init(allTranslationsIndexedByLanguage);
		}

		public NGUICSVExporter(Dictionary<string, Dictionary<string, string>> allTranslationsIndexedByLanguage)
		{
			Init(allTranslationsIndexedByLanguage);
		}

		private void Init(Dictionary<string, Dictionary<string, string>> allTranslationsIndexedByLanguage)
		{
			var keyList = new List<string> {"KEYS"};
			var languageList = new List<string> {"Language"};
			foreach (var kvp in allTranslationsIndexedByLanguage)
			{
				string nativeLanguageName = kvp.Key;
				keyList.Add(nativeLanguageName);
					//I have to think that this is meant as groupid, but I'm not sure how people end up using this
				languageList.Add(nativeLanguageName);
			}

			var _keysMappedToListOfLangaugesIndexedByLanguageIndex = new Dictionary<string, string[]>();

			foreach (var langToDictionary in allTranslationsIndexedByLanguage)
			{
				string nativeName = langToDictionary.Key;
				Dictionary<string, string> keyValuesInLanguage = langToDictionary.Value;
				int indexToAddAt = keyList.IndexOf(nativeName) - 1;

				foreach (var kvp in keyValuesInLanguage)
				{
					if (keysThatMustExistFirst.Contains(kvp.Key)) //skip KEYS and Language
						continue;
					if (!_keysMappedToListOfLangaugesIndexedByLanguageIndex.ContainsKey(kvp.Key))
						_keysMappedToListOfLangaugesIndexedByLanguageIndex[kvp.Key] = new string[languageList.Count - 1];
					_keysMappedToListOfLangaugesIndexedByLanguageIndex[kvp.Key][indexToAddAt] = kvp.Value;
				}
			}

			var allLinesSB = new StringBuilder();
			allLinesSB.AppendLine(string.Join(",", keyList.ToArray()));
			allLinesSB.AppendLine(string.Join(",", languageList.ToArray()));
			foreach (var keyToItems in _keysMappedToListOfLangaugesIndexedByLanguageIndex)
			{
				var tmpList = new List<string>();
				tmpList.Add(keyToItems.Key);
				tmpList.AddRange(keyToItems.Value);
				allLinesSB.AppendLine(string.Join(",", tmpList.ToArray()));
			}
			csvString = allLinesSB.ToString();
		}

		public string getCSV()
		{
			return csvString;
		}
	}

	public class NGUILocalizationCSVImporter
	{
		private readonly Dictionary<string, List<string>> keyNameToValueListIndexedByLanguage =
			new Dictionary<string, List<string>>();

		public NGUILocalizationCSVImporter(string nguiLocalizationCSVText)
		{
			string[] individualLines = nguiLocalizationCSVText.Split(new[] {'\r', '\n'},
				StringSplitOptions.RemoveEmptyEntries);
			if (individualLines.Length < keysThatMustExistFirst.Count)
			{
				Debug.LogError("not enough lines to be a valid csv file, must at least have this many entries:" +
				               keysThatMustExistFirst.Count);
				return;
			}
			for (int j = 0; j < keysThatMustExistFirst.Count; j++)
			{
				if (!individualLines[j].StartsWith(keysThatMustExistFirst[j]))
				{
					Debug.LogError("invalid csv file, expected to have the key start the csv file:" + keysThatMustExistFirst[j] +
					               " at position:" + j);
					Debug.Log("vs individual line number:" + j + " with value:" + individualLines[j]);
					return;
				}
			}

			for (int i = 0; i < individualLines.Length; i++)
			{
				string line = individualLines[i];
				string[] csvStrings = line.Split(new[] {','});
				if (csvStrings.Length < 1)
				{
					Debug.LogError("invalid csv line, no keys found on it:" + line);
					continue;
				}
				string key = csvStrings[0];
				Debug.Log("Processing key:" + key);
				if (string.IsNullOrEmpty(key))
				{
					Debug.LogError("invalid csv line, empty key for csv line:" + line);
					continue;
				}
				if (keyNameToValueListIndexedByLanguage.ContainsKey(key))
				{
					Debug.LogError("invalid csv line, duplicate key in csv:" + key);
					continue;
				}
				var values = new List<string>(csvStrings);
				values.RemoveAt(0); //remove the key

				keyNameToValueListIndexedByLanguage.Add(key, values);
			}
		}

		public Dictionary<string, Dictionary<string, string>> getMapOfLanguagesToKeyValueTranslations()
		{
			var langaugeNameToKeyValuePairMap = new Dictionary<string, Dictionary<string, string>>();

			List<string> languageNamesFromFile = keyNameToValueListIndexedByLanguage["Language"];
			foreach (string languageName in languageNamesFromFile)
			{
				langaugeNameToKeyValuePairMap.Add(languageName, new Dictionary<string, string>());
			}
			//we are re-ordering the csv text into a format that can be consumed per-language instead of per-key
			//the csv comes in with KEYNAME,valueIndexedBylanguage1,valueIndexedBylanguage2,etc and we want to use it as store[languageName][key] instead
			foreach (var kvp in keyNameToValueListIndexedByLanguage)
			{
				if (kvp.Value.Count > languageNamesFromFile.Count) //NOTE: maybe let the user know if they are not *exactly* equal?
				{
					Debug.LogWarning("CSV entry has more lines than there are languages, dropping the data from the extra lines");
				}
				if (kvp.Value.Count > languageNamesFromFile.Count)
				{
					Debug.LogWarning(
						"CSV entry has fewer entries than there are langauges, you will not have translations for entries that are unmapped");
				}
				string key = kvp.Key;
				for (int i = 0; i < kvp.Value.Count; i++)
				{
					if (i >= languageNamesFromFile.Count)
						continue; //dropping extra commas and the data in them, as they map to no known language
					string toInsert = kvp.Value[i];
					string languageName = languageNamesFromFile[i]; //all of the items are implicitly mapped from the language name
					langaugeNameToKeyValuePairMap[languageName].Add(key, toInsert);
				}
			}
			//now to figure out how to properly decode language names....
			//ranch and french -- franch
			return langaugeNameToKeyValuePairMap;
		}
	}
}