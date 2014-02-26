using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace transfluent
{
	public class AuthState : IAuthStateProvider
	{
		//public string currentLanguageCode;
		private readonly TestLoginFlow.ICredentialProvider credentials = new TestLoginFlow.FileBasedCredentialProvider();
		public string apiToken { get; private set; }
		public LanguageList knownLanguages { get; private set; }

		bool isInitialized()
		{
			return string.IsNullOrEmpty(apiToken) && knownLanguages != null;
		}
		public bool Init()
		{
			if (isInitialized()) return true;
			try
			{
				var langRequest = new RequestAllLanguages();
				langRequest.Execute();
				if(langRequest.languagesRetrieved == null)
				{
					return false;
				}
				knownLanguages = langRequest.languagesRetrieved;
				var login = new Login { username = credentials.username, password = credentials.password };
				login.Execute();

				apiToken = login.token;
				if(string.IsNullOrEmpty(apiToken))
					return false;
			}
			catch(Exception e)
			{
				UnityEngine.Debug.LogError("Error initializing auth for transfluent:" + e.Message + " stack:" + e.StackTrace);
				return false;
			}
			return true;
		}

		public bool checkCredentialsOrGetThem()
		{
			return Init();
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
		IAuthStateProvider auth = new AuthState();

		public ApiStateManager()
		{
			auth.checkCredentialsOrGetThem();
		}
		public bool SetText(string key, string value,TransfluentLanguage2 sourceLanguage)
		{
			return SetText(key, value, sourceLanguage, null);
		}
		public bool SetText(string key, string value, TransfluentLanguage2 sourceLanguage,string groupID)
		{
			if(auth.checkCredentialsOrGetThem()) return false;//error initializing

			var saver = new SaveTextKey()
			{
				authToken = auth.apiToken,
				language = sourceLanguage.id,
				text_id = key,
				text = value,
			};
			if (groupID != null)
			{
				saver.group_id = groupID;
			}
			saver.Execute();
			return saver.savedSuccessfully;
		}
		public bool GetText(string key, string value, TransfluentLanguage2 desiredLanguage)
		{
			if(auth.checkCredentialsOrGetThem()) return false;//error initializing

			var saver = new SaveTextKey()
			{
				authToken = auth.apiToken,
				language = desiredLanguage.id,
				text_id = key,
				text = value,
			};
			saver.Execute();
			return saver.savedSuccessfully;
		}

		public bool OrderTranslation(List<string> textIdsToTranslate, TransfluentLanguage2 sourceLanguage,List<TransfluentLanguage2> destinationLanguages,
			int max_words=1000,OrderTranslation.TranslationQuality level=transfluent.OrderTranslation.TranslationQuality.PROFESSIONAL_TRANSLATOR)
		{
			if(auth.checkCredentialsOrGetThem()) return false;//error initializing

			var order = new OrderTranslation()
			{
				source_language = sourceLanguage.id,
				authToken = auth.apiToken,
			};
			order.Execute();
			return true;
		}
	}
}