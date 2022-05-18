using Grpc.Core;
using Greeter;

namespace Greeter.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        var message = request.Language switch {
            "en-GB" => $"Hello, {request.FirstName} {request.LastName}",
            "ro-RO" => $"Salut, {request.FirstName} {request.LastName}",
            "en-AU" => throw new RpcException(new Status(StatusCode.Unimplemented, "Invalid language code"), "We don't speak Australian round here"),
            "en-US" => $"Howdy, {request.FirstName} {request.LastName}",
            _ => $"Greeting, {request.FirstName} {request.LastName}"
        };
        return Task.FromResult(new HelloReply {
            Message = message
        });
    }
}
