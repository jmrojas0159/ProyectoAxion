
using Core.Entities.User;

namespace Core.DataTransferObject.Axion
{
    public class ValidateUserLogin
    {
        public bool Result { get; set; }
        public string Mesage { get; set; }
        public User User { get; set; }
    }
}
