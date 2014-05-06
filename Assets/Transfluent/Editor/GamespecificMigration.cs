#define TRANSFLUENT_EXAMPLE
using System;
#if TRANSFLUENT_EXAMPLE
using strange.examples.strangerocks;
#endif //!TRANSFLUENT_EXAMPLE
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace transfluent
{
	public class GameSpecificMigration : MonoBehaviour
	{
		
		public interface IGameProcessor
		{
			void process(GameObject go, CustomScriptProcessorState processorState);
		}
		public class CustomScriptProcessorState
		{
			private List<GameObject> _blackList;
			private ITranslationUtilityInstance _translationDB;
			private List<string> _stringsToIgnore;  

			public CustomScriptProcessorState(List<GameObject> blackList, ITranslationUtilityInstance translationDb,List<string> stringsToIgnore )
			{
				_blackList = blackList;
				_translationDB = translationDb;
				_stringsToIgnore = stringsToIgnore;
			}

			public void addToBlacklist(GameObject go)
			{
				if(go != null && _blackList.Contains(go) == false)
				{
					_blackList.Add(go);
				}
			}

			public bool shouldIgnoreString(string input)
			{
				return _stringsToIgnore.Contains(input);
			}

			public void addToDB(string key, string value)
			{
				string currentGroup = _translationDB.groupBeingShown;
				TranslationConfigurationSO config = ResourceLoadFacade.LoadConfigGroup(_translationDB.groupBeingShown);
				var translationDictionary = _translationDB.allKnownTranslations;
				GameTranslationSet gameTranslationSet =
					GameTranslationGetter.GetTranslaitonSetFromLanguageCode(config.sourceLanguage.code);

				bool exists = translationDictionary.ContainsKey(key);
				if(!exists)
				{
					translationDictionary.Add(key, key);
				}

				gameTranslationSet.mergeInSet(currentGroup, translationDictionary);
				//_translationDB.allKnownTranslations.Add(key,value);
			}
		}

		public class ButtonViewProcessor : IGameProcessor
		{
			public void process(GameObject go,CustomScriptProcessorState processorState)
			{
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
		
	}
}