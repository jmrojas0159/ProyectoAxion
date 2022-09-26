namespace Core.Entities.User
{
    
    public class AllocatedFunds
    {
        public long Id { get; set; }
        public long InvestmentFundProfileId { get; set; }
        public long UserProfileId { get; set; }
        public decimal Distribution { get; set; }
    
        public virtual InvestmentFundProfile InvestmentFundProfile { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
