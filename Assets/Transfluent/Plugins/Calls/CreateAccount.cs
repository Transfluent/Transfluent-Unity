namespace transfluent
{
	[Route("hello", RestRequestType.GET, "http://transfluent.com/backend-api/#Hello")]
	public class CreateAccount : WebServiceParameters
	{
		//AccountCreationResult
		public CreateAccount(string email, bool termsOfService, bool sendPasswordOverEmail)
		{
			getParameters.Add("email", email);
			getParameters.Add("terms", termsOfService.ToString());
			getParameters.Add("send_password", sendPasswordOverEmail.ToString());
		}

		public AccountCreationResult Parse(string text)
		{
			return GetResponse<AccountCreationResult>(text);
		}
	}
}