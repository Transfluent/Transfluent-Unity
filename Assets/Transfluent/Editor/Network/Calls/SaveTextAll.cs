using System;
using System.Collections.Generic;

namespace transfluent
{
	[Route("text", RestRequestType.POST, "http://transfluent.com/backend-api/#Text")]
	public class SaveTextKey : ITransfluentCall
	{
		//URL: https://transfluent.com/v2/text/ ( HTTPS only)
		//Parameters: text_id, group_id, language, text, invalidate_translations [=1], is_draft, token
		public Type expectedReturnType { get { return typeof(bool); } }

		[Inject(NamedInjections.API_TOKEN)]
		public string authToken { get; set; }

		private readonly Dictionary<string, string> _getParams = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _postParams;

		public SaveTextKey(string text_id, int language, string text, string group_id = null)
		{
			_postParams = new Dictionary<string, string>
			{
				{"text_id", text_id},
				{"language", language.ToString()},
				{"text", text}
			};
			if (group_id != null)
			{
				_postParams.Add("group_id", group_id);
			}
		}
		public Dictionary<string, string> getParameters()
		{
			return _getParams;
		}

		public Dictionary<string, string> postParameters()
		{
			return _postParams;
		}
	}
}