using Marcplaats_Backend.BLL.Models.Entities;
using Marcplaats_Backend.DAL.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Mock
{
    public static class AdvertisementDBMock
    {
        public static void SeedDbOneAd(DataContext context)
        {
            context.Advertisements.Add(new Advertisement()
            {
                ID = Guid.Parse("8194ba5e-a5a1-47ae-8960-8022b29fac79"),
                Title = "TestTitle",
                Description = "TestDescription",
                Price = 10,
                Bidding = false
            });
            context.SaveChanges();
        }
    }
}
