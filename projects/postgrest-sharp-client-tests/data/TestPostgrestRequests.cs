using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Postgrest.Client.Tests.Unit.data
{
    class TestPostgrestRequests
    {
        internal const string TestRoute = "route";
        internal const string TestColumn = "column";
        internal const string TestRowFilterCondition = "foo";
        internal const string TestRowFilterExpression = "eq." + TestRowFilterCondition;

        internal static Dictionary<string, string> TestProcedureArgs = new Dictionary<string, string>
        {
            {"arg1", "val1"}
        };

        internal static List<PostgrestFilter> TestRowFilters => new List<PostgrestFilter>
        {
            new PostgrestFilter(TestColumn, PostgrestFilterOperation.EqualTo, TestRowFilterCondition)
        };

        internal static List<string> TestColumnFilters = new List<string>
        {
            "column1",
            "column2",
            "column3"
        };

        internal static List<PostgrestOrdering> TestOrderings = new List<PostgrestOrdering>()
        {
            new PostgrestOrdering(TestColumn)
        };

        internal static Tuple<int, int?> TestRangeBounded = Tuple.Create<int, int?>(0,10);
        internal static Tuple<int, int?> TestRangeUnbounded = Tuple.Create<int, int?>(10, null);

        internal static PostgrestRequest BaseRead => new PostgrestRequest(TestRoute, PostgrestRequestType.Read);
        internal static PostgrestRequest BaseCreate => new PostgrestRequest(TestRoute, PostgrestRequestType.Create);
        internal static PostgrestRequest BaseUpdate => new PostgrestRequest(TestRoute, PostgrestRequestType.Update);
        internal static PostgrestRequest BaseDelete => new PostgrestRequest(TestRoute, PostgrestRequestType.Delete);
        internal static PostgrestRequest BaseSchema => new PostgrestRequest(TestRoute, PostgrestRequestType.Schema);
        internal static PostgrestRequest BaseProcedure => new PostgrestRequest(TestRoute, PostgrestRequestType.Procedure);

        internal static PostgrestRequest ProcedureWithArgs => new PostgrestRequest(BaseProcedure) 
        {
            ProcedureArgs = TestProcedureArgs
        };

        internal static PostgrestRequest UpdateWithEmptyRowFilters => new PostgrestRequest(BaseUpdate)
        {
            RowFilters = new List<PostgrestFilter>()
        };

        internal static PostgrestRequest UpdateWithRowFilters => new PostgrestRequest(BaseUpdate)
        {
            RowFilters = TestRowFilters
        };

        internal static PostgrestRequest CreateWithData => new PostgrestRequest(BaseCreate)
        {
            Data = new TestPostgrestModels.User()
        };

        internal static PostgrestRequest CreateWithDataAndRowFilters => new PostgrestRequest(CreateWithData)
        {
            RowFilters = TestRowFilters
        };

        internal static PostgrestRequest UpdateWithRowFiltersAndData => new PostgrestRequest(UpdateWithRowFilters)
        {
            Data = new TestPostgrestModels.User()
        };

        internal static PostgrestRequest ReadWithRowFilters => new PostgrestRequest(BaseRead)
        {
            RowFilters = TestRowFilters
        };

        internal static PostgrestRequest DeleteWithRowFilters => new PostgrestRequest(BaseDelete)
        {
            RowFilters = TestRowFilters
        };

        internal static PostgrestRequest ReadWithColumnFilters => new PostgrestRequest(BaseRead)
        {
            ColumnFilters = TestColumnFilters
        };

        internal static PostgrestRequest ReadWithOrdering => new PostgrestRequest(BaseRead)
        {
            Orderings = TestOrderings
        };

        internal static PostgrestRequest ReadWithBoundedRange => new PostgrestRequest(BaseRead)
        {
            LimitRange = TestRangeBounded
        };

        internal static PostgrestRequest ReadWithUnboundedRange => new PostgrestRequest(BaseRead)
        {
            LimitRange = TestRangeUnbounded
        };
    }
}
