using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Cards.Context.Models;

namespace Cards.Context
{
    public class WeddingContext : DbContext
    {

        public WeddingContext ()
            : base( "DefaultConnection" )
        {
        }

        protected override void OnModelCreating ( DbModelBuilder modelBuilder )
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<Person> Persons
        {
            get;
            set;
        }

        public DbSet<Invitation> Invitations
        {
            get;
            set;
        }
    }
}