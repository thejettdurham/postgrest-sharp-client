using System;
using NUnit.Framework;

namespace Postgrest.Client.Tests.Unit
{
    [TestFixture]
    class PostgrestClientTests
    {
        private const string TestToken = "sometoken";
        private const string TestUriPath = "http://someapi.com";

        private class WellFormedClient : PostgrestClient
        {
            protected override string AuthToken => TestToken;
            public override Uri BaseUrl => new Uri(TestUriPath);
        }

        private class NoAuthenticationClient : PostgrestClient
        {
            protected override string AuthToken => null;
            public override Uri BaseUrl => new Uri(TestUriPath);
        }

        private class NullUriClient : PostgrestClient
        {
            protected override string AuthToken => TestToken;
            public override Uri BaseUrl => new Uri(null);
        }

        [Test]
        public void WellFormedClientHasPostgrestAuthenticator()
        {
            var client = new WellFormedClient();
            Assert.That(client.Authenticator, Is.TypeOf(typeof(PostgrestAuthenticator)));
        }

        [Test]
        public void NoAuthClientHasNullAuthenticator()
        {
            var client = new NoAuthenticationClient();
            Assert.That(client.Authenticator, Is.Null);
        }

        [Test]
        public void NullUriClientFailsAtConstruction()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new NullUriClient();
            });
        }
    }
}
