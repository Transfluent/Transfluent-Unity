//handles interaction with core code, allowing hte editor window to focus on presentation.  Also has the nice side effect of avoiding issues in editor files (massive bugginess with optional arguments, etc)
//seperatoion of logic, and 
using System;
using System.Collections.Generic;
using Castle.Core.Internal;

namespace transfluent.editor
{
	public class TestEditorWindowMediator
	{
		private const string DEFAULT_LANGUAGE_CODE = "en-us"; //is this needed?
		private readonly InjectionContext context;
		private LanguageList allLanguagesSupported;
		private TransfluentLanguage currentLanguage; //put this in a view state?

		public TestEditorWindowMediator()
		{
			context = new InjectionContext();
			context.addMapping<ICredentialProvider>(new EditorKeyCredentialProvider());
			context.addMapping<IWebService>(new SyncronousEditorWebRequest());
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
			catch (KeyNotFoundException e)
			{
			} //this is ok... I don't want to rewrite manualGetMapping
			return retVal;
		}

		public bool doAuth(string username, string password)
		{
			if (string.IsNullOrEmpty(getCurrentAuthToken()))
			{
				var login = new Login
					(username, password
					);
				WebServiceReturnStatus result = makeCall(login);

				if (!result.wasSuccessful())
				{
					return false;
				}
				var response = result.Parse<AuthenticationResponse>();

				if (string.IsNullOrEmpty(response.token))
				{
					return false;
				}
				context.addNamedMapping<string>(NamedInjections.API_TOKEN, response.token);
			}
			if (allLanguagesSupported == null)
			{
				var languageRequest = new RequestAllLanguages();
				WebServiceReturnStatus result = makeCall(languageRequest);

				if(result.wasSuccessful())
				{
					return false;
				}

				var rawList = result.Parse<List<Dictionary<string, TransfluentLanguage>>>();
				LanguageList list = languageRequest.GetLanguageListFromRawReturn(rawList);

				allLanguagesSupported = list;
				if (allLanguagesSupported == null) return false;
			}

			return true;
		}

		private bool doAuth()
		{
			KeyValuePair<string, string> usernamePassword = getUserNamePassword();
			return doAuth(usernamePassword.Key, usernamePassword.Value);
		}

		public List<string> getAllLanguageCodes()
		{
			if (!doAuth())
			{
				throw new Exception("Cannot login");
			}
			var languageCodes = new List<string>();
			allLanguagesSupported.languages.ForEach((TransfluentLanguage lang) => { languageCodes.Add(lang.code); });
			return languageCodes;
		}

		public void setCurrentLanugageCode(string languageCode)
		{
			List<string> knownCodes = getAllLanguageCodes();
			if (!knownCodes.Contains(languageCode)) throw new Exception("Tried to set language to an unknown language code");

			currentLanguage = allLanguagesSupported.getLangaugeByCode(languageCode);
		}

		public void invalidateAuth(bool wipeDatastore = false)
		{
			context.removeNamedMapping<string>(NamedInjections.API_TOKEN);
			allLanguagesSupported = null;
			if (wipeDatastore)
				context.manualGetMapping<ICredentialProvider>().save(null, null);
		}

		public string GetText(string textKey, string groupKey = null)
		{
			if (currentLanguage == null) throw new Exception("Must set current language first!");
			var getText = new GetTextKey
				(textKey,
					group_id: groupKey,
					languageID: currentLanguage.id
				);
			WebServiceReturnStatus result = makeCall(getText);
			return result.Parse<string>();
		}

		private bool wasSuccessful(WebServiceReturnStatus status)
		{
			return status.status == ServiceStatus.SUCCESS && status.rawErrorCode == 0;
		}

		private WebServiceReturnStatus makeCall(ITransfluentCall call)
		{
			context.setMappings(call);
			call.getParameters().Add("authToken", getCurrentAuthToken()); //TODO: find another way to do this...
			var service = context.manualGetMapping<IWebService>();

			return service.request(call);
		}

		public void SetText(string textKey, string textValue, string groupKey = null)
		{
			if (currentLanguage == null) throw new Exception("Must set current language first!");
			var saveText = new SaveTextKey
				(textKey,
					text: textValue,
					group_id: groupKey,
					language: currentLanguage.id);
			WebServiceReturnStatus result = makeCall(saveText);
			if(!result.wasSuccessful())
			{
				throw new Exception("DO ERROR HANDLING HERE" + result);
			}
		}

		public List<TransfluentTranslation> knownTextEntries()
		{
			if (currentLanguage == null) throw new Exception("Must set current language first!");

			var getAllKeys = new GetAllExistingTranslationKeys
				(currentLanguage.id);
			WebServiceReturnStatus result = makeCall(getAllKeys);
			if(result.wasSuccessful())
			{
				if (result.rawErrorCode == 400)
				{
					//this just means that there are no codes for this language
					return new List<TransfluentTranslation>();
				}
				throw new Exception("DO ERROR HANDLING HERE" + result);
			}
			var allKeys = result.Parse<Dictionary<string, TransfluentTranslation>>();
			var translationsList = new List<TransfluentTranslation>();
			allKeys.ForEach((KeyValuePair<string, TransfluentTranslation> pair) => { translationsList.Add(pair.Value); });
			return translationsList;
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
			foreach (TransfluentTranslation translation in translations)
			{
				if (string.IsNullOrEmpty(translation.text_id)) continue;
				//don't send empty string as a group id
				if (string.IsNullOrEmpty(translation.group_id)) translation.group_id = null;
				SetText(translation.text_id, translation.text, translation.group_id);
			}
		}
	}
}