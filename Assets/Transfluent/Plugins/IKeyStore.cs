using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEngine;

namespace transfluent
{
	public interface IKeyStore
	{
		string get(string key);
		void set(string key, string value);
	}

	public class InMemoryKeyStore : IKeyStore
	{
		Dictionary<string,string> store = new Dictionary<string, string>();

		public string get(string key)
		{
			return store[key];
		}

		public void set(string key,string value)
		{
			if (!store.ContainsKey(key))
			{
				store.Add(key, value);
			}
			else
			{
				store[key] = value;
			}
		}

		//we can't currently verify that the other set has more values than me, but that's ok for all current uses
		public bool otherDictionaryIsEqualOrASuperset(IKeyStore other)
		{
			if(other == null) return false;
			foreach (KeyValuePair<string,string> kvp in store)
			{
				if (other.get(kvp.Key) != kvp.Value)
				{
					return false;
				}
			}
			return true;
		}
	}

	public class EditorKeyStore : IKeyStore
	{
		public string get(string key)
		{
			return EditorPrefs.GetString(key);
		}

		public void set(string key,string value)
		{
			EditorPrefs.SetString(key, value);
		}
	}

	public class PlayerPrefsKeyStore : IKeyStore
	{
		public string get(string key)
		{
			return PlayerPrefs.GetString(key);
		}

		public void set(string key, string value)
		{
			PlayerPrefs.SetString(key,value);
		}
	}

	public class FileLineBasedKeyStore : IKeyStore
	{
		private string[] lines;
		private List<string> _keyMap;

		public FileLineBasedKeyStore(StreamReader reader, List<string> keyMap)
		{
			var text = reader.ReadToEnd();
			_keyMap = keyMap;
			init(text);
		}
		public FileLineBasedKeyStore(string fileName, List<string> keyMap )
		{
			_keyMap = keyMap;
			init(File.ReadAllText(fileName));
		}

		public void init(string text)
		{
			lines = text.Split(new[] { '\r', '\n' });
			if(_keyMap.Count < lines.Length)
			{
				throw new FileLoadException("More keys requested than there were lines in the file");
			}
		}

		public string get(string key)
		{
			if(_keyMap.Contains(key) == false)
			{
				return "";
			}
			//we checked that these indexes aren't invalid up front
			return lines[_keyMap.IndexOf(key)];
		}

		public void set(string key, string value)
		{
			lines[_keyMap.IndexOf(key)] = value;
		}
	}
	public class CommandLineKeyStore : IKeyStore
	{
		public CommandLineKeyStore()
		{
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
				Debug.LogError("Error getting build flag value from command line;" + e);
				throw;
			}
			return null; //not from command line
		}

		public string get(string key)
		{
			return getBuildFlagValue(key);
		}

		public void set(string key, string value)
		{
			throw new NotImplementedException();
		}
	}
}
