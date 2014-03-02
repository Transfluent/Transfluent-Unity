using NUnit.Framework;
using UnityEngine;
using UnityEditor;

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

	}
}