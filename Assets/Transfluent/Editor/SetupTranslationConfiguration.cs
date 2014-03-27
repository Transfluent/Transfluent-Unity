using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using Pathfinding.Serialization.JsonFx;
using transfluent;
using transfluent.editor;
using UnityEditor;
using UnityEngine;

public class SetupTranslationConfiguration : EditorWindow
{
	private readonly TransfluentEditorWindowMediator _mediator;
	private List<TranslationConfigurationSO> _allKnownConfigurations;
	private LanguageList _languages;
	private string groupidDisplayed = "";
	private bool initialized;
	private int newDestinationLanguageIndex;

	private TranslationConfigurationSO selectedConfig;
	private string sourceLanguageCode;

	public SetupTranslationConfiguration()
	{
		_mediator = new TransfluentEditorWindowMediator();
	}

	[MenuItem("Transfluent/Game Configuration")]
	public static void Init()
	{
		GetWindow<SetupTranslationConfiguration>();
	}

	private void Initialize()
	{
		if (EditorApplication.isUpdating || EditorApplication.isCompiling)
			return;
		_languages = _mediator.getLanguageList();
		_allKnownConfigurations = allKnownConfigurations();
		initialized = true;
	}

	public void OnGUI()
	{
		//NOTE: potential fix for errors while trying to load or create resources while it reloads/compiles the unity editor
		if (!initialized)
		{
			Initialize();
			return;
		}

		if (!GetLanguagesGUI())
		{
			return;
		}
		if (_allKnownConfigurations.Count == 0)
		{
			createANewConfig();
			if (_allKnownConfigurations.Count == 0) return;
		}

		SelectAConfig();
		createANewConfig();
		if (selectedConfig == null)
		{
			return;
		}
		DisplaySelectedTranslationConfiguration(selectedConfig);

		DoTranslation();
	}

	private void DoTranslation()
	{
		//TODO: estimate all remaining text cost
		GUILayout.Space(30);
		GUILayout.Label("Translate all things:");
		_mediator.doAuth();
		string authToken = _mediator.getCurrentAuthToken();
		if(string.IsNullOrEmpty(authToken))
			throw new Exception("Auth token is null");
		var estimator = new TranslationEstimate(authToken);
		estimator.testThing(selectedConfig);
	}

	private void createANewConfig()
	{
		GUILayout.Label("Group Id:");
		groupidDisplayed = GUILayout.TextField(groupidDisplayed);
		if (GUILayout.Button("Create a new Config"))
		{
			if (groupidExists(groupidDisplayed))
			{
				EditorUtility.DisplayDialog("Error", "Group ID Exists, cannot create again", "OK", "");
				return;
			}
			TranslationConfigurationSO config = getOrCreateGameTranslationConfig(groupidDisplayed);
			saveCurrentConfig();

			_allKnownConfigurations.Add(config);

			selectedConfig = config;
		}
	}

	private void saveCurrentConfig()
	{
		TranslationConfigurationSO config = getOrCreateGameTranslationConfig(groupidDisplayed);
		config.translation_set_group = groupidDisplayed;
		EditorUtility.SetDirty(config);
		AssetDatabase.SaveAssets();
	}

