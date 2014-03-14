using System;
using System.Collections.Generic;

namespace transfluent
{
	[Route("texts", RestRequestType.GET, "http://transfluent.com/backend-api/#Texts")] //expected return type?
	public class GetAllExistingTranslationKeys : WebServiceParameters
	{
		public GetAllExistingTranslationKeys(int language, string group_id = null, int limit = 100, int offset = 0)
		{
			if(language <= 0) throw new Exception("INVALID Language in getAllExistingKeys");

			getParameters.Add("language", language.ToString());

			if(!string.IsNullOrEmpty(group_id))
			{
				getParameters.Add("groupid", group_id);
			}
			if(limit > 0)
			{
				getParameters.Add("limit", limit.ToString());
			}
			if(offset > 0)
			{
				getParameters.Add("offset", offset.ToString());
			}
		}

		[Inject(NamedInjections.API_TOKEN)]
		public string authToken { get; set; }

		public Dictionary<string, TransfluentTranslation> Parse(string text)
		{
			return GetResponse<Dictionary<string, TransfluentTranslation>>(text);
		}
	}
}