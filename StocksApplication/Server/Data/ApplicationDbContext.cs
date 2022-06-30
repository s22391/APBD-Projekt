using StocksApplication.Server.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksApplication.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<DailyOhlc> DailyOhlcs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Company>(builder =>
            {
                builder.HasKey(e => e.Id).HasName("Company_pk");
                builder.Property(e => e.Id).UseIdentityColumn();

                builder.Property(e => e.Ticker).IsRequired();
                builder.Property(e => e.Name).IsRequired();
                builder.Property(e => e.HasData).IsRequired();

                builder.HasMany(e => e.Users).WithMany(e => e.Companies);

                builder.ToTable("Company");
            });

            modelBuilder.Entity<DailyOhlc>(builder =>
            {
                builder.HasKey(e => e.Id);
                builder.Property(e => e.Id).ValueGeneratedNever();

                builder.Property(e => e.Symbol).IsRequired();
                builder.Property(e => e.Symbol).IsRequired();
                builder.Property(e => e.Open).IsRequired();
                builder.Property(e => e.High).IsRequired();
                builder.Property(e => e.Low).IsRequired();
                builder.Property(e => e.Close).IsRequired();
                builder.Property(e => e.Volume).IsRequired();
                builder.Property(e => e.AfterHours).IsRequired();
                builder.Property(e => e.PreMarket).IsRequired();

                builder.HasOne(e => e.Company).WithOne(e => e.DailyOhlc).HasForeignKey<DailyOhlc>(e => e.Id);

                builder.ToTable("DailyOhlc");
            });
        }
    }
}
