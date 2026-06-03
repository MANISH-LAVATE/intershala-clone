using Internshala.Application.Common.Interfaces;
using Internshala.Domain.Entities;
using Internshala.Domain.Enums;
using Internshala.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Internshala.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeds realistic sample data for development.
/// Only runs when all main tables are empty.
/// Password for every sample account: Test@1234!
/// </summary>
public static class SampleDataSeeder
{
    // Real password hash is computed once at runtime via IPasswordHasher.
    private static string _passwordHash = string.Empty;

    public static async Task SeedAsync(IServiceProvider services, ILogger logger)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        if (await db.Users.AnyAsync())
        {
            logger.LogInformation("Sample data already present — skipping seed.");
            return;
        }

        logger.LogInformation("Seeding sample data...");

        // Hash the shared sample password once so every seeded account can log in.
        _passwordHash = passwordHasher.Hash("Test@1234!");

        var now = DateTime.UtcNow;
        DateTime D(int daysAgo) => now.AddDays(-daysAgo);

        // ── 1. USERS ─────────────────────────────────────────────────────────
        var admin = U("admin@internshala.dev", "Arjun", "Sharma", UserRole.Admin);

        var studentUsers = new[]
        {
            U("priya.patel@student.dev",    "Priya",    "Patel",    UserRole.Student),
            U("rahul.verma@student.dev",    "Rahul",    "Verma",    UserRole.Student),
            U("sneha.iyer@student.dev",     "Sneha",    "Iyer",     UserRole.Student),
            U("amit.singh@student.dev",     "Amit",     "Singh",    UserRole.Student),
            U("neha.gupta@student.dev",     "Neha",     "Gupta",    UserRole.Student),
            U("rohan.mehta@student.dev",    "Rohan",    "Mehta",    UserRole.Student),
            U("anjali.rao@student.dev",     "Anjali",   "Rao",      UserRole.Student),
            U("vikram.nair@student.dev",    "Vikram",   "Nair",     UserRole.Student),
            U("pooja.sharma@student.dev",   "Pooja",    "Sharma",   UserRole.Student),
            U("kiran.joshi@student.dev",    "Kiran",    "Joshi",    UserRole.Student),
            U("aryan.reddy@student.dev",    "Aryan",    "Reddy",    UserRole.Student),
            U("divya.malhotra@student.dev", "Divya",    "Malhotra", UserRole.Student),
            U("suresh.kumar@student.dev",   "Suresh",   "Kumar",    UserRole.Student),
            U("aisha.khan@student.dev",     "Aisha",    "Khan",     UserRole.Student),
            U("siddharth.bose@student.dev", "Siddharth","Bose",     UserRole.Student),
            U("meera.pillai@student.dev",   "Meera",    "Pillai",   UserRole.Student),
            U("tarun.saxena@student.dev",   "Tarun",    "Saxena",   UserRole.Student),
            U("kavya.desai@student.dev",    "Kavya",    "Desai",    UserRole.Student),
            U("nikhil.mishra@student.dev",  "Nikhil",   "Mishra",   UserRole.Student),
            U("ritu.agarwal@student.dev",   "Ritu",     "Agarwal",  UserRole.Student),
            U("harsh.pandey@student.dev",   "Harsh",    "Pandey",   UserRole.Student),
            U("ananya.chakrabarti@student.dev","Ananya","Chakrabarti",UserRole.Student),
            U("deepak.tiwari@student.dev",  "Deepak",   "Tiwari",   UserRole.Student),
            U("shweta.bansal@student.dev",  "Shweta",   "Bansal",   UserRole.Student),
            U("gaurav.singh@student.dev",   "Gaurav",   "Singh",    UserRole.Student),
        };

        var employerUsers = new[]
        {
            U("hr@techcorp.dev",        "Ravi",   "Kumar",    UserRole.Employer),
            U("careers@infosys.dev",    "Sunita", "Nair",     UserRole.Employer),
            U("jobs@wipro.dev",         "Mohan",  "Reddy",    UserRole.Employer),
            U("hr@flipkart.dev",        "Priya",  "Jain",     UserRole.Employer),
            U("team@zomato.dev",        "Akash",  "Kapoor",   UserRole.Employer),
            U("work@razorpay.dev",      "Ritika", "Sharma",   UserRole.Employer),
            U("hr@cred.dev",            "Sunil",  "Mehta",    UserRole.Employer),
            U("talent@byju.dev",        "Geeta",  "Pillai",   UserRole.Employer),
            U("jobs@paytm.dev",         "Karan",  "Gupta",    UserRole.Employer),
            U("hr@swiggy.dev",          "Nisha",  "Singh",    UserRole.Employer),
            U("careers@ola.dev",        "Ankit",  "Verma",    UserRole.Employer),
            U("hr@freshworks.dev",      "Pooja",  "Iyer",     UserRole.Employer),
            U("team@meesho.dev",        "Rajesh", "Patel",    UserRole.Employer),
            U("jobs@unacademy.dev",     "Sonia",  "Rao",      UserRole.Employer),
            U("hr@cleartax.dev",        "Vivek",  "Saxena",   UserRole.Employer),
            U("talent@zepto.dev",       "Priya",  "Malhotra", UserRole.Employer),
            U("hr@moengage.dev",        "Nitin",  "Kumar",    UserRole.Employer),
            U("careers@browserstack.dev","Kavita","Sharma",   UserRole.Employer),
            U("hr@groww.dev",           "Anand",  "Bose",     UserRole.Employer),
            U("jobs@zerodha.dev",       "Rekha",  "Desai",    UserRole.Employer),
        };

