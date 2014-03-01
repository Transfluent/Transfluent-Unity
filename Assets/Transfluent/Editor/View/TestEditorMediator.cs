using System;
using System.Collections.Generic;

//handles interaction with core code, allowing hte editor window to focus on presentation.  Also has the nice side effect of avoiding issues in editor files (massive bugginess with optional arguments, etc)
//seperatoion of logic, and 

namespace transfluent.editor
{
	public class TestEditorWindowMediator
	{
		private const string DEFAULT_LANGUAGE_CODE = "en-us";  //is this needed?
		private readonly InjectionContext context;
		private LanguageList allLanguagesSupported;
		private TransfluentLanguage currentLanguage;  //put this in a view state?

		public TestEditorWindowMediator()
		{
			context = new InjectionContext();
			context.addMapping<ICredentialProvider>(new EditorKeyCredentialProvider());
			context.addMapping<IWebService>(new DebugSyncronousEditorWebRequest());
		}

		public KeyValuePair<string, string> getUserNamePassword()
		{
			var provider = context.manualGetMapping<ICredentialProvider>();
			return new KeyValuePair<string, string>(provider.username, provider.password);
		}

		public void setUsernamePassword(string username, string password)
		{
			var provider = context.manualGetMapping<ICredentialProvider>();
			provider.save(username, password);
		}

		public bool authIsDone()
		{
			string authToken = getCurrentAuthToken();

			return !string.IsNullOrEmpty(authToken) &&
				   allLanguagesSupported != null;
		}

		private string getCurrentAuthToken()
		{
			string retVal = null;
			try
			{
				retVal = context.manualGetMapping<string>(NamedInjections.API_TOKEN);
			}
			catch(KeyNotFoundException e) { } //this is ok... I don't want to rewrite manualGetMapping
			return retVal;
		}

		public bool doAuth(string username, string password)
		{
			if(string.IsNullOrEmpty(getCurrentAuthToken()))
			{
				var login = new Login
				{
					username = username,
					password = password
				};
				if(!makeCall(login) || string.IsNullOrEmpty(login.token))
				{
					return false;
				}
				context.addNamedMapping<string>(NamedInjections.API_TOKEN, login.token);
			}
			if(allLanguagesSupported == null)
			{
				var languageRequest = new RequestAllLanguages();
				if(!makeCall(languageRequest))
				{
					return false;
				}
				allLanguagesSupported = languageRequest.languagesRetrieved;
				if(allLanguagesSupported == null) return false;
			}

			return true;
		}

		bool doAuth()
		{
			var usernamePassword = getUserNamePassword();
			return doAuth(usernamePassword.Key, usernamePassword.Value);
		}

		public List<string> getAllLanguageCodes()
		{
			if(!doAuth())
			{
				throw new Exception("Cannot login");
			}
			var languageCodes = new List<string>();
			allLanguagesSupported.languages.ForEach((TransfluentLanguage lang) => { languageCodes.Add(lang.code); });
			return languageCodes;
		}

		public void setCurrentLanugageCode(string languageCode)
		{
			var knownCodes = getAllLanguageCodes();
			if(!knownCodes.Contains(languageCode)) throw new Exception("Tried to set language to an unknown language code");

			currentLanguage = allLanguagesSupported.getLangaugeByCode(languageCode);
		}

		public void invalidateAuth(bool wipeDatastore = false)
		{
			context.removeNamedMapping<string>(NamedInjections.API_TOKEN);
			allLanguagesSupported = null;
			if(wipeDatastore)
				context.manualGetMapping<ICredentialProvider>().save(null, null);
		}

		public string GetText(string textKey, string groupKey = null)
		{
			if(currentLanguage == null) throw new Exception("Must set current language first!");
			var getText = new GetTextKey
			{
				text_id = textKey,
				group_id = groupKey,
				languageID = currentLanguage.id
			};
			makeCall(getText);

			return getText.resultOfCall;
		}

		private bool makeCall(ITransfluentCall call)
		{
			context.setMappings(call);
			call.Execute();
			return call.webServiceStatus.status == ServiceStatus.SUCCESS;
		}

		public void SetText(string textKey, string textValue, string groupKey = null)
		{
			if(currentLanguage == null) throw new Exception("Must set current language first!");
			var saveText = new SaveTextKey
			{
				text_id = textKey,
				text = textValue,
				group_id = groupKey,
				language = currentLanguage.id,
			};
			if(!makeCall(saveText))
			{
				throw new Exception("DO ERROR HANDLING HERE" + saveText.webServiceStatus);
			}
		}

		public List<TransfluentTranslation> knownTextEntries()
		{
			if(currentLanguage == null) throw new Exception("Must set current language first!");

			var getAllKeys = new GetAllExistingTranslationKeys
			{
				language = currentLanguage.id
			};
			if(!makeCall(getAllKeys))
			{
				if (getAllKeys.webServiceStatus.rawErrorCode == 400)
				{
					//this just means that there are no codes for this language
					return new List<TransfluentTranslation>();
				}
				throw new Exception("DO ERROR HANDLING HERE" + getAllKeys.webServiceStatus);
			}
			makeCall(getAllKeys);

			return getAllKeys.translations;
		}

		public void setCurrentLanguageFromLanguageCode(string languageCode)
		{
			currentLanguage = allLanguagesSupported.getLangaugeByCode(languageCode);
		}
		public TransfluentLanguage GetCurrentLanguage()
		{
			return currentLanguage;
		}

		public void SetText(List<TransfluentTranslation> translations)
		{
			foreach (var translation in translations)
			{
				if (string.IsNullOrEmpty(translation.text_id)) continue;
				//don't send empty string as a group id
				if (string.IsNullOrEmpty(translation.group_id)) translation.group_id = null;
				SetText(translation.text_id,translation.text,translation.group_id);
			}
		}
	}
}
