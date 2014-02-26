using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace transfluent
{
	public class RestUrl
	{
		public enum RestAction
		{
			AUTHENTICATE,
			HELLO,
			REGISTER,
			LANGUAGES,
			TEXT,
			TEXTS,
			TEXTSTATUS,
			TEXTSTRANSLATE,
			TEXTWORDCOUNT,
			COMBINEDTEXTS_SEND,
			COMBINEDTEXTS_TRANSLATE,
			TEXTSORDERS
		}

		//public enum TransfluentMethodType { Hello, Authenticate, Register, Languages, Text, Texts, TextStatus, 
		//TextsTranslate, TextWordCount, CombinedTexts_Send, CombinedTexts_Translate };
		private static string baseServiceUrl = "https://transfluent.com/v2/";
		public RestAction action;

		public string getURL()
		{
			return baseServiceUrl + action.ToString().ToLower();
		}

		public static string getURL(RestAction action)
		{
			string url = baseServiceUrl;
			switch(action)
			{
				case RestAction.TEXTSORDERS:
					url += "texts/orders/";
					break;
				case RestAction.TEXTSTRANSLATE:
					url += "texts/translate/";
					break;
				case RestAction.TEXTSTATUS:
					url += "text/status/";
					break;
				default:
					url += action.ToString().ToLower();
					break;
			}
			return url;
		}
	}
}
