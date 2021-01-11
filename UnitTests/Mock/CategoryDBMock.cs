using Marcplaats_Backend.BLL.Models.Entities;
using Marcplaats_Backend.DAL.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Mock
{
    public static class CategoryDBMock
    {
        public static void SeedDbOneCategory(DataContext context)
        {
            context.Categories.Add(new Category()
            {
                ID = Guid.Parse("ee3a93a0-a382-47fc-b6f3-017fe453e250"),
                Name = "Vacation"
            });
            context.SaveChanges();
        }
    }
}
