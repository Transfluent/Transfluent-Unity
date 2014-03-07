using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Castle.Core.Internal;
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
			return reader; //Why you readin?  ... we got ourselves a reader here
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
			Dictionary<string,string> randomvalues = new Dictionary<string, string>()
			{
				{"KEYYYYYYY"+UnityEngine.Random.value,"valuuuuuuuueeeee"+UnityEngine.Random.value},
				{"chunky"+UnityEngine.Random.value,"monkey"+UnityEngine.Random.value},
				{"that "+UnityEngine.Random.value,"funky"+UnityEngine.Random.value},
				{"1234","5678"},
				{"abc","def"},
			};
			var inMemory = new InMemoryKeyStore();
			var playerprefs = new PlayerPrefsKeyStore();
			var editor = new EditorKeyStore();
			List<string> values = new List<string>();
			randomvalues.ForEach((KeyValuePair<string, string> kvp) =>
			{
				values.Add(kvp.Value);
			});
			string valueStringNewlineDelimited = string.Join("\n", values.ToArray());
			var fileProvider = new FileLineBasedKeyStore(getMemoryStream(valueStringNewlineDelimited), values);
			Assert.IsTrue(inMemory.otherDictionaryIsEqualOrASuperset(inMemory));
			Assert.IsTrue(inMemory.otherDictionaryIsEqualOrASuperset(playerprefs));
			Assert.IsTrue(inMemory.otherDictionaryIsEqualOrASuperset(editor));
			Assert.IsTrue(inMemory.otherDictionaryIsEqualOrASuperset(fileProvider));

			foreach (KeyValuePair<string, string> kvp in randomvalues)
			{
				UnityEngine.Debug.Log("Setting kvp:" + kvp.Key + " val" + kvp.Value);
				inMemory.set(kvp.Key,kvp.Value);
				playerprefs.set(kvp.Key, kvp.Value);
				editor.set(kvp.Key, kvp.Value);
				fileProvider.set(kvp.Key, kvp.Value);
			}
			standardCheck(inMemory,dictionary);
			standardCheck(playerprefs,dictionary);
			standardCheck(editor,dictionary);
			standardCheck(fileProvider,dictionary);


			Assert.IsTrue(inMemory.otherDictionaryIsEqualOrASuperset(inMemory));
			Assert.IsTrue(inMemory.otherDictionaryIsEqualOrASuperset(playerprefs));
			Assert.IsTrue(inMemory.otherDictionaryIsEqualOrASuperset(editor));
			Assert.IsTrue(inMemory.otherDictionaryIsEqualOrASuperset(fileProvider));
			
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
