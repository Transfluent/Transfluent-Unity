using System.Collections.Generic;
using System.IO;
using transfluent;
using UnityEditor;
using UnityEngine;

public class FindTextMeshReferences 
{
	public static void setKeyInDefaultLanguageDB(string key, string value, string groupid = "")
	{
		//Debug.LogWarning("Make sure to set language to game source language before saving a new translation key");
		Dictionary<string, string> translationDictionary =
			TranslationUtility.getUtilityInstanceForDebugging().allKnownTranslations;
		TranslationConfigurationSO config = ResourceLoadFacade.LoadConfigGroup(groupid);

		GameTranslationSet gameTranslationSet =
			GameTranslationGetter.GetTranslaitonSetFromLanguageCode(config.sourceLanguage.code);

		bool exists = translationDictionary.ContainsKey(key);
		if(!exists)
		{
			translationDictionary.Add(key, key);
		}
		translationDictionary[key] = value; //find a way to make sure the the SO gets set dirty?

		gameTranslationSet.mergeInSet(groupid, translationDictionary);
		//EditorUtility.SetnDirty(TransfluentUtility.getUtilityInstanceForDebugging());
	}

	//[MenuItem("Transfluent/Helpers/Test known key")]
	public static void TestKnownKey()
	{
		Debug.Log(TranslationUtility.get("Start Game"));
	}

	private static bool shouldGlobalizeText(string textIn)
	{
		foreach(string blacklist in GameSpecificMigration.blacklistStringsContaining)
		{
			if(textIn.Contains(blacklist))
				return false;
		}
		return true;
	}

	[MenuItem("Translation/Helpers/Full migration")]
	public static void UpdateReferences()
	{
		AssetScanner.fullMigration();	
	}


	public static List<GameObject> getAllPrefabReferences()
	{
		var retList = new List<GameObject>();
		string[] aMaterialFiles = Directory.GetFiles(Application.dataPath, "*.prefab", SearchOption.AllDirectories);
		foreach(string matFile in aMaterialFiles)
		{
			string assetPath = "Assets" + matFile.Replace(Application.dataPath, "").Replace('\\', '/');
			var go = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));

			retList.Add(go);
		}
		return retList;
	}

}