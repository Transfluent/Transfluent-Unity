using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace transfluent
{
	[Route("texts", RestRequestType.GET, "http://transfluent.com/backend-api/#Texts")] //expected return type?
	public class GetAllExistingTranslationKeys : ITransfluentCall
	{
		public Type expectedReturnType { get { return typeof(Dictionary<string, TransfluentTranslation>); } }
		private readonly Dictionary<string, string> _getParams;
			
		[Inject(NamedInjections.API_TOKEN)]
		public string authToken { get; set; }

		public GetAllExistingTranslationKeys(int language, string group_id = null, int limit = 100, int offset = 0)
		{
			if(language <= 0) throw new Exception("INVALID Language in getAllExistingKeys");
			_getParams = new Dictionary<string, string>
			{
				{"language", language.ToString()},
			};
			if(!string.IsNullOrEmpty(group_id))
			{
				_getParams.Add("groupid", group_id);
			}
			if(limit > 0)
			{
				_getParams.Add("limit", limit.ToString());
			}
			if(offset > 0)
			{
				_getParams.Add("offset", offset.ToString());
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