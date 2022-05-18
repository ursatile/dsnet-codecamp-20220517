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
        public override string ToString() {
            return $"{Make} {Model} ({Color}, {Year})";
        }
        public NewVehiclePriceMessage WithPrice(int price, string currencyCode) {
            var nvpm = new NewVehiclePriceMessage() {
                Make = Make,
                Model = Model,
                Color = Color,
                Year = Year,
                ListedAt = ListedAt,
                Registration = Registration,
                CurrencyCode = currencyCode,
                Price = price
            };
            return nvpm;
        }
    }

    public class NewVehiclePriceMessage : NewVehicleMessage {
        public int Price { get; set; }
        public string CurrencyCode { get; set; }
    }
}
