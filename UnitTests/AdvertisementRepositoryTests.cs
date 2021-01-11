using Marcplaats_Backend.BLL.Models.Entities;
using Marcplaats_Backend.BLL.Repositories;
using Marcplaats_Backend.BLL.RepositoryInterfaces;
using Marcplaats_Backend.DAL.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Mock;
using Xunit;

namespace UnitTests
{
    public class AdvertisementRepositoryTests
    {
        private IAdvertisementRepository advertisementRepo;
        IConfiguration _configuration;

        public AdvertisementRepositoryTests()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"Auth:Token", "marcplaatsmarcplaatsmarcplaats"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
        }

        public static DbContextOptions<DataContext> CreateOptions()
        {
            //This creates the SQLite connection string to in-memory database
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            { DataSource = ":memory:" };
            var connectionString = connectionStringBuilder.ToString();

            //This creates a SqliteConnectionwith that string
            var connection = new SqliteConnection(connectionString);

            //The connection MUST be opened here
            connection.Open();

            //Now we have the EF Core commands to create SQLite options
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseSqlite(connection);

            return builder.Options;
        }

        [Fact]
        public void CreateGoodFlow()
        {
            //SETUP
            var options = CreateOptions();

            using (var context = new DataContext(options))
            {
                advertisementRepo = new AdvertisementRepository(context);

                context.Database.EnsureCreated();
                CategoryDBMock.SeedDbOneCategory(context);

                Advertisement newAd = new Advertisement()
                {
                    Title = "TestTitle",
                    Description = "TestDescription",
                    Price = 10,
                    Bidding = false
                };
                Guid categoryID = Guid.Parse("ee3a93a0-a382-47fc-b6f3-017fe453e250");

                //ATTEMPT
                Task<ServiceResponse<Guid>> service = advertisementRepo.Create(newAd, categoryID);
                ServiceResponse<Guid> response = service.Result;

                //VERIFY
                Assert.True(response.Success);
                Assert.Equal("Advertisement created succesfully.", response.Message);
            }
        }

        [Fact]
        public void CreateBadFlow()
        {
            //SETUP
            var options = CreateOptions();

            using (var context = new DataContext(options))
            {
                advertisementRepo = new AdvertisementRepository(context);

                context.Database.EnsureCreated();
                CategoryDBMock.SeedDbOneCategory(context);

                Advertisement newAd = new Advertisement()
                {
                    Title = "",
                    Description = "",
                    Price = 10,
                    Bidding = false
                };
                Guid categoryID = Guid.Parse("ee3a93a0-a382-47fc-b6f3-017fe453e250");

                //ATTEMPT
                Task<ServiceResponse<Guid>> service = advertisementRepo.Create(newAd, categoryID);
                ServiceResponse<Guid> response = service.Result;

                //VERIFY
                Assert.False(response.Success);
                Assert.Equal("Ad could not be placed.", response.Message);
            }
        }
    }
}
