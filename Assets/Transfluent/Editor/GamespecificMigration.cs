//#define TRANSFLUENT_EXAMPLE

#if TRANSFLUENT_EXAMPLE
using strange.examples.strangerocks;
#endif //!TRANSFLUENT_EXAMPLE
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace transfluent
{
	public class GameSpecificMigration
	{
		public class ButtonViewProcessor : IGameProcessor
		{
			public void process(GameObject go, CustomScriptProcessorState processorState)
			{
#if TRANSFLUENT_EXAMPLE
				var button = go.GetComponent<ButtonView>();
				if(button == null) return;
				if(button.labelMesh != null)
				{
					if(processorState.shouldIgnoreString(button.label))
					{
						processorState.addToBlacklist(go);
						return;
					}
					string newKey = button.label;
					button.labelData.globalizationKey = newKey;

					processorState.addToDB(newKey, newKey);
					processorState.addToBlacklist(go);

					//make sure the button gets saved properly when the scene is closed
					//custom script objects have to manually declare themselves as "dirty"
					EditorUtility.SetDirty(button);
				}
			}
#endif
			}


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
					translationDictionary.Add(key, key);
				translationDictionary[key] = value; //find a way to make sure the the SO gets set dirty?

				gameTranslationSet.mergeInSet(groupid, translationDictionary);
				//EditorUtility.SetnDirty(TransfluentUtility.getUtilityInstanceForDebugging());
			}

			//[MenuItem("Transfluent/Helpers/Test known key")]
			public static void TestKnownKey()
			{
				Debug.Log(TranslationUtility.get("Start Game"));
			}


			[MenuItem("Translation/Helpers/Full migration")]
			public static void UpdateReferences()
			{
				var scanner = new AssetScanner();
				scanner.searchScenes();

				scanner.searchPrefabs();
			}


			public static List<GameObject> getAllPrefabReferences()
			{
				var retList = new List<GameObject>();
				string[] aMaterialFiles = Directory.GetFiles(Application.dataPath, "*.prefab", SearchOption.AllDirectories);
				foreach (string matFile in aMaterialFiles)
				{
					string assetPath = "Assets" + matFile.Replace(Application.dataPath, "").Replace('\\', '/');
					var go = (GameObject) AssetDatabase.LoadAssetAtPath(assetPath, typeof (GameObject));

					retList.Add(go);
				}
				return retList;
			}
		}
	}

	public class CustomScriptProcessorState
	{
		private readonly List<GameObject> _blackList;
		private readonly List<string> _stringsToIgnore;
		private readonly ITranslationUtilityInstance _translationDB;

		public CustomScriptProcessorState(List<GameObject> blackList, ITranslationUtilityInstance translationDb,
			List<string> stringsToIgnore)
		{
			_blackList = blackList;
			_translationDB = translationDb;
			_stringsToIgnore = stringsToIgnore;
		}

		public void addToBlacklist(GameObject go)
		{
			if(go != null && _blackList.Contains(go) == false)
				_blackList.Add(go);
		}

		public bool shouldIgnoreString(string input)
		{
			return _stringsToIgnore.Contains(input);
		}

		public void addToDB(string key, string value)
		{
			string currentGroup = _translationDB.groupBeingShown;
			TranslationConfigurationSO config = ResourceLoadFacade.LoadConfigGroup(_translationDB.groupBeingShown);
			Dictionary<string, string> translationDictionary = _translationDB.allKnownTranslations;
			GameTranslationSet gameTranslationSet =
				GameTranslationGetter.GetTranslaitonSetFromLanguageCode(config.sourceLanguage.code);

			bool exists = translationDictionary.ContainsKey(key);
			if(!exists)
				translationDictionary.Add(key, key);

			gameTranslationSet.mergeInSet(currentGroup, translationDictionary);
			//_translationDB.allKnownTranslations.Add(key,value);
		}
	}

	public interface IGameProcessor
	{
		void process(GameObject go, CustomScriptProcessorState processorState);
	}

	public class TextMeshProcessor : IGameProcessor
	{
		public void process(GameObject go, CustomScriptProcessorState processorState)
		{
			var textMesh = go.GetComponent<TextMesh>();
			if(textMesh == null) return;

			string newKey = textMesh.text;
			processorState.addToDB(newKey, newKey);
			processorState.addToBlacklist(go);

			var translatable = textMesh.GetComponent<LocalizedTextMesh>();
			if(processorState.shouldIgnoreString(textMesh.text))
			{
				processorState.addToBlacklist(go);
				return;
			}

			if(translatable == null)
			{
				translatable = textMesh.gameObject.AddComponent<LocalizedTextMesh>();
				translatable.textmesh = textMesh; //just use whatever the source text is upfront, and allow the user to
			}

			translatable.localizableText.globalizationKey = textMesh.text;
			//For textmesh specificially, this setDirty is not needed according to http://docs.unity3d.com/Documentation/ScriptReference/EditorUtility.SetDirty.html
			//EditorUtility.SetDirty(textMesh);
		}
	}
	public class GUITextProcessor : IGameProcessor
	{
		public void process(GameObject go, CustomScriptProcessorState processorState)
		{
			var guiText = go.GetComponent<GUIText>();
			if(guiText == null) return;

			string newKey = guiText.text;
			processorState.addToDB(newKey, newKey);
			processorState.addToBlacklist(go);

			var translatable = guiText.GetComponent<LocalizedGUIText>();
			if(processorState.shouldIgnoreString(guiText.text))
			{
				processorState.addToBlacklist(go);
				return;
			}

			if(translatable == null)
			{
				translatable = guiText.gameObject.AddComponent<LocalizedGUIText>();
				translatable.guiTextToModify = guiText; //just use whatever the source text is upfront, and allow the user to
			}

			translatable.localizableText.globalizationKey = guiText.text;
			//For guitext and other unity managed objects, this setDirty is not needed according to http://docs.unity3d.com/Documentation/ScriptReference/EditorUtility.SetDirty.html
			EditorUtility.SetDirty(guiText.gameObject);
			EditorUtility.SetDirty(guiText);
		}
	}
}