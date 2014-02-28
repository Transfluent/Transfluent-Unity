
namespace transfluent
{
	public interface ITransfluentCall
	{
		WebServiceReturnStatus webServiceStatus { get; }
		void Execute();
	}
}
