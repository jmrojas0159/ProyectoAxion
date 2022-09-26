namespace Core.Entities.User
{
    using System.Collections.Generic;
   
    public class InvestmentFund
    {
        public InvestmentFund()
        {
            InvestmentFundProfile = new HashSet<InvestmentFundProfile>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Terms { get; set; }
        public decimal PercentagePenalty { get; set; }
        public int PenaltyOnTime { get; set; }
        public string Logo { get; set; }
        public int Profitability { get; set; }
    
        public virtual ICollection<InvestmentFundProfile> InvestmentFundProfile { get; set; }
    }
}
