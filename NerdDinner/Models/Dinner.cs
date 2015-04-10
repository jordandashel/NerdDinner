using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NerdDinner.Models
{
    public class Dinner
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
    }

    public class DinnersDbContext : DbContext
    {
        public DbSet<Dinner> Dinners { get; set; }
        public DbSet<Rsvp> Rsvps { get; set; }
    }
}