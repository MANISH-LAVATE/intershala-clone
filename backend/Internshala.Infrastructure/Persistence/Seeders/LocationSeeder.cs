using Internshala.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Infrastructure.Persistence.Seeders;

public static class LocationSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var locations = new[]
        {
            new Location { Id = 1,  CityName = "Bangalore",   State = "Karnataka",      IsActive = true },
            new Location { Id = 2,  CityName = "Mumbai",      State = "Maharashtra",    IsActive = true },
            new Location { Id = 3,  CityName = "Delhi",       State = "Delhi",          IsActive = true },
            new Location { Id = 4,  CityName = "Hyderabad",   State = "Telangana",      IsActive = true },
            new Location { Id = 5,  CityName = "Chennai",     State = "Tamil Nadu",     IsActive = true },
            new Location { Id = 6,  CityName = "Pune",        State = "Maharashtra",    IsActive = true },
            new Location { Id = 7,  CityName = "Kolkata",     State = "West Bengal",    IsActive = true },
            new Location { Id = 8,  CityName = "Ahmedabad",   State = "Gujarat",        IsActive = true },
            new Location { Id = 9,  CityName = "Jaipur",      State = "Rajasthan",      IsActive = true },
            new Location { Id = 10, CityName = "Lucknow",     State = "Uttar Pradesh",  IsActive = true },
            new Location { Id = 11, CityName = "Noida",       State = "Uttar Pradesh",  IsActive = true },
            new Location { Id = 12, CityName = "Gurgaon",     State = "Haryana",        IsActive = true },
            new Location { Id = 13, CityName = "Kochi",       State = "Kerala",         IsActive = true },
            new Location { Id = 14, CityName = "Bhubaneswar", State = "Odisha",         IsActive = true },
            new Location { Id = 15, CityName = "Coimbatore",  State = "Tamil Nadu",     IsActive = true },
            new Location { Id = 16, CityName = "Indore",      State = "Madhya Pradesh", IsActive = true },
            new Location { Id = 17, CityName = "Chandigarh",  State = "Punjab",         IsActive = true },
            new Location { Id = 18, CityName = "Nagpur",      State = "Maharashtra",    IsActive = true },
            new Location { Id = 19, CityName = "Visakhapatnam", State = "Andhra Pradesh", IsActive = true },
            new Location { Id = 20, CityName = "Surat",       State = "Gujarat",        IsActive = true },
            new Location { Id = 21, CityName = "Mysore",      State = "Karnataka",      IsActive = true },
            new Location { Id = 22, CityName = "Thiruvananthapuram", State = "Kerala",  IsActive = true },
            new Location { Id = 23, CityName = "Patna",       State = "Bihar",          IsActive = true },
            new Location { Id = 24, CityName = "Bhopal",      State = "Madhya Pradesh", IsActive = true },
            new Location { Id = 25, CityName = "Vadodara",    State = "Gujarat",        IsActive = true },
        };

        modelBuilder.Entity<Location>().HasData(locations);
    }
}
