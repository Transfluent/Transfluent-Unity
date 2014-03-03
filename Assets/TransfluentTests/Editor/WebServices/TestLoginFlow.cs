using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using Pathfinding.Serialization.JsonFx;

namespace transfluent.tests
{
	[TestFixture]
	public class TestLoginFlow
	{
		[SetUp]
		[TearDown]
		public void notUsedRightNow()
		{
			service = new SyncronousEditorWebRequest();
		}

		public static string baseServiceUrl = "https://transfluent.com/v2/";
		public static string requestedService = "authenticate";
		public string url = baseServiceUrl + requestedService;
		private IWebService service;
		public FileBasedCredentialProvider Provider;

		[TestFixtureSetUp]
		public void getTestCredentials()
		{
			loadTestCredentials();
		}

		[Test]
		[MaxTime(10000)]
		public void correctLoginTest()
		{
			WebServiceReturnStatus status = service.request(url, new Dictionary<string, string>
			{
				{"email", Provider.username},
				{"password", Provider.password}
			});
			Assert.IsTrue(status.status == ServiceStatus.SUCCESS);

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
		[MaxTime(10000)]
		[ExpectedException(typeof(ApplicatonLevelException))]
		public void emptyLoginPasswordPost()
		{
			service.request(url, new Dictionary<string, string>
			{
				{"email", ""},
				{"password", ""}
			});
		}
		[Test]
		[MaxTime(10000)]
		[ExpectedException(typeof(CallException))]
		public void makeSureApplicationLevelExceptionIsACallException()
		{
			service.request(url, new Dictionary<string, string>
			{
				{"email", ""},
				{"password", ""}
			});
		}

		[Test]
		public void loadTestCredentials()
		{
			Provider = new FileBasedCredentialProvider();
			Assert.IsFalse(string.IsNullOrEmpty(Provider.username));
			Assert.IsFalse(string.IsNullOrEmpty(Provider.password));
		}

		[Test]
		[MaxTime(10000)]
		[ExpectedException(typeof(ApplicatonLevelException))]
		public void noPostLogin()
		{
			WebServiceReturnStatus status = service.request(url); //no password params!
		}

		[Test]
		[MaxTime(10000)]
		[ExpectedException(typeof(ApplicatonLevelException))]
		public void wrongPasswordLogin()
		{
			WebServiceReturnStatus status = service.request(url, new Dictionary<string, string>
			{
				{"email", Provider.username},
				{"password", "thisPasswordIsWrong"}
			});
			Assert.IsTrue(status.status == ServiceStatus.APPLICATION_ERROR);
		}
	}
}