﻿using System;
using UnityEngine;
using UnityEditor;

namespace transfluent
{
	public interface ICredentialProvider
	{
		string username { get; }
		string password { get; }
		void save();
	}

	public class FileBasedCredentialProvider : ICredentialProvider
	{
		private readonly string LocationOfTestCredentials = "Assets/TransfluentTests/Editor/Data/loginPassword.txt";
		public string username { get; protected set; }
		public string password { get; protected set; }
		public void save()
		{
			throw new NotImplementedException();
		}

		public FileBasedCredentialProvider()
		{
			TextAsset textAsset = AssetDatabase.LoadAssetAtPath(LocationOfTestCredentials, typeof(TextAsset)) as TextAsset;
			string[] lines = textAsset.text.Split(new[] { '\r', '\n' });
			username = lines[0];
			password = lines[1];
		}
	}
	public class EditorKeyCredentialProvider : ICredentialProvider
	{
		public const string USERNAME_EDITOR_KEY = "TRANSFLUENT_USERNAME_KEY";
		public const string PASSWORD_EDITOR_KEY = "PASSWORD_EDITOR_KEY";
		public string username { get; protected set; }
		public string password { get; protected set; }

		public void save()
		{
			EditorPrefs.SetString(USERNAME_EDITOR_KEY,username);
			EditorPrefs.SetString(PASSWORD_EDITOR_KEY, password);
		}

		public EditorKeyCredentialProvider()
		{
			username = EditorPrefs.GetString(USERNAME_EDITOR_KEY);
			password = EditorPrefs.GetString(PASSWORD_EDITOR_KEY);
		}
	}

	public class CommandLineCredentialProvider : ICredentialProvider
	{
		public const string USERNAME_COMMAND_LINE_ARGUMENT = "-USERNAME_COMMAND_LINE_ARGUMENT";
		public const string PASSWORD_COMMAND_LINE_ARGUMENT = "-PASSWORD_COMMAND_LINE_ARGUMENT";
		public string username { get; protected set; }
		public string password { get; protected set; }

		public void save()
		{
			throw new NotImplementedException();
		}

		public CommandLineCredentialProvider()
		{
			username = getBuildFlagValue(USERNAME_COMMAND_LINE_ARGUMENT);
			password = getBuildFlagValue(PASSWORD_COMMAND_LINE_ARGUMENT);
		}

		private string getBuildFlagValue(string buildFlag)
		{
			try
			{
				string[] args = Environment.GetCommandLineArgs();
				foreach(string arg in args)
				{
					if(arg.Contains(buildFlag))
					{
						string buildFlagValue = arg.Replace(buildFlag, "");

						return buildFlagValue;
					}
				}
			}
			catch(Exception e)
			{
				Debug.LogError("Error getting build flag value from command line;"+ e);
				throw;
			}
			return null; //not from command line
		}
	}
}