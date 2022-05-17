using EasyNetQ;
using Messages;
const string AMQP = "amqps://wuhrwzeg:J7gFVyAHCH3SohigAX6t2BYmUOv4pKed@jackal.rmq.cloudamqp.com/wuhrwzeg";
var bus = RabbitHutch.CreateBus(AMQP);
var count = 0;
while (true)
{
    Console.WriteLine("Press any key to publish a message!");
    var key = Console.ReadKey(true);
    if (key.KeyChar == 'w')
    {
        var warning = new Warning
        {
            Name = "CodeCamp",
            CreatedAt = DateTimeOffset.UtcNow,
            Number = count++
        };
        bus.PubSub.Publish(warning);
        Console.WriteLine($"WARN: {warning}");
    }
    else
    {
        var greeting = new Greeting
        {
            Name = "CodeCamp",
            CreatedAt = DateTimeOffset.UtcNow,
            Number = count++
        };
        bus.PubSub.Publish(greeting);
        Console.WriteLine($"Sent {greeting}");
    }
}


