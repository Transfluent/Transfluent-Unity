﻿using System;
using System.Collections.Generic;

namespace transfluent
{
	public class Hello
	{
		public string helloWorldText;

		public string name { get; set; }

		[Inject(NamedInjections.API_TOKEN)]
		public string authToken { get; set; }

		[Inject]
		public IWebService service { get; set; }

		public void Execute()
		{
			
			WebServiceReturnStatus status = service.request(RestUrl.getURL(RestUrl.RestAction.HELLO), new Dictionary<string, string>
			{
				{"name", name},
			});

			string responseText = status.text;
			var reader = new ResponseReader<String>
			{
				text = responseText
			};
			reader.deserialize();
			helloWorldText = reader.response;
		}
	}
}
