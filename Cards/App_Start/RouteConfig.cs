using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Cards
{
    public class RouteConfig
    {
        public static void RegisterRoutes ( RouteCollection routes )
        {
            routes.IgnoreRoute( "{resource}.axd/{*pathInfo}" );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new
            //    {
            //        controller = "Home",
            //        action = "Index",
            //        id = UrlParameter.Optional
            //    }
            //);

            routes.MapRoute(
                name: "Mngr",
                url: "Mngr/{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "People",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                constraints: new
                {
                    action = "(?!Show).*"
                }
            );

            // routes.MapRoute(
            //     name: "MngrInvites",
            //     url: "Mngr/Invites/{action}/{id}",
            //     defaults: new
            //     {
            //         controller = "Invitations",
            //         action = "Index",
            //         id = UrlParameter.Optional
            //     }
            //     , constraints: new
            //     {
            //         action = "!Show"
            //     }
            // );

            // routes.MapRoute(
            //    name: "MngrAcc",
            //    url: "Mngr/Account/{action}/{id}",
            //    defaults: new
            //    {
            //        controller = "Account",
            //        action = "Login",
            //        id = UrlParameter.Optional
            //    }
            //);

            routes.MapRoute(
                name: "Invites",
                url: "Invite/{id}",
                defaults: new
                {
                    controller = "Invitations",
                    action = "Show",
                    id = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "Default",
                url: "",
                defaults: new
                {
                    controller = "Invitations",
                    action = "Show",
                    id = UrlParameter.Optional
                }
            );

            routes.MapRoute( name: "Error",
                url: "{*url}",
                defaults: new
            {
                controller = "Invitations",
                action = "Show",
                id = UrlParameter.Optional
            } );
        }
    }
}
