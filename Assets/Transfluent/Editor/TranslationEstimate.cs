using System;
using System.Collections.Generic;
using System.Text;
using transfluent;
using UnityEditor;
using UnityEngine;
using transfluent.editor;

public class TranslationEstimate
{
	public string _token;
	private readonly TransfluentEditorWindowMediator _mediator;

	public TranslationEstimate(TransfluentEditorWindowMediator mediator)
	{
		_mediator = mediator;
	}

	void doAuth()
	{
		_mediator.doAuth();
		string authToken = _mediator.getCurrentAuthToken();
		if(string.IsNullOrEmpty(authToken))
		{
			//TODO: xx-xx only?
			EditorUtility.DisplayDialog("Log in", " Please provide your transfluent credentials to order a translation","OK");
			throw new Exception("Auth token is null");
		}
		_token = authToken;
	}
	public void presentEstimateAndMakeOrder(TranslationConfigurationSO selectedConfig)
	{
		var languageEstimates = new Dictionary<TransfluentLanguage, EstimateTranslationCostVO.Price>();
		if(GUILayout.Button("Translate"))
		{
			doAuth();
			if(string.IsNullOrEmpty(_token))
			{
				return;
			}

			List<string> allLanguageCodes = new List<string>();
			allLanguageCodes.Add(selectedConfig.sourceLanguage.code);
			selectedConfig.destinationLanguages.ForEach((TransfluentLanguage lang)=>{allLanguageCodes.Add(lang.code);});
			DownloadAllGameTranslations.uploadTranslationSet(allLanguageCodes, selectedConfig.translation_set_group);

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
			long sourceSetWordCount = 0;
			foreach (KeyValuePair<string, string> kvp in toTranslate)
			{
				sourceSetWordCount += kvp.Value.Split(' ').Length;
			}
			
			//var knownKeys = sourceSet.getPretranslatedKeys(sourceSet.getAllKeys(), selectedConfig.translation_set_group);
			//var sourceDictionary = sourceSet.getGroup().getDictionaryCopy();
			foreach(TransfluentLanguage lang in selectedConfig.destinationLanguages)
			{
				if(lang.code == "xx-xx")
				{
					simpleEstimateString.AppendFormat("language: {0} est cost: {1}\n", lang.name, "FREE!");
					continue;
				}
				var set = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(lang.code);
				long alreadyTranslatedWordCount = 0;

				if(set != null)
				{
					var destKeys = set.getGroup(group).getDictionaryCopy();
					foreach(KeyValuePair<string, string> kvp in toTranslate)
					{
						if(!destKeys.ContainsKey(kvp.Key))
							alreadyTranslatedWordCount += kvp.Value.Split(' ').Length;
					}
				}

				var oneWordPrice = languageEstimates[lang];
				float costPerWord = float.Parse(oneWordPrice.amount);
				long toTranslateWordcount = sourceSetWordCount - alreadyTranslatedWordCount;
				if(toTranslateWordcount < 0) toTranslateWordcount *= -1;

				float totalCost = costPerWord * toTranslateWordcount;
				
				simpleEstimateString.AppendFormat("language: {0} est cost: {1}\n", lang.name, totalCost);
				//	lang.name, totalCost, oneWordPrice.currency, costPerWord, toTranslateWordcount);
				//simpleEstimateString.AppendFormat("language name: {0} total cost: {1} {2} \n\tCost per word:{3} total words to translate:{4} ",
				//	lang.name, totalCost, oneWordPrice.currency, costPerWord, toTranslateWordcount);
			}

			Debug.Log("Estimated prices");
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
			call.getParameters.Add("token", _token);

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
				group_id: selectedConfig.translation_set_group,
				comment: "Do not replace any strings that look like {0} or {1} as they are a part of formatted text -- ie Hello {0} will turn into Hello Alex or some other string "
				);
		doCall(translate);
	}
}