        var allUsers = new List<User> { admin };
        allUsers.AddRange(studentUsers);
        allUsers.AddRange(employerUsers);

        db.Users.AddRange(allUsers);
        await db.SaveChangesAsync();

        // ── 2. STUDENTS ───────────────────────────────────────────────────────
        var institutions = new[] {
            "IIT Bombay","IIT Delhi","NIT Trichy","BITS Pilani","VIT Vellore",
            "Manipal University","Christ University","Jadavpur University",
            "Anna University","Delhi University","Pune University","NMIMS Mumbai",
            "Symbiosis Pune","SRM University","Amity University","LPU",
            "Chandigarh University","KIIT Bhubaneswar","PSG College","MES College"
        };
        var courses = new[] {
            "B.Tech CSE","B.Tech IT","BCA","B.Sc Computer Science","B.Tech ECE",
            "BBA","MBA","B.Com","B.Sc Statistics","B.Tech Mechanical",
            "B.Design","B.Tech Civil","B.Sc Mathematics","MCA","B.Tech Chemical"
        };
        var genders = new[] { "Male", "Female", "Non-binary" };

        var students = new List<Student>();
        for (int i = 0; i < studentUsers.Length; i++)
        {
            students.Add(new Student
            {
                UserId = studentUsers[i].Id,
                CurrentInstitution = institutions[i % institutions.Length],
                CourseOfStudy = courses[i % courses.Length],
                GraduationYear = (short)(2024 + (i % 3)),
                Gpa = Math.Round(6.5m + (i % 4) * 0.4m, 1),
                Gender = genders[i % 3],
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-21).AddDays(-i * 15)),
                Bio = $"Passionate {courses[i % courses.Length]} student from {institutions[i % institutions.Length]} looking for exciting internship opportunities.",
                LinkedInUrl = $"https://linkedin.com/in/{studentUsers[i].FirstName.ToLower()}-{studentUsers[i].LastName.ToLower()}",
                GitHubUrl = i % 3 == 0 ? $"https://github.com/{studentUsers[i].FirstName.ToLower()}{i}" : null,
                IsProfileComplete = true,
                ProfileCompleteness = (byte)(70 + (i % 4) * 8),
                CreatedAt = D(60 - i),
                UpdatedAt = D(10 - (i % 10))
            });
        }
        db.Students.AddRange(students);
        await db.SaveChangesAsync();

        // ── 3. EMPLOYERS ─────────────────────────────────────────────────────
        var companies = new[]
        {
            ("TechCorp India",    "IT Services",        "https://techcorp.in",  1,  "Bangalore",  500,  2015, 1),
            ("Infosys",           "IT Consulting",       "https://infosys.com",  1,  "Bangalore",  5000, 1981, 1),
            ("Wipro",             "IT Solutions",        "https://wipro.com",    1,  "Hyderabad",  5000, 1945, 1),
            ("Flipkart",          "E-Commerce",          "https://flipkart.com", 3,  "Bangalore",  2000, 2007, 1),
            ("Zomato",            "Food Tech",           "https://zomato.com",   9,  "Gurgaon",    1000, 2008, 1),
            ("Razorpay",          "Fintech",             "https://razorpay.com", 4,  "Bangalore",  400,  2014, 1),
            ("CRED",              "Fintech",             "https://cred.club",    4,  "Bangalore",  300,  2018, 0),
            ("BYJU'S",            "Ed-Tech",             "https://byjus.com",    12, "Bangalore",  2000, 2011, 1),
            ("Paytm",             "Fintech",             "https://paytm.com",    4,  "Noida",      1500, 2010, 1),
            ("Swiggy",            "Food Delivery",       "https://swiggy.com",   9,  "Bangalore",  1000, 2014, 1),
            ("Ola Cabs",          "Mobility Tech",       "https://olacabs.com",  1,  "Bangalore",  800,  2010, 1),
            ("Freshworks",        "SaaS",                "https://freshworks.com",1,  "Chennai",    700,  2010, 1),
            ("Meesho",            "Social Commerce",     "https://meesho.com",   10, "Bangalore",  500,  2015, 0),
            ("Unacademy",         "Ed-Tech",             "https://unacademy.com",12, "Bangalore",  600,  2015, 1),
            ("ClearTax",          "Tax Tech",            "https://cleartax.in",  4,  "Bangalore",  200,  2011, 1),
            ("Zepto",             "Quick Commerce",      "https://zepto.in",     9,  "Mumbai",     300,  2021, 0),
            ("MoEngage",          "Marketing Tech",      "https://moengage.com", 2,  "Bangalore",  200,  2014, 1),
            ("BrowserStack",      "Dev Tools",           "https://browserstack.com",1,"Mumbai",    400,  2011, 1),
            ("Groww",             "Investment Tech",     "https://groww.in",     4,  "Bangalore",  400,  2016, 1),
            ("Zerodha",           "Stock Broking",       "https://zerodha.com",  4,  "Bangalore",  350,  2010, 1),
        };

        var employers = new List<Employer>();
        for (int i = 0; i < employerUsers.Length; i++)
        {
            var c = companies[i];
            var sizeMap = new Dictionary<int, string> { {200,"51-200"},{300,"201-500"},{400,"201-500"},{500,"201-500"},{600,"201-500"},{700,"501-1000"},{800,"501-1000"},{1000,"1001-5000"},{1500,"1001-5000"},{2000,"1001-5000"},{5000,"5001+"} };
            var sizeKey = c.Item6; var sizeStr = sizeMap.ContainsKey(sizeKey) ? sizeMap[sizeKey] : "51-200";

            employers.Add(new Employer
            {
                UserId = employerUsers[i].Id,
                CompanyName = c.Item1,
                Description = $"{c.Item1} is a leading {c.Item2} company headquartered in {c.Item5}.",
                CompanyWebsite = c.Item3,
                IndustryId = c.Item4,
                CompanySize = sizeStr,
                HeadquartersCity = c.Item5,
                Founded = (short)c.Item7,
                IsVerified = c.Item8 == 1,
                VerifiedAt = c.Item8 == 1 ? D(100) : null,
                LinkedInUrl = $"https://linkedin.com/company/{c.Item1.ToLower().Replace(" ", "-").Replace("'", "")}",
                CreatedAt = D(90 - i),
                UpdatedAt = D(5)
            });
        }
        db.Employers.AddRange(employers);
        await db.SaveChangesAsync();

        // ── 4. INTERNSHIPS ───────────────────────────────────────────────────
        var internshipData = new[]
        {
            // (title, desc, categoryId, locationId?, type, stipMin, stipMax, months, openings, empIdx, skillIds[])
            ("Software Engineering Intern", "Work on real-world .NET/Angular projects", 1, (int?)1, InternshipType.InOffice, 15000, 25000, 3, (short)3, 0, new[]{7,22,8}),
            ("Frontend Developer Intern",   "Build React/Angular UI for our platform",  1, (int?)1, InternshipType.Remote,   12000, 20000, 2, (short)5, 1, new[]{3,2,15}),
            ("Data Science Intern",         "Analyze user data using Python and ML",     8, (int?)1, InternshipType.Hybrid,   18000, 28000, 3, (short)2, 0, new[]{1,9,10}),
            ("ML Engineer Intern",          "Build and deploy ML models at scale",        8, (int?)4, InternshipType.Remote,   20000, 30000, 6, (short)2, 2, new[]{1,9,20}),
            ("Marketing Intern",            "Run digital marketing campaigns",            2, (int?)12,InternshipType.InOffice, 8000,  15000, 2, (short)4, 4, new[]{11,12,17}),
            ("Business Dev Intern",         "Identify and close new partnerships",        3, (int?)3, InternshipType.InOffice, 10000, 18000, 3, (short)3, 3, new[]{17,18,10}),
            ("Finance Intern",              "Assist in financial modelling and analysis", 4, (int?)2, InternshipType.InOffice, 12000, 20000, 2, (short)2, 5, new[]{16,10,4}),
            ("UI/UX Design Intern",         "Create wireframes and prototypes in Figma",  6, (int?)1, InternshipType.Remote,   10000, 18000, 3, (short)3, 6, new[]{15,14,19}),
            ("Content Writing Intern",      "Write blogs, social posts, and newsletters", 7, (int?)3, InternshipType.Remote,   6000,  10000, 2, (short)5, 7, new[]{13,17,12}),
            ("HR Intern",                   "Support recruitment and onboarding",          5, (int?)1, InternshipType.InOffice, 8000,  12000, 2, (short)4, 8, new[]{17,18,16}),
            ("Backend Developer Intern",    "Build REST APIs with Node.js and MongoDB",   1, (int?)6, InternshipType.Remote,   12000, 20000, 3, (short)3, 9, new[]{5,8,22}),
            ("Android Developer Intern",    "Develop Android apps with Flutter",          1, (int?)1, InternshipType.Hybrid,   15000, 22000, 4, (short)2, 10, new[]{23,22,1}),
            ("Product Management Intern",   "Work with product teams on roadmaps",        3, (int?)12,InternshipType.InOffice, 15000, 25000, 3, (short)2, 11, new[]{17,18,10}),
            ("Social Media Intern",         "Manage Instagram, Twitter, LinkedIn pages",  14,(int?)2, InternshipType.Remote,   5000,  10000, 2, (short)5, 12, new[]{11,19,17}),
            ("Operations Intern",           "Streamline processes and vendor management", 9, (int?)9, InternshipType.InOffice, 8000,  14000, 3, (short)3, 13, new[]{16,17,18}),
            ("Research Intern",             "Conduct market research and surveys",        15,(int?)null,InternshipType.Remote, 8000,  12000, 3, (short)4, 14, new[]{10,17,16}),
            ("Full Stack Developer Intern",  "Work on Angular + .NET full-stack app",     1, (int?)1, InternshipType.Hybrid,   18000, 28000, 6, (short)3, 15, new[]{4,7,8}),
            ("DevOps Intern",               "Manage CI/CD pipelines and cloud infra",     1, (int?)1, InternshipType.InOffice, 15000, 25000, 3, (short)2, 16, new[]{20,21,22}),
            ("Data Analyst Intern",         "Create dashboards and reports using PowerBI",8, (int?)2, InternshipType.Hybrid,   12000, 18000, 3, (short)3, 17, new[]{25,10,16}),
            ("Sales Intern",                "Generate leads and handle client calls",     10,(int?)3, InternshipType.InOffice, 7000,  12000, 2, (short)6, 18, new[]{17,10,18}),
            ("Cloud Engineer Intern",       "Work with AWS services and serverless",       1, (int?)1, InternshipType.Remote,   18000, 25000, 4, (short)2, 19, new[]{20,21,22}),
            ("Graphic Design Intern",       "Design marketing collaterals and UI assets",  6, (int?)6, InternshipType.Remote,   8000,  14000, 2, (short)4, 4, new[]{14,15,19}),
            ("Financial Analyst Intern",    "Model P&L statements and investment cases",  4, (int?)1, InternshipType.InOffice, 15000, 22000, 3, (short)2, 5, new[]{16,10,25}),
            ("Embedded Systems Intern",     "Program microcontrollers and IoT devices",   1, (int?)5, InternshipType.InOffice, 12000, 18000, 3, (short)2, 2, new[]{6,22,1}),
            ("Legal Intern",                "Draft contracts and research case law",       11,(int?)3, InternshipType.InOffice, 10000, 15000, 3, (short)2, 3, new[]{17,16,18}),
            ("SEO Intern",                  "Improve organic search rankings",             2, (int?)null,InternshipType.Remote,6000,  10000, 2, (short)5, 12, new[]{12,11,13}),
            ("Mobile App Intern",           "Build cross-platform apps with React Native", 1, (int?)4, InternshipType.Remote,  14000, 22000, 3, (short)3, 9, new[]{24,3,22}),
            ("Supply Chain Intern",         "Manage logistics and inventory tracking",    18,(int?)8, InternshipType.InOffice, 9000,  14000, 3, (short)3, 7, new[]{16,17,18}),
            ("Event Management Intern",     "Plan and execute corporate events",          17,(int?)2, InternshipType.InOffice, 7000,  12000, 2, (short)4, 4, new[]{17,18,19}),
            ("Public Relations Intern",     "Draft press releases and media pitches",     7, (int?)3, InternshipType.Hybrid,   8000,  14000, 2, (short)3, 6, new[]{13,17,12}),
        };

        var internships = new List<Internship>();
        foreach (var d in internshipData)
        {
            var (title, desc, catId, locId, type, stipMin, stipMax, months, openings, empIdx, _) = d;
            internships.Add(new Internship
            {
                EmployerId = employers[empIdx].Id,
                CategoryId = catId,
                LocationId = locId,
                Title = title,
                Description = $"{desc}. You'll work closely with our engineering and product teams to deliver real impact. " +
                              "This is a paid internship with potential for pre-placement offer.",
                Responsibilities = "- Collaborate with senior engineers\n- Write clean, tested code\n- Participate in sprint planning\n- Present work in weekly demos",
                Requirements = $"- Pursuing {(catId == 1 ? "B.Tech/MCA/BCA" : catId == 8 ? "B.Tech/M.Sc Statistics" : "relevant degree")}\n- Strong fundamentals\n- Proactive attitude",
                InternshipType = type,
                StipendMin = stipMin,
                StipendMax = stipMax,
                IsPaid = true,
                DurationMonths = (byte)months,
                StartDateType = "Immediate",
                OpeningsCount = openings,
                ApplicationDeadline = DateOnly.FromDateTime(now.AddDays(20 + (empIdx * 3))),
                Status = InternshipStatus.Active,
                IsActive = true,
                ApplicationsCount = 0,
                ViewsCount = 20 + empIdx * 7,
                PublishedAt = D(30 - empIdx),
                CreatedAt = D(30 - empIdx),
                UpdatedAt = D(5)
            });
        }
        db.Internships.AddRange(internships);
        await db.SaveChangesAsync();

        // ── 5. INTERNSHIP SKILLS ─────────────────────────────────────────────
        for (int i = 0; i < internshipData.Length; i++)
        {
            var skillIds = internshipData[i].Item11;
            foreach (var sid in skillIds)
            {
                var skill = await db.Skills.FindAsync(sid);
                if (skill != null)
                    internships[i].Skills.Add(skill);
            }
        }
        await db.SaveChangesAsync();

        // ── 6. JOBS ───────────────────────────────────────────────────────────
        var jobData = new[]
        {
            ("Software Engineer",           1,  (int?)1,  InternshipType.InOffice, 600000,  1000000, 0, new[]{7,4,22}),
            ("Senior React Developer",       1,  (int?)1,  InternshipType.Remote,   1200000, 2000000, 3, new[]{3,2,5}),
            ("Data Scientist",               8,  (int?)1,  InternshipType.Hybrid,   1000000, 1800000, 2, new[]{1,9,10}),
            ("Product Manager",              3,  (int?)12, InternshipType.InOffice, 1500000, 2500000, 4, new[]{17,18,10}),
            ("Digital Marketing Manager",    2,  (int?)3,  InternshipType.InOffice, 600000,  1000000, 2, new[]{11,12,17}),
            ("Financial Analyst",            4,  (int?)2,  InternshipType.InOffice, 700000,  1200000, 1, new[]{16,10,25}),
            ("UI/UX Designer",               6,  (int?)1,  InternshipType.Remote,   800000,  1400000, 2, new[]{15,14,19}),
            ("Backend Engineer",             1,  (int?)6,  InternshipType.Hybrid,   900000,  1500000, 2, new[]{5,8,22}),
            ("DevOps Engineer",              1,  (int?)1,  InternshipType.Remote,   1000000, 1800000, 3, new[]{20,21,22}),
            ("Business Analyst",             3,  (int?)11, InternshipType.InOffice, 700000,  1100000, 1, new[]{10,16,17}),
            ("Content Strategist",           7,  (int?)3,  InternshipType.Remote,   500000,   900000, 2, new[]{13,17,12}),
            ("HR Manager",                   5,  (int?)1,  InternshipType.InOffice, 600000,  1000000, 4, new[]{17,18,16}),
            ("Android Developer",            1,  (int?)4,  InternshipType.InOffice, 900000,  1500000, 2, new[]{23,22,6}),
            ("Cloud Architect",              1,  (int?)1,  InternshipType.Remote,   1800000, 3000000, 6, new[]{20,21,8}),
            ("Growth Hacker",                2,  (int?)1,  InternshipType.Hybrid,   700000,  1200000, 2, new[]{11,12,10}),
            ("Operations Manager",           9,  (int?)9,  InternshipType.InOffice, 700000,  1100000, 4, new[]{16,17,18}),
            ("Full Stack Developer",         1,  (int?)1,  InternshipType.InOffice, 1000000, 1600000, 2, new[]{4,7,3}),
            ("Machine Learning Engineer",    8,  (int?)1,  InternshipType.Remote,   1400000, 2400000, 3, new[]{1,9,20}),
            ("Sales Manager",               10,  (int?)2,  InternshipType.InOffice, 700000,  1300000, 3, new[]{17,10,18}),
            ("Legal Counsel",               11,  (int?)3,  InternshipType.InOffice, 1200000, 2000000, 5, new[]{17,16,18}),
        };

        var jobs = new List<Job>();
        for (int i = 0; i < jobData.Length; i++)
        {
            var (title, catId, locId, type, salMin, salMax, expYrs, _) = jobData[i];
            var empIdx = i % employers.Count;
            jobs.Add(new Job
            {
                EmployerId = employers[empIdx].Id,
                CategoryId = catId,
                LocationId = locId,
                Title = title,
                Description = $"We are looking for a talented {title} to join our team. You will work on challenging problems and ship products used by millions.",
                Requirements = $"- {expYrs}+ years of relevant experience\n- Strong problem-solving skills\n- Experience with required tech stack\n- Good communication skills",
                JobType = type,
                SalaryMin = salMin,
                SalaryMax = salMax,
                ExperienceYearsMin = expYrs,
                ApplicationDeadline = DateOnly.FromDateTime(now.AddDays(30 + i * 2)),
                Status = InternshipStatus.Active,
                IsActive = true,
                ApplicationsCount = 0,
                ViewsCount = 30 + i * 5,
                PublishedAt = D(25 - i),
                CreatedAt = D(25 - i),
                UpdatedAt = D(3)
            });
        }
        db.Jobs.AddRange(jobs);
        await db.SaveChangesAsync();

        // ── 7. JOB SKILLS ────────────────────────────────────────────────────
        for (int i = 0; i < jobData.Length; i++)
        {
            foreach (var sid in jobData[i].Item8)
            {
                var skill = await db.Skills.FindAsync(sid);
                if (skill != null)
                    jobs[i].Skills.Add(skill);
            }
        }
        await db.SaveChangesAsync();

        // ── 8. EDUCATIONS ────────────────────────────────────────────────────
        var degrees = new[] { "Bachelor of Technology", "Bachelor of Science", "Bachelor of Commerce", "Bachelor of Arts", "Master of Technology" };
        var fields  = new[] { "Computer Science", "Information Technology", "Electronics", "Business Administration", "Statistics", "Mathematics" };

        var educations = new List<Education>();
        for (int i = 0; i < students.Count; i++)
        {
            educations.Add(new Education
            {
                StudentId = students[i].Id,
                Institution = institutions[i % institutions.Length],
                Degree = degrees[i % degrees.Length],
                FieldOfStudy = fields[i % fields.Length],
                StartYear = (short)(2020 + (i % 3)),
                EndYear = i % 5 == 0 ? null : (short)(2024 + (i % 3)),
                IsCurrent = i % 5 == 0,
                Grade = Math.Round(7.0m + (i % 4) * 0.5m, 1),
                CreatedAt = D(50),
                UpdatedAt = D(10)
            });
        }
        db.Educations.AddRange(educations);
        await db.SaveChangesAsync();

        // ── 9. EXPERIENCES ───────────────────────────────────────────────────
        var jobTitles = new[] { "Software Intern", "Research Assistant", "Teaching Assistant", "Marketing Intern", "Data Analyst Intern" };
        var expCompanies = new[] { "Google", "Amazon", "Microsoft", "Flipkart", "Infosys", "TCS", "StartupXYZ" };

        var experiences = new List<Experience>();
        for (int i = 0; i < 20; i++)
        {
            experiences.Add(new Experience
            {
                StudentId = students[i].Id,
                Title = jobTitles[i % jobTitles.Length],
                Company = expCompanies[i % expCompanies.Length],
                Description = $"Worked on {(i % 2 == 0 ? "software development" : "data analysis")} projects. Gained hands-on experience with modern tech stacks.",
                StartDate = DateOnly.FromDateTime(DateTime.Today.AddMonths(-8 - i)),
                EndDate = i % 4 == 0 ? null : DateOnly.FromDateTime(DateTime.Today.AddMonths(-2 - (i % 3))),
                IsCurrent = i % 4 == 0,
                CreatedAt = D(40),
                UpdatedAt = D(8)
            });
        }
        db.Experiences.AddRange(experiences);
        await db.SaveChangesAsync();

        // ── 10. RESUMES ──────────────────────────────────────────────────────
        var resumes = new List<Resume>();
        for (int i = 0; i < 20; i++)
        {
            resumes.Add(new Resume
            {
                StudentId = students[i].Id,
                FileUrl = $"https://cdn.internshala-clone.dev/resumes/{students[i].Id}/resume.pdf",
                FileName = $"{studentUsers[i].FirstName}_{studentUsers[i].LastName}_Resume.pdf",
                FileSizeBytes = 80000 + i * 5000,
                UploadedAt = D(15 + i),
                CreatedAt = D(15 + i),
                UpdatedAt = D(15 + i)
            });
        }
        db.Resumes.AddRange(resumes);
        await db.SaveChangesAsync();

        // ── 11. APPLICATIONS ─────────────────────────────────────────────────
        var appStatuses = new[] {
            ApplicationStatus.Applied, ApplicationStatus.Applied, ApplicationStatus.UnderReview,
            ApplicationStatus.Shortlisted, ApplicationStatus.Applied, ApplicationStatus.Rejected,
            ApplicationStatus.Selected, ApplicationStatus.Applied, ApplicationStatus.UnderReview,
            ApplicationStatus.Applied
        };

        var coverLetters = new[] {
            "I am deeply interested in this role. My experience with the tech stack makes me a strong candidate.",
            "This internship aligns perfectly with my career goals. I am eager to contribute and learn.",
            "I have been following your company's growth and would love to be a part of your journey.",
            "My academic projects closely mirror the work described here. I am confident I can add value.",
            "I am a quick learner with a passion for building scalable systems. Excited to apply!"
        };

        var applications = new List<Internshala.Domain.Entities.Application>();
        for (int i = 0; i < 35; i++)
        {
            var studIdx = i % students.Count;
            var isJob = i % 5 == 0 && i / 5 < jobs.Count;
            var status = appStatuses[i % appStatuses.Length];
            applications.Add(new Internshala.Domain.Entities.Application
            {
                StudentId = students[studIdx].Id,
                ListingType = isJob ? "Job" : "Internship",
                InternshipId = isJob ? null : internships[i % internships.Count].Id,
                JobId = isJob ? jobs[i / 5 % jobs.Count].Id : null,
                Status = status,
                CoverLetter = coverLetters[i % coverLetters.Length],
                ResumeUrl = $"https://cdn.internshala-clone.dev/resumes/{students[studIdx].Id}/resume.pdf",
                AvailabilityDate = DateOnly.FromDateTime(now.AddDays(7 + i)),
                ExpectedStipend = isJob ? null : (int?)(8000 + i * 500),
                EmployerNote = status == ApplicationStatus.Shortlisted ? "Good profile, scheduled for interview." : null,
                RejectionReason = status == ApplicationStatus.Rejected ? "Position filled with a more experienced candidate." : null,
                WithdrawnAt = status == ApplicationStatus.Withdrawn ? D(2) : null,
                CreatedAt = D(20 - (i % 20)),
                UpdatedAt = D(5 - (i % 5))
            });
        }
        db.Applications.AddRange(applications);
        await db.SaveChangesAsync();

        // Update ApplicationsCount on internships/jobs
        foreach (var app in applications)
        {
            if (app.InternshipId.HasValue)
            {
                var intern = internships.FirstOrDefault(x => x.Id == app.InternshipId);
                if (intern != null) intern.ApplicationsCount++;
            }
            else if (app.JobId.HasValue)
            {
                var job = jobs.FirstOrDefault(x => x.Id == app.JobId);
                if (job != null) job.ApplicationsCount++;
            }
        }
        await db.SaveChangesAsync();

        // ── 12. APPLICATION STATUS HISTORIES ─────────────────────────────────
        var histories = new List<ApplicationStatusHistory>();
        for (int i = 0; i < applications.Count; i++)
        {
            var app = applications[i];
            if (app.Status != ApplicationStatus.Applied)
            {
                histories.Add(new ApplicationStatusHistory
                {
                    ApplicationId = app.Id,
                    FromStatus = ApplicationStatus.Applied,
                    ToStatus = app.Status,
                    Comment = app.Status == ApplicationStatus.Shortlisted ? "Moved after resume review." :
                              app.Status == ApplicationStatus.Selected ? "Congratulations! Offer letter will follow." :
                              app.Status == ApplicationStatus.Rejected ? "Thank you for your interest." : null,
                    ChangedBy = employers[i % employers.Count].UserId,
                    CreatedAt = D(10 - (i % 10)),
                    UpdatedAt = D(10 - (i % 10))
                });
            }
        }
        db.ApplicationStatusHistories.AddRange(histories);
        await db.SaveChangesAsync();

        // ── 13. COURSES ──────────────────────────────────────────────────────
        var courseData = new[]
        {
            ("Python for Data Science",         "Master Python for data analysis and ML",          1,  CourseLevel.Beginner,     true,  0m,    40, "Rohan Gupta",    "English"),
            ("Full Stack Web Development",      "Build modern web apps with MERN stack",            1,  CourseLevel.Intermediate, true,  0m,    60, "Anjali Sharma",  "English"),
            ("Machine Learning A-Z",            "Complete ML with Python and R",                    8,  CourseLevel.Advanced,     false, 1999m, 80, "Rahul Singh",    "English"),
            ("Digital Marketing Mastery",       "SEO, SEM, Social Media and Analytics",             2,  CourseLevel.Beginner,     true,  0m,    30, "Priya Kapoor",   "English"),
            ("UI/UX Design Fundamentals",       "Design principles using Figma and Adobe XD",       6,  CourseLevel.Beginner,     true,  0m,    25, "Sneha Iyer",     "English"),
            ("Financial Modelling & Valuation", "DCF, LBO, Comparable Company Analysis",            4,  CourseLevel.Intermediate, false, 2499m, 50, "Vikram Malhotra","English"),
            ("React.js Advanced",               "Hooks, Context API, Redux, and Testing",           1,  CourseLevel.Advanced,     false, 1499m, 35, "Karan Verma",    "English"),
            ("Business Communication",          "Professional writing and presentation skills",      3,  CourseLevel.Beginner,     true,  0m,    20, "Meera Pillai",   "English"),
            ("AWS Cloud Practitioner",          "Prepare for AWS Cloud Practitioner exam",          1,  CourseLevel.Intermediate, false, 1999m, 45, "Deepak Tiwari",  "English"),
            ("SQL & Database Management",       "Relational DB design and complex queries",         8,  CourseLevel.Beginner,     true,  0m,    30, "Ananya Roy",     "English"),
            ("Product Management 101",          "From ideation to launch: the PM lifecycle",        3,  CourseLevel.Intermediate, true,  0m,    35, "Nikhil Mehta",   "English"),
            ("Java Spring Boot",                "Build production-grade REST APIs with Spring",     1,  CourseLevel.Intermediate, false, 1299m, 40, "Suresh Kumar",   "English"),
            ("Excel for Business Analytics",    "Pivot tables, VLOOKUP, Power Query mastery",      4,  CourseLevel.Beginner,     true,  0m,    25, "Ritu Agarwal",   "English"),
            ("Graphic Design with Canva",       "Create stunning visuals for social media",         6,  CourseLevel.Beginner,     true,  0m,    15, "Kavya Desai",    "English"),
            ("Angular & TypeScript Mastery",    "Build enterprise Angular 18 applications",         1,  CourseLevel.Advanced,     false, 1799m, 50, "Arjun Sharma",   "English"),
        };

        var courseEntities = new List<Course>();
        for (int i = 0; i < courseData.Length; i++)
        {
            var (title, desc, catId, level, isFree, price, hours, instructor, lang) = courseData[i];
            courseEntities.Add(new Course
            {
                Title = title,
                Description = $"{desc}. This course is designed for {(level == CourseLevel.Beginner ? "complete beginners" : level == CourseLevel.Intermediate ? "students with basic knowledge" : "experienced professionals")}. " +
                              "You will get hands-on projects, quizzes, and a certificate upon completion.",
                Instructor = instructor,
                CategoryId = catId,
                Level = level,
                DurationHours = hours,
                IsFree = isFree,
                Price = price,
                IsPublished = true,
                EnrolledCount = 50 + i * 30,
                Language = lang,
                Prerequisites = level == CourseLevel.Beginner ? "No prerequisites required" :
                                level == CourseLevel.Intermediate ? "Basic programming knowledge" : "2+ years experience",
                WhatYouLearn = $"• Core concepts of {title}\n• Hands-on projects\n• Industry best practices\n• Certificate of completion",
                ThumbnailUrl = $"https://cdn.internshala-clone.dev/courses/thumb_{i + 1}.jpg",
                CreatedAt = D(45 - i),
                UpdatedAt = D(5)
            });
        }
        db.Courses.AddRange(courseEntities);
        await db.SaveChangesAsync();

        // ── 14. COURSE MODULES ───────────────────────────────────────────────
        var modules = new List<CourseModule>();
        var moduleTemplates = new[] {
            ("Introduction & Setup",          "Get your environment ready and understand the course structure.", 30, true),
            ("Core Concepts",                 "Deep dive into fundamental concepts with practical examples.",   45, false),
            ("Hands-on Project",              "Build a real-world project applying everything learned so far.", 60, false),
        };

        for (int ci = 0; ci < courseEntities.Count; ci++)
        {
            for (int mi = 0; mi < moduleTemplates.Length; mi++)
            {
                var (mTitle, mDesc, mDuration, isPreview) = moduleTemplates[mi];
                modules.Add(new CourseModule
                {
                    CourseId = courseEntities[ci].Id,
                    Title = $"Module {mi + 1}: {mTitle}",
                    Description = mDesc,
                    VideoUrl = isPreview ? $"https://cdn.internshala-clone.dev/courses/{ci + 1}/module_{mi + 1}.mp4" : null,
                    Content = isPreview ? null : "Full content available after enrollment.",
                    OrderIndex = mi + 1,
                    DurationMinutes = mDuration,
                    IsPreview = isPreview,
                    CreatedAt = D(44 - ci),
                    UpdatedAt = D(5)
                });
            }
        }
        db.CourseModules.AddRange(modules);
        await db.SaveChangesAsync();

        // ── 15. ENROLLMENTS ──────────────────────────────────────────────────
        var enrollments = new List<Enrollment>();
        for (int i = 0; i < 25; i++)
        {
            var studIdx = i % students.Count;
            var courseIdx = i % courseEntities.Count;
            var progress = (byte)(i % 3 == 0 ? 100 : i % 3 == 1 ? 60 : 30);
            enrollments.Add(new Enrollment
            {
                StudentId = students[studIdx].Id,
                CourseId = courseEntities[courseIdx].Id,
                ProgressPercent = progress,
                CompletedModules = progress == 100 ? 3 : progress == 60 ? 2 : 1,
                CompletedAt = progress == 100 ? D(5) : null,
                LastAccessedAt = D(1 + i % 7),
                CreatedAt = D(30 - i),
                UpdatedAt = D(1 + i % 7)
            });
        }
        db.Enrollments.AddRange(enrollments);
        await db.SaveChangesAsync();

        // ── 16. NOTIFICATIONS ────────────────────────────────────────────────
        var notifications = new List<Notification>();
        for (int i = 0; i < applications.Count && i < 25; i++)
        {
            var app = applications[i];
            if (app.Status != ApplicationStatus.Applied)
            {
                var listing = app.InternshipId.HasValue
                    ? internships.First(x => x.Id == app.InternshipId).Title
                    : jobs.First(x => x.Id == app.JobId).Title;

                notifications.Add(new Notification
                {
                    UserId = students[i % students.Count].UserId,
                    Type = "ApplicationStatusUpdate",
                    Title = $"Application {app.Status}",
                    Message = $"Your application for '{listing}' has been updated to {app.Status}.",
                    ActionUrl = $"/applications/{app.Id}",
                    IsRead = i % 3 != 0,
                    ReadAt = i % 3 != 0 ? D(2) : null,
                    CreatedAt = D(8 - (i % 8)),
                    UpdatedAt = D(8 - (i % 8))
                });
            }
        }
        // Add a few welcome notifications
        for (int i = 0; i < 5; i++)
        {
            notifications.Add(new Notification
            {
                UserId = studentUsers[i].Id,
                Type = "Welcome",
                Title = "Welcome to Internshala!",
                Message = "Complete your profile to get noticed by top employers.",
                ActionUrl = "/profile",
                IsRead = false,
                CreatedAt = D(30 - i),
                UpdatedAt = D(30 - i)
            });
        }
        db.Notifications.AddRange(notifications);
        await db.SaveChangesAsync();

        logger.LogInformation(
            "Sample data seeded: {Users} users, {Students} students, {Employers} employers, " +
            "{Internships} internships, {Jobs} jobs, {Applications} applications, " +
            "{Courses} courses, {Modules} modules, {Enrollments} enrollments.",
            allUsers.Count, students.Count, employers.Count,
            internships.Count, jobs.Count, applications.Count,
            courseEntities.Count, modules.Count, enrollments.Count);
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static User U(string email, string first, string last, UserRole role)
    {
        return new User
        {
            Email = email,
            PasswordHash = _passwordHash,
            FirstName = first,
            LastName = last,
            Role = role,
            IsEmailVerified = true,
            IsActive = true,
            EmailVerifiedAt = DateTime.UtcNow.AddDays(-60),
            CreatedAt = DateTime.UtcNow.AddDays(-60),
            UpdatedAt = DateTime.UtcNow.AddDays(-5)
        };
    }
}
