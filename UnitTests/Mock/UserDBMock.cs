using Marcplaats_Backend.BLL.Models.Entities;
using Marcplaats_Backend.DAL.Context;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace UnitTests.Mock
{
    public static class UserDBMock
    {
        public static void AddDefaultRoles(DataContext context)
        {
            context.Roles.Add(new Role()
            {
                Name = "User"
            });
            context.Roles.Add(new Role()
            {
                Name = "Seller"
            });
            context.SaveChanges();
        }

        public static void SeedDbOneUser(DataContext context)
        {
            CreatePasswordHash("Tester123", out byte[] passwordHash, out byte[] passwordSalt);
            context.Users.Add(new User()
            {
                ID = Guid.Parse("4861d24e-0af5-4e05-b9ca-0d77f8397ec0"),
                Username = "Test",
                Email = "Test@test.com",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            });
            context.SaveChanges();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //Create password hash with salt
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
