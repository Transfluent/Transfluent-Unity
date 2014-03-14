namespace transfluent.editor
{
	public class EditorKeyCredentialProvider : ICredentialProvider
	{
		public const string USERNAME_EDITOR_KEY = "TRANSFLUENT_USERNAME_KEY";
		public const string PASSWORD_EDITOR_KEY = "PASSWORD_EDITOR_KEY";
		public IKeyStore keyStore = new EditorKeyStore();

		public EditorKeyCredentialProvider()
		{
			username = keyStore.get(USERNAME_EDITOR_KEY);
			password = keyStore.get(PASSWORD_EDITOR_KEY);
		}

		public string username { get; protected set; }

		public string password { get; protected set; }

		public void save(string newUsername, string newPassword)
		{
			keyStore.set(USERNAME_EDITOR_KEY, newUsername);
			keyStore.set(PASSWORD_EDITOR_KEY, newPassword);
			username = newUsername;
			password = newPassword;
		}

		public void clear()
		{
			username = password = null;
			keyStore.set(USERNAME_EDITOR_KEY, "");
			keyStore.set(PASSWORD_EDITOR_KEY, "");
		}
	}
}