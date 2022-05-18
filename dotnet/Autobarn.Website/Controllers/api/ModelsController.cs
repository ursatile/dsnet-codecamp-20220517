using Autobarn.Data;
using Autobarn.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using Autobarn.Messages;
using Autobarn.Website.Models;
using EasyNetQ;

namespace Autobarn.Website.Controllers.api {
    [Route("api/[controller]")]
    [ApiController]
    public class ModelsController : ControllerBase {
        private readonly IAutobarnDatabase db;
        private readonly IBus bus;

        public ModelsController(IAutobarnDatabase db, IBus bus)
        {
            this.db = db;
            this.bus = bus;
        }

        [HttpGet]
        public IEnumerable<Model> Get() {
            return db.ListModels();
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id) {
            var vehicleModel = db.FindModel(id);
            if (vehicleModel == default) return NotFound();
            var result = vehicleModel.ToDynamic();
            result._actions = new {
                create = new {
                    href = $"/api/models/{vehicleModel.Code}",
                    type = "application/json",
                    name = $"Create a new {vehicleModel.Manufacturer.Name} {vehicleModel.Name}",
                    method = "POST",
                }
            };
            return Ok(result);
        }

        // POST api/vehicles
        [HttpPost("{id}")]
        public IActionResult Post(string id, [FromBody] VehicleDto dto) {
            var existing = db.FindVehicle(dto.Registration);
            if (existing != default) return Conflict($"Sorry, vehicle with registration {dto.Registration} is already in our database and you can't sell the same car twice!");

            var vehicleModel = db.FindModel(id);
            var vehicle = new Vehicle {
                Registration = dto.Registration,
                Color = dto.Color,
                Year = dto.Year,
                VehicleModel = vehicleModel
            };
            db.CreateVehicle(vehicle);
            PublishNewVehicleNotification(vehicle);
            return Created($"/api/vehicles/{vehicle.Registration}",
                vehicle.ToHal());
        }

        private void PublishNewVehicleNotification(Vehicle vehicle) {
            var m = new NewVehicleMessage {
                Registration = vehicle.Registration,
                Make = vehicle.VehicleModel.Manufacturer.Name,
                Color = vehicle.Color,
                Model = vehicle.VehicleModel.Name,
                Year = vehicle.Year,
                Features = new[]
                {
                    "Metallic paint",
                    "Blu-ray player",
                    "Alloy wheels",
                    "Flux capacitor"
                }
            };
            bus.PubSub.Publish(m);

        }
    }
}