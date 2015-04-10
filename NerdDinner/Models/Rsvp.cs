using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NerdDinner.Models
{
    public class Rsvp
    {
        public int RsvpId { get; set; }
        public string AttendeeName { get; set; }
        public virtual Dinner Dinner { get; set; }    
    }
}