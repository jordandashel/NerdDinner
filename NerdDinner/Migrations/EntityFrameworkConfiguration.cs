using NerdDinner.Models;

namespace NerdDinner.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class EntityFrameworkConfiguration : DropCreateDatabaseIfModelChanges<DinnersDbContext>
    {
        protected override void Seed(NerdDinner.Models.DinnersDbContext context)
        {
            context.Dinners.Add(new Dinner
            {
                Title = "Cooking with E-Z Cheez",
                Address = "1234 Main St",
                ContactPhone = "206-867-5309",
                Country = "USA",
                Description = "Cooking with the most versatile ingredient in your pantry",
                EventDate = DateTime.Now.Date,
                HostedBy = "J. Child",
                Latitude = 4f,
                Longitude = 5f
            });

            context.Dinners.Add(
                new Dinner
                {
                    Title = "\"The Room\" Viewing",
                    Address = "1234 Main St",
                    ContactPhone = "206-867-5309",
                    Country = "USA",
                    Description = "Hard-core Wiseau fans come together to appreciate his genius",
                    EventDate = DateTime.Now.Date,
                    HostedBy = "Hey Mark",
                    Latitude = 45f,
                    Longitude = -92f
                });

            context.SaveChanges();
        }

    }
}
