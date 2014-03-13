namespace transfluent
{
	[Route("authenticate", RestRequestType.GET, "http://transfluent.com/backend-api/#Authenticate")]
	public class Login : WebServiceParameters
	{
		public Login(string username, string password)
		{
			getParameters.Add("email", username);
			getParameters.Add("password", password);
		}

		public AuthenticationResponse Parse(string text)
		{
			return GetResponse<AuthenticationResponse>(text);
		}
	}
}