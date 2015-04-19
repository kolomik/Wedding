using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Cards.Models;

namespace Cards
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync ( IdentityMessage message )
        {
            // Plug in your email service here to send an email.
            return Task.FromResult( 0 );
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync ( IdentityMessage message )
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult( 0 );
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager ( IUserStore<ApplicationUser> store )
            : base( store )
        {
        }

        private ApplicationUser GetUser ()
        {
            return new ApplicationUser()
            {
                Email = "lcd@mngr.wed",
                UserName = "Lcd",
                Id = "1",
                SecurityStamp = "athfgjnfdgrfg"
            };
        }

        public override Task<ApplicationUser> FindByIdAsync ( string userId )
        {
            return Task.FromResult( GetUser() );
        }

        public override Task<ApplicationUser> FindByNameAsync ( string userName )
        {
            //return base.FindByNameAsync(userName);
            return Task.FromResult( GetUser() );
        }

        public override Task<bool> IsLockedOutAsync ( string userId )
        {
            return Task.FromResult( false );
        }

        public override Task<bool> CheckPasswordAsync ( ApplicationUser user, string password )
        {
            return Task.FromResult( user.Email == "lcd@mngr.wed" && password == ";.x+w=8f:-b62Jf" );
        }


        public static ApplicationUserManager Create ( IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context )
        {
            var manager = new ApplicationUserManager( new UserStore<ApplicationUser>( context.Get<ApplicationDbContext>() ) );
            // Configure validation logic for usernames
            //manager.UserValidator = new UserValidator<ApplicationUser>( manager )
            //{
            //    AllowOnlyAlphanumericUserNames = false,
            //    RequireUniqueEmail = true
            //};

            //// Configure validation logic for passwords
            //manager.PasswordValidator = new PasswordValidator
            //{
            //    RequiredLength = 6,
            //    RequireNonLetterOrDigit = true,
            //    RequireDigit = true,
            //    RequireLowercase = true,
            //    RequireUppercase = true,
            //};

            // Configure user lockout defaults
            //manager.UserLockoutEnabledByDefault = true;
            //manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes( 5 );
            //manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            //manager.RegisterTwoFactorProvider( "Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            //{
            //    MessageFormat = "Your security code is {0}"
            //} );
            //manager.RegisterTwoFactorProvider( "Email Code", new EmailTokenProvider<ApplicationUser>
            //{
            //    Subject = "Security Code",
            //    BodyFormat = "Your security code is {0}"
            //} );
            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();
            //var dataProtectionProvider = options.DataProtectionProvider;
            //if ( dataProtectionProvider != null )
            //{
            //    manager.UserTokenProvider =
            //        new DataProtectorTokenProvider<ApplicationUser>( dataProtectionProvider.Create( "ASP.NET Identity" ) );
            //}

            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager ( ApplicationUserManager userManager, IAuthenticationManager authenticationManager )
            : base( userManager, authenticationManager )
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync ( ApplicationUser user )
        {

            return user.GenerateUserIdentityAsync( ( ApplicationUserManager ) UserManager );
        }

        public override async Task<SignInStatus> PasswordSignInAsync ( string userName, string password, bool isPersistent, bool shouldLockout )
        {
            if ( UserManager == null )
            {
                return SignInStatus.Failure;
            }
            var user = await UserManager.FindByNameAsync( userName );
            if ( user == null )
            {
                return SignInStatus.Failure;
            }
            //if ( await UserManager.IsLockedOutAsync( user.Id ) )
            //{
            //    return SignInStatus.LockedOut;
            //}
            if ( await UserManager.CheckPasswordAsync( user, password ) )
            {
                return await Task.Run( () =>
                {
                    SignInAsync( user, isPersistent, false );
                    return SignInStatus.Success;
                } );
            }
            //if ( shouldLockout )
            //{
            //    // If lockout is requested, increment access failed count which might lock out the user
            //    await UserManager.AccessFailedAsync( user.Id );
            //    if ( await UserManager.IsLockedOutAsync( user.Id ) )
            //    {
            //        return SignInStatus.LockedOut;
            //    }
            //}
            return SignInStatus.Failure;
        }

        public override async Task SignInAsync ( ApplicationUser user, bool isPersistent, bool rememberBrowser )
        {
            var userIdentity = await CreateUserIdentityAsync( user );
            // Clear any partial cookies from external or two factor partial sign ins
            AuthenticationManager.SignOut( DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie );
            if ( rememberBrowser )
            {
                var rememberBrowserIdentity = AuthenticationManager.CreateTwoFactorRememberBrowserIdentity( ConvertIdToString( user.Id ) );
                AuthenticationManager.SignIn( new AuthenticationProperties
                {
                    IsPersistent = isPersistent
                }, userIdentity, rememberBrowserIdentity );
            }
            else
            {
                AuthenticationManager.SignIn( new AuthenticationProperties
                {
                    IsPersistent = isPersistent
                }, userIdentity );
            }
        }


        public static ApplicationSignInManager Create ( IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context )
        {
            return new ApplicationSignInManager( context.GetUserManager<ApplicationUserManager>(), context.Authentication );
        }
    }
}
