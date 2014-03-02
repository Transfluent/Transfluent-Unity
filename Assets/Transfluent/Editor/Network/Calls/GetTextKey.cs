using System;
using System.Collections.Generic;

namespace transfluent
{
	[Route("text", RestRequestType.GET,"http://transfluent.com/backend-api/#Text")]
	public class GetTextKey : WebServiceParameters
	{
		[Inject(NamedInjections.API_TOKEN)]
		public string authToken { get; set; }

		public GetTextKey(string text_id, int languageID, string group_id=null)
		{
			getParameters.Add("text_id", text_id);
			getParameters.Add("language", languageID.ToString());
			if(group_id != null)
			{
				getParameters.Add("group_id", group_id);
			}
		}

		public string Parse(string text)
		{
			return GetResponse<string>(text);
		}
	}
}