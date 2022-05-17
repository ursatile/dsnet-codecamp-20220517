namespace Messages;
public class Greeting
{
    public string Name { get; set; } = "World";
    public DateTimeOffset CreatedAt { get; set; }
    public string MachineName { get; set; } = Environment.MachineName;
    public int Number { get;set;}

    public override string ToString()
    {
        return $"{Number}: {Name} from {MachineName} at {CreatedAt:O}";
    }
}
