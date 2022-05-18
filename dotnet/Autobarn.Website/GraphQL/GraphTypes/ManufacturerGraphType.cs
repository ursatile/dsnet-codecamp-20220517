using Autobarn.Data.Entities;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.GraphTypes
{
    public sealed class ManufacturerGraphType : ObjectGraphType<Manufacturer> {
        public ManufacturerGraphType() {
            Field(m => m.Name);
            Field(m => m.Code);
        }
    }
}