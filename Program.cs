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