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
		private static ITransfluentUtilityInstance _instance = createNewInstance();

		private static LanguageList _LanguageList;

		private TranslationUtility()
		{
			changeStaticInstanceConfigBasedOnTranslationConfigurationGroup(); //load default translation group info
		}

		public static ITransfluentUtilityInstance getUtilityInstanceForDebugging()
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

			ITransfluentUtilityInstance tmpInstance = createNewInstance(destinationLanguageCode, translationGroup);
			if(tmpInstance != null)
			{
				_instance.setNewDestinationLanguage(tmpInstance.allKnownTranslations);
				if(Application.isPlaying)
					OnLanguageChanged();

				return true;
			}
			
			return false;
		}

		//[MenuItem("Transfluent/Helpers/Test Change to EN-US")]
		public static void ChangeStaticConfigToUS()
		{
			changeStaticInstanceConfig("en-us");
		}

		//[MenuItem("Transfluent/Helpers/Test Change to FR-FR")]
		public static void ChangeStaticConfigToFRFR()
		{
			changeStaticInstanceConfig("fr-fr");
		}

		[MenuItem("Transfluent/Helpers/Test find")]
		public static void testHelpfind()
		{
			//GetComponentsInChildren( typeof(Transform), true );
			var gos = GameObject.FindObjectsOfType<GameObject>();
			foreach (GameObject go in gos)
			{
				if (go.name == "idle_view")
				{
					Debug.LogError("FOUND IDLE VIEW");
				}
			}
		}

		//[MenuItem("Transfluent/Helpers/Test OnLocalize")]
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
		
		public static ITransfluentUtilityInstance createNewInstance(string destinationLanguageCode = "", string group = "")
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
			bool enableCapture = false;
#if UNITY_EDITOR
			if (Application.isEditor)
			{
				enableCapture = getCaptureMode();
			}
#endif //UNTIY_EDITOR

			TransfluentLanguage dest = _LanguageList.getLangaugeByCode(destinationLanguageCode);
			if (dest == null)
			{
				TranslationConfigurationSO defaultConfigInfo = ResourceLoadFacade.LoadConfigGroup(group);
				string newDestinationLanguageCode = defaultConfigInfo.sourceLanguage.code;
				/*
				if (string.IsNullOrEmpty(destinationLanguageCode))
				{
					Debug.Log("Using default destination language code, as was given an empty language code");
				}
				else
					Debug.Log("Could not load destination language code:" + destinationLanguageCode + " so falling back to source game language code:" + destinationLanguageCode);
				 */
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
			
			var newTranslfuentUtilityInstance = new TransfluentUtilityInstance
			{
				allKnownTranslations = keysInLanguageForGroupSpecified,
				destinationLanguage = dest,
				groupBeingShown = group,
			};
			if (enableCapture)
			{
				newTranslfuentUtilityInstance = new AutoCaptureTranslationUtiliityInstance()
				{
					allKnownTranslations = keysInLanguageForGroupSpecified,
					destinationLanguage = dest,
					groupBeingShown = group,
					doCapture = enableCapture,
					coreTransltionSet = destLangDB,
				};
			}
			return newTranslfuentUtilityInstance;
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
		[MenuItem("Transfluent/Helpers/Enable Capture Mode")]
		static void EnableCaptureMode()
		{
			setCaptureMode(true);
			_instance = createNewInstance();
		}

		[MenuItem("Transfluent/Helpers/Disable Capture Mode")]
		static void DisableCaptureMode()
		{
			setCaptureMode(false);
			_instance = createNewInstance();
		}

		static bool getCaptureMode()
		{
			return EditorPrefs.GetBool("CAPTURE_MODE");
		}
		static void setCaptureMode(bool toCapture)
		{
			EditorPrefs.SetBool("CAPTURE_MODE", toCapture);
		}
	}

	//an interface for handling translaitons
	public interface ITransfluentUtilityInstance
	{
		void setNewDestinationLanguage(Dictionary<string, string> transaltionsInSet);
		string getFormattedTranslation(string sourceText, params object[] formatStrings);
		string getTranslation(string sourceText);
		Dictionary<string, string> allKnownTranslations { get; set; }
	}

	public class TransfluentUtilityInstance : ITransfluentUtilityInstance
	{
		public Dictionary<string, string> allKnownTranslations { get; set; }

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
			if (allKnownTranslations != null)
			{
				if (allKnownTranslations.ContainsKey(sourceText))
				{
					return allKnownTranslations[sourceText];
				}
			}
			else
			{
				//Debug.LogError("KNOWN TRANSLATIONS NOT SET");
			}
			
			return sourceText;
		}
	}

	public class AutoCaptureTranslationUtiliityInstance : TransfluentUtilityInstance, ITransfluentUtilityInstance
	{
		public bool doCapture { get; set; }
		public GameTranslationSet coreTransltionSet { get; set; }

		List<string> formattedTextToIgnore = new List<string>(); 

		public new void setNewDestinationLanguage(Dictionary<string, string> transaltionsInSet)
		{
			Debug.LogWarning("SWITCHING Language, likely not intentional for auto capture transltion utility");
			coreTransltionSet = null;
		}

		public new string getFormattedTranslation(string sourceText, params object[] formatStrings)
		{
			string formattedString = string.Format(getTranslation(sourceText), formatStrings);
			if (!formattedTextToIgnore.Contains(formattedString))
			{
				formattedTextToIgnore.Add(formattedString);
			}
			return formattedString;
		}

		public new string getTranslation(string sourceText)
		{
			var translation = base.getTranslation(sourceText);
			
			if(allKnownTranslations != null)
			{
				if(doCapture && !allKnownTranslations.ContainsKey(sourceText) && !formattedTextToIgnore.Contains(sourceText))
				{
					allKnownTranslations.Add(sourceText, sourceText);
					//TODO: find a cleaner way to add a better way to manually add a translation here 
					coreTransltionSet.mergeInSet(groupBeingShown,allKnownTranslations);
				}
				if (formattedTextToIgnore.Contains(sourceText))
				{
					//Debug.LogError("blacklisted formatted translation:" + sourceText);
				}
			}
			return translation;
		}
	}
}