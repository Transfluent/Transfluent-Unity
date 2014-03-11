using UnityEditor;
using UnityEngine;
using System.Collections;

namespace transfluent
{
	public class ResourceLoadFacade
	{
		public static LanguageList getLanguageList()
		{
			try
			{
				return Resources.Load<LanguageListSO>("LanguageList").list;
			}
			catch
			{
				return null;
			}
			
		}
		public static string TranslationConfigurationSOFileNameFromGroupID(string groupid)
		{
			return "TranslationConfigurationSO_" + groupid;
		}

		[MenuItem("asink/testthing")]
		public static void testLoadConfigGroup()
		{
			Debug.Log("location:"+TranslationConfigurationSOFileNameFromGroupID(""));
			var config = LoadConfigGroup("");
			Debug.Log("Config is null:"+(config == null));

		}

		public static TranslationConfigurationSO LoadConfigGroup(string configGroup)
		{
			return LoadResource<TranslationConfigurationSO>(TranslationConfigurationSOFileNameFromGroupID(configGroup));
		}

		public static T LoadResource<T>(string path) where T : UnityEngine.Object
		{
			return Resources.Load<T>(path);
		}

	}
}

