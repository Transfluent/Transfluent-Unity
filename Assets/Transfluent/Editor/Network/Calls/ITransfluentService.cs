using System;
using System.Collections.Generic;

namespace transfluent
{
	public interface ITransfluentCall
	{
		Dictionary<string, string> getParameters();
		Dictionary<string, string> postParameters();
		Type expectedReturnType { get; }
		//T Parse<T>(WebServiceReturnStatus resultOfWebService);
	}
}
