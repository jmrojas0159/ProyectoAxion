using Core.GlobalRepository.SQL.User;
using Core.Entities.User;
using Data.Common.Implementation;
using DataAccess.UserModule.UnitOfWork;

namespace DataAccess.UserModule.Repository
{
    
    
    
    public  partial class FinancialMovementsFundsRepository : Repository<FinancialMovementsFunds>, IFinancialMovementsFundsRepository
    {
        private IUserContext _context;
    	
    
        public FinancialMovementsFundsRepository(IUserContext uow): base(uow)
        {
    	
    	    this.SetContext();
        }
    
            private void SetContext()
            {
                this._context = UnitOfWork as IUserContext;
            }
    
    
    }
}
