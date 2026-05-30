using Internshala.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Infrastructure.Persistence.Seeders;

public static class SkillSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var skills = new[]
        {
            new Skill { Id = 1,  Name = "Python",          Slug = "python",           IsActive = true },
            new Skill { Id = 2,  Name = "JavaScript",      Slug = "javascript",       IsActive = true },
            new Skill { Id = 3,  Name = "React",           Slug = "react",            IsActive = true },
            new Skill { Id = 4,  Name = "Angular",         Slug = "angular",          IsActive = true },
            new Skill { Id = 5,  Name = "Node.js",         Slug = "nodejs",           IsActive = true },
            new Skill { Id = 6,  Name = "Java",            Slug = "java",             IsActive = true },
            new Skill { Id = 7,  Name = "C#",              Slug = "csharp",           IsActive = true },
            new Skill { Id = 8,  Name = "SQL",             Slug = "sql",              IsActive = true },
            new Skill { Id = 9,  Name = "Machine Learning",Slug = "machine-learning", IsActive = true },
            new Skill { Id = 10, Name = "Data Analysis",   Slug = "data-analysis",    IsActive = true },
            new Skill { Id = 11, Name = "Digital Marketing",Slug = "digital-marketing",IsActive = true },
            new Skill { Id = 12, Name = "SEO",             Slug = "seo",              IsActive = true },
            new Skill { Id = 13, Name = "Content Writing", Slug = "content-writing",  IsActive = true },
            new Skill { Id = 14, Name = "Graphic Design",  Slug = "graphic-design",   IsActive = true },
            new Skill { Id = 15, Name = "Figma",           Slug = "figma",            IsActive = true },
            new Skill { Id = 16, Name = "Excel",           Slug = "excel",            IsActive = true },
            new Skill { Id = 17, Name = "Communication",   Slug = "communication",    IsActive = true },
            new Skill { Id = 18, Name = "Leadership",      Slug = "leadership",       IsActive = true },
            new Skill { Id = 19, Name = "Canva",           Slug = "canva",            IsActive = true },
            new Skill { Id = 20, Name = "AWS",             Slug = "aws",              IsActive = true },
            new Skill { Id = 21, Name = "Docker",          Slug = "docker",           IsActive = true },
            new Skill { Id = 22, Name = "Git",             Slug = "git",              IsActive = true },
            new Skill { Id = 23, Name = "Flutter",         Slug = "flutter",          IsActive = true },
            new Skill { Id = 24, Name = "React Native",    Slug = "react-native",     IsActive = true },
            new Skill { Id = 25, Name = "PowerBI",         Slug = "powerbi",          IsActive = true },
        };

        modelBuilder.Entity<Skill>().HasData(skills);
    }
}
