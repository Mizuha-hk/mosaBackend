using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using mosaCupBackend.Models.DbModels;

namespace mosaCupBackend.Data
{
    public class mosaCupBackendContext : DbContext
    {
        public mosaCupBackendContext (DbContextOptions<mosaCupBackendContext> options)
            : base(options)
        {
        }

        public DbSet<mosaCupBackend.Models.DbModels.UserData> UserData { get; set; } = default!;

        public DbSet<mosaCupBackend.Models.DbModels.Follow> Follow { get; set; } = default!;

        public DbSet<mosaCupBackend.Models.DbModels.Post> Post { get; set; } = default!;

        public DbSet<mosaCupBackend.Models.DbModels.Like> Like { get; set; } = default!;

        public DbSet<mosaCupBackend.Models.DbModels.Notification> Notification { get; set; } = default!;
    }
}
