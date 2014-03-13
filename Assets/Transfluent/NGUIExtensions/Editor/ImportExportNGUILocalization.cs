using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using transfluent;
using UnityEditor;
using UnityEngine;
using System.Collections;

//editor time utility to get ngui serialization into and out of ngui's format
public class ImportExportNGUILocalization
{
	static List<string> keysThatMustExistFirst = new List<string>() { "KEYS", "Language" };

	//write tests.  lots of tests.  This has to work *perfectly* for the keying to map well.  Other people use different language identifiers than transfluent
	protected static string takeLanguageNameAndTurnItIntoAKnownLanguageCode(string languageNameInItsOwnLanguage)
	{
		var languageCode = "en-us";
		return languageCode;
	}

	protected static string takeLanguageCodeAndTurnItIntoNativeName(string languageCode)
	{
		string languageNameInItsOwnLanguage = "en-us";
		return languageNameInItsOwnLanguage;
	}

	//NOTE: this belongs elsewhere
	public string getNGUISetLocalizationLanguageName()
	{
		return EditorPrefs.GetString("Language");
	}

	public void setNGUILocalizationLanguage(string languageName)
	{
		EditorPrefs.SetString("Language",languageName);
	}

	public class NGUILocalizationCSVImporter
	{
		private Dictionary<string, List<string>> keyNameToValueListIndexedByLanguage = new Dictionary<string, List<string>>();
		
		public NGUILocalizationCSVImporter(string nguiLocalizationCSVText)
		{
			string[] individualLines = nguiLocalizationCSVText.Split(new char[] {'\r', '\n'},
				StringSplitOptions.RemoveEmptyEntries);
			if (individualLines.Length < keysThatMustExistFirst.Count)
			{
				Debug.LogError("not enough lines to be a valid csv file, must at least have this many entries:" + keysThatMustExistFirst.Count);
				return;
			}
			for(int j=0;j<keysThatMustExistFirst.Count;j++)
			{
				if (individualLines[j].StartsWith(keysThatMustExistFirst[j]))
				{
					Debug.LogError("invalid csv file, expected to have the key start the csv file:"+keysThatMustExistFirst[j] + " at position:"+j);
					return;
				}
			}

			for(int i =0;i<individualLines.Length;i++)
			{
				string line = individualLines[i];
				string[] csvStrings = line.Split(new char[] { ','});
				if (csvStrings.Length < 1)
				{
					Debug.LogError("invalid csv line, no keys found on it:"+ line);
					continue;
				}
				string key = csvStrings[0];
				if(string.IsNullOrEmpty(key))
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

				keyNameToValueListIndexedByLanguage.Add(key,values);
			}
			
		}

		public Dictionary<string, Dictionary<string, string>> getMapOfLanguagesToKeyValueTranslations()
		{
			var langaugeNameToKeyValuePairMap = new Dictionary<string, Dictionary<string, string>>();

			List<string> languageNamesFromFile = keyNameToValueListIndexedByLanguage["Language"];
			foreach (string languageName in languageNamesFromFile)
			{
				langaugeNameToKeyValuePairMap.Add(languageName,new Dictionary<string, string>());
			}
			//we are re-ordering the csv text into a format that can be consumed per-language instead of per-key
			//the csv comes in with KEYNAME,valueIndexedBylanguage1,valueIndexedBylanguage2,etc and we want to use it as store[languageName][key] instead
			foreach (KeyValuePair<string, List<string>> kvp in keyNameToValueListIndexedByLanguage)
			{
				if (kvp.Value.Count > languageNamesFromFile.Count)  //NOTE: maybe let the user know if they are not *exactly* equal?
				{
					Debug.LogWarning("CSV entry has more lines than there are languages, dropping the data from the extra lines");
				}
				if (kvp.Value.Count > languageNamesFromFile.Count)
				{
					Debug.LogWarning("CSV entry has fewer entries than there are langauges, you will not have translations for entries that are unmapped");
				}
				string key = kvp.Key;
				for(int i=0;i<kvp.Value.Count;i++)
				{
					if (i >= languageNamesFromFile.Count)
						continue; //dropping extra commas and the data in them, as they map to no known language
					string toInsert = kvp.Value[i];
					string languageName = languageNamesFromFile[i];  //all of the items are implicitly mapped from the language name
					langaugeNameToKeyValuePairMap[languageName].Add(key,toInsert);
				}
			}
			//now to figure out how to properly decode language names....
			//ranch and french -- franch
			return langaugeNameToKeyValuePairMap;
		}
		
	}

