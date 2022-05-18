using System;
using System.Collections.Generic;
using System.Linq;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.Queries {
    public sealed class VehicleQueries : ObjectGraphType {
        private readonly IAutobarnDatabase db;

        public VehicleQueries(IAutobarnDatabase db) {
            this.db = db;

            Field<ListGraphType<VehicleGraphType>>("vehicles",
                "Query to list all vehicles in the system",
                resolve: GetAllVehicles);

            Field<VehicleGraphType>("vehicle",
                "Query to retrieve a single vehicle",
                new QueryArguments(MakeNonNullStringArgument("registration",
                    "The registration (licence plate) of the vehicle you want to retrieve")),
                resolve: GetVehicle);

            Field<ListGraphType<VehicleGraphType>>("vehiclesByColor",
                "Query to retrieve all vehicles of a particular color",
                new QueryArguments(MakeNonNullStringArgument("color",
                    "What color cars do you want?")),
                resolve: GetVehiclesByColor);

        }

        private IEnumerable<Vehicle> GetVehiclesByColor(IResolveFieldContext<object> context) {
            var color = context.GetArgument<string>("color");
            return db.ListVehicles().Where(v => v.Color.Contains(color, StringComparison.InvariantCultureIgnoreCase));
        }

        private QueryArgument MakeNonNullStringArgument(string name, string description) {
            return new QueryArgument<NonNullGraphType<StringGraphType>> {
                Name = name, Description = description
            };
        }

        private Vehicle GetVehicle(IResolveFieldContext<object> context) {
            var reg = context.GetArgument<string>("registration");
            return db.FindVehicle(reg);
        }

        private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> context) {
            return db.ListVehicles();
        }
    }
}
