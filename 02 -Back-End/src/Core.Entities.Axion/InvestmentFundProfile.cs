
namespace Core.Entities.User
{
    using System.Collections.Generic;
    
    public class InvestmentFundProfile
    {
        public InvestmentFundProfile()
        {
            AllocatedFunds = new HashSet<AllocatedFunds>();
        }
    
        public long Id { get; set; }
        public int ProfileId { get; set; }
        public int InvestmentFundId { get; set; }
        public decimal Distribution { get; set; }
    
        public virtual ICollection<AllocatedFunds> AllocatedFunds { get; set; }
        public virtual InvestmentFund InvestmentFund { get; set; }
        public virtual Profile Profile { get; set; }
    }
}
