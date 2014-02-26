using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using transfluent;

namespace transfluent
{
	public class Hello
	{
		public string helloWorldText;
		public string name { get; set; }

		public void Execute()
		{
			IWebService service = new SyncronousEditorWebRequest();
			ReturnStatus status = service.request(RestUrl.getURL(RestUrl.RestAction.HELLO), new Dictionary<string, string>
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
