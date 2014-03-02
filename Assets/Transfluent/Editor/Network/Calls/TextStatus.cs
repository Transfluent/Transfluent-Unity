using System;
using System.Collections.Generic;

namespace transfluent
{
	[Route("text/status", RestRequestType.GET, "http://transfluent.com/backend-api/#TextStatus")]
	public class TextStatus : ITransfluentCall
	{
		[Inject(NamedInjections.API_TOKEN)]
		public string authToken { get; set; }

		private Dictionary<string, string> _getParams;

		public TextStatus(int language_id, string text_id, string group_id = null)
		{
			_getParams = new Dictionary<string, string>
			{
				{"text_id", text_id},
				{"language", language_id.ToString()},
			};
			if (!string.IsNullOrEmpty(group_id)) _getParams.Add("group_id", group_id);
		}

		public Dictionary<string, string> getParameters()
		{
			return _getParams;
		}

		public Dictionary<string, string> postParameters()
		{
			throw new System.NotImplementedException();
		}

		public Type expectedReturnType { get { return typeof(TextStatusResult); } }
	}
}