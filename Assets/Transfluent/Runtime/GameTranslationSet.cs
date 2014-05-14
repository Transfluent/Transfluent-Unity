using System;
using System.Collections.Generic;
using transfluent;
using UnityEngine;

public class GameTranslationSet : ScriptableObject
{
	[SerializeField]
	private List<GroupOfTranslations> allTranslations = new List<GroupOfTranslations>();

	public TransfluentLanguage language = new TransfluentLanguage();

	public List<string> allGroups()
	{
		var list = new List<string>();
		foreach(GroupOfTranslations group in allTranslations)
		{
			list.Add(group.groupid);
		}
		return list;
	}

	public GroupOfTranslations getGroup(string groupid = "")
	{
		GroupOfTranslations targetGroup = null;
		foreach(GroupOfTranslations group in allTranslations)
		{
			if(group.groupid == groupid)
			{
				targetGroup = group;
				break;
			}
		}
		if(targetGroup == null)
		{
			targetGroup = new GroupOfTranslations()
			{
				groupid = groupid
			};
			allTranslations.Add(targetGroup);
		}
		return targetGroup;
	}

	public void mergeInSet(string groupID, Dictionary<string, string> toMerge)
	{
		var group = getGroup(groupID);
		group.addTranslations(toMerge);
	}

	public int wordCountOfGroup(string groupid)
	{
		int wc = 0;
		var allGroups = this.allGroups();
		if(!allGroups.Contains(groupid)) return wc;
		var group = getGroup(groupid);
		return group.getDictionaryCopy().Keys.Count;
	}

	[Serializable]
	public class GroupOfTranslations
	{
		public string groupid = "";
		public List<Translation> translations = new List<Translation>();

		public Dictionary<string, string> getDictionaryCopy()
		{
			var dic = new Dictionary<string, string>();
			foreach(Translation translation in translations)
			{
				if(dic.ContainsKey(translation.key))
				{
					Debug.LogWarning(string.Format("Two keys of the same value present:{0} overwriting existing value:{1} with new value{2}", translation.key, dic[translation.key], translation.value));
					dic[translation.key] = translation.value;
				}
				else
				{
					dic.Add(translation.key, translation.value);
				}
			}
			return dic;
		}

		//add/merge in all translations in set
		public void addTranslations(Dictionary<string, string> toAdd)
		{
			var dictionaryOfExistingKeys = getDictionaryCopy();
			foreach(KeyValuePair<string, string> kvp in toAdd)
			{
				if(dictionaryOfExistingKeys.ContainsKey(kvp.Key))
				{
					foreach(Translation translation in translations)
					{
						if(translation.key == kvp.Key)
						{
							translation.value = kvp.Value;
						}
					}
				}
				else
				{
					translations.Add(new Translation(kvp));
				}
			}
		}

		[Serializable]
		public class Translation
		{
			public string key;
			public string value;

			public Translation()
			{
			}

			public Translation(KeyValuePair<string, string> kvp)
			{
				key = kvp.Key;
				value = kvp.Value;
			}
		}
	}
}

#if false
public class RuntimeTranslationSet
{
	private string groupid = "";
	Dictionary<string, string> translations = new Dictionary<string, string>();

	public RuntimeTranslationSet(GameTranslationSet.GroupOfTranslations serializedGroup)
	{
		groupid = serializedGroup.groupid;
		translations = serializedGroup.getDictionaryCopy();
	}
}
#endif