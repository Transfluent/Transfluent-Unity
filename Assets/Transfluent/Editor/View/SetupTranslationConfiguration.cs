using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using transfluent;
using transfluent.editor;
using UnityEditor;
using UnityEngine;

[Obsolete]
public class SetupTranslationConfiguration : EditorWindow
{
	private readonly TransfluentEditorWindowMediator _mediator;
	private LanguageList _languages;
	private string defaultLanguageCode = "en-gb";
	private const string SOURCE_GAME_LANGUAGE_KEY = "SOURCE_GAME_LANGUAGE";
	private string sourceLanguageCode;
	IKeyStore store = new EditorKeyStore();

	public SetupTranslationConfiguration()
	{
		_mediator = new TransfluentEditorWindowMediator();
		
		if(string.IsNullOrEmpty(store.get(SOURCE_GAME_LANGUAGE_KEY)))
		{
			store.set(SOURCE_GAME_LANGUAGE_KEY,defaultLanguageCode);
		}
		sourceLanguageCode = store.get(SOURCE_GAME_LANGUAGE_KEY);
		_languages = _mediator.getLanguageList();
	}

	[MenuItem("Window/Setup Tranlsation Configuraiton")]
	public static void Init()
	{
		GetWindow<SetupTranslationConfiguration>();
	}

	public void OnGUI()
	{
		if (_languages == null)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Please connect to the internet to initialize properly");
			if (GUILayout.Button("initialize known languages from internet"))
			{
				_languages = _mediator.getLanguageList();
			}
			EditorGUILayout.EndHorizontal();
			return;
		}
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Select the source language of your game");
		var newLanguage = showLanguageSelector("Language:", _languages.getLangaugeByCode(sourceLanguageCode));
		if (sourceLanguageCode != newLanguage.code)
		{
			store.set(SOURCE_GAME_LANGUAGE_KEY, newLanguage.code);
			sourceLanguageCode = newLanguage.code;
			Debug.Log("NEW SOURCE LANGUAGE CODE:"+sourceLanguageCode);
			
		}
		
		EditorGUILayout.EndHorizontal();
	}

	//TODO: refactor this to a utility
	GameTranslationSet getOrCreateGameTranslationSet(string languageCode)
	{
		var set = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(sourceLanguageCode);
		if(set == null)
		{
			set = ResourceCreator.CreateSO<GameTranslationSet>(GameTranslationGetter.fileNameFromLanguageCode(languageCode));
		}
		return set;
	}	

	public void setAllDestinationLanguagesMenu()
	{
		var sourceLanguageSet = getOrCreateGameTranslationSet(sourceLanguageCode);
		
	}

	public TransfluentLanguage showLanguageSelector(string label,TransfluentLanguage currentLanguageSelection)
	{
		List<string> languageNames = _mediator.getAllLanguageCodes();
		int currentLanguageIndex = 0;
		if(currentLanguageSelection != null)
			currentLanguageIndex = languageNames.IndexOf(currentLanguageSelection.code);
		int newLanguageIndex = EditorGUILayout.Popup(label, currentLanguageIndex, languageNames.ToArray());
		if(newLanguageIndex >= 0 && newLanguageIndex < _languages.languages.Count)
		{
			return _languages.languages[newLanguageIndex];
		}
		else
		{
			return currentLanguageSelection;
		}
		
	}

}