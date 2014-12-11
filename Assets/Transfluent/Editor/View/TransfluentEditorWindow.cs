using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace transfluent.editor
{
	public class TransfluentEditorWindow : EditorWindow
	{
		private readonly TransfluentEditorWindowMediator _mediator;

		private readonly LoginGUI loginScreen;
		private readonly TextsGUI textGui;

		public TransfluentEditorWindow()
		{
			_mediator = new TransfluentEditorWindowMediator();
			loginScreen = new LoginGUI(_mediator);
			textGui = new TextsGUI(_mediator);
		}

		[MenuItem("Translation/View My Transfluent Account")]
		public static void Init()
		{
			var window = GetWindow<TransfluentEditorWindow>();
			window.Show();
		}

		private void OnGUI()
		{
			if(!_mediator.authIsDone())
			{
				loginScreen.doGUI();
				return;
			}
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("reset auth"))
			{
				_mediator.invalidateAuth();
				loginScreen.GetCredentialsFromDataStore();
			}
			bool languageChanged = showCurrentLanguage();

			if(_mediator.GetCurrentLanguage() == null)
			{
				return;
			}
			EditorGUILayout.EndHorizontal();

			if(languageChanged)
				textGui.Refresh();

			textGui.doGUI();
		}

		private bool showAllLanugages;

		public bool showCurrentLanguage()
		{
			var languageList = _mediator.getLanguageList();

			showAllLanugages = EditorGUILayout.Toggle("Show all langauges, not just simplified list", showAllLanugages);
			var languageNames = showAllLanugages ?
						languageList.getListOfIdentifiersFromLanguageList() :
						languageList.getSimplifiedListOfIdentifiersFromLanguageList();

			TransfluentLanguage currentLanguage = _mediator.GetCurrentLanguage();
			int currentLanguageIndex = -1;
			if(currentLanguage != null)
				currentLanguageIndex = languageNames.IndexOf(currentLanguage.name);

			int newLanguageIndex = EditorGUILayout.Popup("Current language", currentLanguageIndex, languageNames.ToArray());
			if(currentLanguageIndex != newLanguageIndex)
			{
				currentLanguage = languageList.getLangaugeByName(languageNames[newLanguageIndex]);
				_mediator.setCurrentLanguage(currentLanguage);
				return true;
			}
			return false;
		}

		public class LoginGUI
		{
			private readonly TransfluentEditorWindowMediator _mediator;
			private string currentPassword;
			private string currentUsername;

			public LoginGUI(TransfluentEditorWindowMediator mediator)
			{
				_mediator = mediator;
				GetCredentialsFromDataStore();
			}

			public void GetCredentialsFromDataStore()
			{
				KeyValuePair<string, string> usernamePassword = _mediator.getUserNamePassword();
				currentUsername = usernamePassword.Key;
				currentPassword = usernamePassword.Value;
			}

			public void doGUI()
			{
				EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("<color=blue>Don't have an account? In-editor translations done by professional translators!</color>", new GUIStyle() { richText = true }))
				{
					Application.OpenURL("https://www.transfluent.com/register/");
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				currentUsername = EditorGUILayout.TextField("username", currentUsername);
				currentPassword = EditorGUILayout.PasswordField("password", currentPassword);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();

				//if(GUILayout.Button("save"))
				//{
				//	_mediator.setUsernamePassword(currentUsername, currentPassword);
				//}
				if(GUILayout.Button("login"))
				{
					if(_mediator.doAuth(currentUsername, currentPassword))
					{
						_mediator.setUsernamePassword(currentUsername, currentPassword);
					}	
				}
				EditorGUILayout.EndHorizontal();
			}
		}

		public class TextsGUI
		{
			private readonly TransfluentEditorWindowMediator _mediator;
			private readonly List<TransfluentTranslation> dirtyTranslations = new List<TransfluentTranslation>();
			private List<TransfluentTranslation> _translations;
			private string knownTexts;
			public List<TransfluentTranslation> newTranslations = new List<TransfluentTranslation>();
			private double secondsSinceLastGotAllTexts;

			public TextsGUI(TransfluentEditorWindowMediator mediator)
			{
				_mediator = mediator;
			}

			public void Refresh()
			{
				double timeInSecondsSinceUnityStartedUp = EditorApplication.timeSinceStartup;
				if((timeInSecondsSinceUnityStartedUp - secondsSinceLastGotAllTexts) < 1)
				{
					//ignore button spam
					return;
				}
				secondsSinceLastGotAllTexts = timeInSecondsSinceUnityStartedUp;
				InternalRefresh();
			}

			private void InternalRefresh()
			{
				dirtyTranslations.Clear();

				_translations = _mediator.knownTextEntries();

				newTranslations.Clear();
			}

			private void displayTranslation(TransfluentTranslation translation)
			{
				string textAtStart = translation.text;
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("key:", translation.text_id);
				string textAfterDisplaying = EditorGUILayout.TextField("value", textAtStart);
				EditorGUILayout.TextField("groupid", translation.group_id);
				EditorGUILayout.EndHorizontal();
				if(textAtStart != textAfterDisplaying)
				{
					if(!dirtyTranslations.Contains(translation))
						dirtyTranslations.Add(translation);
				}
			}

			private void displayTranslationAndAllowEntireThingToBeModified(TransfluentTranslation translation)
			{
				EditorGUILayout.BeginHorizontal();
				translation.text_id = EditorGUILayout.TextField("key", translation.text_id);
				translation.text = EditorGUILayout.TextField("value", translation.text);
				translation.group_id = EditorGUILayout.TextField("groupid", translation.group_id);
				EditorGUILayout.EndHorizontal();
			}

			private Vector2 translationScrollPosition = Vector2.zero;

			public void doGUI()
			{
				if(_translations == null) Refresh();
				if(_translations == null) return;
				translationScrollPosition = EditorGUILayout.BeginScrollView(translationScrollPosition);
				foreach(TransfluentTranslation translation in _translations)
				{
					if(newTranslations.Contains(translation))
					{
						displayTranslationAndAllowEntireThingToBeModified(translation);
					}
					else
					{
						EditorGUILayout.BeginHorizontal();
						displayTranslation(translation);
						EditorGUILayout.EndHorizontal();
					}
				}
				EditorGUILayout.EndScrollView();

				EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("Create New Translation"))
				{
					var newTranslation = new TransfluentTranslation
					{
						language = _mediator.GetCurrentLanguage()
					};
					_translations.Add(newTranslation);
					newTranslations.Add(newTranslation);
					dirtyTranslations.Add(newTranslation);
				}

				if(GUILayout.Button("send changes"))
				{
					_mediator.SetText(dirtyTranslations);
					dirtyTranslations.Clear();
					newTranslations.Clear();
					InternalRefresh();
				}
				EditorGUILayout.EndHorizontal();
				//EditorGUILayout.TextField("Known keys", knownTexts,GUILayout.ExpandHeight(true));
			}
		}
	}
}