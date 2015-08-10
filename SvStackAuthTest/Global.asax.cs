using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Funq;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;

namespace SvStackAuthTest
{
    public class Global : System.Web.HttpApplication
    {

        public class GServiceHost : AppHostBase
        {
            public GServiceHost() : base("GService", typeof(TestService).Assembly)
            {
            }

            public override void Configure(Container container)
            {
                Func<string, string> localize = s => HostContext.AppHost.ResolveLocalizedString(s, null);
                Plugins.Add(new SessionFeature());
                SetConfig(new HostConfig(){AllowSessionIdsInHttpParams = true});

                var appSettings = new AppSettings();
               
                var authFeature = new AuthFeature(() => new AuthUserSession(), new IAuthProvider[] { new CredentialsAuthProvider{ SkipPasswordVerificationForInProcessRequests = true }, new FacebookAuthProvider(appSettings) }) { HtmlRedirect = null };
                authFeature.IncludeAssignRoleServices = false;

                Plugins.Add(new ServerEventsFeature()
                {
                    LimitToAuthenticatedUsers = true,
                    NotifyChannelOfSubscriptions = false,
                });
                Plugins.Add(new RegistrationFeature());
                Plugins.Add(authFeature);
                var authRepo = new InMemoryAuthRepository();
                container.Register<IUserAuthRepository>(authRepo);
                authRepo.CreateUserAuth(new UserAuth() { UserName = "testuser" }, "testpassword");
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            GServiceHost gHost = new GServiceHost();
            gHost.Init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}