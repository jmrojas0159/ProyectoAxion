namespace Core.Entities.User
{
    using System.Collections.Generic;
    
    public class Profile
    {
        public Profile()
        {
            InvestmentFundProfile = new HashSet<InvestmentFundProfile>();
            UserProfile = new HashSet<UserProfile>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<InvestmentFundProfile> InvestmentFundProfile { get; set; }
        public virtual ICollection<UserProfile> UserProfile { get; set; }
    }
}
