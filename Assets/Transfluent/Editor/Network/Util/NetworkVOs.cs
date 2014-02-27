﻿using System;
using UnityEngine;
using System.Collections;

namespace transfluent
{
	[Serializable]
	public class TransfluentLanguage2
	{
		public const string BACKWARDS_LANGUAGE_NAME = "xx-xx";

		public string name;
		public string code;
		public int id;

		public TransfluentLanguage2() { }

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
	public class TransfluentTranslation
	{
		public class Text
		{
			public string source { get; set; }
			public string translated { get; set; }
		}

		public string order_id { get; set; }
		public string status_text { get; set; }
		public string status { get; set; }
		public int source_language { get; set; }
		public int target_language { get; set; }
		//public Nullable<int> group_id { get; set; } //it is null if there was not one set
		public string key { get; set; }
		public string key_id { get; set; }
		public Text text { get; set; }
		
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
		public String type { get; set; }
		public String message { get; set; }
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
		public string status { get; set; }
		public Error error;
		public object result { get; set; }

		//public string response;
		public bool isOK()
		{
			return status == ResponseStatus.OK.ToString();
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
		public string status { get; set; }
		public Error error;
		public T response { get; set; }

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
		public string token { get; set; }
		public string expires { get; set; }
	}

	[Serializable]
	public class TranslateRequest
	{
		public TransfluentLanguage2[] TargetLanguage2s { get; set; }
		public TransfluentLanguage2 sourceLangauge { get; set; }
		public string text_identifier { get; set; }

		public string authToken { get; set; }
	}
}