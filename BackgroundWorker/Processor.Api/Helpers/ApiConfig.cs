namespace Processor.Api.Helpers
{
    using Hangfire;
    using Ninject;
    using Ninject.Web.Common;
    using Processor.Api.App_Start;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public static class ApiConfig
    {
        internal static void RegisterServices(IKernel kernel)
        {
            kernel.RegisterJobsWorker();
        }

        public static Ninject.Syntax.IBindingNamedWithOrOnSyntax<T> InRequestAndBackgroundScope<T>(this Ninject.Syntax.IBindingWhenInNamedWithOrOnSyntax<T> syntax)
        {
            return syntax.InNamedOrBackgroundJobScope(context => GetScopeFromContext(context));
        }

        public static Ninject.Syntax.IBindingNamedWithOrOnSyntax<T> InRequestAndBackgroundOrThreadScope<T>(this Ninject.Syntax.IBindingWhenInNamedWithOrOnSyntax<T> syntax)
        {
            return syntax.InNamedOrBackgroundJobScope(context =>
            {
                var requestScope = GetScopeFromContext(context);
                if (requestScope == null)
                {
                    if (JobActivatorScope.Current == null)
                    {
                        return System.Threading.Thread.CurrentThread;
                    }
                    else
                    {
                        return JobActivatorScope.Current;
                    }
                }
                else
                {
                    return requestScope;
                }
            });
        }

        private static object GetScopeFromContext(Ninject.Activation.IContext context)
        {
            return context.Kernel.Components.GetAll<INinjectHttpApplicationPlugin>()
                                            .Select(c => c.GetRequestScope(context))
                                            .FirstOrDefault(s => s != null);
        }

        public static bool IsDebug()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}