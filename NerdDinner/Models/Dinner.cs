using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace NerdDinner.Models
{
    public partial class Dinner
    {
        [Key]
        public int DinnerId { get; set; }
        
        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EventDate { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Description { get; set; }
        
        [StringLength(20)]
        [Display(Name = "Host")]
        public string HostedBy { get; set; }
        
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone")]
        [RegularExpression(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}")]
        public string ContactPhone { get; set; }
        
        [Required]
        public string Address { get; set; }
        
        [Required]
        public string Country { get; set; }

        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public virtual IList<Rsvp> Rsvps { get; set; }

        public bool IsHostedBy(string userName)
        {
            return HostedBy.Equals(userName, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool IsUserRegistered(string userName)
        {
            return Rsvps.Any(r => r.AttendeeName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}