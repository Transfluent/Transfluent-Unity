using System;

namespace transfluent
{
	[Route("free/text/word/count/", RestRequestType.GET, "http://transfluent.com/backend-api/#FreeTextWordCount")]
	public class EstimateTranslationCost : WebServiceParameters
	{
		public EstimateTranslationCost(
			string textToEstimate,
			int sourceLanguageId,
			int targetLanguage,
			string currency_code = "USD",
			OrderTranslation.TranslationQuality quality = OrderTranslation.TranslationQuality.PAIR_OF_TRANSLATORS
			)
		{
			getParameters.Add("free_text", textToEstimate);
			getParameters.Add("source_language", sourceLanguageId.ToString());
			getParameters.Add("targetLanguage", targetLanguage.ToString());
			getParameters.Add("currency_code", currency_code);

			if (quality != OrderTranslation.TranslationQuality.PAIR_OF_TRANSLATORS)
				getParameters.Add("quality", quality.ToString());

			
		}

		//https://transfluent.com/v2/free/text/word/count/?source_language=1&target_language=500&free_text=hello
		//{"status":"OK","response":{"count":1,"unit":"Word","price":{"amount":"0.27","currency":"USD"},"level":3}}
		public EstimateTranslationCostVO Parse(string text)
		{
			return GetResponse<EstimateTranslationCostVO>(text);
		}
	}

	[Serializable]
	public class EstimateTranslationCostVO
	{
		public string count;
		public string unit; //ie Word

		public class Price
		{
			public string amount; //"0.27"
			public string currency;//"USD"
		}
	}
}