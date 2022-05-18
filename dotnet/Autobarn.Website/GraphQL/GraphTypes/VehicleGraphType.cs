using System;
using Autobarn.Data.Entities;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.GraphTypes {
    public sealed class VehicleGraphType : ObjectGraphType<Vehicle> {
        public VehicleGraphType() {
            Name = "vehicle";
            Field(v => v.Year);
            Field(v => v.Registration);
            Field(v => v.Color);
            Field(v => v.VehicleModel,
                nullable: false,
                type: typeof(ModelGraphType)
            ).Description("What model of vehicle is this?");
        }
    }
}
