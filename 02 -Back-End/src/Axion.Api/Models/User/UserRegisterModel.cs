
#region

using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;
using Core.DataTransferObject.Enumerations;
using Newtonsoft.Json;

#endregion

namespace Axion.Api.Models.User
{
    public class UserRegisterModel
    {
        public int Id { get; set; }
        public int IdRole { get; set; }
        public int IdCountry { get; set; }
        public int IdStatus { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [MembershipPassword(
            MinRequiredNonAlphanumericCharacters = 0,
            MinNonAlphanumericCharactersError = "Tu contraseña debe contener al menos un símbolo (!, @, #, Etc).",
            ErrorMessage = "Su contraseña debe tener 8 caracteres y contener al menos un símbolo (!, @, #, Etc).",
            MinRequiredPasswordLength = 8
        )]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [JsonIgnore]
        public string UserName => Email;

        [Required]
        public string FullName { get; set; }

        [MinLength(6)]
        [Range(0, long.MaxValue, ErrorMessage = "Ingrese un número entero válido.")]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Photo { get; set; }

    }
}