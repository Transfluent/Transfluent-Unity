using System;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

namespace transfluent
{
	[Route("texts/translate", RestRequestType.GET, "http://transfluent.com/backend-api/#TextsTranslate")]
	public class OrderTranslation : ITransfluentCall
	{
		public Type expectedReturnType { get { return typeof(TextsTranslateResult); } }

		public enum TranslationQuality
		{
			PAIR_OF_TRANSLATORS = 3,
			PROFESSIONAL_TRANSLATOR = 2,
			NATIVE_SPEAKER = 1,
		}

		//group_id, source_language, target_languages, texts, comment, callback_url, max_words [=1000], level [=2], token
		[Inject(NamedInjections.API_TOKEN)]
		public string authToken { get; set; }

		private Dictionary<string, string> _getParams;

		public OrderTranslation(int source_language, int[] target_languages, string[] texts, string comment=null, int max_words = 1000, TranslationQuality level = TranslationQuality.PROFESSIONAL_TRANSLATOR,string group_id=null)
		{
			var containerOfTextIDsToUse = new List<TextIDToTranslateContainer>();
			foreach(string toTranslate in texts)
			{
				containerOfTextIDsToUse.Add(new TextIDToTranslateContainer
				{
					id = toTranslate
				});
			}

			_getParams = new Dictionary<string, string>
			{
				{"source_language", source_language.ToString()},
				{"target_languages", JsonWriter.Serialize(target_languages)},
				{"texts", JsonWriter.Serialize(containerOfTextIDsToUse)},
			};
			if(level != 0)
			{
				_getParams.Add("level", ((int)level).ToString());
			}
			if(group_id != null)
			{
				_getParams.Add("group_id", group_id);
			}
			if(!string.IsNullOrEmpty(comment))
			{
				_getParams.Add("comment", comment);
			}
			if(max_words > 0)
			{
				_getParams.Add("max_words", max_words.ToString());
			}
		}
		public TextsTranslateResult Parse(WebServiceReturnStatus status)
		{
			string responseText = status.text;

			var reader = new ResponseReader<TextsTranslateResult>
			{
				text = responseText
			};
			reader.deserialize();
			return reader.response;
		}

		[Serializable]
		public class TextIDToTranslateContainer
		{
			public string id;
		}

		[Serializable]
		public class TextsTranslateResult
		{
			public int ordered_word_count;
			public int word_count;
		}

		public Dictionary<string, string> getParameters()
		{
			return _getParams;
		}

		public Dictionary<string, string> postParameters()
		{
			throw new NotImplementedException();
		}
	}
}