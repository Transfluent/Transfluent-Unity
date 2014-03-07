using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace transfluent.editor
{
	public class FileBasedCredentialProvider : ICredentialProvider
	{
		private const string LocationOfTestCredentials = "Assets/TransfluentTests/Editor/Data/loginPassword.txt";
		public IKeyStore keyStore = new FileLineBasedKeyStore(LocationOfTestCredentials, new List<string>() { "username", "password" });
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
}