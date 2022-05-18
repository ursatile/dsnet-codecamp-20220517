using System;
using System.IO;
using System.Threading.Tasks;
using Autobarn.Messages;
using Autobarn.PricingEngine;
using EasyNetQ;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace Autobarn.PricingClient {
    class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();


        static async Task Main(string[] args) {
            var grpcUrl = config.GetConnectionString("AutobarnPricingServerUrl");
            using var channel = GrpcChannel.ForAddress(grpcUrl);
            using var bus = RabbitHutch.CreateBus(config.GetConnectionString("AutobarnRabbitMq"));
            var handler = MakeHandler(bus, new Pricer.PricerClient(channel));
            await bus.PubSub.SubscribeAsync("autobarn.pricingclient", handler);
            Console.WriteLine("Listening for NewVehicleMessages...");
            Console.ReadLine();
        }

        private static Func<NewVehicleMessage, Task> MakeHandler(IBus bus, Pricer.PricerClient grpcClient) {
            return async nvm => {
                Console.WriteLine($"Calculating price for {nvm}");
                var request = new PriceRequest {
                    Manufacturer = nvm.Make,
                    Model = nvm.Model,
                    Color = nvm.Color,
                    Year = nvm.Year
                };
                var priceReply = await grpcClient.GetPriceAsync(request);
                Console.WriteLine($"{priceReply.Price} {priceReply.CurrencyCode}");
                var nvpm = nvm.WithPrice(priceReply.Price, priceReply.CurrencyCode);
                await bus.PubSub.PublishAsync(nvpm);
            };

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
