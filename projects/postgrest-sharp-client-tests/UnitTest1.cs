using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Postgrest.Client;

namespace postgrest_sharp_client_tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Fluent
            var myFoo =
                new SimplePostgrestClient().ExecuteAndGetData<object>(PostgrestRequest.Read("devices")
                    .Singular()
                    .Where("deviceid", PostgrestFilter.EqualTo("someuuid"))
                    );

            // NonFluent
            var client = new SimplePostgrestClient();
            var request = new PostgrestRequestNonFluent("devices", PostgrestRequestType.Read)
            {
                AsSingular = true,
                RowFilters = new Dictionary<string, PostgrestFilter>
                {
                    {"deviceid", PostgrestFilter.EqualTo("someuuid") }
                }
            };

        }
    }
}
