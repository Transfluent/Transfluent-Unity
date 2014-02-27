using System.Collections.Generic;

namespace transfluent
{
	public class Login
	{
		public string token;
		public string username { get; set; }
		public string password { get; set; }

		[Inject]
		public IWebService service { get; set; }

		public void Execute()
		{
			ReturnStatus status = service.request(RestUrl.getURL(RestUrl.RestAction.AUTHENTICATE), new Dictionary<string, string>
			{
				{"email", username},
				{"password", password}
			});

			string responseText = status.text;
			var reader = new ResponseReader<AuthenticationResponse>
			{
				text = responseText
			};
			reader.deserialize();
			token = reader.response.token;
		}
	}
}