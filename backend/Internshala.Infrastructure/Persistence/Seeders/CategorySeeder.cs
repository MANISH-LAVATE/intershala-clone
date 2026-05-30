using Internshala.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Infrastructure.Persistence.Seeders;

public static class CategorySeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var categories = new[]
        {
            new Category { Id = 1, Name = "Engineering & Technology", Slug = "engineering-technology", IsActive = true },
            new Category { Id = 2, Name = "Marketing", Slug = "marketing", IsActive = true },
            new Category { Id = 3, Name = "Business Development", Slug = "business-development", IsActive = true },
            new Category { Id = 4, Name = "Finance & Accounting", Slug = "finance-accounting", IsActive = true },
            new Category { Id = 5, Name = "Human Resources", Slug = "human-resources", IsActive = true },
            new Category { Id = 6, Name = "Design & UX", Slug = "design-ux", IsActive = true },
            new Category { Id = 7, Name = "Content & Journalism", Slug = "content-journalism", IsActive = true },
            new Category { Id = 8, Name = "Data Science & Analytics", Slug = "data-science-analytics", IsActive = true },
            new Category { Id = 9, Name = "Operations", Slug = "operations", IsActive = true },
            new Category { Id = 10, Name = "Sales", Slug = "sales", IsActive = true },
            new Category { Id = 11, Name = "Legal", Slug = "legal", IsActive = true },
            new Category { Id = 12, Name = "Education & Teaching", Slug = "education-teaching", IsActive = true },
            new Category { Id = 13, Name = "Healthcare & Medicine", Slug = "healthcare-medicine", IsActive = true },
            new Category { Id = 14, Name = "Social Media", Slug = "social-media", IsActive = true },
            new Category { Id = 15, Name = "Research", Slug = "research", IsActive = true },
            new Category { Id = 16, Name = "Architecture", Slug = "architecture", IsActive = true },
            new Category { Id = 17, Name = "Event Management", Slug = "event-management", IsActive = true },
            new Category { Id = 18, Name = "Supply Chain", Slug = "supply-chain", IsActive = true },
            new Category { Id = 19, Name = "Media & Entertainment", Slug = "media-entertainment", IsActive = true },
            new Category { Id = 20, Name = "NGO / Social Work", Slug = "ngo-social-work", IsActive = true },
        };

        modelBuilder.Entity<Category>().HasData(categories);
    }
}
