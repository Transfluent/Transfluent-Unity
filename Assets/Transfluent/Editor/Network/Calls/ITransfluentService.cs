using transfluent;
using UnityEngine;
using UnityEditor;


namespace transfluent
{
	public interface ITransfluentCall
	{
		WebServiceReturnStatus webServiceStatus { get; }
		void Execute();
	}
}
