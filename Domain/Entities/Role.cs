namespace Domain.Entities
{
    public class Role
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal TransactionLimit { get; set; }
        public decimal DailyLimit { get; set; }
        public ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}
