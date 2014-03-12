using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

namespace transfluent
{
	public class TransfluentUtility
	{
		//TODO: keep sets of language/group and allow for explict load/unload statements
		//the implication of that is that any ongui/other client would need to declare set groups for their activiites in some way
		static TransfluentUtilityInstance _instance = new TransfluentUtilityInstance(); 
		
		private static LanguageList _LanguageList;
		
		public static TransfluentUtilityInstance getUtilityInstanceForDebugging()
		{
			return _instance;
		}

		TransfluentUtility()
		{
			changeStaticInstanceConfig(); //load default translation group info
		}

		//todo: convert into a factory
		public static bool changeStaticInstanceConfig(string destinationLanguageCode="", string translationGroup="")
		{
			var tmpInstance = createNewInstance(destinationLanguageCode, translationGroup);
			if (tmpInstance != null)
			{
				_instance = tmpInstance;
				return true;
			}
			else
			{
				return false;
			}
		}

		public static TransfluentUtilityInstance createNewInstance(string destinationLanguageCode = "", string translationGroup = "")
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
			var destLangDB = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(destinationLanguageCode);
			var destLangDBList = destLangDB == null ? new List<TransfluentTranslation>() : destLangDB.allTranslations;
			bool groupIsEmpty = string.IsNullOrEmpty(translationGroup);

			var keysInLanguageForGroupSpecified = new Dictionary<string, string>();
			foreach(TransfluentTranslation translation in destLangDBList)
			{
				if(groupIsEmpty && string.IsNullOrEmpty(translation.group_id))
				{
					keysInLanguageForGroupSpecified.Add(translation.text_id, translation.text);
				}
				else
				{
					if(translation.group_id == translationGroup)
					{
						keysInLanguageForGroupSpecified.Add(translation.text_id, translation.text);
					}
				}
			}
			return new TransfluentUtilityInstance()
			{
				allKnownTranslations = keysInLanguageForGroupSpecified,
				destinationLanguage = dest,
				groupBeingShown = translationGroup
			};
		}

		public static string getTranslation(string sourceText)
		{
			return _instance.getTranslation(sourceText);
		}

		//same format as string.format for now, not tokenized
		//ie "Hi, my name is {0}" instead of "Hi, my name is $NAME" or some other scheme
		public static string getFormattedTranslation(string sourceText, params object[] formatStrings)
		{
			return _instance.getFormattedTranslation(sourceText,formatStrings);
		}
		
	}

	//an interface for handling translaitons
	public class TransfluentUtilityInstance
	{
		public Dictionary<string, string> allKnownTranslations;
		public TransfluentLanguage destinationLanguage { get; set; }
		public string groupBeingShown { get; set; }

		public void setNewDestinationLanguage(Dictionary<string,string> transaltionsInSet)
		{
			allKnownTranslations.Clear();
		}


		//same format as string.format for now, not tokenized
		//ie "Hi, my name is {0}" instead of "Hi, my name is $NAME" or some other scheme
		public string getFormattedTranslation(string sourceText, params object[] formatStrings)
		{
			//TODO: add notes to formatted strings for translators to not change *exact* format
			//or alternatively... take them out and put them back in
			return string.Format(getTranslation(sourceText), formatStrings);
		}

		public string getTranslation(string sourceText)
		{
			if (allKnownTranslations != null && allKnownTranslations.ContainsKey(sourceText))
			{
				return allKnownTranslations[sourceText];
			}
			return sourceText;
		}
	}

	
}