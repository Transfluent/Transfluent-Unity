using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

//test just the parsing of messages

namespace transfluent.tests
{
	[TestFixture]
	public class TestCallParsers
	{
		private readonly Dictionary<Type, WebServiceParameters> allParsers = new Dictionary<Type, WebServiceParameters>();

		[TestFixtureSetUp]
		public void IniitalSetup()
		{
			setupDictionaryOfAllAvailableClasses();
		}

		//[Test]
		public void setupDictionaryOfAllAvailableClasses()
		{
			addToDicitonary(new CreateAccount("email", true, false));

			addToDicitonary(new GetAllExistingTranslationKeys(1));
			addToDicitonary(new GetAllOrders());
			addToDicitonary(new GetTextKey("TESTKEY", 1));
			addToDicitonary(new Hello("world"));
			addToDicitonary(new Login("lll", "lll"));
			addToDicitonary(new OrderTranslation(1, new[] { 500 }, new[] { "NONE" }));
			addToDicitonary(new RequestAllLanguages());
			addToDicitonary(new SaveTextKey("DOES_NOT_EXIST", 1, "BLAH"));
			addToDicitonary(new TextStatus(1, "TEST"));

			Type parentClassOfWebservices = typeof(WebServiceParameters);
			IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(p => parentClassOfWebservices.IsAssignableFrom(p));
			foreach(Type type1 in types)
			{
				if(type1 == parentClassOfWebservices)
					continue;
				Assert.Contains(type1, allParsers.Keys);
			}
		}

		private void addToDicitonary(WebServiceParameters call)
		{
			var context = new InjectionContext();
			context.addMapping<IResponseReader>(new ResponseReader());
			context.addNamedMapping<string>(NamedInjections.API_TOKEN, "faketoken");
			context.setMappings(call);
			allParsers.Add(call.GetType(), call);
		}

		private string getTestFile(string filename)
		{
			string jsonFileBasePath = "Assets/TransfluentTests/Editor/Data/JsonResponses/";
			var textAsset = AssetDatabase.LoadAssetAtPath(jsonFileBasePath + filename + ".txt", typeof(TextAsset)) as TextAsset;
			return textAsset.text;
		}

		private T getType<T>() where T : class
		{
			return allParsers[typeof(T)] as T;
		}

		//should I wrap JsonTypeCoercionException with an app-specific exception?
		//In theory, the parse()
		[Test]
		[ExpectedException(typeof(ApplicatonLevelException))]
		public void TestAuthenticateFailure()
		{
			var login = getType<Login>();
			string file = getTestFile("LoginFailure");
			try
			{
				login.Parse(file);
				//AuthenticationResponse result = login.Parse(file);
			}
			catch(ApplicatonLevelException e)
			{
				Assert.NotNull(e.details);
				Assert.AreEqual(e.details.type, "EBackendSecurityViolation");
			}
			AuthenticationResponse result2 = login.Parse(file);
			Assert.IsNull(result2);
		}

		[Test]
		public void TestAuthenticateOK()
		{
			var login = getType<Login>();

			string file = getTestFile("LoginOK");

			AuthenticationResponse result = login.Parse(file);

			Assert.NotNull(result);
			Assert.NotNull(result.token);
			Assert.NotNull(result.expires);
		}

		[Test]
		public void TestFileLoad()
		{
			string sampleResponse =
				@"{""status"":""OK"",""response"":{""token"":""ABABABABABABABABABABABABABABABABABABABABABABABABABABABABABABABABABA..."", ""expires"":""On password change""}}";
			string fromFile = getTestFile("LoginOK");
			Assert.IsNotNull(fromFile);
			Assert.AreEqual(sampleResponse, fromFile);
		}
	}
}