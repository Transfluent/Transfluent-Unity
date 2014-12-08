using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace transfluent
{
	//a simple static class wrapper to provide basic functionality for global access
	public class TranslationUtility
	{  
		private static ITranslationUtilityInstance _instance;

		private static LanguageList _LanguageList;

		private TranslationUtility()
		{
			if(_instance == null)
			{
				_instance = createNewInstance();
			}
			changeStaticInstanceConfigBasedOnTranslationConfigurationGroup(); //load default translation group info
		}

		public static ITranslationUtilityInstance getUtilityInstanceForDebugging()
		{
			if(_instance == null)
			{
				changeStaticInstanceConfigBasedOnTranslationConfigurationGroup();
			}

			return _instance;
		}

		public static void changeStaticInstanceConfigBasedOnTranslationConfigurationGroup(string group = "")
		{
			var config = ResourceLoadFacade.LoadConfigGroup(group);
			if(config == null) Debug.LogWarning("No default translation configuration found");

			changeStaticInstanceConfig(config.sourceLanguage.code, group);
		}

		//convert into a factory?
		public static bool changeStaticInstanceConfig(string destinationLanguageCode = "", string translationGroup = "")
		{
			//Debug.LogError("LOADING STATIC CONFIG: "+ destinationLanguageCode + " translation group:"+translationGroup);

			ITranslationUtilityInstance tmpInstance = createNewInstance(destinationLanguageCode, translationGroup);
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

		//[MenuItem("Transfluent/Helpers/Test OnLocalize")]
		public static void OnLanguageChanged()
		{
			GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
			for(int i = 0; i < allObjects.Length; i++)
			{
				GameObject go = allObjects[i];
				if(go.transform.parent != null) continue; //skip any non-root messages
				go.BroadcastMessage("OnLocalize", options: SendMessageOptions.DontRequireReceiver);
			}
		}

		//public static event Action OnLanguageChanged;

		public static ITranslationUtilityInstance createNewInstance(string destinationLanguageCode = "", string group = "")
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
			if(Application.isEditor)
			{
				enableCapture = getCaptureMode();
			}
#endif //UNTIY_EDITOR

			TransfluentLanguage dest = _LanguageList.getLangaugeByCode(destinationLanguageCode);
			if(dest == null)
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

			var newTranslfuentUtilityInstance = new TranslationUtilityInstance
			{
				allKnownTranslations = keysInLanguageForGroupSpecified,
				destinationLanguage = dest,
				groupBeingShown = group,
			};
			if(enableCapture)
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
			if(_instance == null){
				_instance = createNewInstance(); //the semantics of this used to work in the initializer.  it seems to no longer work that way
			}
			return _instance.getTranslation(sourceText);
		}

		//not reccommended for game-time use, as it allocates memory
		public static string[] get(string[] sourceTexts)
		{
			if(sourceTexts == null) return null;
			string[] destTexts = new string[sourceTexts.Length];
			for(int i = 0; i < sourceTexts.Length; i++)
			{
				destTexts[i] = get(sourceTexts[i]);
			}
			return destTexts;
		}

		//same format as string.format for now, not tokenized
		//ie "Hi, my name is {0}" instead of "Hi, my name is $NAME" or some other scheme
		public static string getFormatted(string sourceText, params object[] formatStrings)
		{
			return _instance.getFormattedTranslation(sourceText, formatStrings);
		}

#if UNITY_EDITOR

		[MenuItem("Translation/Helpers/Enable Capture Mode")]
		private static void EnableCaptureMode()
		{
			setCaptureMode(true);
			_instance = createNewInstance();
		}

		[MenuItem("Translation/Helpers/Disable Capture Mode")]
		private static void DisableCaptureMode()
		{
			setCaptureMode(false);
			_instance = createNewInstance();
		}

#endif

		private static bool getCaptureMode()
		{
#if UNITY_EDITOR
			return EditorPrefs.GetBool("CAPTURE_MODE");
#else
			return false;
#endif
		}

		private static void setCaptureMode(bool toCapture)
		{
#if UNITY_EDITOR
			EditorPrefs.SetBool("CAPTURE_MODE", toCapture);
#endif //UNITY_EDITOR
		}
	}

	//an interface for handling translaitons
	public interface ITranslationUtilityInstance
	{
		void setNewDestinationLanguage(Dictionary<string, string> transaltionsInSet);

		string getFormattedTranslation(string sourceText, params object[] formatStrings);

		string getTranslation(string sourceText);

		string groupBeingShown { get; set; }

		Dictionary<string, string> allKnownTranslations { get; set; }
	}

	public class TranslationUtilityInstance : ITranslationUtilityInstance
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
			if(allKnownTranslations != null)
			{
				if(allKnownTranslations.ContainsKey(sourceText))
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

	public class AutoCaptureTranslationUtiliityInstance : TranslationUtilityInstance, ITranslationUtilityInstance
	{
		public bool doCapture { get; set; }

		public GameTranslationSet coreTransltionSet { get; set; }

		private List<string> formattedTextToIgnore = new List<string>();

		public new void setNewDestinationLanguage(Dictionary<string, string> transaltionsInSet)
		{
			Debug.LogWarning("SWITCHING Language, likely not intentional for auto capture transltion utility");
			coreTransltionSet = null;
		}

		public new string getFormattedTranslation(string sourceText, params object[] formatStrings)
		{
			string formattedString = string.Format(getTranslation(sourceText), formatStrings);

			if(!formattedTextToIgnore.Contains(formattedString))
			{
				formattedTextToIgnore.Add(formattedString);
			}
			addTranslationIfNotKnown(sourceText);

			return formattedString;
		}

		private void addTranslationIfNotKnown(string translationText)
		{
			if(allKnownTranslations == null) return;
			if(doCapture && !allKnownTranslations.ContainsKey(translationText) && !formattedTextToIgnore.Contains(translationText))
			{
				allKnownTranslations.Add(translationText, translationText);
				coreTransltionSet.mergeInSet(groupBeingShown, allKnownTranslations);
			}
		}

		public new string getTranslation(string sourceText)
		{
			var translation = base.getTranslation(sourceText);

			addTranslationIfNotKnown(sourceText);

			return translation;
		}
	}
}