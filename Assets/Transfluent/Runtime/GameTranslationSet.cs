using System;
using System.Collections.Generic;
using transfluent;
using UnityEngine;

//namespace transfluent
//{
public class GameTranslationSet : ScriptableObject
{
	public List<TransfluentTranslation> allTranslations = new List<TransfluentTranslation>();

	/*
	public void setPair(string key, string value, string groupid = "")
	{
		bool groupIsEmpty = string.IsNullOrEmpty(groupid);
		if (groupIsEmpty)
		{
		}
		else
		{
			
		}
	}
	*/

	public int getWordCount(string group = "")
	{
		return getListOfTranslationsInGroup(group).Count;
	}

	public Dictionary<string, string> getKeyValuePairs(string group = "")
	{
		var dictionary = new Dictionary<string, string>();
		if(allTranslations == null) return dictionary;

		bool groupIsEmpty = string.IsNullOrEmpty(group);

		foreach(TransfluentTranslation translation in allTranslations)
		{
			if(groupIsEmpty && string.IsNullOrEmpty(translation.group_id))
			{
				if(dictionary.ContainsKey(translation.text_id))
				{
					Debug.LogError("Dictionary already contains key:" + translation.text_id);
					continue;
				}
				dictionary.Add(translation.text_id, translation.text);
			}
			else
			{
				if(translation.group_id == group)
				{
					dictionary.Add(translation.text_id, translation.text);
				}
			}
		}
		return dictionary;
	}

	public List<TransfluentTranslation> getListOfTranslationsInGroup(string groupid = null)
	{
		List<TransfluentTranslation> translations = new List<TransfluentTranslation>();
		bool groupIsEmpty = string.IsNullOrEmpty(groupid);
		foreach(TransfluentTranslation translation in allTranslations)
		{
			if(groupIsEmpty && string.IsNullOrEmpty(translation.group_id))
			{
				translations.Add(translation);
			}
			else
			{
				if(groupid == translation.group_id)
				{
					translations.Add(translation);
				}
			}
		}
		return translations;
	}

	//assumes same language and same groupid
	public void mergeInNewListOfTranslations(List<TransfluentTranslation> translations)
	{
		if(translations == null || translations.Count == 0) return;

		var translationGroupidPairsToSetup = new Dictionary<KeyValuePair<string, int>, Dictionary<string, string>>();
		var handledLanguages = new List<TransfluentLanguage>();
		foreach(TransfluentTranslation translation in translations)
		{
			var set = new KeyValuePair<string, int>(translation.group_id, translation.language.id);
			if(!translationGroupidPairsToSetup.ContainsKey(set))
			{
				handledLanguages.Add(translation.language);
				//Debug.Log("Adding new set:" + translation.group_id + " and" + language.id);
				translationGroupidPairsToSetup.Add(set, new Dictionary<string, string>());
			}
			var dictionaryToInsertInto = translationGroupidPairsToSetup[set];

			dictionaryToInsertInto.Add(translation.text_id, translation.text);
		}
		foreach(KeyValuePair<KeyValuePair<string, int>, Dictionary<string, string>> kvp in translationGroupidPairsToSetup)
		{
			int langaugeId = kvp.Key.Value;  //oof... review this
			TransfluentLanguage language = null;
			handledLanguages.ForEach((TransfluentLanguage test) => { if(test.id == langaugeId) language = test; });
			mergeInNewListOfTranslations(kvp.Value, kvp.Key.Key, language);
		}
	}

	public void mergeInNewListOfTranslations(Dictionary<string, string> newTranslations, string groupid = null, TransfluentLanguage langauge = null)
	{
		if(allTranslations == null) allTranslations = new List<TransfluentTranslation>();
		if(langauge == null)
		{
			if(allTranslations.Count == 0) throw new Exception("Cannot infer language of inserted translations, please insert at least one with the language parameter");
			langauge = allTranslations[0].language;	
		}
		//modify existing entries first
		List<TransfluentTranslation> existingTranslations = getListOfTranslationsInGroup(groupid);
		List<string> keysAlreadyExistedAndModifiedWithNewValue = new List<string>();
		foreach(TransfluentTranslation translation in existingTranslations)
		{
			if(newTranslations.ContainsKey(translation.text_id))
			{
				//Debug.Log("key already esits:" + translation.text_id + " and:" + newTranslations[translation.text_id] );
				translation.text = newTranslations[translation.text_id];
				keysAlreadyExistedAndModifiedWithNewValue.Add(translation.text_id);
			}
		}
		foreach(KeyValuePair<string, string> kvp in newTranslations)
		{
			if(keysAlreadyExistedAndModifiedWithNewValue.Contains(kvp.Key)) continue;
			//Debug.Log("Ading new translation:"+kvp);
			allTranslations.Add(new TransfluentTranslation()
			{
				group_id = groupid,
				language = langauge,
				text = kvp.Value,
				text_id = kvp.Key
			});
		}
	}
}

//}