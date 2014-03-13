using UnityEngine;
using System.Collections.Generic;

namespace transfluent
{
	public class GameTranslationSet : ScriptableObject
	{
		public List<TransfluentTranslation> allTranslations = new List<TransfluentTranslation>();

		public Dictionary<string, string> getKeyValuePairs(string group="")
		{
			var dictionary = new Dictionary<string, string>();
			if (allTranslations == null) return dictionary;
			foreach (TransfluentTranslation translation in allTranslations)
			{
				dictionary.Add(translation.text_id, translation.text);
			}
			bool groupIsEmpty = string.IsNullOrEmpty(group);

			foreach(TransfluentTranslation translation in allTranslations)
			{
				if(groupIsEmpty && string.IsNullOrEmpty(translation.group_id))
				{
					dictionary.Add(translation.text_id, translation.text);
				}
				else
				{
					if(translation.group_id == group)
					{
						dictionary.Add(translation.text_id, translation.text);
					}
				}
			}
			return dictionary;
		}
	}
}