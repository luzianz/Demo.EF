using Microsoft.EntityFrameworkCore;
using System;

namespace Demo.EF.DataAccess {

	using Models;

	public class SampleDbContext : DbContext {

		public SampleDbContext() =>
			this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

		public SampleDbContext(DbContextOptions options) : base(options) =>
			this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

		public DbSet<Thing> Things { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlite("Data Source=data.db;");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) =>
			modelBuilder
				.Entity<Thing>()
				.HasKey(p => p.Id);
	}
}