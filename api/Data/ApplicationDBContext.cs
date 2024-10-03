using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : DbContext //კლასი ApplicationDBContext – ეს არის კლასის განსაზღვრა, რომელიც მემკვიდრეობით მიიღებს ფუნქციონალს DbContext-ისგან. DbContext არის Entity Framework-ის ძირითადი კლასი, რომელიც გამოიყენება მონაცემთა ბაზასთან ურთიერთობისთვის.


    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)//კონსტრუქტორი ApplicationDBContext(DbContextOptions dbContextOptions) – ეს კონსტრუქტორი პასუხისმგებელია ApplicationDBContext კლასის ობიექტის შექმნაზე. მასში გადაეცემა პარამეტრი DbContextOptions, რომელიც განსაზღვრავს, თუ როგორ უნდა კავშირდეს პროგრამა მონაცემთა ბაზასთან (მაგალითად, რომელ მონაცემთა ბაზას გამოიყენებს, რა იქნება მისი კავშირი და სხვა კონფიგურაციები). ეს პარამეტრი შემდეგ გადაეცემა მშობელ DbContext კლასს კონსტრუქტორის მეშვეობით : base(dbContextOptions).
        {
            
        }
        public DbSet<Stock> Stock { get; set;} //მონაცემთა ბაზის ცხრილი დაკავშირებულია Stock კლასთან. ანუ ეს არის მონაცემთა ბაზის Stock ცხრილი. ეს საშუალებას იძლევა, მონაცემთა ბაზის ოპერაციები (CRUD) იმართოს კოდით და არა SQL ბრძანებების საშუალებით.
        public DbSet<Comment> Comments { get; set;}
    }
}