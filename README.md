# create project
```sh
dotnet new console -o Demo.EF
cd Demo.EF
```

# Install Packages
```sh
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore
```

## Enable `dotnet ef` cli 
Add to a new `ItemGroup` in your `.csproj` file
```xml
<ItemGroup>
  <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.1" />
</ItemGroup>
```

## Restore
```sh
dotnet restore
```

# Create a sample model
```csharp
using System;
namespace Demo.EF.Models {
	public class Thing {
		public int Id { get; set; }
		public string Message { get; set; }
	}
}
```

# Create a sample `DbContext`
```csharp
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
```

# Create migration
Note: the name 'InitialCreate' is arbitrary
```sh
dotnet ef migrations add InitialCreate
```

# Create the database schema
```sh
dotnet ef database update
```

# Test
```csharp
using System;
using System.Collections.Generic;
namespace Demo.EF {
	using Models;
	using DataAccess;
	class Program {
		static void Main (string[] args) {
			Insert ();
			foreach (var thing in Query ()) {
				System.Console.WriteLine(thing.Message);
			}
		}
		static void Insert () {
			using (var dbContext = new SampleDbContext ()) {
				dbContext.Add (new Thing { Message = "sample" });
				dbContext.SaveChanges ();
			}
		}
		static IEnumerable<Thing> Query () {
			using (var dbContext = new SampleDbContext ()) {
				foreach (var thing in dbContext.Things) yield return thing;
			}
		}
	}
}
```