using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Postgrest.Client.Tests.Unit
{
    [TestFixture]
    public class PostgrestOrderingTests
    {
        private static readonly PostgrestOrdering SimpleOrdering = new PostgrestOrdering("column1");
        private static readonly PostgrestOrdering OrderingWithAscending = new PostgrestOrdering("column2")
        {
            IsAscending = true
        };
        private static readonly PostgrestOrdering OrderingWithNullsFirst = new PostgrestOrdering("column3")
        {
            NullsFirst = true
        };
        private static readonly PostgrestOrdering OrderingWithAll = new PostgrestOrdering("column4")
        {
            IsAscending = false,
            NullsFirst = false
        };

        private static readonly PostgrestOrdering OrderingWithNullColumn = new PostgrestOrdering(null);

        #region Valid Data Tests

        private static IEnumerable<TestCaseData> PassingOrderingToExpressionCases()
        {
            yield return new TestCaseData(SimpleOrdering, "column1");
            yield return new TestCaseData(OrderingWithAscending, "column2.asc");
            yield return new TestCaseData(OrderingWithNullsFirst, "column3.nullsfirst");
            yield return new TestCaseData(OrderingWithAll, "column4.desc.nullslast");
        }

        [Test, TestCaseSource(nameof(PassingOrderingToExpressionCases))]
        public void PassingOrderingToExpression(PostgrestOrdering ordering, string expectedExpression)
        {
            Assert.That(ordering.OrderExpression, Is.EqualTo(expectedExpression));
        }

        private static IEnumerable<TestCaseData> PassingOrderingsToParameterCases()
        {
            yield return new TestCaseData(new List<PostgrestOrdering>
            {
                SimpleOrdering, OrderingWithAscending, OrderingWithNullsFirst, OrderingWithAll
            }, new KeyValuePair<string, string>("order", "column1,column2.asc,column3.nullsfirst,column4.desc.nullslast"));
        }

        [Test, TestCaseSource(nameof(PassingOrderingsToParameterCases))]
        public void PassingOrderingsToParameter(List<PostgrestOrdering> orderings,
            KeyValuePair<string, string> expectedParameter)
        {
            Assert.That(PostgrestOrdering.BuildOrderParameter(orderings), Is.EqualTo(expectedParameter));
        }

        #endregion

        #region Invalid Data Tests

        private static IEnumerable<TestCaseData> FailingOrderingToExpressionCases()
        {
            yield return new TestCaseData(OrderingWithNullColumn);
        }

        [Test, TestCaseSource(nameof(FailingOrderingToExpressionCases))]
        public void FailingOrderingToExpression(PostgrestOrdering ordering)
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                var throws = ordering.OrderExpression;
            });
        }

        #endregion

    }
}
