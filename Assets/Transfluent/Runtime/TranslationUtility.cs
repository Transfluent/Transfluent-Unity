using System.Collections.Generic;
using System.Security.Permissions;
using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif

namespace transfluent
{
	public class TranslationUtility
	{
		//TODO: keep sets of language/group and allow for explict load/unload statements
		//the implication of that is that any ongui/other client would need to declare set groups for their activiites in some way
		private static readonly TransfluentUtilityInstance _instance = createNewInstance();

		private static LanguageList _LanguageList;

		private TranslationUtility()
		{
			changeStaticInstanceConfigBasedOnTranslationConfigurationGroup(); //load default translation group info
		}

		public static TransfluentUtilityInstance getUtilityInstanceForDebugging()
		{
			changeStaticInstanceConfigBasedOnTranslationConfigurationGroup();
			return _instance;
		}

		public static void changeStaticInstanceConfigBasedOnTranslationConfigurationGroup(string group="")
		{
			var config = ResourceLoadFacade.LoadConfigGroup(group);
			if(config == null) Debug.LogWarning("No default translation configuration found");

			changeStaticInstanceConfig(config.sourceLanguage.code, group);
		}

		//convert into a factory?
		public static bool changeStaticInstanceConfig(string destinationLanguageCode = "", string translationGroup = "")
		{
			//Debug.LogError("LOADING STATIC CONFIG: "+ destinationLanguageCode + " translation group:"+translationGroup);

			TransfluentUtilityInstance tmpInstance = createNewInstance(destinationLanguageCode, translationGroup);
			if(tmpInstance != null)
			{
				_instance.setNewDestinationLanguage(tmpInstance.allKnownTranslations);
				if(Application.isPlaying)
					OnLanguageChanged();

				return true;
			}
			
			return false;
		}

		[MenuItem("Helpers/Test Change to EN-US")]
		public static void ChangeStaticConfigToUS()
		{
			changeStaticInstanceConfig("en-us");
		}

		[MenuItem("Helpers/Test Change to FR-FR")]
		public static void ChangeStaticConfigToFRFR()
		{
			changeStaticInstanceConfig("fr-fr");
		}

		[MenuItem("Helpers/Test find")]
		public static void testHelpfind()
		{
			//GetComponentsInChildren( typeof(Transform), true );
			var gos = GameObject.FindSceneObjectsOfType(typeof (GameObject)) as GameObject[];
			foreach (GameObject go in gos)
			{
				if (go.name == "idle_view")
				{
					Debug.LogError("FOUND IDLE VIEW");
				}
			}
		}

		[MenuItem("Helpers/Test OnLocalize")]
		public static void OnLanguageChanged()
		{
			
			GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
			for (int i=0;i<allObjects.Length;i++)
			{
				GameObject go = allObjects[i];
				if (go.transform.parent != null) continue; //skip any non-root messages
				go.BroadcastMessage("OnLocalize",options:SendMessageOptions.DontRequireReceiver);
			}
		}
		//public static event Action OnLanguageChanged;
		
		public static TransfluentUtilityInstance createNewInstance(string destinationLanguageCode = "", string group = "")
		{
			if(_LanguageList == null)
			{
				_LanguageList = ResourceLoadFacade.getLanguageList();
			}

			if(_LanguageList == null)
			{
				Debug.LogError("Could not load new language list");
				return null;
			}

			TransfluentLanguage dest = _LanguageList.getLangaugeByCode(destinationLanguageCode);
			if (dest == null)
			{
				TranslationConfigurationSO defaultConfigInfo = ResourceLoadFacade.LoadConfigGroup(group);
				string newDestinationLanguageCode = defaultConfigInfo.sourceLanguage.code;
				if(string.IsNullOrEmpty(destinationLanguageCode))
					Debug.Log("Using default destination language code, as was given an empty language code");
				else
				Debug.Log("Could not load destination language code:" + destinationLanguageCode + " so falling back to source game language code:" + destinationLanguageCode);
				destinationLanguageCode = newDestinationLanguageCode;
				
				dest = _LanguageList.getLangaugeByCode(destinationLanguageCode);
				//dest = _LanguageList.getLangaugeByCode
			}
			GameTranslationSet destLangDB = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(destinationLanguageCode);
			Dictionary<string, string> keysInLanguageForGroupSpecified = destLangDB != null
				? destLangDB.getGroup(group).getDictionaryCopy()
				: new Dictionary<string, string>();
#if UNITY_EDITOR
			EditorUtility.SetDirty(destLangDB);
#endif
			return new TransfluentUtilityInstance
			{
				allKnownTranslations = keysInLanguageForGroupSpecified,
				destinationLanguage = dest,
				groupBeingShown = group
			};
		}

		public static string get(string sourceText)
		{
			return _instance.getTranslation(sourceText);
		}

		//same format as string.format for now, not tokenized
		//ie "Hi, my name is {0}" instead of "Hi, my name is $NAME" or some other scheme
		public static string getFormatted(string sourceText, params object[] formatStrings)
		{
			return _instance.getFormattedTranslation(sourceText, formatStrings);
		}
	}

	//an interface for handling translaitons
	public class TransfluentUtilityInstance
	{
		public Dictionary<string, string> allKnownTranslations;

		public TransfluentLanguage destinationLanguage { get; set; }

		public string groupBeingShown { get; set; }

		public void setNewDestinationLanguage(Dictionary<string, string> transaltionsInSet)
		{
			if(allKnownTranslations != null)
				allKnownTranslations.Clear();
			allKnownTranslations = transaltionsInSet;
		}

		//same format as string.format for now, not tokenized
		//ie "Hi, my name is {0}" instead of "Hi, my name is $NAME" or some other scheme
		public string getFormattedTranslation(string sourceText, params object[] formatStrings)
		{
			return string.Format(getTranslation(sourceText), formatStrings);
		}

		public string getTranslation(string sourceText)
		{
			if(allKnownTranslations != null && allKnownTranslations.ContainsKey(sourceText))
			{
				return allKnownTranslations[sourceText];
			}
			return sourceText;
		}
	}
}