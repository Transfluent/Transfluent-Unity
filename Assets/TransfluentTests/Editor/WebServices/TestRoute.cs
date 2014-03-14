using NUnit.Framework;

namespace transfluent.tests
{
	[TestFixture]
	public class RouteTest
	{
		[Test]
		public void getSimpleURL()
		{
			var login = new Login("", "");
			Assert.IsNotNull(RestUrl.GetURL(login));
		}

		private const string fakeRestPath = "foo/bar";
		private const string fakeHelpUrl = "http://lmgtfy.com/‎";

		[Test]
		public void testKnownUrl()
		{
			var route = RestUrl.GetRouteAttribute(typeof(TestRoutePath));
			Assert.AreEqual(route.route, fakeRestPath);
			Assert.AreEqual(route.helpUrl, fakeHelpUrl);
			Assert.AreEqual(route.requestType, RestRequestType.POST);
		}

		[Route(fakeRestPath, RestRequestType.POST, fakeHelpUrl)]
		public class TestRoutePath
		{
		}
	}
}