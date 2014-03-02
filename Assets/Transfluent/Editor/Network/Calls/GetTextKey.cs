using System;
using System.Collections.Generic;

namespace transfluent
{
	[Route("text", RestRequestType.GET,"http://transfluent.com/backend-api/#Text")]
	public class GetTextKey : ITransfluentCall
	{
		public Type expectedReturnType { get { return typeof(string); } }

		[Inject(NamedInjections.API_TOKEN)]
		public string authToken { get; set; }

		private readonly Dictionary<string, string> _getParams;
		public GetTextKey(string text_id, int languageID, string group_id=null)
		{
			_getParams = new Dictionary<string, string>
			{
				{"text_id", text_id},
				{"language", languageID.ToString()},
			};

			if(group_id != null)
			{
				_getParams.Add("group_id", group_id);
			}
		}

		public Dictionary<string, string> getParameters()
		{
			return _getParams;
		}

		public Dictionary<string, string> postParameters()
		{
			throw new NotImplementedException();
		}
	}
}