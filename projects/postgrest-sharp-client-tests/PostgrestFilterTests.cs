using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Postgrest.Client.Tests
{
    [TestFixture]
    class PostgrestFilterTests
    {
        private static readonly PostgrestFilter EqFilter = new PostgrestFilter("column", PostgrestFilterOperation.EqualTo, "something");
        private static readonly PostgrestFilter GtFilter = new PostgrestFilter("column", PostgrestFilterOperation.GreaterThan, 1.ToString());
        private static readonly PostgrestFilter LtFilter = new PostgrestFilter("column", PostgrestFilterOperation.LessThan, 1.ToString());
        private static readonly PostgrestFilter NotGteFilter = new PostgrestFilter("column", PostgrestFilterOperation.GreaterThanOrEqualTo, 1.ToString())
        {
            Negate = true
        };
        private static readonly PostgrestFilter LteFilter = new PostgrestFilter("column", PostgrestFilterOperation.LessThanOrEqualTo, 1.ToString());
        private static readonly PostgrestFilter NotLikeFilter = new PostgrestFilter("column", PostgrestFilterOperation.Like, "something*")
        {
            Negate = true
        };
        private static readonly PostgrestFilter IlikeFilter = new PostgrestFilter("column", PostgrestFilterOperation.ILike, "something*");
        private static readonly PostgrestFilter FullTextFilter = new PostgrestFilter("column", PostgrestFilterOperation.FullText, "some long text thing");
        private static readonly PostgrestFilter IsFilter = new PostgrestFilter("column", PostgrestFilterOperation.Is, "null");
        private static readonly PostgrestFilter NotInFilter = new PostgrestFilter("column", PostgrestFilterOperation.In, string.Join(",", "a", "b", "c"))
        {
            Negate = true
        };

        private static readonly PostgrestFilter NullColumnFilter = new PostgrestFilter(null, PostgrestFilterOperation.EqualTo, "something");
        private static readonly PostgrestFilter NullConditionFilter = new PostgrestFilter("column", PostgrestFilterOperation.EqualTo, null);

        #region Valid Data Tests

        private static IEnumerable<TestCaseData> PassingFiltersToExpressions()
        {
            yield return new TestCaseData(EqFilter, "eq.something");
            yield return new TestCaseData(GtFilter, "gt.1");
            yield return new TestCaseData(LtFilter, "lt.1");
            yield return new TestCaseData(NotGteFilter, "not.gte.1");
            yield return new TestCaseData(LteFilter, "lte.1");
            yield return new TestCaseData(NotLikeFilter, "not.like.something*");
            yield return new TestCaseData(IlikeFilter, "ilike.something*");
            yield return new TestCaseData(FullTextFilter, "@@.some long text thing");
            yield return new TestCaseData(IsFilter, "is.null");
            yield return new TestCaseData(NotInFilter, "not.in.a,b,c");
        }

        [Test, TestCaseSource(nameof(PassingFiltersToExpressions))]
        public void PassingFiltersToExpression(PostgrestFilter filter, string expectedExpression)
        {
            Assert.That(filter.FilterExpression, Is.EqualTo(expectedExpression));
        }

        #endregion

        #region Invalid Data tests

        private static IEnumerable<TestCaseData> FiltersThrowingNullRefOnExpression()
        {
            yield return new TestCaseData(NullConditionFilter);
        }

        [Test, TestCaseSource(nameof(FiltersThrowingNullRefOnExpression))]
        public void ExpressionsThrowNullReference(PostgrestFilter filter)
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                // ReSharper disable once UnusedVariable
                var throwsNull = filter.FilterExpression;
            });
        }

        #endregion

    }
}
