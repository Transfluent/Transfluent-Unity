using System;
using System.Collections.Generic;
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
		private readonly Dictionary<string, string> store = new Dictionary<string, string>();

		public string get(string key)
		{
			return store[key];
		}

		public void set(string key, string value)
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
			if (other == null) return false;
			foreach (var kvp in store)
			{
				if (other.get(kvp.Key) != kvp.Value)
				{
					return false;
				}
			}
			return true;
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
			PlayerPrefs.SetString(key, value);
		}
	}

	

	public class CommandLineKeyStore : IKeyStore
	{
		public string get(string key)
		{
			return getBuildFlagValue(key);
		}

		public void set(string key, string value)
		{
			throw new NotImplementedException();
		}

		private string getBuildFlagValue(string buildFlag)
		{
			try
			{
				string[] args = Environment.GetCommandLineArgs();
				foreach (string arg in args)
				{
					if (arg.Contains(buildFlag))
					{
						string buildFlagValue = arg.Replace(buildFlag, "");

						return buildFlagValue;
					}
				}
			}
			catch (Exception e)
			{
				Debug.LogError("Error getting build flag value from command line;" + e);
				throw;
			}
			return null; //not from command line
		}
	}
}