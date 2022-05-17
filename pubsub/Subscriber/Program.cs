using EasyNetQ;
using Messages;
const string AMQP = "amqps://wuhrwzeg:J7gFVyAHCH3SohigAX6t2BYmUOv4pKed@jackal.rmq.cloudamqp.com/wuhrwzeg";
var bus = RabbitHutch.CreateBus(AMQP);
const string subscriberId = "SUBSCRIBER";
bus.PubSub.Subscribe<Greeting>(subscriberId, greeting => Console.WriteLine(greeting));
Console.WriteLine("Listening for messages...");
Console.ReadLine();
Console.WriteLine("Exiting cleanly. Thank you!");
