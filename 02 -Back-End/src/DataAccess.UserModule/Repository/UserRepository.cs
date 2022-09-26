using Core.GlobalRepository.SQL.User;
using Core.Entities.User;
using Data.Common.Implementation;
using DataAccess.UserModule.UnitOfWork;

namespace DataAccess.UserModule.Repository
{
    
    
    
    public  partial class UserRepository : Repository<User>, IUserRepository
    {
        private IUserContext _context;
    	
    
        public UserRepository(IUserContext uow): base(uow)
        {
    	
    	    this.SetContext();
        }
    
            private void SetContext()
            {
                this._context = UnitOfWork as IUserContext;
            }
    
    
    }
}
