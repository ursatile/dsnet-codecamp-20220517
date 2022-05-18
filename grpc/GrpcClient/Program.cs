
using Greeter;
using Grpc.Net.Client;

// See https://aka.ms/new-console-template for more information
using var channel = GrpcChannel.ForAddress("https://localhost:7148");
var client = new Greeter.Greeter.GreeterClient(channel);
Console.WriteLine("Ready! Press a key to send a gRPC call...");
var count = 1;
while (true) {
    Console.WriteLine("1: en-GB, 2; ro-RO, 3: en-AU, 4: en-US:");
    var language = Console.ReadKey(true).KeyChar;
    var request = new HelloRequest {
        FirstName = $"CodeCamp",
        LastName = $"{count++}",
        Language = language switch {
            '1' => "en-GB",
            '2' => "ro-RO",
            '3' => "en-AU",
            '4' => "en-US",
            _ => String.Empty
        }
    };
    try {
        var reply = await client.SayHelloAsync(request);
        Console.WriteLine(reply.Message);
    } catch (Exception ex) {
        Console.WriteLine("ERROR" + ex.Message);
    }

}

