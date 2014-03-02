using System;
using System.Collections.Generic;

namespace transfluent
{
	[Route("hello", RestRequestType.GET, "http://transfluent.com/backend-api/#Hello")]
	public class Hello : WebServiceParameters
	{
		[Inject(NamedInjections.API_TOKEN)]
		public string authToken { get; set; }

		public Hello(string name)
		{
			getParameters.Add("name",name);
		}

		public string Parse(string text)
		{
			return GetResponse<string>(text);
		}
	}
}
