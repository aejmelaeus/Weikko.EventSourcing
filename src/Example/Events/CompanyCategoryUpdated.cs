namespace Example.Events
{
    public class CompanyCategoryUpdated : EventBase
    {
        public string Id { get; set; }
        public string NewCategory { get; set; }
    }
}