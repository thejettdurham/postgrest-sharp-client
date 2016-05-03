using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Postgrest.Client;
using Postgrest.Client.Sample;
using RestSharp;

namespace postgrest_sharp_client_tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var client = new SimplePostgrestClient
            {
                ExtraHeaders = new List<HttpHeader>
                {
                    new HttpHeader
                    {
                        Name = "header",
                        Value = "something"
                    },
                    new HttpHeader
                    {
                        Name = "header2",
                        Value = "Something Else"
                    }
                }
            };

            var loginProcedure = new PostgrestRequest("login", PostgrestRequestType.Procedure)
            {
                ProcedureArgs = new Dictionary<string, string>
                {
                    {"User", "jsmith"},
                    {"Pass", "password"}
                }
            };

            var token = client.ExecuteAndGetData<object>(loginProcedure);

            var readDevice = new PostgrestRequest("devices", PostgrestRequestType.Read)
            {
                AsSingular = true,
                RowFilters = new List<PostgrestFilter>
                {
                    new PostgrestFilter("deviceid", PostgrestFilterOperation.EqualTo, "someuuid")
                }
            };

            var device = client.ExecuteAndGetData<object>(readDevice);

        }
    }
}
