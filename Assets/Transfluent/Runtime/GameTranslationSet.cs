using System.Collections.Generic;
using UnityEngine;

namespace transfluent
{
	public class GameTranslationSet : ScriptableObject
	{
		public List<TransfluentTranslation> allTranslations = new List<TransfluentTranslation>();

		public Dictionary<string, string> getKeyValuePairs(string group = "")
		{
			var dictionary = new Dictionary<string, string>();
			if (allTranslations == null) return dictionary;

			bool groupIsEmpty = string.IsNullOrEmpty(group);

			foreach (TransfluentTranslation translation in allTranslations)
			{
				if (groupIsEmpty && string.IsNullOrEmpty(translation.group_id))
				{
					if (dictionary.ContainsKey(translation.text_id))
					{
						Debug.LogError("Dictionary already contains key:" + translation.text_id);
						continue;
					}
					dictionary.Add(translation.text_id, translation.text);
				}
				else
				{
					if (translation.group_id == group)
					{
						dictionary.Add(translation.text_id, translation.text);
					}
				}
			}
			return dictionary;
		}
	}
}