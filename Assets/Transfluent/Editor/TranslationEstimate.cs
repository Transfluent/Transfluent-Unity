using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using transfluent.editor;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace transfluent
{
	public class TranslationEstimate
	{
		public string _token;
		private readonly TransfluentEditorWindowMediator _mediator;

		public TranslationEstimate(TransfluentEditorWindowMediator mediator)
		{
			_mediator = mediator;
		}

		public int numberOfMissingTranslationsBetweenLanguages(TransfluentLanguage sourceLang, TransfluentLanguage destLang, string groupid)
		{
			var sourceSet = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(sourceLang.code);
			var destSet = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(destLang.code);

			var sourceGroup = sourceSet.getGroup(groupid);
			var sourceKeys = new List<string>(sourceGroup.getDictionaryCopy().Keys);
			var destKeys = new List<string>();
			if(destSet != null)
			{
				destKeys.AddRange(destSet.getGroup(groupid).getDictionaryCopy().Keys);
			}
			return numberOfMissingKeysFromLists(sourceKeys, destKeys);
		}

		public int numberOfMissingKeysFromLists(List<string> sourceList, List<string> listWithPotentiallyMissingKeys)
		{
			int numberOfMissingKeys = 0;
			foreach(string sourceKey in sourceList)
			{
				if(!listWithPotentiallyMissingKeys.Contains(sourceKey))
				{
					numberOfMissingKeys++;
				}
			}
			return numberOfMissingKeys;
		}

		private void doAuth()
		{
			_mediator.doAuth();
			string authToken = _mediator.getCurrentAuthToken();
			if(string.IsNullOrEmpty(authToken))
			{
				//TODO: xx-xx only?
				EditorUtility.DisplayDialog("Log in", " Please provide your transfluent credentials to order a translation", "OK");
				throw new Exception("Auth token is null");
			}
			_token = authToken;
		}

		private OrderFlowAsync _asyncFlow = null;

		public void presentEstimateAndMakeOrder(TranslationConfigurationSO selectedConfig)
		{
			//var languageEstimates = new Dictionary<TransfluentLanguage, EstimateTranslationCostVO.Price>();
			if(_asyncFlow != null)
			{
				if(!_asyncFlow.orderIsDone())
				{
					if(GUILayout.Button("ORDERING..."))
					{
					};
					return;
				}
			}

			EstimateTranslationCostVO.Price costPerWordFromSourceLanguage = null;
			//Debug.Log("ASYNC FLOW:"+(_asyncFlow != null).ToString() + " isdone:"+(_asyncFlow != null && _asyncFlow.orderIsDone()));
			if(GUILayout.Button("Translate"))
			{
				doAuth();
				if(string.IsNullOrEmpty(_token))
				{
					return;
				}

				string group = selectedConfig.translation_set_group;
				var sourceSet = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(selectedConfig.sourceLanguage.code);
				if(sourceSet == null || sourceSet.getGroup(group) == null)
				{
					EditorUtility.DisplayDialog("ERROR", "No messages in group", "OK");
					return;
				}

				StringBuilder simpleEstimateString = new StringBuilder();

				//find the first language that returns a result for "hello" and use that for the cost
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
						costPerWordFromSourceLanguage = estimate.price;
						//simpleEstimateString.Append(printedEstimate);
						//Debug.Log("Estimate:" + JsonWriter.Serialize(estimate));
						break;
					}
					catch(Exception e)
					{
						Debug.LogError("Error estimating prices: "+e);
					}
				}

				var toTranslate = sourceSet.getGroup(group).getDictionaryCopy();
				long sourceSetWordCount = 0;
				foreach(KeyValuePair<string, string> kvp in toTranslate)
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

					var oneWordPrice = costPerWordFromSourceLanguage;
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
					_asyncFlow = new OrderFlowAsync(selectedConfig, _token);
					_asyncFlow.startFlow();
					//doTranslation(selectedConfig);
				}
			}
		}

		private WebServiceReturnStatus doCall(WebServiceParameters call)
		{
			IWebService req = new SyncronousEditorWebRequest();
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

		public void doTranslation2(TranslationConfigurationSO selectedConfig)
		{
			OrderFlowAsync orderFlow = new OrderFlowAsync(selectedConfig, _token);
			orderFlow.startFlow();
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
			Stopwatch sw = new Stopwatch();
			sw.Start();
			var translate = new OrderTranslation(selectedConfig.sourceLanguage.id,
					target_languages: destLanguageIDs.ToArray(),
					texts: textsToTranslate.ToArray(),
					level: selectedConfig.QualityToRequest,
					group_id: selectedConfig.translation_set_group,
					comment: "Do not replace any strings that look like {0} or {1} as they are a part of formatted text -- ie Hello {0} will turn into Hello Alex or some other string "
					);
			doCall(translate);
			Debug.Log("full request time:" + sw.Elapsed);
		}
	}

	public class OrderFlowAsync
	{
		private bool _orderDone = false;
		/*
		[MenuItem("asink/wwwtest")]
		public static void wwwtest()
		{
			var wait = new EditorWWWWaitUntil(new WWW("http://www.yahoo.com"), (WWW val)=> { Debug.Log(val.text); });
		}
		*/

		public bool orderIsDone()
		{
			return _orderDone;
		}

		private TranslationConfigurationSO _selectedConfig;
		private string _token;

		public OrderFlowAsync(TranslationConfigurationSO selectedConfig, string token)
		{
			_selectedConfig = selectedConfig;
			_token = token;
		}

		public void startFlow()
		{
			saveMySourceText();
		}

		private void saveMySourceText()
		{
			GameTranslationSet set = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(_selectedConfig.sourceLanguage.code);
			var keysToTranslate = set.getGroup(_selectedConfig.translation_set_group).getDictionaryCopy();

			//save all of our keys before requesting to transalate them, otherwise we can get errors
			var uploadAll = new SaveSetOfKeys(_selectedConfig.sourceLanguage.id,
				keysToTranslate,
				_selectedConfig.translation_set_group
				);

			doCall(uploadAll, saveMyDestinationLanguageText);
		}

		private void saveMyDestinationLanguageText()
		{
			List<WebServiceParameters> requestsToSaveLocalStrings = new List<WebServiceParameters>();

			foreach(TransfluentLanguage lang in _selectedConfig.destinationLanguages)
			{
				GameTranslationSet set = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(lang.code);
				if(set == null)
				{
					continue;
				}
				var keysToTranslate = set.getGroup(_selectedConfig.translation_set_group).getDictionaryCopy();
				//save all of our keys before requesting to transalate them, otherwise we can get errors
				var uploadAll = new SaveSetOfKeys(lang.id,
					keysToTranslate,
					_selectedConfig.translation_set_group
					);
				requestsToSaveLocalStrings.Add(uploadAll);
			}
			doCalls(requestsToSaveLocalStrings, ordermytranslation);
		}

		private void doCalls(List<WebServiceParameters> calls, Action callback)
		{
			if(calls.Count == 0)
			{
				if(callback != null)
				{
					callback();
				}
				return;
			}
			var nextCall = calls[0];
			calls.RemoveAt(0);
			doCall(nextCall, () => doCalls(calls, callback));
		}

		private void doCall(WebServiceParameters call, Action callback)
		{
			call.getParameters.Add("token", _token);
			new EditorWWWWaitUntil(call, (WebServiceReturnStatus status) =>
				{
					if(status.httpErrorCode > 0 && status.httpErrorCode != 200)
					{
						EditorUtility.DisplayDialog("ERROR", "Error while ordering translation. Please try again.", "OK");
					}
					if(callback != null)
					{
						callback();
					}
				}
			);
		}

		public void ordermytranslation()
		{
			//just queue up all the calls until first real stopping point
			List<int> destLanguageIDs = new List<int>();
			GameTranslationSet set = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(_selectedConfig.sourceLanguage.code);
			var keysToTranslate = set.getGroup(_selectedConfig.translation_set_group).getDictionaryCopy();
			List<string> textsToTranslate = new List<string>(keysToTranslate.Keys);

			_selectedConfig.destinationLanguages.ForEach((TransfluentLanguage lang) => { destLanguageIDs.Add(lang.id); });
			Stopwatch sw = new Stopwatch();
			sw.Start();
			var translate = new OrderTranslation(_selectedConfig.sourceLanguage.id,
					target_languages: destLanguageIDs.ToArray(),
					texts: textsToTranslate.ToArray(),
					level: _selectedConfig.QualityToRequest,
					group_id: _selectedConfig.translation_set_group,
					comment: "Do not replace any strings that look like {0} or {1} as they are a part of formatted text -- ie Hello {0} will turn into Hello Alex or some other string "
					);
			doCall(translate, () =>
			{
				Debug.Log("ORDER DONE");
				_orderDone = true; EditorUtility.DisplayDialog("Success", "Transltion order complete!", "OK");
			});
		}
	}
}