using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace transfluent
{
	public interface ICredentialProvider
	{
		string username { get; }
		string password { get; }
		void save(string newUsername,string newPassword);
		void clear();
	}

	public class FileBasedCredentialProvider : ICredentialProvider
	{
		private const string LocationOfTestCredentials = "Assets/TransfluentTests/Editor/Data/loginPassword.txt";
		public IKeyStore keyStore = new FileLineBasedKeyStore(LocationOfTestCredentials,new List<string>(){"username","password"});
		public string username { get; protected set; }
		public string password { get; protected set; }

		public void save(string newUsername, string newPassword)
		{
			throw new NotImplementedException();
		}

		public void clear()
		{
			throw new NotImplementedException();
		}

		public FileBasedCredentialProvider()
		{
			var textAsset = AssetDatabase.LoadAssetAtPath(LocationOfTestCredentials, typeof(TextAsset)) as TextAsset;
			string[] lines = textAsset.text.Split(new[] { '\r', '\n' });
			username = lines[0];
			password = lines[1];
		}
	}
	public class EditorKeyCredentialProvider : ICredentialProvider
	{
		public IKeyStore keyStore = new EditorKeyStore();
		public const string USERNAME_EDITOR_KEY = "TRANSFLUENT_USERNAME_KEY";
		public const string PASSWORD_EDITOR_KEY = "PASSWORD_EDITOR_KEY";
		public string username { get; protected set; }
		public string password { get; protected set; }

		public void save(string newUsername, string newPassword)
		{
			keyStore.set(USERNAME_EDITOR_KEY, newUsername);
			keyStore.set(PASSWORD_EDITOR_KEY, newPassword);
			username = newUsername;
			password = newPassword;
		}

		public void clear()
		{
			username = password = null;
			keyStore.set(USERNAME_EDITOR_KEY, "");
			keyStore.set(PASSWORD_EDITOR_KEY, "");
		}

		public EditorKeyCredentialProvider()
		{
			username = keyStore.get(USERNAME_EDITOR_KEY);
			password = keyStore.get(PASSWORD_EDITOR_KEY);
		}
	}

	public class CommandLineCredentialProvider : ICredentialProvider
	{
		public IKeyStore keyStore = new CommandLineKeyStore();
		public const string USERNAME_COMMAND_LINE_ARGUMENT = "-USERNAME_COMMAND_LINE_ARGUMENT";
		public const string PASSWORD_COMMAND_LINE_ARGUMENT = "-PASSWORD_COMMAND_LINE_ARGUMENT";
		public string username { get; protected set; }
		public string password { get; protected set; }

		public void save(string newUsername, string newPassword)
		{
			throw new NotImplementedException();
		}

		public void clear()
		{
			throw new NotImplementedException();
		}

		public CommandLineCredentialProvider()
		{
			username = keyStore.get(USERNAME_COMMAND_LINE_ARGUMENT);
			password = keyStore.get(PASSWORD_COMMAND_LINE_ARGUMENT);
		}

	}
}