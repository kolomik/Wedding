using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cards.Context.Models
{
    public class Invitation
    {

        [Key]
        [Required]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        public Guid ID
        {
            get;
            set;
        }


        [MaxLength( 55 )]
        [Index( "FriendlyNameIndex", IsUnique = true )]
        public string FriendlyName
        {
            get;
            set;
        }

        [Required]
        public bool IsAccepted
        {
            get;
            set;
        }

        public List<Person> Persons
        {
            get;
            set;
        }
    }
}