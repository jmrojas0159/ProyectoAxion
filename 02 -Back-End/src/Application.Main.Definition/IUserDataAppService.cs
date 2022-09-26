using Core.DataTransferObject.Axion;
using Core.Entities.User;
using System.Collections.Generic;

namespace Application.Main.Definition
{
    public interface IUserDataAppService
    {
        ValidateUserLogin CheckUser(string username, string password);
    }
}
