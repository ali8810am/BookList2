namespace Api.Data
{
    public abstract class BaseDomainObject
    {
        public int Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set;}
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set;}

    }
}
