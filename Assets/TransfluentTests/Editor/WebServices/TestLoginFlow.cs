using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using Pathfinding.Serialization.JsonFx;
using transfluent.editor;

namespace transfluent.tests
{
	[TestFixture]
	public class TestLoginFlow
	{
		private const int MAX_MILLISECONDS_TO_WAIT = 20000;
		[SetUp]
		[TearDown]
		public void notUsedRightNow()
		{
			service = new SyncronousEditorWebRequest();
		}

		public static string baseServiceUrl = "https://transfluent.com/v2/";
		public static string requestedService = "authenticate";
		public string loginUrl = baseServiceUrl + requestedService;
		private IWebService service;
		public FileBasedCredentialProvider Provider;

		[TestFixtureSetUp]
		public void getTestCredentials()
		{
			loadTestCredentials();
		}

		[Test]
		[MaxTime(MAX_MILLISECONDS_TO_WAIT)]
		public void correctLoginTest()
		{
			WebServiceReturnStatus status = service.request(loginUrl, new Dictionary<string, string>
			{
				{"email", Provider.username},
				{"password", Provider.password}
			});

			string responseText = status.text;
			Assert.IsNotNull(responseText);
			Assert.IsTrue(responseText.Length > 0);

			var container = JsonReader.Deserialize<ResponseContainer<AuthenticationResponse>>(responseText);

			Assert.IsNotNull(container);
			Assert.IsNull(container.error);

			Assert.IsNotNull(container.response);
			Assert.IsFalse(string.IsNullOrEmpty(container.response.token));
			Assert.IsFalse(string.IsNullOrEmpty(container.response.expires));
		}

		[Test]
		[MaxTime(MAX_MILLISECONDS_TO_WAIT)]
		[ExpectedException(typeof(ApplicatonLevelException))]
		public void emptyLoginPasswordPost()
		{
			service.request(loginUrl, new Dictionary<string, string>
			{
				{"email", ""},
				{"password", ""}
			});
		}
		[Test]
		[MaxTime(MAX_MILLISECONDS_TO_WAIT)]
		public void makeSureApplicationLevelExceptionIsACallException()
		{
			Assert.Catch<CallException>(()=>service.request(loginUrl, new Dictionary<string, string>
			{
				{"email", ""},
				{"password", ""}
			}));
		}

		[Test]
		public void loadTestCredentials()
		{
			Provider = new FileBasedCredentialProvider();
			Assert.IsFalse(string.IsNullOrEmpty(Provider.username));
			Assert.IsFalse(string.IsNullOrEmpty(Provider.password));
		}

		[Test]
		[MaxTime(MAX_MILLISECONDS_TO_WAIT)]
		[ExpectedException(typeof(ApplicatonLevelException))]
		public void noPostLogin()
		{
			service.request(loginUrl); //no password params!
		}

		[Test]
		[MaxTime(MAX_MILLISECONDS_TO_WAIT)]
		[ExpectedException(typeof(ApplicatonLevelException))]
		public void wrongPasswordLogin()
		{
			service.request(loginUrl, new Dictionary<string, string>
			{
				{"email", Provider.username},
				{"password", "thisPasswordIsWrong"}
			});
		}
	}
}