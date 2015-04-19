using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Cards.Context.Models;

namespace Cards.Context
{
    public class ListHelper
    {

        public static List<PersonView> GetPersons ( Guid? inviteId )
        {

            var context = new WeddingContext();
            Invitation invite = null;
            if ( inviteId.HasValue )
                invite = context.Invitations.Include( x => x.Persons ).FirstOrDefault( x => x.ID == inviteId.Value );

            var p = context.Invitations
                .Include( x => x.Persons );
            if ( inviteId.HasValue )
                p = p.Where( x => x.ID != inviteId );
            var usedPersons = p.SelectMany( x => x.Persons )
                .Select( x => x.ID )
                .Distinct().ToList();
            var r = from person in context.Persons
                    where !usedPersons.Contains( person.ID )
                    select new PersonView()
                    {
                        IsSelected = false,//invite != null && invite.Persons.Any( x => x.ID == person.ID ),
                        Person = person
                    };
            var l = r.ToList();
            if ( invite != null )
                foreach ( PersonView personView in l )
                {
                    personView.IsSelected = invite.Persons.Contains( personView.Person );
                }
            return l;
        }
    }

    public class PersonView
    {
        public bool IsSelected
        {
            get;
            set;
        }

        public Person Person
        {
            get;
            set;
        }
    }
}