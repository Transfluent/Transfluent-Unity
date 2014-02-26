using System.Collections.Generic;
using NUnit.Framework;
using Pathfinding.Serialization.JsonFx;
using transfluent;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

[TestFixture]
public class TestLoginFlow
{
	public interface ICredentialProvider
	{
		string username { get; }
		string password { get; }
	}

	public class FileBasedCredentialProvider : ICredentialProvider
	{
		private readonly string LocationOfTestCredentials = "Assets/TransfluentTests/Editor/Data/loginPassword.txt";
		public string username { get; protected set; }
		public string password { get; protected set; }

		public FileBasedCredentialProvider()
		{
			TextAsset textAsset = AssetDatabase.LoadAssetAtPath(LocationOfTestCredentials, typeof(TextAsset)) as TextAsset;
			string[] lines = textAsset.text.Split(new[] { '\r', '\n' });
			username = lines[0];
			password = lines[1];
		}
	}
	public class EditorKeyCredentialProvicer : ICredentialProvider
	{
		public const string USERNAME_EDITOR_KEY = "TRANSFLUENT_USERNAME_KEY";
		public const string PASSWORD_EDITOR_KEY = "PASSWORD_EDITOR_KEY";
		public string username { get; protected set; }
		public string password { get; protected set; }

		public EditorKeyCredentialProvicer()
		{
			username = EditorPrefs.GetString(USERNAME_EDITOR_KEY);
			password = EditorPrefs.GetString(PASSWORD_EDITOR_KEY);
		}
	}
	public static string baseServiceUrl = "https://transfluent.com/v2/";
	public static string requestedService = "authenticate";
	public string url = baseServiceUrl + requestedService;
	private IWebService service;
	public FileBasedCredentialProvider Provider;

	[TestFixtureSetUp]
	public void getTestCredentials()
	{
		loadTestCredentials	();
	}

	[Test]
	public void loadTestCredentials()
	{
		Provider = new FileBasedCredentialProvider();
		Assert.IsFalse(string.IsNullOrEmpty(Provider.username));
		Assert.IsFalse(string.IsNullOrEmpty(Provider.password));
	}

	[SetUp]
	[TearDown]
	public void notUsedRightNow()
	{
		service = new SyncronousEditorWebRequest();
	}

	[Test]
	[MaxTime(2000)]
	public void noPostLogin()
	{
		ReturnStatus status = service.request(url); //no password params!
		Assert.IsTrue(status.status == ServiceStatus.APPLICATION_ERROR);
	}

	[Test]
	[MaxTime(2000)]
	public void wrongPasswordLogin()
	{
		ReturnStatus status = service.request(url, new Dictionary<string, string>()
		{
			{"email", Provider.username},
			{"password", "thisPasswordIsWrong"}
		});
		
		Assert.IsTrue(status.status == ServiceStatus.APPLICATION_ERROR);
	}

	[Test]
	[MaxTime(2000)]
	public void correctLoginTest()
	{
		ReturnStatus status = service.request(url, new Dictionary<string, string>()
		{
			{"email", Provider.username},
			{"password", Provider.password}
		});
		Assert.IsTrue(status.status == ServiceStatus.SUCCESS);

		string responseText = status.text;
		Assert.IsNotNull(responseText);
		Assert.IsTrue(responseText.Length > 0);
		
		var container = JsonReader.Deserialize <ResponseContainer<AuthenticationResponse>>(responseText);
		
		Assert.IsNotNull(container);
		Assert.IsNull(container.error);

		Assert.IsNotNull(container.response);
		Assert.IsFalse(string.IsNullOrEmpty(container.response.token));
		Assert.IsFalse(string.IsNullOrEmpty(container.response.expires));
	}

}