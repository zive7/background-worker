namespace Processor.Api
{
    using Hangfire;
    using Hangfire.Dashboard;
    using Hangfire.SqlServer;
    using Owin;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;

    public partial class Startup
    {
        public void ConfigureHangfire(IAppBuilder app)
        {
            ConfigureStorage();

            app.UseHangfireServer();

            ConfigureDashboard(app);
        }

        private void ConfigureStorage()
        {
            string connectionstring = "data source=(localdb)\\MSSQLLocalDB;initial catalog=LocalBWorkerDB;integrated security=True;multipleactiveresultsets=True;application name=EntityFramework;";
            var sqlStorage = new SqlServerStorage(connectionstring);
            var options = new BackgroundJobServerOptions
            {
                ServerName = "(localdb)\\MSSQLLocalDB"
            };
            JobStorage.Current = sqlStorage;

            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionstring);
        }

        private static void ConfigureDashboard(IAppBuilder app)
        {
            DashboardOptions options = new DashboardOptions();

                options.AuthorizationFilters = new List<IAuthorizationFilter>
                {
                    new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
                    {
                        RequireSsl = false,
                        LoginCaseSensitive = true,
                        SslRedirect = false,
                        Users = new[]
                        {
                            new BasicAuthAuthorizationUser
                            {
                                Login = "test",
                                PasswordClear = "testt@!!!"
                            }
                        }
                    })
                };

            app.UseHangfireDashboard("/jobs", options);
        }
    }
}