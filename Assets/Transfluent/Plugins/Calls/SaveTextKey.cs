namespace transfluent
{
	[Route("text", RestRequestType.POST, "http://transfluent.com/backend-api/#Text")]
	public class SaveTextKey : WebServiceParameters
	{
		//URL: https://transfluent.com/v2/text/ ( HTTPS only)
		//Parameters: text_id, group_id, language, text, invalidate_translations [=1], is_draft, token

		public SaveTextKey(string text_id, int language, string text, string group_id = null)
		{
			postParameters.Add("text_id", text_id);
			postParameters.Add("language", language.ToString());
			postParameters.Add("text", text);

			if(group_id != null)
			{
				postParameters.Add("group_id", group_id);
			}
		}

		[Inject(NamedInjections.API_TOKEN)]
		public string authToken { get; set; }

		public bool Parse(string text)
		{
			return GetResponse<bool>(text);
		}
	}
}