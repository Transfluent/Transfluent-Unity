using System;
using System.Collections.Generic;

namespace transfluent
{
	[Route("hello", RestRequestType.GET, "http://transfluent.com/backend-api/#Hello")]
	public class Hello : ITransfluentCall
	{
		[Inject(NamedInjections.API_TOKEN)]
		public string authToken { get; set; }

		private Dictionary<string, string> _getParams;

		public Hello(string name)
		{
			_getParams = new Dictionary<string, string>
			{
				{"name", name},
			};
		}

		public string Parse(WebServiceReturnStatus status)
		{
			string responseText = status.text;
			var reader = new ResponseReader<String>
			{
				text = responseText
			};
			reader.deserialize();
			return reader.response;
		}

		public Dictionary<string, string> getParameters()
		{
			return _getParams;
		}

		public Dictionary<string, string> postParameters()
		{
			throw new NotImplementedException();
		}

		public Type expectedReturnType { get { return typeof(string); } }
	}
}
