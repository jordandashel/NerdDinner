using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdDinner.Models
{
    public interface IDinnerRepository
    {
        IQueryable<Dinner> FindAllDinners();
        IQueryable<Dinner> FindByLocation(float latitude, float longitude);
        IQueryable<Dinner> FindUpcomingDinners();
        Dinner GetDinner(int id);

        void Add(Dinner dinner);
        void Delete(Dinner dinner);

        void Save();
    }
}
