namespace OA.Domain.Events
{
    public class BaseEvent
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }
    }
}
