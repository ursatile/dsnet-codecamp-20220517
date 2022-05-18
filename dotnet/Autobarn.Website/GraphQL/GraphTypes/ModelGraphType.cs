using Autobarn.Data.Entities;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.GraphTypes
{
    public sealed class ModelGraphType : ObjectGraphType<Model> {
        public ModelGraphType() {
            Name = "model";
            Field(m => m.Name);
            Field(m => m.Code);
            Field(m => m.Manufacturer, nullable: false,
                    type: typeof(ManufacturerGraphType))
                .Description("The manufacturer of this model of vehicle");
        }
    }
}