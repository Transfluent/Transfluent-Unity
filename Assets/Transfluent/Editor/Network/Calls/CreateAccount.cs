using System;
using System.Collections.Generic;

namespace transfluent
{
	[Route("hello", RestRequestType.GET, "http://transfluent.com/backend-api/#Hello")]
	public class CreateAccount : ITransfluentCall
	{
		public Type expectedReturnType { get { return typeof (AccountCreationResult); } }

		private Dictionary<string, string> _getParams;
		public CreateAccount(string email, bool termsOfService, bool sendPasswordOverEmail)
		{
			_getParams = new Dictionary<string, string>
			{
				{"email", email},
				{"terms", termsOfService.ToString()},
				{"send_password", sendPasswordOverEmail.ToString()}
			};
		}

		public Dictionary<string, string> getParameters()
		{
			return _getParams;
		}

		public Dictionary<string, string> postParameters()
		{
			return new Dictionary<string, string>();
		}

	}
}
