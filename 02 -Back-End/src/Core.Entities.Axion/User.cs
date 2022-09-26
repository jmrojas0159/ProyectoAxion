namespace Core.Entities.User
{
    using System;
    using System.Collections.Generic;
    
    public class User
    {
        public User()
        {
            UserProfile = new HashSet<UserProfile>();
        }
    
        public long Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string Identification { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? IdentificationType { get; set; }
    
        public virtual ICollection<UserProfile> UserProfile { get; set; }
    }
}
