using System;
using System.Diagnostics;
using NUnit.Framework;
using transfluent;

[TestFixture]
public class InjectionTests 
{
	public class TestInjectionTarget
	{
		[Inject]
		public string testString { get; set; }
		[Inject("Special")]
		public string specialString { get; set; }
	}

	[TestFixtureSetUp]
	public void setupTheTests()
	{
		
	}

	[Test]
	public void CreateContextSetString()
	{
		InjectionContext context = new InjectionContext();
		string testText = "HELLO WORLD";
		context.addMapping<string>(testText);
		var target = new TestInjectionTarget();
		Assert.IsNullOrEmpty(target.testString);
		context.setMappings(target);
		Assert.IsNotNullOrEmpty(target.testString);
		Assert.AreEqual(target.testString,testText);
	}

	[Test]
	[ExpectedException(typeof(UnboundInjectionException))]
	public void CreateContextAndFail()
	{
		InjectionContext context = new InjectionContext();
		//context.addMapping<string>("HELLO WORLD");
		var target = new TestInjectionTarget();
		Assert.IsNullOrEmpty(target.testString);
		context.setMappings(target);
		Assert.IsNotNullOrEmpty(target.testString);
	}

	[Test]
	public void TestNamedInjection()
	{
		InjectionContext context = new InjectionContext();
		string standardKey = "HELLO WORLD";
		string namedKey = "SPECIAL STRING";
		context.addMapping<string>(standardKey);
		context.addNamedMapping<string>("Special", namedKey);
		var target = new TestInjectionTarget();
		Assert.IsNullOrEmpty(target.testString);
		context.setMappings(target);
		Assert.AreSame(target.testString,standardKey);
		Assert.AreSame(target.specialString, namedKey);
		Assert.AreNotEqual(standardKey,namedKey);
		Assert.AreNotEqual(target.testString, target.specialString);
	}
}
