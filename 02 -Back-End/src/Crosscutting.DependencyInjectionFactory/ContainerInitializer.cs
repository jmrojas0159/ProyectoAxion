#region

using Application.Main.Definition;
using Application.Main.Implementation.AppServices;
using Core.GlobalRepository.SQL.User;
using DataAccess.UserModule.Repository;
using DataAccess.UserModule.UnitOfWork;
using Microsoft.Practices.Unity;


#endregion

namespace Crosscutting.DependencyInjectionFactory
{

    public static class ContainerInitializer
    {
        public static void InitializeContainer(this IUnityContainer container)
        {
            container.RegisterType<IUserContext, UserContext>(new PerResolveLifetimeManager());
            //Repositories
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<IClientRepository, ClientRepository>();
            container.RegisterType<ITransactionRepository, TransactionRepository>();

            //AppService
            container.RegisterType<IUserDataAppService, UserAppService>();
            container.RegisterType<IProcessDataAppService, ProcessAppService>();
        }
    }
}