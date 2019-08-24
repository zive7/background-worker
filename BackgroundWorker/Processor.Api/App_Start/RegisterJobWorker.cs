namespace Processor.Api.App_Start
{
    using Hangfire;
    using Hangfire.SqlServer;
    using Ninject;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public static class RegisterJobWorker
    {
        public static void RegisterJobsWorker(this IKernel kernel)
        {
            string connectionstring = "data source=(localdb)\\MSSQLLocalDB;initial catalog=LocalBWorkerDB;integrated security=True;multipleactiveresultsets=True;application name=EntityFramework;";
            var sqlStorage = new SqlServerStorage(connectionstring);
            var options = new BackgroundJobServerOptions
            {
                ServerName = "(localdb)\\MSSQLLocalDB"
            };
            JobStorage.Current = sqlStorage;

            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionstring);

            //HangFire
            //GlobalConfiguration.Configuration.UseNinjectActivator(kernel);
        }
    }
}