using Autofac;
using Database;
using OnlineWalletServer.Authentication;
using Services;
using Services.Transactions;

namespace OnlineWalletServer
{
    public class ModuleApp : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<ModuleServices>();

            //builder.RegisterType<WalletDbContext>().AsSelf().PropertiesAutowired();
            builder.RegisterType<WalletDbContext>().AsSelf();
            builder.RegisterType<Authenticator>().As<IAuthenticator>();//.PropertiesAutowired();
            builder.RegisterType<TransactionPerformer>().As<ITransactionPerformer>().PropertiesAutowired();

        }
    }
}