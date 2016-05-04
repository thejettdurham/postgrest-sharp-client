using System.Collections.Generic;
using NUnit.Framework;

namespace Postgrest.Client.Tests
{
    [TestFixture]
    public class PostgrestOrderingTests
    {
        private static IEnumerable<TestCaseData> PassingOrderingToExpressionCases()
        {
            yield return new TestCaseData(new PostgrestOrdering("column"), "column");
            yield return new TestCaseData(new PostgrestOrdering("column")
            {
                IsAscending = true
            }, "column.asc");
            yield return new TestCaseData(new PostgrestOrdering("column")
            {
                IsAscending = false,
                NullsFirst = true
            }, "column.desc.nullsfirst");
        } 

        [Test, TestCaseSource(nameof(PassingOrderingToExpressionCases))]
        public void PassingOrderingToExpression(PostgrestOrdering ordering, string expectedExpression)
        {
            Assert.That(ordering.OrderExpression, Is.EqualTo(expectedExpression));
        }
    }
}
