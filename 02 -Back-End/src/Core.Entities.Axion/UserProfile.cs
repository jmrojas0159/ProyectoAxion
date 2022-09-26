namespace Core.Entities.User
{
    using System.Collections.Generic;
    
    public class UserProfile
    {
        public UserProfile()
        {
            AllocatedFunds = new HashSet<AllocatedFunds>();
        }
    
        public long Id { get; set; }
        public long UserId { get; set; }
        public int ProfileId { get; set; }
    
        //Navegations properties
        public virtual ICollection<AllocatedFunds> AllocatedFunds { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual User User { get; set; }
    }
}
