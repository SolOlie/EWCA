using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using DAL.Migrations;
using Entities;
using Entities.Entities;
using TrackerEnabledDbContext;
using TrackerEnabledDbContext.Common.Configuration;
using TrackerEnabledDbContext.Common.Extensions;
using TrackerEnabledDbContext.Common.Models;
using TrackerEnabledDbContext.Identity;

namespace DAL.DB
{
    public class CADBContext :TrackerContext
    {
        public CADBContext() : base("CustomerAccountingDB")
        {
            Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CADBContext, Configuration>());
            GlobalTrackingConfig.DisconnectedContext = true;
            GlobalTrackingConfig.SetSoftDeletableCriteria<ISoftDelete>(entity => entity.SoftDelete);

        }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Changelog> Changelogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AssetType> AssetTypes { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Switch> Switches { get; set; }
        public DbSet<Port> Ports { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Changelog>().Property(e => e.ChangedDate).HasColumnType("datetime2");
            modelBuilder.Entity<Asset>().Property(e => e.InstallationDate).HasColumnType("datetime2");
            modelBuilder.Entity<Customer>().Property(e => e.Date).HasColumnType("datetime2");

            modelBuilder.Entity<Changelog>().HasRequired(c => c.User).WithMany(e => e.Changelogs);
            modelBuilder.Entity<Customer>().HasMany(x => x.ContactPersons).WithOptional(x => x.IsContactForCustomer);
            modelBuilder.Entity<Asset>().HasRequired(a => a.Customer).WithMany(c => c.Assets).WillCascadeOnDelete(true);
            modelBuilder.Entity<Asset>().HasMany(a => a.Changelogs).WithRequired(a => a.Asset);
            modelBuilder.Entity<Asset>().HasOptional(a => a.Type).WithMany(a => a.Assets);
            modelBuilder.Entity<Asset>().HasMany(x => x.FileAttachments).WithRequired(x => x.Asset);
            modelBuilder.Entity<File>().HasOptional(x => x.ContentFile).WithRequired(x => x.File);
            modelBuilder.Entity<Asset>().HasOptional(s => s.Port).WithOptionalDependent(p => p.Asset);
            modelBuilder.Entity<Switch>().HasMany(p => p.Ports).WithRequired(s => s.Switch);
            modelBuilder.Entity<Customer>().HasMany(s => s.Switches).WithRequired(s => s.Customer);
            modelBuilder.Entity<Asset>().HasOptional(s => s.Switch).WithRequired(p => p.Asset);
            modelBuilder.Entity<Switch>()
                .Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
