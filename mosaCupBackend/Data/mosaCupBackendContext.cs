using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using mosaCupBackend;
using mosaCupBackend.Models.DbModels;

namespace mosaCupBackend.Data
{
    public class mosaCupBackendContext : DbContext
    {
        public mosaCupBackendContext (DbContextOptions<mosaCupBackendContext> options)
            : base(options)
        {
        }

        public DbSet<mosaCupBackend.Models.DbModels.userData> UserData { get; set; } = default!;

        public DbSet<mosaCupBackend.Models.DbModels.follow> Follow { get; set; } = default!;

        public DbSet<mosaCupBackend.Models.DbModels.post> Post { get; set; } = default!;

        public DbSet<mosaCupBackend.Models.DbModels.like> Like { get; set; } = default!;

        public DbSet<mosaCupBackend.Models.DbModels.notification> Notification { get; set; } = default!;
    }
}
