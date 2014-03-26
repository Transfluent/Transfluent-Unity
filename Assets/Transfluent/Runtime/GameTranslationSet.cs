using System;
using System.Collections.Generic;
using transfluent;
using UnityEngine;

//NOTE: last time I had the transfluent namespace enabled for this, the script dissapeared
//this is almost certainly due to the optional parameters in the function definitoins
//namespace transfluent
//{
public class GameTranslationSet : ScriptableObject
{
	public Dictionary<string, Dictionary<string, string>> groupToDictionary =
		new Dictionary<string, Dictionary<string, string>>();

	public TransfluentLanguage langaugeThatTranslationsAreIn;

	private Dictionary<string, string> getSubDictionary(string groupid = "")
	{
		if (groupid == null) groupid = "";
		if (!groupToDictionary.ContainsKey(groupid)) groupToDictionary.Add(groupid, new Dictionary<string, string>());
		return groupToDictionary[groupid];
	}

	public void setPair(string key, string value, string groupid = "")
	{
		Dictionary<string, string> groupDictionary = getSubDictionary(groupid);
		groupDictionary.Add(key, value);
	}

	//cleanup stepafter merging or performing any large comparisons
	private void cullEmptyDictionaries()
	{
		var toRemove = new List<string>();

		foreach (var keyValuePair in groupToDictionary)
		{
			Dictionary<string, string> subDictionary = keyValuePair.Value;
			if (subDictionary.Keys.Count == 0) //if there's nothing in it
			{
				toRemove.Add(keyValuePair.Key);
			}
		}
		toRemove.ForEach((string keyToRemove) => { groupToDictionary.Remove(keyToRemove); });
	}

	public List<string> getAllKeys()
	{
		var strings = new List<string>();
		foreach (var groupToDic in groupToDictionary)
		{
			foreach (var kvp in groupToDic.Value)
			{
				strings.Add(kvp.Key);
			}
		}
		return strings;
	}

	public List<string> getAllValues()
	{
		var strings = new List<string>();
		foreach (var groupToDic in groupToDictionary)
		{
			foreach (var kvp in groupToDic.Value)
			{
				strings.Add(kvp.Value);
			}
		}
		return strings;
	}

	public long getWordCount(string group = "")
	{
		Dictionary<string, string> dic = getSubDictionary(group);

		long wordCount = 0;
		foreach (var kvp in dic)
		{
			wordCount += kvp.Value.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries).Length;
		}
		return wordCount;
	}

	public List<string> getPretranslatedKeys(List<string> keysToIgnoreProbablyAlreadyTranslated, string group = "")
	{
		var keysUntranslated = new List<string>();
		Dictionary<string, string> dic = getSubDictionary(group);

		foreach (var kvp in dic)
		{
			if (!keysToIgnoreProbablyAlreadyTranslated.Contains(kvp.Key))
				keysUntranslated.Add(kvp.Value);
		}
		return keysUntranslated;
	}

	public Dictionary<string, string> getKeyValuePairs(string group = "")
	{
		Dictionary<string, string> groupDictionary = getSubDictionary(group);

		var dictionaryCopy = new Dictionary<string, string>(groupDictionary);

		return dictionaryCopy;
	}

	//assumes same language and same groupid
	public void mergeInNewListOfTranslations(List<TransfluentTranslation> translations)
	{
		if (translations == null || translations.Count == 0) return;

		var translationGroupidPairsToSetup = new Dictionary<KeyValuePair<string, int>, Dictionary<string, string>>();
		var handledLanguages = new List<TransfluentLanguage>();
		foreach (TransfluentTranslation translation in translations)
		{
			var set = new KeyValuePair<string, int>(translation.group_id, translation.language.id);
			if (!translationGroupidPairsToSetup.ContainsKey(set))
			{
				handledLanguages.Add(translation.language);
				//Debug.Log("Adding new set:" + translation.group_id + " and" + language.id);
				translationGroupidPairsToSetup.Add(set, new Dictionary<string, string>());
			}
			Dictionary<string, string> dictionaryToInsertInto = translationGroupidPairsToSetup[set];

			dictionaryToInsertInto.Add(translation.text_id, translation.text);
		}

		foreach (var kvp in translationGroupidPairsToSetup)
		{
			int langaugeId = kvp.Key.Value; //oof... review this
			if (langaugeId != langaugeThatTranslationsAreIn.id) continue; //warn here?

			mergeInNewListOfTranslations(kvp.Value, kvp.Key.Key);
		}
	}

	public void mergeInNewListOfTranslations(Dictionary<string, string> newTranslations, string groupid = null)
	{
		//actually add the new translations
		foreach (var kvp in newTranslations)
		{
			setPair(kvp.Key, kvp.Value, groupid);
		}
		cullEmptyDictionaries();
	}
}

//}