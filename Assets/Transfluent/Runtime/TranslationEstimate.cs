using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using transfluent;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class TranslationEstimate
{
	public void testThing(TranslationConfigurationSO selectedConfig)
	{
		var languageEstimates = new Dictionary<TransfluentLanguage, EstimateTranslationCostVO.Price>();
		if(GUILayout.Button("TEST TRANSLATE"))
		{
			StringBuilder simpleEstimateString = new StringBuilder();

			foreach(TransfluentLanguage lang in selectedConfig.destinationLanguages)
			{
				try
				{
					//TODO: other currencies
					var call = new EstimateTranslationCost("hello", selectedConfig.sourceLanguage.id,
														lang.id, quality: selectedConfig.QualityToRequest);
					var callResult = doCall(call);
					EstimateTranslationCostVO estimate = call.Parse(callResult.text);
					string printedEstimate = string.Format("Language:{0} cost: {1} {2}\n", lang.name, estimate.price.amount, estimate.price.currency);
					languageEstimates.Add(lang, estimate.price);
					simpleEstimateString.Append(printedEstimate);
					//Debug.Log("Estimate:" + JsonWriter.Serialize(estimate));
				}
				catch(Exception e)
				{
					languageEstimates.Add(lang, new EstimateTranslationCostVO.Price()
					{
						amount = "ERR",
						currency = e.Message
					});
					Debug.LogError("Error estimating prices");
				}

			}
			string group = selectedConfig.translation_set_group;

			var sourceSet = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(selectedConfig.sourceLanguage.code);
			var toTranslate = sourceSet.getGroup(group).getDictionaryCopy();
			//var knownKeys = sourceSet.getPretranslatedKeys(sourceSet.getAllKeys(), selectedConfig.translation_set_group);
			//var sourceDictionary = sourceSet.getGroup().getDictionaryCopy();
			var langToWordsToTranslateCount = new Dictionary<TransfluentLanguage, long>();
			foreach(TransfluentLanguage lang in selectedConfig.destinationLanguages)
			{
				var set = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(lang.code);
				var destKeys = set.getGroup(group).getDictionaryCopy();
				long wordCount = 0;
				foreach (KeyValuePair<string,string> kvp in toTranslate)
				{
					if(!destKeys.ContainsKey(kvp.Key))
						wordCount += kvp.Value.Split(' ').Length;
				}
				langToWordsToTranslateCount.Add(lang, wordCount);
			}

			if(EditorUtility.DisplayDialog("Estimates", "Estimated cost(only additions counted in estimate):\n" + simpleEstimateString, "OK", "Cancel"))
			{
				foreach(TransfluentLanguage lang in selectedConfig.destinationLanguages)
				{
					var oneWordPrice = languageEstimates[lang];
					float cost = float.Parse(oneWordPrice.amount);
					long totalNumberOfWords = langToWordsToTranslateCount[lang];
					float totalCost = cost * totalNumberOfWords;

					Debug.Log("Lang cost:" + totalCost + " total number of words:" + totalNumberOfWords + " per word cost:" + oneWordPrice.amount);
				}
				Debug.Log("GOT THING");
			}
		}
	}
	private WebServiceReturnStatus doCall(WebServiceParameters call)
	{
		var req = new SyncronousEditorWebRequest();
		try
		{
			WebServiceReturnStatus result = req.request(call);
			return result;
		}
		catch(HttpErrorCode code)
		{
			Debug.Log(code.code + " http error");
			throw;
		}
	}
}
