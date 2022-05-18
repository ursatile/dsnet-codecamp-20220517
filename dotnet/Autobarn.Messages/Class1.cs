using System;

namespace Autobarn.Messages {
    public class NewVehicleMessage {
        public string Make { get; set; }
        public string Model { get; set; }
        public string Registration { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public DateTimeOffset ListedAt { get; set; } = DateTimeOffset.UtcNow;
        public string[] Features { get; set; } = { };
    }
}
