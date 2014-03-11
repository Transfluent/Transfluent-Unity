using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;
using UnityTest;

namespace transfluent
{
	public class TransfluentUtility
	{
		public static TransfluentUtility utility = new TransfluentUtility();

		private bool _failedSetup;
		private TransfluentUtilityInstance _instance;
		private LanguageList _newList;

		public TransfluentUtilityInstance getUtilityInstanceForDebugging()
		{
			return _instance;
		}
		public TransfluentUtility()
		{
			setTranslationGroup();
			//Init("en-us", "xx-xx");
		}
		
		public TransfluentUtility(string sourceLanguage, string destinationLanguageCode)
		{
			Init(sourceLanguage, destinationLanguageCode);
		}

		public TransfluentUtility(TranslationConfigurationSO so,string destinationLanguageCode=null)
		{
			string destCode = destinationLanguageCode;
			if (destCode == null)
			{
				destCode = so.sourceLanguage.code;
			}
			Init(so.sourceLanguage.code, destCode);
		}

		public void setTranslationGroup(string translationGroup="")
		{
			var config = ResourceLoadFacade.LoadConfigGroup(translationGroup);
			//config = ResourceLoadFacade.LoadConfigGroup("");
			if (config == null) throw new Exception("could not load default config group:"+translationGroup);
			Init(config.sourceLanguage.code, config.sourceLanguage.code);
		}

		

		public bool failedSetup
		{
			get { return _failedSetup; }
		}

		public string getTranslation(string sourceText)
		{
			if (_instance == null)
			{
				return sourceText; //TODO: add the missing translations db in this condition
			}
			return _instance.getTranslation(sourceText);
		}

		//same format as string.format for now, not tokenized
		//ie "Hi, my name is {0}" instead of "Hi, my name is $NAME" or some other scheme
		public string getFormattedTranslation(string sourceText,params object[] formatStrings)
		{
			if(_instance == null)
			{
				return string.Format(sourceText,formatStrings); //TODO: add the missing translations db in this condition
			}
			return _instance.getFormattedTranslation(sourceText,formatStrings);
		}

		private void Init(string sourceLanguageCode, string destinationLanguageCode)
		{
			try
			{
				setupIndividualAssets(sourceLanguageCode,destinationLanguageCode);
			}
			catch (Exception e)
			{
				Debug.LogError("Failure to set up transfluent translations assets:" + e.Message + " stack:" + e.StackTrace);
				_failedSetup = true;
			}
		}

		private void setupIndividualAssets(string sourceLanguageCode, string destinationLanguageCode)
		{
			_newList =  ResourceLoadFacade.getLanguageList();
			if(_newList == null)
			{
				_failedSetup = true;
				Debug.LogError("Could not load new language list");
				return;
				//Init(sourceLang, destLang);
			}

			TransfluentLanguage dest = _newList.getLangaugeByCode(destinationLanguageCode);
			TransfluentLanguage source = _newList.getLangaugeByCode(sourceLanguageCode);
			var missingTranslations = GameTranslationGetter.GetMissingTranslationSet(source.id, dest.id);
			var destLangDB = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(destinationLanguageCode);

			var missingTranslationsList = missingTranslations == null ? null : missingTranslations.allTranslations;
			var destLangDBList = destLangDB == null ? new List<TransfluentTranslation>() : destLangDB.allTranslations;

#if UNITY_EDITOR
			if (missingTranslations == null)
			{
				//create one?
				//missingTranslations = ResourceCreator.CreateSO<GameTranslationSet>(GameTranslationGetter.fileNameFromLanguageCode(languageCode));
			}
			else
			{
				EditorUtility.SetDirty(missingTranslations);
				EditorUtility.SetDirty(destLangDB);
			}
			
#endif
			_instance = new TransfluentUtilityInstance
			{
				destinationLanguage = dest,
				sourceLanguage = source,
				languageList = _newList,
				destinationLanguageTranslationDB = destLangDBList,
				missingTranslationDB = missingTranslationsList,
			};
			_instance.init();
		}
		public void setLanguage(string languageCode)
		{
			if(_instance == null)
			{
				Debug.Log("Language translation has not yet beet set up, cannot set language now");
				return;
			}

			TransfluentLanguage source = _instance.sourceLanguage;
			TransfluentLanguage dest = _newList.getLangaugeByCode(languageCode);



			//TODO: replace this immediately with something that is specific to the editor
			var knownTranslations = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(dest.code);
			var missingTranslations = GameTranslationGetter.GetMissingTranslationSet(source.id, dest.id);
			
			var missingTranslationsList = missingTranslations == null ? null : missingTranslations.allTranslations;
			var destLangDBList = knownTranslations == null ? new List<TransfluentTranslation>() : knownTranslations.allTranslations;

			_instance.setNewDestinationLanguage(destLangDBList, missingTranslationsList);

		}
	}

	//NOTE: this is using the source text as the text key itself
	public class TransfluentUtilityInstance
	{
		private readonly Dictionary<string, string> allKnownTranslations = new Dictionary<string, string>();
		private readonly List<string> notTranslatedCache = new List<string>();
		public TransfluentLanguage sourceLanguage { get; set; }
		public TransfluentLanguage destinationLanguage { get; set; }
		public List<TransfluentTranslation> missingTranslationDB { get; set; }  //TODO: note this is funcitonatliy only for the editor. make sure this is not touched at runtime
		public List<TransfluentTranslation> destinationLanguageTranslationDB { get; set; }
		public LanguageList languageList { get; set; }


		private void addNewMissingTranslation(string sourceText)
		{
			
			string textId = sourceText;
			bool shouldAdd = missingTranslationDB.TrueForAll((TransfluentTranslation otherlang) =>
			{
				if (textId == otherlang.text_id)
				{
					return false;
				}
						
				return true;
			});
			
			if(!shouldAdd)
				return;

			missingTranslationDB.Add(new TransfluentTranslation
			{
				group_id = sourceLanguage.id.ToString(),
				language = destinationLanguage, 
				text = sourceText,
				text_id = textId
			});

		}

		public void setNewDestinationLanguage(List<TransfluentTranslation> translationsInNewDestinationLanguage,List<TransfluentTranslation> missingTranslations)
		{
			destinationLanguageTranslationDB = translationsInNewDestinationLanguage;
			missingTranslationDB = missingTranslations;
			allKnownTranslations.Clear();
			notTranslatedCache.Clear();
			init();
		}

		public bool init()
		{
			foreach (TransfluentTranslation trans in destinationLanguageTranslationDB)
			{
				allKnownTranslations.Add(trans.text_id, trans.text);
			}
			return true;
		}

		/*
		public string getGroupId()
		{
			return "UNITY_AUTO_GENERATED";
		}*/

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
			if (allKnownTranslations.ContainsKey(sourceText))
			{
				return allKnownTranslations[sourceText];
			}
			if (missingTranslationDB != null && !notTranslatedCache.Contains(sourceText))
			{
				notTranslatedCache.Add(sourceText);

				addNewMissingTranslation(sourceText);
			}
			return sourceText;
		}
	}

	
}