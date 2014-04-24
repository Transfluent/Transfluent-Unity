using System;
using System.Collections.Generic;
using transfluent;
using UnityEditor;
using UnityEngine;

public class SimpleLineBasedImporter
{
	//simple case where key=value for a line based file
	[MenuItem("Translation/Database/Import Selected Newline based File")]
	private static void ImportFileToDefaultGroupAndSourceLanguageDB() //does not import it to a group
	{
		TextAsset ta = Selection.activeObject as TextAsset;
		if(ta == null)
		{
			Debug.LogError("Selected item is not a text asset");
			return;
		}
		string[] toSplit = new string[] { "\n", "\r\n" };
		string[] newKeys = ta.text.Split(toSplit, StringSplitOptions.RemoveEmptyEntries);

		var dic = new Dictionary<string, string>();
		foreach(string key in newKeys)
		{
			if(!dic.ContainsKey(key))
				dic.Add(key, key);
		}

		var translationConfig = ResourceLoadFacade.LoadConfigGroup("");
		var currentSet = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(translationConfig.sourceLanguage.code);
		currentSet.mergeInSet("", dic);
		/*
		var languageCodeList = new List<string> { translationConfig.sourceLanguage.code };
		translationConfig.destinationLanguages.ForEach((TransfluentLanguage lang) => { languageCodeList.Add(lang.code); });

		DownloadAllGameTranslations.uploadTranslationSet(languageCodeList, translationConfig.translation_set_group);
		*/
	}
}