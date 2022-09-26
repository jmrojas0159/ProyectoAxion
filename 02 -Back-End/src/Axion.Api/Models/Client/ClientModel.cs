#region

using System;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Axion.Api.Models
{
    public class ClientModel
    {
        public int Id { get; set; }
        [Required] public int IdIdentificationType { get; set; }
        public int IdStatus { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Identification { get; set; }
        [Required] public string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }

        [Required] public string Adress { get; set; }
        [Required] public string Email { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }
        [Required] public string Gender { get; set; }
        [Required] public int Age { get; set; }
        [Required] public string SocialCondition { get; set; }
        [Required] public string CivilState { get; set; }
        [Required] public string Country { get; set; }
        [Required] public string City { get; set; }
        [Required] public string Profession { get; set; }

    }
}