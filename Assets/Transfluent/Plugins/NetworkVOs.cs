using System;
using UnityEngine;

namespace transfluent
{
	[Serializable]
	public class TransfluentLanguage
	{
		public const string BACKWARDS_LANGUAGE_NAME = "xx-xx";

		public string name;
		public string code;
		public int id;

		public TransfluentLanguage() { }

		public override string ToString()
		{
			return string.Format("Languages{0} with code{1} name:{2}", id, code, name);
		}
	}

	 // for /v2/texts
	[Serializable]
	public class TransfluentSaveTextsResult
	{
		//"word_count":2,"saved_texts_count":2,"not_changed_count":0,"failed_count":0,"failed_keys":""
		public int word_count;
		public int saved_texts_count;
		public int not_changed_count;
		public int failed_count;
		public string failed_keys;
	}

	[Serializable]
	public class TransfluentOrder 
	{
		[Serializable]
		public class Text
		{
			public string source;
			public string translated;
		}

		public string order_id;
		public string status_text;
		public string status;
		public int source_language;
		public int target_language;
		//public Nullable<int> group_id; //it is null if there was not one set
		public string key;
		public string key_id;
		public Text text;
	}

	[Serializable]
	public class AccountCreationResult
	{
		public string token;
		public string password;
	}
	[Serializable]
	public class TransfluentTranslation
	{
		public string text_id;
		public string group_id;
		public TransfluentLanguage language;
		public string text;
	}

	[Serializable]
	public class TextStatusResult
	{
		public bool is_translated;
	}

	[Serializable]
	public class Error
	{
		public Error() { }
		public String type;
		public String message;
	}
	[Serializable]
	public class EmptyResponseContainer
	{
		public EmptyResponseContainer() { }

		public enum ResponseStatus
		{
			OK,
			ERROR
		}
		public string status;
		public Error error;
		public object result;

		//public string response;
		public bool isOK()
		{
			return status == ResponseStatus.OK.ToString() && error == null;
		}
	}

	[Serializable]
	public class ResponseContainer<T>
	{
		public ResponseContainer() { }

		public enum ResponseStatus
		{
			OK,
			ERROR
		}
		public string status;
		public Error error;
		public T response;

		//public string response;
		public bool isOK()
		{
			return status == ResponseStatus.OK.ToString();
		}

		public override string ToString()
		{
			return string.Format("status:{0} error:{1} response:{2}",status,error,response);
		}
	}

	[Serializable]
	public class AuthenticationResponse
	{
		public string token;
		public string expires;
	}

	[Serializable]
	public class TranslateRequest
	{
		public TransfluentLanguage[] TargetLanguageS;
		public TransfluentLanguage sourceLangauge;
		public string text_identifier;

		public string authToken;
	}
}