using System;
using System.Collections.Generic;

namespace transfluent
{
	[Route("authenticate", RestRequestType.GET, "http://transfluent.com/backend-api/#Authenticate")]
	public class Login : ITransfluentCall
	{
		public Type expectedReturnType { get { return typeof(AuthenticationResponse); } }

		public Dictionary<string, string> _getParams;
		public Login(string username,string password)
		{
			_getParams = new Dictionary<string, string>
			{
				{"email", username},
				{"password", password}
			};
		}

		public Dictionary<string, string> getParameters()
		{
			return _getParams;
		}

		public Dictionary<string, string> postParameters()
		{
			throw new System.NotImplementedException();
		}
	}
}