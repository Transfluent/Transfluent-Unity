using System.Collections.Generic;
using System.Text;
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
	private bool advancedOptions;
	private bool showAllLanguages;

	private TranslationConfigurationSO selectedConfig;
	private string sourceLanguageCode;

	public SetupTranslationConfiguration()
	{
		_mediator = new TransfluentEditorWindowMediator();
	}

	[MenuItem("Translation/Game Configuration", false, 100)]
	public static void Init()
	{
		var window = GetWindow<SetupTranslationConfiguration>();
		window.Show();
	}

	private void Initialize()
	{
		if(EditorApplication.isUpdating || EditorApplication.isCompiling)
			return;
		if(Event.current.type != EventType.Repaint)
			return;
		_languages = _mediator.getLanguageList();
		_allKnownConfigurations = allKnownConfigurations();
		initialized = true;
	}

	private Vector2 scrollwindowPos = Vector2.zero;

	public void OnGUI()
	{
		//NOTE: potential fix for errors while trying to load or create resources while it reloads/compiles the unity editor
		if(!initialized)
		{
			Initialize();
			return;
		}
		scrollwindowPos = GUILayout.BeginScrollView(scrollwindowPos);
		DrawContent();
		GUILayout.EndScrollView();
	}

	public void DrawContent()
	{
		if(!GetLanguagesGUI())
		{
			return;
		}
		if(_allKnownConfigurations.Count == 0)
		{
			createANewConfig();
			if(_allKnownConfigurations.Count == 0) return;
		}

		advancedOptions = EditorGUILayout.Toggle("Advanced Options", advancedOptions);
		if(advancedOptions)
		{
			showAllLanguages = EditorGUILayout.Toggle("Show all langauges, not just simplified list", showAllLanguages);
			SelectAConfig();
			createANewConfig();
		}
		else
		{
			selectedConfig = getOrCreateGameTranslationConfig("");
		}

		if(selectedConfig == null)
		{
			return;
		}
		DisplaySelectedTranslationConfiguration(selectedConfig);

		GUILayout.Space(30);

		if(GUILayout.Button("SHOW MISSING TRANSLATION COUNTS"))
		{
			if(_estimate == null)
			{
				_estimate = new TranslationEstimate(_mediator);
			}
			StringBuilder sb = new StringBuilder();
			foreach(var dest in selectedConfig.destinationLanguages)
			{
				int missing = _estimate.numberOfMissingTranslationsBetweenLanguages(selectedConfig.sourceLanguage,
					dest, selectedConfig.translation_set_group);
				if(missing > 0)
				{
					sb.AppendFormat("Language {0} is missing {1} translations\n", dest.name, missing);
				}
			}
			EditorUtility.DisplayDialog("MISSING", "Missing this many translations:\n" + sb.ToString(), "OK");
		}

		GUILayout.Space(30);
		GUILayout.Label("Account options:");

		if(!userHasSetCredentials())
		{
			GUILayout.Label("Log in to translate your text as well as upload and download your local translations");
			ShowLoginFields();
			//return;
		}
		else
		{
			if(GUILayout.Button("LOG OUT OF ACCOUNT"))
			{
				_mediator.setUsernamePassword("", "");
			}
		}
		ShowUploadDownload();

		DoTranslation();
	}

	private TranslationEstimate _estimate;

	private void DoTranslation()
	{
		//TODO: review estimation algorithm
		GUILayout.Space(30);
		GUILayout.Label("Translate all known language from source to destination languages:");

		if(_estimate == null)
		{
			_estimate = new TranslationEstimate(_mediator);
		}

		_estimate.presentEstimateAndMakeOrder(selectedConfig);
	}

	private void createANewConfig()
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

	private int _selectedSourceLanguageIndex = -1;
	private void DisplaySelectedTranslationConfiguration(TranslationConfigurationSO so)
	{
		List<string> knownLanguageDisplayNames = showAllLanguages ?
			_languages.getListOfIdentifiersFromLanguageList() :
			_languages.getSimplifiedListOfIdentifiersFromLanguageList();

		int sourceLanguageIndex = _selectedSourceLanguageIndex;
		if(sourceLanguageIndex == -1) //if we're initialized, use the last known thing
			sourceLanguageIndex = knownLanguageDisplayNames.IndexOf(so.sourceLanguage.name);
		
		if(sourceLanguageIndex < 0) sourceLanguageIndex = 0;
		EditorGUILayout.LabelField(string.Format("group identifier:{0}", so.translation_set_group));
		EditorGUILayout.LabelField(string.Format("source language:{0}", so.sourceLanguage.name));

		_selectedSourceLanguageIndex = EditorGUILayout.Popup(sourceLanguageIndex, knownLanguageDisplayNames.ToArray());
		if(GUILayout.Button("SET Source to this language: " + knownLanguageDisplayNames[sourceLanguageIndex]))
		{
			so.sourceLanguage = _languages.getLangaugeByName(knownLanguageDisplayNames[_selectedSourceLanguageIndex]);
		}

		EditorGUILayout.LabelField("destination language(s):");
		TransfluentLanguage removeThisLang = null;

		foreach(TransfluentLanguage lang in so.destinationLanguages)
		{
			GUILayout.Space(10);
			EditorGUILayout.LabelField("destination language:" + lang.name);
			if(GUILayout.Button("Remove", GUILayout.Width(100)))
			{
				removeThisLang = lang;
			}
		}
		if(removeThisLang != null)
		{
			so.destinationLanguages.Remove(removeThisLang);
			saveCurrentConfig();
		}

		GUILayout.Space(30);

		newDestinationLanguageIndex = EditorGUILayout.Popup(newDestinationLanguageIndex, knownLanguageDisplayNames.ToArray());

		if(GUILayout.Button("Specify a new Destination Language"))
		{
			TransfluentLanguage lang = _languages.getLangaugeByName(knownLanguageDisplayNames[newDestinationLanguageIndex]);
			if(so.sourceLanguage.id == lang.id)
			{
				EditorUtility.DisplayDialog("Error", "Cannot have the source language be the destination language", "OK", "");
				return;
			}
			foreach(TransfluentLanguage exists in so.destinationLanguages)
			{
				if(exists.id != lang.id) continue;
				EditorUtility.DisplayDialog("Error", "You already have added this language: " + lang.name, "OK", "");
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
			so.QualityToRequest = (OrderTranslation.TranslationQuality)newIndex + 1;
		}
	}

	private bool userHasSetCredentials()
	{
		KeyValuePair<string, string> usernamePassword = _mediator.getUserNamePassword();

		return (!string.IsNullOrEmpty(usernamePassword.Key) && !string.IsNullOrEmpty(usernamePassword.Value));
	}

	private TransfluentEditorWindow.LoginGUI _loginGui;

	private void ShowLoginFields()
	{
		if(!userHasSetCredentials())
		{
			if(_loginGui == null)
			{
				_loginGui = new TransfluentEditorWindow.LoginGUI(_mediator);
			}
			_loginGui.doGUI();
		}
	}

	private void ShowUploadDownload()
	{
		var so = selectedConfig;

		GUILayout.Space(30);
		if(GUILayout.Button("Upload all local text"))
		{
			var languageCodeList = new List<string> { so.sourceLanguage.code };
			so.destinationLanguages.ForEach((TransfluentLanguage lang) => { languageCodeList.Add(lang.code); });

			DownloadAllGameTranslations.uploadTranslationSet(languageCodeList, so.translation_set_group);
		}
		if(GUILayout.Button("Download all translations"))
		{
			if(EditorUtility.DisplayDialog("Downloading", "Downloading will overwrite any local changes to existing keys do you want to proceed?", "OK", "Cancel / Let me upload first"))
			{
				var languageCodeList = new List<string> { so.sourceLanguage.code };
				so.destinationLanguages.ForEach((TransfluentLanguage lang) => { languageCodeList.Add(lang.code); });
				DownloadAllGameTranslations.downloadTranslationSetsFromLanguageCodeList(languageCodeList, so.translation_set_group);
			}
		}
	}

	private void SelectAConfig()
	{
		EditorGUILayout.LabelField("Select a config");
		var knownConfigNames = new List<string>();
		int selectedIndex = _allKnownConfigurations.Count > 0 ? 1 : 0;
		knownConfigNames.Add("No Config");

		foreach(TranslationConfigurationSO so in _allKnownConfigurations)
		{
			knownConfigNames.Add("Group:" + so.translation_set_group);
		}

		if(selectedConfig != null)
		{
			selectedIndex = knownConfigNames.IndexOf("Group:" + selectedConfig.translation_set_group);
		}

		int newIndex = EditorGUILayout.Popup(selectedIndex, knownConfigNames.ToArray());
		if(newIndex != 0)
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
	private List<TranslationConfigurationSO> allKnownConfigurations()
	{
		var list = new List<TranslationConfigurationSO>();
		list.AddRange(Resources.LoadAll<TranslationConfigurationSO>(""));
		return list;
	}
}