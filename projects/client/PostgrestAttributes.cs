using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postgrest.Client
{
    /// <summary>
    /// Marks a property in a PostgrestModel as the primary key. Each PostgrestModel subclass is required to have one property with this attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PostgrestPrimaryKeyAttribute : Attribute
    {
    }
}
