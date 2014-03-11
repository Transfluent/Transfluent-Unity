﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace transfluent
{
	public class TransfluentUtility
	{
		public static TransfluentUtility utility = new TransfluentUtility();

		private bool _failedSetup;
		private TransfluentUtilityInstance _instance;
		private LanguageList _newList;

		private string destLang;
		private string sourceLang;

		public TransfluentUtility()
		{
			Init("en-us", "xx-xx");
		}

		public TransfluentUtility(string sourceLanguage, string destinationLanguage)
		{
			Init(sourceLanguage, destinationLanguage);
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
			//TODO: add notes to formatted strings for translators to not change *exact* format
			//or alternatively... take them out and put them back in
			return string.Format(getTranslation(sourceText), formatStrings);
		}

		private void Init(string sourceLanguage, string destinationLanguage)
		{
			sourceLang = sourceLanguage;
			destLang = destinationLanguage;

			try
			{
				onGotList(ResourceLoadFacade.getLanguageList());
			}
			catch (Exception e)
			{
				Debug.LogError("Failure to set up transfluent translations assets:" + e.Message + " stack:" + e.StackTrace);
				_failedSetup = true;
			}
		}

		private void onGotList(LanguageList newList)
		{
			if (newList == null)
			{
				_failedSetup = true;
				Debug.LogError("Could not load new language list");
				return;
				//Init(sourceLang, destLang);
			}

			_newList = newList;

			TransfluentLanguage dest = _newList.getLangaugeByCode(destLang);
			TransfluentLanguage source = _newList.getLangaugeByCode(sourceLang);
			//TODO: replace this immediately with something that is specific to the editor
			var missingTranslations = GameTranslationGetter.GetMissingTranslationSet(source.id, dest.id);

			_instance = new TransfluentUtilityInstance
			{
				destinationLanguage = dest,
				sourceLanguage = source,
				languageList = _newList,
				destinationLanguageTranslationDB = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(destLang),
				missingTranslationDB = missingTranslations,
			};
			_instance.init();
		}
	}

	//NOTE: this is using the source text as the text key itself
	public class TransfluentUtilityInstance
	{
		private readonly Dictionary<string, string> allKnownTranslations = new Dictionary<string, string>();
		private readonly List<string> notTranslatedCache = new List<string>();
		public TransfluentLanguage sourceLanguage { get; set; }
		public TransfluentLanguage destinationLanguage { get; set; }
		public GameTranslationSet missingTranslationDB { get; set; }
		public GameTranslationSet destinationLanguageTranslationDB { get; set; }
		public LanguageList languageList { get; set; }


		private void addNewMissingTranslation(string sourceText)
		{
			
			string textId = sourceText;
			bool shouldAdd = missingTranslationDB.allTranslations.TrueForAll((TransfluentTranslation otherlang) =>
			{
				if (textId == otherlang.text_id)
				{
					return false;
				}
						
				return true;
			});

			if(!shouldAdd)
				return;

			missingTranslationDB.allTranslations.Add(new TransfluentTranslation
			{
				group_id = sourceLanguage.id.ToString(),
				language = destinationLanguage, 
				text = sourceText,
				text_id = textId
			});
#if UNITY_EDITOR
			EditorUtility.SetDirty(missingTranslationDB);
#endif
		}

		public bool init()
		{
			foreach (TransfluentTranslation trans in destinationLanguageTranslationDB.allTranslations)
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