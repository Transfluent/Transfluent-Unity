using System;
using NUnit.Framework;

namespace transfluent.tests
{
	[TestFixture]
	public class InjectionTests
	{
		public interface ITestInjectionTarget
		{
			string testString { get; set; }
			string specialString { get; set; }
		}

		public class ClassWithATestInterfaceToBeInjected
		{
			[Inject]
			public IStringProvider testStringProvider { get; set; }
		}

		public interface IStringProvider
		{
			string myString { get; set; }
		}

		public class StringProviderConcrete1 : IStringProvider
		{
			public string myString { get; set; }
		}

		public class StringProviderConcrete2 : IStringProvider
		{
			public string myString { get; set; }
		}

		public class TestInjectionTarget1
		{
			[Inject]
			public string testString { get; set; }
		}

		public class TestInjectionTarget
		{
			[Inject]
			public string testString { get; set; }

			[Inject(NamedInjections.INTERNAL_TESTING_SPECIAL_KEY)]
			public string specialString { get; set; }
		}

		[TestFixtureSetUp]
		public void setupTheTests()
		{
		}

		[Test]
		[ExpectedException(typeof (UnboundInjectionException))]
		public void CreateContextAndFail()
		{
			var context = new InjectionContext();
			//context.addMapping<string>("HELLO WORLD");
			var target = new TestInjectionTarget1();
			Assert.IsNullOrEmpty(target.testString);
			context.setMappings(target);
			Assert.IsNotNullOrEmpty(target.testString);
		}

		[Test]
		public void CreateContextSetString()
		{
			var context = new InjectionContext();
			const string testText = "HELLO WORLD";
			//context.addMapping<string>(testText);
			context.addMapping<String>(testText);
			var target = new TestInjectionTarget1();
			Assert.IsNullOrEmpty(target.testString);
			context.setMappings(target);
			Assert.IsNotNullOrEmpty(target.testString);
			Assert.AreEqual(target.testString, testText);
		}

		[Test]
		public void TestInterfaceSettingToMap()
		{
			var context = new InjectionContext();
			context.addMapping<IStringProvider>(new StringProviderConcrete1());
			var target = new ClassWithATestInterfaceToBeInjected();

			context.setMappings(target);

			Assert.IsTrue(target.testStringProvider is StringProviderConcrete1);
			Assert.IsFalse(target.testStringProvider is StringProviderConcrete2);
		}

		[Test]
		public void TestNamedInjection()
		{
			var context = new InjectionContext();
			const string standardKey = "HELLO WORLD";
			const string namedKey = "SPECIAL STRING";
			context.addMapping<string>(standardKey);
			context.addNamedMapping<string>(NamedInjections.INTERNAL_TESTING_SPECIAL_KEY, namedKey);
			var target = new TestInjectionTarget();
			Assert.IsNullOrEmpty(target.testString);
			context.setMappings(target);
			Assert.AreSame(target.testString, standardKey);
			Assert.AreSame(target.specialString, namedKey);
			Assert.AreNotEqual(standardKey, namedKey);
			Assert.AreNotEqual(target.testString, target.specialString);
		}
	}
}