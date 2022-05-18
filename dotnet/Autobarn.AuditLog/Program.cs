using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Configuration;

namespace Autobarn.AuditLog {
    internal class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();

        static async Task Main(string[] args) {
            var amqp = config.GetConnectionString("AutobarnRabbitMq");
            using var bus = RabbitHutch.CreateBus(amqp);
            await bus.PubSub.SubscribeAsync<NewVehicleMessage>(
                "autobarn.auditlog",
                HandleNewVehicleMessage);
            Console.WriteLine("Subscribed to NewVehicleMessage");
            Console.ReadLine();
        }

        private static Task HandleNewVehicleMessage(NewVehicleMessage arg) {
            Console.WriteLine("Received a NewVehicleMessage");
            Console.WriteLine(
                $"{arg.Registration} ({arg.Make} {arg.Model}, {arg.Color}, {arg.Year}) - listed at {arg.ListedAt}");
            foreach (var f in arg.Features) {
                Console.WriteLine($"* {f}");
            }
            return Task.CompletedTask;
        }

        private static IConfigurationRoot ReadConfiguration() {
            var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
