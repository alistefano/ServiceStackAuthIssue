using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack;
using SvStackAuthTest;

namespace AuthTest
{
    [TestClass]
    public class HeadersAndQueryStringAuth
    {
        [TestMethod]
        public void CanAccessWithIdOnHeader()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            var msg = rnd.Next().ToString();
            
            var client = new JsonServiceClient(Config.AbsoluteBaseUri);
            var authResponse = client.Post(new Authenticate {UserName = "testuser", Password = "testpassword", RememberMe = false});

            var cookieValues = client.GetCookieValues();
            client.ClearCookies();

            //Test Auth with ID Header
            client.Headers.Add("X-ss-id", cookieValues["ss-id"]);

            var testResponse = client.Get(new TestRequest { Msg = msg });
            Assert.AreEqual(msg, testResponse.Msg);
        }

        [TestMethod]
        public void CanAccessWithPIdOnHeader()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            var msg = rnd.Next().ToString();

            var client = new JsonServiceClient(Config.AbsoluteBaseUri);
            var authResponse = client.Post(new Authenticate { UserName = "testuser", Password = "testpassword", RememberMe = true });

            var cookieValues = client.GetCookieValues();
            client.ClearCookies();

            //Test Auth with PID Header
            client.Headers.Remove("X-ss-id");
            client.Headers.Add("X-ss-pid", cookieValues["ss-pid"]);
            client.Headers.Add("X-ss-opt", cookieValues["ss-opt"]);

            var testResponse = client.Get(new TestRequest { Msg = msg });
            Assert.AreEqual(msg, testResponse.Msg);
        }

        [TestMethod]
        public void CanAccessWithIdOnQueryString()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            var msg = rnd.Next().ToString();

            var client = new JsonServiceClient(Config.AbsoluteBaseUri);
            var authResponse =
                client.Post(new Authenticate {UserName = "testuser", Password = "testpassword", RememberMe = false});

            var cookieValues = client.GetCookieValues();

            var rawRequestUri = Config.AbsoluteBaseUri + "/test/" + msg + "?format=json&ss-id=" + cookieValues["ss-id"];
            TestResponse testResponse = rawRequestUri.GetJsonFromUrl().FromJson<TestResponse>();

            Assert.AreEqual(msg, testResponse.Msg);
        }

        [TestMethod]
        public void CanAccessWithPIdOnQueryString()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            var msg = rnd.Next().ToString();

            var client = new JsonServiceClient(Config.AbsoluteBaseUri);
            var authResponse =
                client.Post(new Authenticate { UserName = "testuser", Password = "testpassword", RememberMe = true });

            var cookieValues = client.GetCookieValues();

            var rawRequestUri = Config.AbsoluteBaseUri + "/test/" + msg + "?format=json&ss-pid=" + cookieValues["ss-pid"] + "&ss-opt=" + cookieValues["ss-opt"];
            TestResponse testResponse = rawRequestUri.GetJsonFromUrl().FromJson<TestResponse>();

            Assert.AreEqual(msg, testResponse.Msg);
        }
    }
}
