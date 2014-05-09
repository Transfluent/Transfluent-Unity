using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Pathfinding.Serialization.JsonFx;
using transfluent.editor;
using UnityEngine;
using UnityEditor;

namespace transfluent
{
	public class FileBasedSend
	{
		//TODO: how to encode content so that group ids are handled
		public string SendFileContents(Dictionary<string,string> keys,TransfluentLanguage sourceLanguage, string groupid,string comment )
		{//<?xml version=""1.0"" encoding=""UTF-8"" ?>
			string headerFormat = @"<?xml version=""1.0"" encoding=""UTF-8"" ?>
<locale>
	<id>{0}</id>
	<code>{1}</code>
	<name>{2}</name>
";
			//ie 148, en-us, "English US"
			string header = string.Format(headerFormat, sourceLanguage.id, sourceLanguage.code, sourceLanguage.name);
			string footer = "</locale>";

			string textsFormat = "<texts>{0}</texts>";
			string individualTextFormat = @"<text>
			<textId>{0}</textId>
			<textString>{1}</textString>
		</text>
";
			StringBuilder sb = new StringBuilder();
			foreach (KeyValuePair<string, string> kvp in keys)
			{
				sb.AppendFormat(individualTextFormat, kvp.Key, kvp.Value);
			}
			
			string texts = string.Format(textsFormat, sb.ToString());


			string contentToSend = string.Format("{0}\n{1}\n{2}", header, texts, footer);

			return contentToSend;
		}

		[MenuItem("Transfluent/test full loop")]
		public static void testLoop()
		{
			var list = ResourceLoadFacade.getLanguageList();
			GameTranslationSet source = GameTranslationGetter.GetTranslaitonSetFromLanguageCode("en-us");
			var sender = new FileBasedSend();
			var contents = sender.SendFileContents(source.getGroup().getDictionaryCopy(), list.getLangaugeByCode("en-us"), "", "");

			sender.RecieveFile(contents);
		}

		[MenuItem("Transfluent/test get content")]
		public static void getTestContent()
		{
			var list = ResourceLoadFacade.getLanguageList();
			GameTranslationSet source = GameTranslationGetter.GetTranslaitonSetFromLanguageCode("en-us");
			var sender = new FileBasedSend();
			var contents = sender.SendFileContents(source.getGroup().getDictionaryCopy(), list.getLangaugeByCode("en-us"), "", "");
			Debug.Log("Contents :" + contents);
		}

		[MenuItem("Transfluent/testInput")]
		public static void DoTestInput()
		{
			string filePath = Application.dataPath + Path.DirectorySeparatorChar + "TransfluentLocalization_en-us.xml";
			string contents = File.ReadAllText(filePath);
			var sender = new FileBasedSend();
			sender.RecieveFile(contents);
		}

		[MenuItem("Transfluent/test full loop english content")]
		public static void getTestSaveEnglishContent()
		{
			var list = ResourceLoadFacade.getLanguageList();
			GameTranslationSet source = GameTranslationGetter.GetTranslaitonSetFromLanguageCode("en-us");
			var sender = new FileBasedSend();
			var contents = sender.SendFileContents(source.getGroup().getDictionaryCopy(), list.getLangaugeByCode("en-us"), "", "");
			Debug.Log("Contents :" + contents);

			TransfluentEditorWindowMediator mediator = new TransfluentEditorWindowMediator();
			mediator.doAuth();
			string authToken = mediator.getCurrentAuthToken();
			string fileIdentifier = "testfile";
			var sourceLang = list.getLangaugeByCode("en-us");
			
			var saveCall = new FileBasedSaveCall(fileIdentifier, sourceLang.id,authToken ,contents);
			var caller = new SyncronousEditorWebRequest();
			var returnStatus = caller.request(saveCall);

			Debug.Log("saved file:");
			Debug.Log(JsonWriter.Serialize(returnStatus));
			Debug.Log("auth token:" + authToken);
			var translateRequest = new FileTranslate("Do not modify strings like {0} as they are dynamic text", new int[] {3,4},
				OrderTranslation.TranslationQuality.NATIVE_SPEAKER,fileIdentifier,sourceLang.id,authToken );
			var translateReturn = caller.request(translateRequest);

			Debug.Log("translate request file:");
			Debug.Log(JsonWriter.Serialize(translateReturn));

			var translateResultRequest = new FileBasedRead(fileIdentifier, sourceLang.id, authToken);
			var translateResultReturn = caller.request(translateResultRequest);
			Debug.Log("translate resulting file:");
			Debug.Log(JsonWriter.Serialize(translateResultReturn));
		}
		public void RecieveFile(string text)
		{
			/*
			string languageidPattern = "<id>(.*)</id>";
			var languageidStringMatch = Regex.Match(text, languageidPattern);
			int langaugeid = int.Parse(languageidStringMatch.Groups[1].ToString());
			Debug.Log("language id : " + langaugeid);
			 */

			//string textField = "";
			//var matches = Regex.Match(String.Format("{0}-{1}", "127.0.0.1", "192.168.0.1"), "(?<startIP>.*)-(?<endIP>.*)");
			var matches = Regex.Matches(text, @"<text>\s*<textId>(.*)</textId>\s*<textString>(.*)</textString>\s*</text>",RegexOptions.Multiline);
			foreach (Match match in matches)
			{
				Debug.Log(string.Format("{0} {1}",match.Groups[1],match.Groups[2]));
			}
			/* @"(}(?<formatNumber>\d+){)";
			var rx = new Regex(pattern, RegexOptions.Multiline);
			string result = rx.Replace(inputString, "{$2}");
			return result;*/
		}
	}
	//string comment, int[] target_languages,OrderTranslation.TranslationQuality quality
	//format [=UTF-8], content, type, save_only_data, identifier, language, token
	[Route("file/save", RestRequestType.POST, "http://transfluent.com/backend-api/#FileSave")]
	public class FileBasedSaveCall : WebServiceParameters
	{

