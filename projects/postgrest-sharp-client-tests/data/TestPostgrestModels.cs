using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postgrest.Client.Tests.Unit.data
{
    class TestPostgrestModels
    {
        internal const string TestModelCsvRepresentation = "csv";
        internal const string TestModelDefaultName = "user";

        public class User : PostgrestModel
        {
            public override string Csv => TestModelCsvRepresentation;

            public string Name { get; set; } = TestModelDefaultName;
            public string Password { get; set; }
        }
    }
}
