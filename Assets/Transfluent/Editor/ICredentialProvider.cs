using UnityEngine;
using UnityEditor;

namespace transfluent
{
	public interface ICredentialProvider
	{
		string username { get; }
		string password { get; }
	}

	public class FileBasedCredentialProvider : ICredentialProvider
	{
		private readonly string LocationOfTestCredentials = "Assets/TransfluentTests/Editor/Data/loginPassword.txt";
		public string username { get; protected set; }
		public string password { get; protected set; }

		public FileBasedCredentialProvider()
		{
			TextAsset textAsset = AssetDatabase.LoadAssetAtPath(LocationOfTestCredentials, typeof(TextAsset)) as TextAsset;
			string[] lines = textAsset.text.Split(new[] { '\r', '\n' });
			username = lines[0];
			password = lines[1];
		}
	}
	public class EditorKeyCredentialProvicer : ICredentialProvider
	{
		public const string USERNAME_EDITOR_KEY = "TRANSFLUENT_USERNAME_KEY";
		public const string PASSWORD_EDITOR_KEY = "PASSWORD_EDITOR_KEY";
		public string username { get; protected set; }
		public string password { get; protected set; }

		public EditorKeyCredentialProvicer()
		{
			username = EditorPrefs.GetString(USERNAME_EDITOR_KEY);
			password = EditorPrefs.GetString(PASSWORD_EDITOR_KEY);
		}
	}
}