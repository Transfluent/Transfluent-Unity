using System;
using System.Collections.Generic;

namespace transfluent
{
	[Route("text/status", RestRequestType.GET, "http://transfluent.com/backend-api/#TextStatus")]
	public class TextStatus : WebServiceParameters
	{
		[Inject(NamedInjections.API_TOKEN)]
		public string authToken { get; set; }

		public TextStatus(int language_id, string text_id, string group_id = null)
		{
			getParameters.Add("text_id", text_id);
			getParameters.Add("language", language_id.ToString());
			if(!string.IsNullOrEmpty(group_id)) 
				getParameters.Add("group_id", group_id);
		}

		public TextStatusResult Parse(string text)
		{
			return GetResponse<TextStatusResult>(text);
		}
	}
}