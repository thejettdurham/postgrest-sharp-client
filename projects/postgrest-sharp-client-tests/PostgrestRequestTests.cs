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
            yield return new TestCaseData(TestPostgrestRequests.BaseSchema);
        }

        private static IEnumerable<TestCaseData> ValidProcedureRequests()
        {
            yield return new TestCaseData(TestPostgrestRequests.BaseProcedure);
            yield return new TestCaseData(TestPostgrestRequests.ProcedureWithArgs);
        }

        private static IEnumerable<TestCaseData> InvalidOperationRequests()
        {
            yield return new TestCaseData(TestPostgrestRequests.BaseCreate);
            yield return new TestCaseData(TestPostgrestRequests.BaseUpdate);
            yield return new TestCaseData(TestPostgrestRequests.UpdateWithEmptyRowFilters);
            yield return new TestCaseData(TestPostgrestRequests.UpdateWithRowFilters);
            yield return new TestCaseData(TestPostgrestRequests.BaseDelete);
        }

        private static IEnumerable<TestCaseData> ValidRequestsWithRowFilters()
        {
            yield return new TestCaseData(TestPostgrestRequests.UpdateWithRowFiltersAndData);
            yield return new TestCaseData(TestPostgrestRequests.ReadWithRowFilters);
            yield return new TestCaseData(TestPostgrestRequests.DeleteWithRowFilters);
        }

        private static IEnumerable<TestCaseData> ValidReadRequestsWithRange()
        {
            yield return new TestCaseData(TestPostgrestRequests.ReadWithBoundedRange, "0-10");
            yield return new TestCaseData(TestPostgrestRequests.ReadWithUnboundedRange, "10-");
        }

        [Test, TestCaseSource(nameof(ValidSchemaTestRequests))]
        public void ValidSchemaRequestsAreOptions(PostgrestRequest testRequest)
        {
            var request = new PostgrestRequestTestAdapter(testRequest).PrepareRequest();

            Assert.That(request.Method, Is.EqualTo(Method.OPTIONS));
        }

        [Test, TestCaseSource(nameof(ValidProcedureRequests))]
        public void ValidProcedureRequestArePost(PostgrestRequest testRequest)
        {
            var request = new PostgrestRequestTestAdapter(testRequest).PrepareRequest();

            Assert.That(request.Method, Is.EqualTo(Method.POST));
        }

        [Test, TestCaseSource(nameof(ValidProcedureRequests))]
        public void ValidProcedureRequestHaveRpcRoute(PostgrestRequest testRequest)
        {
            var request = new PostgrestRequestTestAdapter(testRequest).PrepareRequest();
            Assert.That(request.Resource, Is.EqualTo("rpc/" + TestPostgrestRequests.TestRoute));
        }

        [Test, TestCaseSource(nameof(ValidProcedureRequests))]
        public void ValidProcedureRequestHaveBodyIfArgs(PostgrestRequest testRequest)
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

        [Test, TestCaseSource(nameof(InvalidOperationRequests))]
        public void VolatileRequestsThrowingInvalidOperation(PostgrestRequest testRequest)
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                // ReSharper disable once UnusedVariable
                var throws = new PostgrestRequestTestAdapter(testRequest).PrepareRequest();
            });
        }

        [Test]
        public void CreateRequestRowFiltersAreIgnored()
        {
            var request = new PostgrestRequestTestAdapter(TestPostgrestRequests.CreateWithDataAndRowFilters).PrepareRequest();

            var rowFilterParams = request.Parameters.FindAll(p => p.Type == ParameterType.QueryString 
                && p.Name == TestPostgrestRequests.TestColumn 
                && (string)p.Value == TestPostgrestRequests.TestRowFilterExpression);

            Assert.That(0, Is.EqualTo(rowFilterParams.Count));
        }

        [Test, TestCaseSource(nameof(ValidRequestsWithRowFilters))]
        public void RowFilterAdditionWorksForNonCreateRequests(PostgrestRequest testRequest)
        {
            var request = new PostgrestRequestTestAdapter(testRequest).PrepareRequest();

            var rowFilterParams = request.Parameters.FindAll(p => p.Type == ParameterType.QueryString
                && p.Name == TestPostgrestRequests.TestColumn
                && (string)p.Value == TestPostgrestRequests.TestRowFilterExpression);

            Assert.That(TestPostgrestRequests.TestRowFilters.Count, Is.EqualTo(rowFilterParams.Count));
        }

        [Test]
        public void ColumnFilterQueryParamIsCorrect()
        {
            var request = new PostgrestRequestTestAdapter(TestPostgrestRequests.ReadWithColumnFilters).PrepareRequest();

            var selectQueryParam = request.Parameters.Single(p => p.Type == ParameterType.QueryString
                && p.Name == "select");

            Assert.That(selectQueryParam.Value.ToString(), Is.EqualTo("column1,column2,column3"));
        }

        [Test]
        public void OrderingQueryParamIsCorrect()
        {
            var request = new PostgrestRequestTestAdapter(TestPostgrestRequests.ReadWithOrdering).PrepareRequest();

            var orderQueryParam = request.Parameters.Single(p => p.Type == ParameterType.QueryString
                && p.Name == "order");

            Assert.That(orderQueryParam.Value.ToString(), Is.EqualTo(TestPostgrestRequests.TestColumn));
        }

        [Test, TestCaseSource(nameof(ValidReadRequestsWithRange))]
        public void LimitHeaderIsCorrect(PostgrestRequest testRequest, string expectedRangeExpression)
        {
            var request = new PostgrestRequestTestAdapter(testRequest).PrepareRequest();

            var limitHeader = request.Parameters.Single(p => p.Type == ParameterType.HttpHeader
                && p.Name == "Range");

            Assert.That(limitHeader.Value.ToString(), Is.EqualTo(expectedRangeExpression));
        }

        [Test]
        public void CreateRequestIsPost()
        {
            var request = new PostgrestRequestTestAdapter(TestPostgrestRequests.CreateWithData).PrepareRequest();

            Assert.That(request.Method, Is.EqualTo(Method.POST));
        }

        [Test]
        public void DeleteRequestIsDelete()
        {
            var request = new PostgrestRequestTestAdapter(TestPostgrestRequests.DeleteWithRowFilters).PrepareRequest();

            Assert.That(request.Method, Is.EqualTo(Method.DELETE));
        }

        [Test]
        public void ReadRequestIsGet()
        {
            var request = new PostgrestRequestTestAdapter(TestPostgrestRequests.BaseRead).PrepareRequest();

            Assert.That(request.Method, Is.EqualTo(Method.GET));
        }
        
        [Test]
        public void UpdateRequestIsPatch()
        {
            var request = new PostgrestRequestTestAdapter(TestPostgrestRequests.UpdateWithRowFiltersAndData).PrepareRequest();

            Assert.That(request.Method, Is.EqualTo(Method.PATCH));
        }
       
    }
}