using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Postgrest.Client.Tests.Unit.data;
using RestSharp;

namespace Postgrest.Client.Tests.Unit
{
    [TestFixture]
    class PostgrestRequestTests
    {
        //What does adding params/headers do to the internal state of the object?
        // Parameters aren't interpreted until the request is sent to a client
        //test valid and invalid requests for each type of request

        [Test]
        public void SimpleCloneWorks()
        {
            var r = new PostgrestRequest("route", PostgrestRequestType.Create);
            var r2 = new PostgrestRequest(r);
            Assert.That(r, Is.EqualTo(r2));
        }

        [Test]
        public void CloneWithChangedMembersWorks()
        {
            var r = new PostgrestRequest("route", PostgrestRequestType.Create)
            {
                ReadAsCsv = true
            };
            var r2 = new PostgrestRequest(r)
            {
                ReadAsCsv = false,
                AsSingular = true
            };

            Assert.That(r, Is.Not.EqualTo(r2));
            Assert.That(r.ReadAsCsv, Is.Not.EqualTo(r2.ReadAsCsv));
        }

        private static IEnumerable<TestCaseData> ValidSchemaTestRequests()
        {
            var testReqs = new TestPostgrestRequests();

            yield return new TestCaseData(testReqs.BaseSchema);
        }

        [Test, TestCaseSource(nameof(ValidSchemaTestRequests))]
        public void SchemaRequestPreparation(PostgrestRequest testRequest)
        {
            var request = new PostgrestRequestTestAdapter(testRequest).PrepareRequest();

            Assert.That(request.Method, Is.EqualTo(Method.OPTIONS));
        }

        private static IEnumerable<TestCaseData> ValidProcedureRequests()
        {
            var testReqs = new TestPostgrestRequests();

            yield return new TestCaseData(testReqs.BaseProcedure);
            yield return new TestCaseData(testReqs.ProcedureWithArgs);
        }

        [Test, TestCaseSource(nameof(ValidProcedureRequests))]
        public void ProcedureRequestsArePost(PostgrestRequest testRequest)
        {
            var request = new PostgrestRequestTestAdapter(testRequest).PrepareRequest();

            Assert.That(request.Method, Is.EqualTo(Method.POST));
        }

        [Test, TestCaseSource(nameof(ValidProcedureRequests))]
        public void ProcedureRequestsWithArgsHaveBody(PostgrestRequest testRequest)
        {
            var request = new PostgrestRequestTestAdapter(testRequest).PrepareRequest();

            if (request.ProcedureArgs != null)
            {
                Assert.NotNull(request.Parameters.Single(p => p.Type == ParameterType.RequestBody));
            }
            else
            {
                Assert.True(true);
            }
        }
    }
}
