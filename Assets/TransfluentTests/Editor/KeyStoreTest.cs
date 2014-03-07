using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace transfluent.tests
{
	[TestFixture]
	internal class KeyStoreTest
	{
		private Dictionary<string, string> dictionary;
		private List<string> keys;
		private List<string> values;
			
		[TestFixtureSetUp]
		public void oneTime()
		{
			
		}

		[SetUp]
		public void setup()
		{
			//TODO: set up quick file
			dictionary = new Dictionary<string, string>()
			{
				{"TEST_KEY_FROM_TESTERVILLE","username@company.com"},
				{"THIS_KEY_WILL_ALSO_NOT_EXIST_AS_A_PASSWORDs","thisisabadpassword"},
			};
			keys = new List<string>();
			values = new List<string>();

			foreach(KeyValuePair<string, string> kvp in dictionary)
			{
				keys.Add(kvp.Key);
				values.Add(kvp.Value);
			}
		}

		StreamReader getMemoryStream(string input)
		{
			byte[] bytes =  Encoding.UTF8.GetBytes(input);
			var mem = new MemoryStream(bytes);
			var reader = new StreamReader(mem);
			return reader;  //Why you readin?  ... we got ourselves a reader here
		}
		[Test]
		public void testFileStreamKeyStore()
		{
			string valueStringNewlineDelimited = string.Join("\n", values.ToArray());
			var reader = getMemoryStream(valueStringNewlineDelimited);

			var store = new FileLineBasedKeyStore(reader,keys);
			foreach (KeyValuePair<string, string> kvp in dictionary)
			{
				Assert.AreEqual(store.get(kvp.Key),dictionary[kvp.Key]);
			}
			standardCheck(store,dictionary); //try setting then retrieving
			//Debug.Log(string.Format("TEST: username:{0} password:{1}",store.get("username"),store.get("password")));
		}

		[Test]
		public void testEditorKey()
		{
			var store = new EditorKeyStore();

			standardCheck(store, dictionary);
		}
		[Test]
		public void testPlayerPrefsKey()
		{
			var store = new PlayerPrefsKeyStore();

			standardCheck(store, dictionary);
		}

		[Test]
		public void inMemoryKeyStoreCheck()
		{
			var store = new InMemoryKeyStore();
			standardCheck(store,dictionary);
		}

		[Test]
		public void testEqualityOfInMemoryDictionary()
		{
			var inMemory = new InMemoryKeyStore();
			var playerprefs = new PlayerPrefsKeyStore();
			var editor = new EditorKeyCredentialProvider();
			var fileProvider = new FileBasedCredentialProvider()
		}

		void standardCheck(IKeyStore store, Dictionary<string, string> keyValueMap)
		{
			foreach(KeyValuePair<string, string> kvp in keyValueMap)
			{
				store.set(kvp.Key, kvp.Value);
			}
			//make sure that it wasn't just giving you the last result put in
			foreach(KeyValuePair<string, string> kvp in keyValueMap)
			{
				Assert.AreEqual(store.get(kvp.Key), keyValueMap[kvp.Key]);
			}
		}
	}
}
