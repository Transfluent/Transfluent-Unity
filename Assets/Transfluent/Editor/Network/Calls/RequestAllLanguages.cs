using System;
using System.Collections.Generic;

namespace transfluent
{
	

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