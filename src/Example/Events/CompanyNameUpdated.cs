namespace Example.Events
{
    public class CompanyNameUpdated : EventBase
    {
        public string Id { get; set; }
        public string NewName { get; set; }
    }
}