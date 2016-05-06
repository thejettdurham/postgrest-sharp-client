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
            yield return new TestCaseData(new TestPostgrestModels.User(), "{\"name\":\"user\",\"password\":null}");
            yield return new TestCaseData(new TestPostgrestModels.UserOverrideContract(), "{\"Name\":\"user\",\"Password\":null}");
        }

        private static IEnumerable<TestCaseData> ObjectsToMinimalJson()
        {
            yield return new TestCaseData(new TestPostgrestModels.User(), "{\"name\":\"user\"}");
            yield return new TestCaseData(new TestPostgrestModels.UserOverrideContract(), "{\"Name\":\"user\"}");
        }

        [Test, TestCaseSource(nameof(ObjectsToFullJson))]
        public void ToJsonWorks(PostgrestModel testModel, string expectedJson)
        {
            Assert.That(testModel.ToJson(), Is.EqualTo(expectedJson));
        }

        [Test, TestCaseSource(nameof(ObjectsToMinimalJson))]
        public void ToMinimalJsonWorks(PostgrestModel testModel, string expectedJson)
        {
            Assert.That(testModel.ToMinimalJson(), Is.EqualTo(expectedJson));
        }
    }
}