	private void DisplaySelectedTranslationConfiguration(TranslationConfigurationSO so)
	{
		List<string> knownLanguageDisplayNames = _languages.getListOfIdentifiersFromLanguageList();
		int sourceLanguageIndex = knownLanguageDisplayNames.IndexOf(so.sourceLanguage.name);

		EditorGUILayout.LabelField("group identifier:" + so.translation_set_group);
		EditorGUILayout.LabelField("source language:" + so.sourceLanguage.name);

		sourceLanguageIndex = EditorGUILayout.Popup(sourceLanguageIndex, knownLanguageDisplayNames.ToArray());
		if (GUILayout.Button("SET Source to this language" + knownLanguageDisplayNames[sourceLanguageIndex]))
		{
			so.sourceLanguage = _languages.getLangaugeByName(knownLanguageDisplayNames[sourceLanguageIndex]);
		}

		EditorGUILayout.LabelField("destination language(s):");
		TransfluentLanguage removeThisLang = null;

		foreach (TransfluentLanguage lang in so.destinationLanguages)
		{
			GUILayout.Space(10);
			EditorGUILayout.LabelField("destination language:" + lang.name);
			if (GUILayout.Button("Remove", GUILayout.Width(100)))
			{
				removeThisLang = lang;
			}
		}
		if (removeThisLang != null)
		{
			so.destinationLanguages.Remove(removeThisLang);
			saveCurrentConfig();
		}

		GUILayout.Space(30);

		newDestinationLanguageIndex = EditorGUILayout.Popup(newDestinationLanguageIndex, knownLanguageDisplayNames.ToArray());

		if (GUILayout.Button("Create a new Destination Language"))
		{
			TransfluentLanguage lang = _languages.languages[newDestinationLanguageIndex];
			if (so.sourceLanguage.id == lang.id)
			{
				EditorUtility.DisplayDialog("Error", "Cannot have the source language be the destination language", "OK", "");
				return;
			}
			foreach (TransfluentLanguage exists in so.destinationLanguages)
			{
				if (exists.id != lang.id) continue;
				EditorUtility.DisplayDialog("Error", "You already have added this language", "OK", "");
				return;
			}

			so.destinationLanguages.Add(lang);

			GUILayout.Space(10);
			
			saveCurrentConfig();
		}
		GUILayout.Space(10);
		var translationQualityStrings = new List<string>()
			{
				OrderTranslation.TranslationQuality.NATIVE_SPEAKER.ToString(),
				OrderTranslation.TranslationQuality.PROFESSIONAL_TRANSLATOR.ToString(),
				OrderTranslation.TranslationQuality.PAIR_OF_TRANSLATORS.ToString(),
			};
		int currentIndex = translationQualityStrings.IndexOf(so.QualityToRequest.ToString());
		int newIndex = EditorGUILayout.Popup("Desired Translation Quality:", currentIndex, translationQualityStrings.ToArray());
		if(newIndex != currentIndex)
		{
			so.QualityToRequest = (OrderTranslation.TranslationQuality)newIndex+1;
		}
	}

	private void SelectAConfig()
	{
		EditorGUILayout.LabelField("Select a config");
		var knownConfigNames = new List<string>();
		int selectedIndex = _allKnownConfigurations.Count > 0 ? 1 : 0;
		knownConfigNames.Add("No Config");

		foreach (TranslationConfigurationSO so in _allKnownConfigurations)
		{
			knownConfigNames.Add("Group:" + so.translation_set_group);
		}

		if (selectedConfig != null)
		{
			selectedIndex = knownConfigNames.IndexOf("Group:" + selectedConfig.translation_set_group);
		}

		int newIndex = EditorGUILayout.Popup(selectedIndex, knownConfigNames.ToArray());
		if (newIndex != 0)
		{
			selectedConfig = _allKnownConfigurations[newIndex - 1];
		}
		else
		{
			selectedConfig = null;
		}
	}

	private bool groupidExists(string groupid)
	{
		return
			!(_allKnownConfigurations.TrueForAll(
				(TranslationConfigurationSO so) => { return so.translation_set_group != groupid; }));
	}

	public static TranslationConfigurationSO getOrCreateGameTranslationConfig(string groupid)
	{
		string fileName = ResourceLoadFacade.TranslationConfigurationSOFileNameFromGroupID(groupid);
		TranslationConfigurationSO config =
			ResourceLoadFacade.LoadConfigGroup(groupid) ??
			ResourceCreator.CreateSO<TranslationConfigurationSO>(fileName);

		config.translation_set_group = groupid;
		return config;
	}

	private bool GetLanguagesGUI()
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
		}
		return _languages != null;
	}

	//find all possible configuration SO's
	private List<TranslationConfigurationSO> allKnownConfigurations()
	{
		var list = new List<TranslationConfigurationSO>();
		list.AddRange(Resources.LoadAll<TranslationConfigurationSO>(""));
		return list;
	}
}