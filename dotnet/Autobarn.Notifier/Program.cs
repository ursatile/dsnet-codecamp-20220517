using System;
using System.IO;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Configuration;

namespace Autobarn.Notifier {
    internal class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();

        static async Task Main(string[] args) {
            var amqp = config.GetConnectionString("AutobarnRabbitMq");
            using var bus = RabbitHutch.CreateBus(amqp);
            await bus.PubSub.SubscribeAsync<NewVehiclePriceMessage>(
                "autobarn.notifier",
                HandleNewVehiclePriceMessage);
            Console.WriteLine("Subscribed to NewVehiclePriceMessage");
            Console.ReadLine();
        }

        private static Task HandleNewVehiclePriceMessage(NewVehiclePriceMessage nvpm) {
            Console.WriteLine("Received a NewVehicleMessage");
            Console.WriteLine($"{nvpm} - listed at {nvpm.ListedAt}");
            Console.WriteLine($"Price: {nvpm.Price} {nvpm.CurrencyCode}");
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
