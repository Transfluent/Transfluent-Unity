using Castle.Core.Internal;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text;
using transfluent.editor;
using UnityEngine;

namespace transfluent.tests
{
	[TestFixture]
	internal class KeyStoreTest
	{
		[SetUp]
		public void setup()
		{
			dictionary = new Dictionary<string, string>
			{
				{"TEST_KEY_FROM_TESTERVILLE", "username@company.com"},
				{"THIS_KEY_WILL_ALSO_NOT_EXIST_AS_A_PASSWORDs", "thisisabadpassword"},
			};
			keys = new List<string>();
			values = new List<string>();

			foreach(var kvp in dictionary)
			{
				keys.Add(kvp.Key);
				values.Add(kvp.Value);
			}
		}

		private Dictionary<string, string> dictionary;
		private List<string> keys;
		private List<string> values;

		[TestFixtureSetUp]
		public void oneTime()
		{
		}

		private StreamReader getMemoryStream(string input)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(input);
			var mem = new MemoryStream(bytes);
			var reader = new StreamReader(mem);
			return reader; //Why you readin?  ... we got ourselves a reader here
		}

		private void standardCheck(IKeyStore store, Dictionary<string, string> keyValueMap)
		{
			foreach(var kvp in keyValueMap)
			{
				store.set(kvp.Key, kvp.Value);
			}
			//make sure that it wasn't just giving you the last result put in by iterating over the entire dictionary again
			foreach(var kvp in keyValueMap)
			{
				Assert.AreEqual(store.get(kvp.Key), keyValueMap[kvp.Key]);
			}
		}

		[Test]
		public void inMemoryKeyStoreCheck()
		{
			var store = new InMemoryKeyStore();
			standardCheck(store, dictionary);
		}

		[Test]
		public void testEditorKey()
		{
			var store = new EditorKeyStore();

			standardCheck(store, dictionary);
		}

		[Test]
		public void testEqualityOfInMemoryDictionary()
		{
			var randomvalues = new Dictionary<string, string>
			{
				{"KEYYYYYYY" + Random.value, "valuuuuuuuueeeee" + Random.value},
				{"chunky" + Random.value, "monkey" + Random.value},
				{"that " + Random.value, "funky" + Random.value},
				{"1234", "5678"},
				{"abc", "def"},
			};
			var inMemory = new InMemoryKeyStore();
			var playerprefs = new PlayerPrefsKeyStore();
			var editor = new EditorKeyStore();
			var values = new List<string>();
			randomvalues.ForEach((KeyValuePair<string, string> kvp) => { values.Add(kvp.Value); });
			string valueStringNewlineDelimited = string.Join("\n", values.ToArray());
			var fileProvider = new FileLineBasedKeyStore(getMemoryStream(valueStringNewlineDelimited), values);
			Assert.IsTrue(inMemory.otherDictionaryIsEqualOrASuperset(inMemory));
			Assert.IsTrue(inMemory.otherDictionaryIsEqualOrASuperset(playerprefs));
			Assert.IsTrue(inMemory.otherDictionaryIsEqualOrASuperset(editor));
			Assert.IsTrue(inMemory.otherDictionaryIsEqualOrASuperset(fileProvider));

			foreach(var kvp in randomvalues)
			{
				Debug.Log("Setting kvp:" + kvp.Key + " val" + kvp.Value);
				inMemory.set(kvp.Key, kvp.Value);
				playerprefs.set(kvp.Key, kvp.Value);
				editor.set(kvp.Key, kvp.Value);
				fileProvider.set(kvp.Key, kvp.Value);
			}
			standardCheck(inMemory, dictionary);
			standardCheck(playerprefs, dictionary);
			standardCheck(editor, dictionary);
			standardCheck(fileProvider, dictionary);

			Assert.IsTrue(inMemory.otherDictionaryIsEqualOrASuperset(inMemory));
			Assert.IsTrue(inMemory.otherDictionaryIsEqualOrASuperset(playerprefs));
			Assert.IsTrue(inMemory.otherDictionaryIsEqualOrASuperset(editor));
			Assert.IsTrue(inMemory.otherDictionaryIsEqualOrASuperset(fileProvider));

			var provider = new FileBasedCredentialProvider();
			Assert.AreEqual(provider.username, "alex@transfluent.com");
			Debug.Log("HELLO" + provider.username);
		}

		[Test]
		public void testFileStreamKeyStore()
		{
			string valueStringNewlineDelimited = string.Join("\n", values.ToArray());
			StreamReader reader = getMemoryStream(valueStringNewlineDelimited);

			var store = new FileLineBasedKeyStore(reader, keys);
			foreach(var kvp in dictionary)
			{
				Assert.AreEqual(store.get(kvp.Key), dictionary[kvp.Key]);
			}
			standardCheck(store, dictionary); //try setting then retrieving
			//Debug.Log(string.Format("TEST: username:{0} password:{1}",store.get("username"),store.get("password")));
		}

		[Test]
		public void testPlayerPrefsKey()
		{
			var store = new PlayerPrefsKeyStore();

			standardCheck(store, dictionary);
		}
	}
}