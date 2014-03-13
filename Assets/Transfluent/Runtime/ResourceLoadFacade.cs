using UnityEngine;

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

		public static TranslationConfigurationSO LoadConfigGroup(string configGroup)
		{
			return LoadResource<TranslationConfigurationSO>(TranslationConfigurationSOFileNameFromGroupID(configGroup));
		}

		public static T LoadResource<T>(string path) where T : Object
		{
			return Resources.Load<T>(path);
		}
	}
}