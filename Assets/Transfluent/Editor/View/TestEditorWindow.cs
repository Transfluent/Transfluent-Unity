using UnityEditor;
using UnityEngine;

namespace transfluent.editor
{
	public class TestEditorWindow : EditorWindow
	{
		private readonly TestEditorWindowMediator _mediator;
		[MenuItem("Window/Transfluent Helper")]
		static void Init()
		{
			GetWindow<TestEditorWindow>();
		}

		public TestEditorWindow()
		{
			_mediator = new TestEditorWindowMediator();
			loginScreen = new LoginGUI(_mediator);
		}

		private readonly LoginGUI loginScreen;
		void OnGUI()
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


			EditorGUILayout.EndHorizontal();
		}

		public class TextsGUI
		{
			private readonly TestEditorWindowMediator _mediator;
			private string knownTexts;

			private double secondsSinceLastGotAllTexts;
			public TextsGUI(TestEditorWindowMediator mediator)
			{
				_mediator = mediator;
				Refresh();
			}

			void Refresh()
			{
				double timeInSecondsSinceUnityStartedUp = EditorApplication.timeSinceStartup;
				if((timeInSecondsSinceUnityStartedUp - secondsSinceLastGotAllTexts) < 1)
				{
					//spamming it.. ignoringr
					return;
				}
				secondsSinceLastGotAllTexts = timeInSecondsSinceUnityStartedUp;
				// _mediator.getAllKnownTextEntries();
			}

		}


		public class LoginGUI
		{
			private readonly TestEditorWindowMediator _mediator;
			private string currentUsername;
			private string currentPassword;

			public LoginGUI(TestEditorWindowMediator mediator)
			{
				_mediator = mediator;
				GetCredentialsFromDataStore();
			}

			public void GetCredentialsFromDataStore()
			{
				var usernamePassword = _mediator.getUserNamePassword();
				currentUsername = usernamePassword.Key;
				currentPassword = usernamePassword.Value;
			}
			public void doGUI()
			{
				EditorGUILayout.BeginHorizontal();
				currentUsername = EditorGUILayout.TextField("username", currentUsername);
				currentPassword = EditorGUILayout.TextField("password", currentPassword);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();

				if(GUILayout.Button("save"))
				{
					_mediator.setUsernamePassword(currentUsername, currentPassword);
				}
				if(GUILayout.Button("authenticate"))
				{
					if(_mediator.doAuth(currentUsername, currentPassword))
					{
						_mediator.setUsernamePassword(currentUsername, currentPassword);
					}
				}
				EditorGUILayout.EndHorizontal();
			}

		}

	}


}
