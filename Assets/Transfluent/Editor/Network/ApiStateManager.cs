using System;
using System.Collections.Generic;
using UnityEngine;

namespace transfluent
{
	public class AuthState : IAuthStateProvider
	{
		//public string currentLanguageCode;
		private readonly ICredentialProvider credentials = new FileBasedCredentialProvider();
		public string apiToken { get; private set; }
		public LanguageList knownLanguages { get; private set; }

		public bool checkCredentialsOrGetThem()
		{
			return Init();
		}

		private bool isInitialized()
		{
			return string.IsNullOrEmpty(apiToken) && knownLanguages != null;
		}

		public bool Init()
		{
			if (isInitialized()) return true;
			try
			{
				var langRequest = new RequestAllLanguages(){service = new SyncronousEditorWebRequest()};
				langRequest.Execute();
				if (langRequest.languagesRetrieved == null)
				{
					return false;
				}
				knownLanguages = langRequest.languagesRetrieved;
				var login = new Login {username = credentials.username, password = credentials.password, service = new SyncronousEditorWebRequest()};
				login.Execute();

				apiToken = login.token;
				if (string.IsNullOrEmpty(apiToken))
					return false;
			}
			catch (Exception e)
			{
				Debug.LogError("Error initializing auth for transfluent:" + e.Message + " stack:" + e.StackTrace);
				return false;
			}
			return true;
		}
	}

	public interface IAuthStateProvider
	{
		string apiToken { get; }
		LanguageList knownLanguages { get; }
		bool checkCredentialsOrGetThem();
	}

	public class ApiStateManager
	{
		private readonly IAuthStateProvider auth = new AuthState();

		public ApiStateManager()
		{
			auth.checkCredentialsOrGetThem();
		}

		private string getApiToken()
		{
			bool haveCredentials = auth.checkCredentialsOrGetThem();
			if (!haveCredentials) return null;
			return auth.apiToken;
		}

		private LanguageList getLanguageList()
		{
			bool haveCredentials = auth.checkCredentialsOrGetThem();
			if (!haveCredentials) return null; //throw an exception?
			return auth.knownLanguages;
		}

		public bool SetAuth(IAuthenticatedCall callObject)
		{
			callObject.authToken = getApiToken();
			return !string.IsNullOrEmpty(callObject.authToken);
		}

		public List<int> languageIDsFromLanguageCodes(List<string> languageCodes)
		{
			LanguageList list = getLanguageList();
			if (list == null)
				throw new Exception("Tried to use language list with null list"); //TODO: handle state better than this
			var languageIds = new List<int>();
			languageCodes.ForEach((string Code) => { languageIds.Add(list.getLangaugeByCode(Code).id); });
			return languageIds;
		}

		public List<TransfluentLanguage2> languagesFromIds(List<int> languageIDs)
		{
			LanguageList list = getLanguageList();
			if (list == null)
				throw new Exception("Tried to use language list with null list"); //TODO: handle state better than this
			var languages = new List<TransfluentLanguage2>();
			languageIDs.ForEach((int id) => { languages.Add(list.getLangaugeByID(id)); });
			return languages;
		}
	}


	public interface IServiceCall
	{
		void Execute();
	}

	public interface IAuthenticatedCall
	{
		string authToken { get; set; }
	}
}