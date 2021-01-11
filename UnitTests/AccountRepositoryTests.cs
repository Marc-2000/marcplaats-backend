using Marcplaats_Backend.BLL.Models.Entities;
using Marcplaats_Backend.BLL.Repositories;
using Marcplaats_Backend.BLL.RepositoryInterfaces;
using Marcplaats_Backend.DAL.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTests.Mock;
using Xunit;

namespace UnitTests
{
    public class AccountRepositoryTests
    {
        private IAccountRepository accountRepo;
        IConfiguration _configuration;
        public AccountRepositoryTests()
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
        public void RegisterGoodFlow()
        {
            //SETUP
            var options = CreateOptions();

            using (var context = new DataContext(options))
            {
                accountRepo = new AccountRepository(context, _configuration);

                context.Database.EnsureCreated();
                UserDBMock.AddDefaultRoles(context);

                User newUser = new User()
                {
                    Username = "Test",
                    Email = "Test@test.com",
                };
                string password = "Test123";

                //ATTEMPT
                Task<ServiceResponse<Guid>> service = accountRepo.Register(newUser, password);
                ServiceResponse<Guid> response = service.Result;

                //VERIFY
                Assert.True(response.Success);
                Assert.Equal("User registered succesfully.", response.Message);
            }
        }

        [Fact]
        public void RegisterWithExistingEmail()
        {
            //SETUP
            var options = CreateOptions();

            using (var context = new DataContext(options))
            {
                accountRepo = new AccountRepository(context, _configuration);

                context.Database.EnsureCreated();
                UserDBMock.AddDefaultRoles(context);
                UserDBMock.SeedDbOneUser(context);

                User newUser = new User()
                {
                    Username = "Tester",
                    Email = "Test@test.com",
                };
                string password = "Test123";

                //ATTEMPT
                Task<ServiceResponse<Guid>> service = accountRepo.Register(newUser, password);
                ServiceResponse<Guid> response = service.Result;

                //VERIFY
                Assert.False(response.Success);
                Assert.Equal("Email already taken.", response.Message);
            }
        }

        [Fact]
        public void LoginGoodFlow()
        {
            //SETUP
            var options = CreateOptions();

            using (var context = new DataContext(options))
            {
                accountRepo = new AccountRepository(context, _configuration);

                context.Database.EnsureCreated();
                UserDBMock.SeedDbOneUser(context);


                string username = "Test";
                string password = "Tester123";

                //ATTEMPT
                Task<ServiceResponse<User>> service = accountRepo.Login(username, password);
                ServiceResponse<User> response = service.Result;

                //VERIFY
                Assert.True(response.Success);
                Assert.Equal("Successfully logged in!", response.Message);
            }
        }

        [Fact]
        public void LoginWithWrongPassword()
        {
            //SETUP
            var options = CreateOptions();

            using (var context = new DataContext(options))
            {
                accountRepo = new AccountRepository(context, _configuration);

                context.Database.EnsureCreated();
                UserDBMock.SeedDbOneUser(context);

                string username = "Test";
                string password = "Tester1234";

                //ATTEMPT
                Task<ServiceResponse<User>> service = accountRepo.Login(username, password);
                ServiceResponse<User> response = service.Result;

                //VERIFY
                Assert.False(response.Success);
                Assert.Equal("Credentials are not valid!", response.Message);
            }
        }

        [Fact]
        public void LoginWithoutExistingUsername()
        {
            //SETUP
            var options = CreateOptions();

            using (var context = new DataContext(options))
            {
                accountRepo = new AccountRepository(context, _configuration);

                context.Database.EnsureCreated();
                UserDBMock.SeedDbOneUser(context);

                string username = "Username";
                string password = "Tester123";

                //ATTEMPT
                Task<ServiceResponse<User>> service = accountRepo.Login(username, password);
                ServiceResponse<User> response = service.Result;

                //VERIFY
                Assert.False(response.Success);
                Assert.Equal("Credentials are not valid!", response.Message);
            }
        }

        [Fact]
        public void GetUserGoodFlow()
        {
            //SETUP
            var options = CreateOptions();

            using (var context = new DataContext(options))
            {
                accountRepo = new AccountRepository(context, _configuration);

                context.Database.EnsureCreated();
                UserDBMock.SeedDbOneUser(context);
                User user = new User()
                {
                    ID = Guid.Parse("4861d24e-0af5-4e05-b9ca-0d77f8397ec0"),
                    Username = "Test",
                    Email = "Test@test.com"
                };

                Guid id = Guid.Parse("4861d24e-0af5-4e05-b9ca-0d77f8397ec0");

                //ATTEMPT
                Task<User> service = accountRepo.Get(id);
                User response = service.Result;

                //VERIFY
                Assert.NotNull(response);
            }
        }

        [Fact]
        public void GetUserBadFlow()
        {
            //SETUP
            var options = CreateOptions();

            using (var context = new DataContext(options))
            {
                accountRepo = new AccountRepository(context, _configuration);

                context.Database.EnsureCreated();

                Guid id = Guid.Parse("4861d24e-0af5-4e05-b9ca-0d77f8397ec0");

                //ATTEMPT
                Task<User> service = accountRepo.Get(id);
                User response = service.Result;

                //VERIFY
                Assert.Null(response);
            }
        }
    }
}