		public FileBasedSaveCall( string file_identifier_should_be_group_id,
			int sourceLanguage,string token,string content)
		{
			getParameters.Add("type", "XML-file");
			getParameters.Add("save_only_data","true");
			getParameters.Add("identifier", file_identifier_should_be_group_id);
			getParameters.Add("language",sourceLanguage.ToString());
			getParameters.Add("token",token);

			postParameters.Add("content", getbase64(content));
		}

		string getbase64(string content)
		{
			var bytes = Encoding.UTF8.GetBytes(content);
			var base64 = Convert.ToBase64String(bytes);
			return base64;
		}
	}

	//callback_url, comment, target_languages, level [=3], identifier, language, token
	[Route("file/translate", RestRequestType.POST, "http://transfluent.com/backend-api/#FileTranslate")]
	public class FileTranslate : WebServiceParameters
	{
		public FileTranslate(string comment, int[] target_languages,OrderTranslation.TranslationQuality quality,
			string file_identifier_should_be_group_id,int sourceLanguage, string token)
		{
			postParameters.Add("ignore_me","ignoreme");
			getParameters.Add("callback_url", "http://www.yahoo.com");
			getParameters.Add("comment",comment);
			getParameters.Add("level",((int)quality).ToString());
			getParameters.Add("identifier", file_identifier_should_be_group_id);
			getParameters.Add("language",sourceLanguage.ToString());
			getParameters.Add("token",token);
			getParameters.Add("target_languages", JsonWriter.Serialize(target_languages));

		}
	}
	[Route("file/status", RestRequestType.POST, "http://transfluent.com/backend-api/#FileStatus")]
	public class FileStatus : WebServiceParameters
	{
		public FileStatus(string comment, int[] target_languages, OrderTranslation.TranslationQuality quality,
			string file_identifier_should_be_group_id, int sourceLanguage, string token)
		{
			getParameters.Add("callback_url", "http://www.yahoo.com");
			getParameters.Add("comment", comment);
			getParameters.Add("level", ((int)quality).ToString());
			getParameters.Add("identifier", file_identifier_should_be_group_id);
			getParameters.Add("language", sourceLanguage.ToString());
			getParameters.Add("token", token);
			getParameters.Add("target_languages", JsonWriter.Serialize(target_languages));
		}
		//"progress":"11.49%","word_count":87,"word_count_translated":10
		public class FileStatusResponse
		{
			public string progress;
			public int word_count;
			public int word_count_translated;
		}
	}

	//NOTE: not currently supporting the Content-Disposition: attachment; filename=
	//header download
	//identifier, language, token
	[Route("file/read", RestRequestType.POST, "http://transfluent.com/backend-api/#FileRead")]
	public class FileBasedRead : WebServiceParameters
	{

		public FileBasedRead(string file_identifier_should_be_group_id,
			int sourceLanguage, string token)
		{
			getParameters.Add("identifier", file_identifier_should_be_group_id);
			getParameters.Add("language", sourceLanguage.ToString());
			getParameters.Add("token", token);
		}


		//undocumented.  specific to input probably.  not sure if it's just the raw response or if it's encoded
		public class FileReadResponse
		{
			
		}
	}

}