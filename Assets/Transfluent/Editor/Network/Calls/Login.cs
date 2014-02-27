using System.Collections.Generic;

namespace transfluent
{
	public class Login : ITransfluentCall
	{
		public string token;
		public string username { get; set; }
		public string password { get; set; }

		[Inject]
		public IWebService service { get; set; }

		public WebServiceReturnStatus webServiceStatus { get; private set; }

		public void Execute()
		{
			webServiceStatus = service.request(RestUrl.getURL(RestUrl.RestAction.AUTHENTICATE), new Dictionary<string, string>
			{
				{"email", username},
				{"password", password}
			});

			string responseText = webServiceStatus.text;
			var reader = new ResponseReader<AuthenticationResponse>
			{
				text = responseText
			};
			reader.deserialize();
			token = reader.response.token;
		}
	}
}