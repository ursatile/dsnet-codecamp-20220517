using System;
using System.IO;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Autobarn.Notifier {
    internal class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();
        private static HubConnection hub;
        static async Task Main(string[] args)
        {
            var url = config["AutobarnSignalRHubUrl"];
            hub = new HubConnectionBuilder().WithUrl(url).Build();
            await hub.StartAsync();
            Console.WriteLine("connected to SignalR!");
            var amqp = config.GetConnectionString("AutobarnRabbitMq");
            using var bus = RabbitHutch.CreateBus(amqp);
            await bus.PubSub.SubscribeAsync<NewVehiclePriceMessage>(
                "autobarn.notifier",
                HandleNewVehiclePriceMessage);
            Console.WriteLine("Subscribed to NewVehiclePriceMessage");
            Console.ReadLine();
        }

        private static async Task HandleNewVehiclePriceMessage(NewVehiclePriceMessage nvpm) {
            Console.WriteLine("Received a NewVehicleMessage");
            Console.WriteLine($"{nvpm} - listed at {nvpm.ListedAt}");
            Console.WriteLine($"Price: {nvpm.Price} {nvpm.CurrencyCode}");
            await hub.SendAsync("NotifyWebUsers", "autobarn.notifier", JsonConvert.SerializeObject(nvpm));
            Console.WriteLine("Sent JSON to SignalR");
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
