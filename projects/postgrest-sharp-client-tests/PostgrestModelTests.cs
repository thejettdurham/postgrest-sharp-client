using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Postgrest.Client.Tests.Unit.data;

namespace Postgrest.Client.Tests.Unit
{
    [TestFixture]
    class PostgrestModelTests
    {
        private static IEnumerable<TestCaseData> ObjectsToFullJson()
        {
            yield return new TestCaseData(new TestPostgrestModels.User(), "{\"Name\":\"user\",\"Password\":null}");
        }

        private static IEnumerable<TestCaseData> ObjectsToMinimalJson()
        {
            yield return new TestCaseData(new TestPostgrestModels.User(), "{\"Name\":\"user\"}");
        }

        [Test, TestCaseSource(nameof(ObjectsToFullJson))]
        public void ToJsonWorks(PostgrestModel testModel, string expectedJson)
        {
            Assert.That(testModel.Json, Is.EqualTo(expectedJson));
        }

        [Test, TestCaseSource(nameof(ObjectsToMinimalJson))]
        public void ToMinimalJsonWorks(PostgrestModel testModel, string expectedJson)
        {
            Assert.That(testModel.MinimalJson, Is.EqualTo(expectedJson));
        }
    }
}
