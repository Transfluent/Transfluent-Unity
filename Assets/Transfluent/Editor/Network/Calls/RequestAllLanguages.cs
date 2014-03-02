using System;
using System.Collections.Generic;

namespace transfluent
{
	public class LanguageList
	{
		public List<TransfluentLanguage> languages { get; set; }

		public TransfluentLanguage getLangaugeByID(int id)
		{
			return languages.Find((TransfluentLanguage lang) => { return lang.id == id; });
		}

		public TransfluentLanguage getLangaugeByCode(string code)
		{
			return languages.Find((TransfluentLanguage lang) => { return lang.code == code; });
		}

		public TransfluentLanguage getLangaugeByName(string name)
		{
			return languages.Find((TransfluentLanguage lang) => { return lang.name == name; });
		}
	}

	[Route("languages", RestRequestType.GET, "http://transfluent.com/backend-api/#Languages")]
	public class RequestAllLanguages : WebServiceParameters
	{
		public Type expectedReturnType { get { return typeof(LanguageList); } }

		public RequestAllLanguages()
		{
		}

		public LanguageList Parse(string text)
		{
			var rawParams = GetResponse<List<Dictionary<string, TransfluentLanguage>>>(text);
			return GetLanguageListFromRawReturn(rawParams);
		}

		public LanguageList GetLanguageListFromRawReturn(List<Dictionary<string, TransfluentLanguage>> rawReturn)
		{
			var languages = new List<TransfluentLanguage>();
			foreach(var listitem in rawReturn)
			{
				foreach(var kvp in listitem)
				{
					languages.Add(kvp.Value);
				}
			}
			var retrieved = new LanguageList
			{
				languages = languages
			};
			return retrieved;
		}

	}
}