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

		
	}

	[MenuItem("Window/makeConfig")]
	public static void makeMenuItem()
	{
		getOrCreateGameTranslationConfig("");
		getOrCreateGameTranslationConfig("testGroup");
	}

	private bool initialized = false;

	void Initialize()
	{
		if (EditorApplication.isUpdating || EditorApplication.isCompiling)
			return;
		_languages = _mediator.getLanguageList();
		_allKnownConfigurations = allKnownConfigurations();
		initialized = true;
	}


	private string groupidDisplayed="";
	public void OnGUI()
	{
		//NOTE: potential fix for errors while trying to load or create resources while it reloads/compiles the unity editor
		if (!initialized)
		{
			Initialize();
			return;
		}

		if(!GetLanguagesGUI())
		{
			return;
		}
		if(_allKnownConfigurations.Count == 0)
		{
			GUILayout.Label("Group Id:");
			groupidDisplayed = GUILayout.TextField(groupidDisplayed);
			if(GUILayout.Button("Create a new Config"))
			{
				if(groupidExists(groupidDisplayed))
				{
					EditorUtility.DisplayDialog("Error", "Group ID Exists, cannot create again", "OK", "");
					return;
				}
				TranslationConfigurationSO config = getOrCreateGameTranslationConfig(groupidDisplayed); //TODO: handle other config spaces
				config.translation_set_group = groupidDisplayed;
 
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

	private int newSourceLanguageIndex = 0;
	private int newDestinationLanguageIndex = 0;
	void DisplaySelectedTranslationConfiguration(TranslationConfigurationSO so)
	{
		List<string> knownLanguageDisplayNames = _languages.getListOfIdentifiersFromLanguageList();

		EditorGUILayout.LabelField("group identifier:" + so.translation_set_group);
		EditorGUILayout.LabelField("source language:" + so.sourceLanguage.name);
		List<string> identifiers = _languages.getListOfIdentifiersFromLanguageList();
		newSourceLanguageIndex = EditorGUILayout.Popup(newSourceLanguageIndex, knownLanguageDisplayNames.ToArray());
		if(GUILayout.Button("SET Source to this language" + knownLanguageDisplayNames[newSourceLanguageIndex]))
		{
			so.sourceLanguage = _languages.getLangaugeByName(knownLanguageDisplayNames[newSourceLanguageIndex]);
		}

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

		

		newDestinationLanguageIndex = EditorGUILayout.Popup(newDestinationLanguageIndex, knownLanguageDisplayNames.ToArray());

		if(GUILayout.Button("Create a new Destination Language"))
		{
			TransfluentLanguage lang = _languages.languages[newDestinationLanguageIndex];
			if (so.sourceLanguage.id == lang.id)
			{
				EditorUtility.DisplayDialog("Error", "Cannot have the source language be the destination language", "OK", "");
				return;
			}
			foreach (TransfluentLanguage exists in so.destinationLanguages)
			{
				if (exists.id == lang.id)
				{
					EditorUtility.DisplayDialog("Error", "You already have added this language", "OK", "");
					return;
				}
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
			knownConfigNames.Add("Group:"+so.translation_set_group);
		}
		if(selectedConfig != null)
		{
			selectedIndex = knownConfigNames.IndexOf("Group:" + selectedConfig.translation_set_group);
		}

		int newIndex = EditorGUILayout.Popup(selectedIndex, knownConfigNames.ToArray());
		if(newIndex != 0)
		{
			selectedConfig = _allKnownConfigurations[newIndex-1];
		}
		else
		{
			selectedConfig = null;
		}
	}

	bool groupidExists(string groupid)
	{
		return
			!(_allKnownConfigurations.TrueForAll(
				(TranslationConfigurationSO so) => { return so.translation_set_group != groupid; }));
	}
	static string configFileNameFromGroupID(string groupid)
	{
		return "TranslationConfigurationSO_" + groupid;
	}
	static public TranslationConfigurationSO getOrCreateGameTranslationConfig(string groupid)
	{
		string fileName = configFileNameFromGroupID(groupid);
		var config =
			ResourceLoadFacade.LoadResource<TranslationConfigurationSO>(fileName);

		if(config == null)
		{
			config = ResourceCreator.CreateSO<TranslationConfigurationSO>(fileName);
		}
		config.translation_set_group = groupid;
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