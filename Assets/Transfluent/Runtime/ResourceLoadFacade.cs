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

		public static T LoadResource<T>(string path) where T : UnityEngine.Object
		{
			return Resources.Load<T>(path);
		}

	}
}

