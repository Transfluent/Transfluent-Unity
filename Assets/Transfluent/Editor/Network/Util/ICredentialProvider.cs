namespace transfluent
{
	public interface ICredentialProvider
	{
		string username { get; }
		string password { get; }
		void save(string newUsername, string newPassword);
		void clear();
	}
}