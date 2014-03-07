using System.Collections.Generic;
using UnityEditor;

namespace transfluent.editor
{
	public class SimpleGUIGameWindow : EditorWindow
	{
		private readonly TransfluentEditorWindowMediator _mediator;
		private readonly TransfluentEditorWindow.LoginGUI loginScreen;

		public SimpleGUIGameWindow()
		{
			_mediator = new TransfluentEditorWindowMediator();
			loginScreen = new TransfluentEditorWindow.LoginGUI(_mediator);
		}

		[MenuItem("Window/Transfluent Helper")]
		public static void Init()
		{
			GetWindow<SimpleGUIGameWindow>();
		}

		public void OnGUI()
		{
			if (!_mediator.authIsDone())
			{
				loginScreen.doGUI();
				return;
			}
			EditorGUILayout.BeginHorizontal();
			bool languageChanged = showCurrentLanguage();

			if (_mediator.GetCurrentLanguage() == null)
			{
				return;
			}
			EditorGUILayout.EndHorizontal();
		}

		public bool showCurrentLanguage()
		{
			List<string> languageNames = _mediator.getAllLanguageCodes();
			TransfluentLanguage currentLanguage = _mediator.GetCurrentLanguage();
			int currentLanguageIndex = 0;
			if (currentLanguage != null)
				currentLanguageIndex = languageNames.IndexOf(currentLanguage.code);
			int newLanguageIndex = EditorGUILayout.Popup("Current language", currentLanguageIndex, languageNames.ToArray());
			if (currentLanguageIndex != newLanguageIndex)
			{
				_mediator.setCurrentLanguageFromLanguageCode(languageNames[newLanguageIndex]);
				return true;
			}
			return false;
		}
	}
}