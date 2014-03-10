using System.Collections.Generic;
using transfluent;
using transfluent.editor;
using UnityEngine;
using UnityEditor;

public class SetupTranslationConfiguration2 : EditorWindow
{
	[MenuItem("Window/SetupTranslationConfiguration2")]
	public static void Init()
	{
		GetWindow<SetupTranslationConfiguration2>();
	}

	private readonly TransfluentEditorWindowMediator _mediator;
	private LanguageList _languages;
	private string sourceLanguageCode;
	private TranslationConfigurationSO selectedConfig;

	private List<TranslationConfigurationSO> _allKnownConfigurations;
	public SetupTranslationConfiguration2()
	{
		_mediator = new TransfluentEditorWindowMediator();

		//config = CreateInstance<>()

		//= ResourceCreator.CreateSO<GameTranslationSet>(GameTranslationGetter.fileNameFromLanguageCode(languageCode));

		_languages = _mediator.getLanguageList();
		_allKnownConfigurations = allKnownConfigurations();
	}

	private string groupidDisplayed;
	public void OnGUI()
	{
		if(!GetLanguagesGUI())
		{
			return;
		}
		if(_allKnownConfigurations.Count == 0)
		{
			groupidDisplayed = GUILayout.TextField("Group id");
			if(GUILayout.Button("Create a new Config"))
			{
				if(groupidExists(groupidDisplayed))
				{
					EditorUtility.DisplayDialog("Error", "Group ID Exists, cannot create again", "OK", "");
					return;
				}
				TranslationConfigurationSO config = getOrCreateGameTranslationConfig(groupidDisplayed); //TODO: handle other config spaces
				_allKnownConfigurations.Add(config);
			}
			if(_allKnownConfigurations.Count == 0) return;
		}

		SelectAConfig();
		if(selectedConfig == null)
		{
			return;
		}
		DisplaySelectedTranslationConfiguration(selectedConfig);

	}

	private int newDestinationLanguageIndex = 0;
	void DisplaySelectedTranslationConfiguration(TranslationConfigurationSO so)
	{
		EditorGUILayout.LabelField("group identifier:" + so.translation_set_group);
		EditorGUILayout.LabelField("source language:" + so.sourceLanguage.name);

		EditorGUILayout.LabelField("destination language(s):");
		TransfluentLanguage removeThisLang = null;
		foreach(TransfluentLanguage lang in so.destinationLanguages)
		{
			EditorGUILayout.LabelField("destination language:" + lang.name);
			if(GUILayout.Button("Remove"))
			{
				removeThisLang = lang;
			}
		}
		if(removeThisLang != null)
		{
			so.destinationLanguages.Remove(removeThisLang);
		}

		List<string> knownLanguageDisplayNames = _languages.getListOfIdentifiersFromLanguageList();

		newDestinationLanguageIndex = EditorGUILayout.Popup(newDestinationLanguageIndex, knownLanguageDisplayNames.ToArray());

		if(GUILayout.Button("Create a new Destination Language"))
		{
			TransfluentLanguage lang = _languages.languages[newDestinationLanguageIndex];
			if(!so.destinationLanguages.TrueForAll((TransfluentLanguage exists) => { return exists.id == lang.id; }))
			{
				EditorUtility.DisplayDialog("Error", "You already have added this language", "OK", "");
				return;
			}
			so.destinationLanguages.Add(lang);
		}

	}


	void SelectAConfig()
	{
		EditorGUILayout.LabelField("Select a config");
		List<string> knownConfigNames = new List<string>();
		int selectedIndex = 0;
		knownConfigNames.Add("No Config");
		foreach(TranslationConfigurationSO so in _allKnownConfigurations)
		{
			knownConfigNames.Add(so.translation_set_group);
		}
		if(selectedConfig != null)
		{
			selectedIndex = knownConfigNames.IndexOf(selectedConfig.translation_set_group);
		}

		int newIndex = EditorGUILayout.Popup(selectedIndex, knownConfigNames.ToArray());
		if(newIndex != 0)
		{
			selectedConfig = _allKnownConfigurations[newIndex];
		}
		else
		{
			selectedConfig = null;
		}
	}

	bool groupidExists(string groupid)
	{
		return
			(_allKnownConfigurations.TrueForAll(
				(TranslationConfigurationSO so) => { return so.translation_set_group != groupid; }));
	}
	string configFileNameFromGroupID(string groupid)
	{
		return "TranslationConfigurationSO_" + groupid;
	}
	TranslationConfigurationSO getOrCreateGameTranslationConfig(string groupid)
	{
		string fileName = configFileNameFromGroupID(groupid);
		var config =
			ResourceLoadFacade.LoadResource<TranslationConfigurationSO>(fileName);

		if(config == null)
		{
			config = ResourceCreator.CreateSO<TranslationConfigurationSO>(fileName);
		}
		return config;
	}

	bool GetLanguagesGUI()
	{
		if(_languages == null)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Please connect to the internet to initialize properly");
			if(GUILayout.Button("initialize known languages from internet"))
			{
				_languages = _mediator.getLanguageList();
			}
			EditorGUILayout.EndHorizontal();
		}
		return _languages != null;
	}

	//find all possible configuration SO's
	List<TranslationConfigurationSO> allKnownConfigurations()
	{
		var list = new List<TranslationConfigurationSO>();
		list.AddRange(Resources.LoadAll<TranslationConfigurationSO>(""));
		return list;
	}
}