using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Test.Models;
namespace Test.Migrations
{
        internal sealed class Configuration : DbMigrationsConfiguration<Test.Models.ApplicationDbContext>
        {
            public Configuration()
            {
                AutomaticMigrationsEnabled = true;
            }

            protected override void Seed(Test.Models.ApplicationDbContext context)
            {
                var manager = new UserManager<ApplicationUser>(
                                new UserStore<ApplicationUser>(
                                    new ApplicationDbContext()));

                for (int i = 0; i < 4; i++)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = string.Format("User{0}", i.ToString()),

                        // Add the following so our Seed data is complete:
                        FirstName = string.Format("FirstName{0}", i.ToString()),
                        LastName = string.Format("LastName{0}", i.ToString()),
                        Email = string.Format("Email{0}@Example.com", i.ToString()),
                    };
                    manager.Create(user, string.Format("Password{0}", i.ToString()));
                }
         }
        }
    

}