	public class NGUICSVExporter
	{
		private Dictionary<string, List<string>> _keysMappedToListOfLangaugesIndexedByLanguageIndex;
		private string csvString;
		//NOTE: it appears as if groupid maps roughly to KEY.  But most of the time KEY is the language in those files... so I'm not sure I should take that for granted
		public NGUICSVExporter(List<TransfluentLanguage> languagesToExportTo,string groupid="")
		{
			var allTranslationsIndexedByLanguage = new Dictionary<TransfluentLanguage, Dictionary<string, string>>();
			//var destLangDB = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(destinationLanguageCode);
			foreach(TransfluentLanguage lang in languagesToExportTo)
			{
				GameTranslationSet destLangDB = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(lang.code);
				if(destLangDB == null || destLangDB.allTranslations == null)
				{
					Debug.LogWarning("could not find any information for language:"+lang);
					continue;
				}
				Dictionary<string,string> translations = destLangDB.getKeyValuePairs(groupid);
				allTranslationsIndexedByLanguage.Add(lang, translations);
			}

			List<string> keyList = new List<string>();
			List<string> languageList = new List<string>();
			foreach (TransfluentLanguage lang in languagesToExportTo)
			{
				if (!allTranslationsIndexedByLanguage.ContainsKey(lang))
				{
					Debug.LogWarning("Desired output language is not being exported, as there are no translation entries:"+ lang);
					continue;
				}
				string nativeLanguageName = takeLanguageCodeAndTurnItIntoNativeName(lang.code);
				keyList.Add(nativeLanguageName); //I have to think that this is meant as groupid, but I'm not sure how people end up using this
				languageList.Add(nativeLanguageName);
			}

			_keysMappedToListOfLangaugesIndexedByLanguageIndex = new Dictionary<string, List<string>>();
			foreach (TransfluentLanguage lang in languagesToExportTo)
			{
				Dictionary<string, string> keyValuesInLanguage = allTranslationsIndexedByLanguage[lang];
				int indexToAddAt = keyList.IndexOf(takeLanguageCodeAndTurnItIntoNativeName(lang.code));
				string nativeName = takeLanguageCodeAndTurnItIntoNativeName(lang.code);
				foreach (KeyValuePair<string, string> kvp in keyValuesInLanguage)
				{
					if(_keysMappedToListOfLangaugesIndexedByLanguageIndex.ContainsKey(kvp.Key))
						_keysMappedToListOfLangaugesIndexedByLanguageIndex.Add(kvp.Key, new List<string>(languagesToExportTo.Count));
					_keysMappedToListOfLangaugesIndexedByLanguageIndex[kvp.Key].Insert(indexToAddAt, kvp.Value);
				}
			}

			StringBuilder allLinesSB = new StringBuilder();
			allLinesSB.Append(string.Join(",", keyList.ToArray()));
			allLinesSB.Append(string.Join(",", languageList.ToArray()));
			foreach(KeyValuePair<string,List<string>> keyToItems in _keysMappedToListOfLangaugesIndexedByLanguageIndex)
			{
				var tmpList = new List<string>();
				tmpList.Add(keyToItems.Key);
				tmpList.AddRange(keyToItems.Value);
				allLinesSB.Append(string.Join(",", tmpList.ToArray()));
			}
			csvString = allLinesSB.ToString();
		}

		public string getCSV()
		{
			return csvString;
		}

	}
}
