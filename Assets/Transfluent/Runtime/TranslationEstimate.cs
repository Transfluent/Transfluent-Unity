using Assets.Transfluent.Plugins.Calls;
using System;
using System.Collections.Generic;
using System.Text;
using transfluent;
using UnityEditor;
using UnityEngine;

public class TranslationEstimate
{
	public string token;

	public TranslationEstimate(string tokenIn)
	{
		token = tokenIn;
	}

	public void testThing(TranslationConfigurationSO selectedConfig)
	{
		var languageEstimates = new Dictionary<TransfluentLanguage, EstimateTranslationCostVO.Price>();
		if(GUILayout.Button("Translate"))
		{
			string group = selectedConfig.translation_set_group;
			var sourceSet = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(selectedConfig.sourceLanguage.code);
			if(sourceSet == null || sourceSet.getGroup(group) == null)
			{
				EditorUtility.DisplayDialog("ERROR", "No messages in group", "OK");
				return;
			}

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
					//string printedEstimate = string.Format("Language:{0} cost per word: {1} {2}\n", lang.name, estimate.price.amount, estimate.price.currency);
					languageEstimates.Add(lang, estimate.price);
					//simpleEstimateString.Append(printedEstimate);
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

			
			var toTranslate = sourceSet.getGroup(group).getDictionaryCopy();
			//var knownKeys = sourceSet.getPretranslatedKeys(sourceSet.getAllKeys(), selectedConfig.translation_set_group);
			//var sourceDictionary = sourceSet.getGroup().getDictionaryCopy();
			var langToWordsToTranslateCount = new Dictionary<TransfluentLanguage, long>();
			foreach(TransfluentLanguage lang in selectedConfig.destinationLanguages)
			{
				var set = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(lang.code);
				var destKeys = set.getGroup(group).getDictionaryCopy();
				long wordCount = 0;
				foreach(KeyValuePair<string, string> kvp in toTranslate)
				{
					if(!destKeys.ContainsKey(kvp.Key))
						wordCount += kvp.Value.Split(' ').Length;
				}
				langToWordsToTranslateCount.Add(lang, wordCount);

				var oneWordPrice = languageEstimates[lang];
				float costPerWord = float.Parse(oneWordPrice.amount);
				float totalCost = costPerWord * wordCount;
				simpleEstimateString.AppendFormat("language name: {0} total cost: {1} {2} \n\tCost per word:{3} total words:{4} ",
					lang.name, totalCost, oneWordPrice.currency, costPerWord, wordCount);

			}
			Debug.Log("GOT THING");
			if(EditorUtility.DisplayDialog("Estimates", "Estimated cost(only additions counted in estimate):\n" + simpleEstimateString, "OK", "Cancel"))
			{
				
				doTranslation(selectedConfig);
			}
		}
	}

	private WebServiceReturnStatus doCall(WebServiceParameters call)
	{
		var req = new SyncronousEditorWebRequest();
		try
		{
			call.getParameters.Add("token", token);

			WebServiceReturnStatus result = req.request(call);
			return result;
		}
		catch(HttpErrorCode code)
		{
			Debug.Log(code.code + " http error");
			throw;
		}
	}

	public void doTranslation(TranslationConfigurationSO selectedConfig)
	{
		List<int> destLanguageIDs = new List<int>();
		GameTranslationSet set = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(selectedConfig.sourceLanguage.code);
		var keysToTranslate = set.getGroup(selectedConfig.translation_set_group).getDictionaryCopy();
		List<string> textsToTranslate = new List<string>(keysToTranslate.Keys);

		//save all of our keys before requesting to transalate them, otherwise we can get errors
		var uploadAll = new SaveSetOfKeys(selectedConfig.sourceLanguage.id,
			keysToTranslate,
			selectedConfig.translation_set_group
			);
		doCall(uploadAll);

		selectedConfig.destinationLanguages.ForEach((TransfluentLanguage lang) => { destLanguageIDs.Add(lang.id); });
		var translate = new OrderTranslation(selectedConfig.sourceLanguage.id,
				target_languages: destLanguageIDs.ToArray(),
				texts: textsToTranslate.ToArray(),
				level: selectedConfig.QualityToRequest,
				group_id: selectedConfig.translation_set_group
				);
		doCall(translate);
	}
}