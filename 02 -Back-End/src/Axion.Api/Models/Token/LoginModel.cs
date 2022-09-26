
#region

using System.ComponentModel.DataAnnotations;
using Core.DataTransferObject.Enumerations;

#endregion

namespace Axion.Api.Models.Token
{
    public class LoginModel
    {
        [Required] public string UserName { get; set; }

        [Required] public string Password { get; set; }

    }
}