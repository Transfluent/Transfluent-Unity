using System;
using System.Collections.Generic;
using NUnit.Framework;
using transfluent;
using transfluent.editor;
using UnityEngine;
using Random = UnityEngine.Random;

[TestFixture]
public class TestTransfluentTranslationUtility
{
	[TestFixtureSetUp]
	public void testCreation()
	{
	}

	[Test]
	public void testLoading()
	{
		//by default the utility goes to backwards language
		string thisTextDoesNotExist = "THIS DOES NOT EXIST" + Random.value;
		//string noDestinationLanguageSet = TransfluentUtility.utility.getTranslation();
		TransfluentUtility util = new TransfluentUtility("en-us","en-us");
		util.getTranslation(thisTextDoesNotExist);
	}
}
