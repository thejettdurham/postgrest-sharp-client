using System;
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
            var myFoo =
                new SimplePostgrestClient().ExecuteAndGetData<object>(PostgrestRequest.Read("devices")
                    .Singular()
                    .Where("deviceid", PostgrestFilter.EqualTo("someuuid")));
            //var r = PostgrestRequest.Read()
        }
    }
}
