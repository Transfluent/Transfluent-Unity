using System;

namespace transfluent.editor
{
	public class CommandLineCredentialProvider : ICredentialProvider
	{
		public const string USERNAME_COMMAND_LINE_ARGUMENT = "-USERNAME_COMMAND_LINE_ARGUMENT";
		public const string PASSWORD_COMMAND_LINE_ARGUMENT = "-PASSWORD_COMMAND_LINE_ARGUMENT";
		public IKeyStore keyStore = new CommandLineKeyStore();

		public CommandLineCredentialProvider()
		{
			username = keyStore.get(USERNAME_COMMAND_LINE_ARGUMENT);
			password = keyStore.get(PASSWORD_COMMAND_LINE_ARGUMENT);
		}

		public string username { get; protected set; }
		public string password { get; protected set; }

		public void save(string newUsername, string newPassword)
		{
			throw new NotImplementedException();
		}

		public void clear()
		{
			throw new NotImplementedException();
		}
	}
}