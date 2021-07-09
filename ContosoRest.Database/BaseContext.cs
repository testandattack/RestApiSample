using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using ContosoRest.Database.DbModels;


namespace ContosoRest.Database
{
    public class BaseContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public BaseContext(DbContextOptions<UserContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            this._loggerFactory = loggerFactory;
        }

        public BaseContext(DbContextOptions<AdminContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            this._loggerFactory = loggerFactory;
        }

        #region DbSets______________________________
        public virtual DbSet<Contoso> Contosos { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(this._loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contoso>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(128)
                    .IsRequired();
            });
        }
    }
}
