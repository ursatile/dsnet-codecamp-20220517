using EasyNetQ;
using Messages;
const string AMQP = "amqps://wuhrwzeg:J7gFVyAHCH3SohigAX6t2BYmUOv4pKed@jackal.rmq.cloudamqp.com/wuhrwzeg";
var bus = RabbitHutch.CreateBus(AMQP);
const string subscriberId = "dylanbeattie"; // change this to your name
bus.PubSub.Subscribe<Greeting>(subscriberId, HandleGreeting, x => x.WithAutoDelete());
bus.PubSub.Subscribe<Warning>(subscriberId, HandleWarning, x => x.WithAutoDelete());
Console.WriteLine("Listening for messages...");
Console.ReadLine();
Console.WriteLine("Exiting cleanly. Thank you!");

static void HandleWarning(Warning w) {
    var oldColor = System.Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(w);
    Console.ForegroundColor = oldColor;
}

static void HandleGreeting(Greeting g) {
    // if (g.Number % 5 == 0) {
    //     throw new Exception("Greetings with a number ending in 5 are not supported!");
    // }
    Console.WriteLine(g);
    //Thread.Sleep(2000);
}