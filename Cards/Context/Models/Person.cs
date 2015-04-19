using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cards.Context.Models
{
    public class Person
    {
        [Key]
        [Required]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        public int ID
        {
            get;
            set;
        }

        [Required]
        public string ShortName
        {
            get;
            set;
        }

        [Required]
        public string FullName
        {
            get;
            set;
        }
    }
}