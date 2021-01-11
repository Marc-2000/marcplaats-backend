using Google.Apis.Auth;
using Marcplaats_Backend.BLL.DTOs;
using Marcplaats_Backend.BLL.Models.Entities;
using Marcplaats_Backend.BLL.RepositoryInterfaces;
using Marcplaats_Backend.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Marcplaats_Backend.BLL.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public AccountRepository(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<User>> GoogleLogin(GoogleJsonWebSignature.Payload payload)
        {
            ServiceResponse<User> response = new ServiceResponse<User>();
            User user = new User();
            if (await EmailExists(payload.Email))
            {
                user = await _context.Users.Include(pr => pr.UserRoles).ThenInclude(r => r.Role).FirstOrDefaultAsync(x => x.Email.ToLower().Equals(payload.Email.ToLower()));
            }
            else
            {
                user.Email = payload.Email;
                user.Username = payload.GivenName + payload.FamilyName;

                //Add default role to new user
                Role defaultRole = _context.Roles.FirstOrDefault(x => x.Name.Equals("User"));

                //Set join-table for entity framework
                UserRole newPersonRole = new UserRole
                {
                    UserID = user.ID,
                    User = user,
                    RoleID = defaultRole.ID,
                    Role = defaultRole
                };

                //Add user and user-role to database
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                await _context.UserRoles.AddAsync(newPersonRole);
                await _context.SaveChangesAsync();
                user = await _context.Users.Include(pr => pr.UserRoles).ThenInclude(r => r.Role).FirstOrDefaultAsync(x => x.Email.ToLower().Equals(payload.Email.ToLower()));
            }
            user.PasswordHash = null;
            user.PasswordSalt = null;
            //response.Data = user;
            response.Token = CreateToken(user);
            return response;
        }

        public async Task<ServiceResponse<User>> Login(string username, string password)
        {
            // Create new empty response
            ServiceResponse<User> response = new ServiceResponse<User>();

            //Retrieve user with e-mail from request
            User user = await _context.Users.Include(pr => pr.UserRoles).ThenInclude(r => r.Role).FirstOrDefaultAsync(x => x.Username.ToLower().Equals(username.ToLower()));

            if (user == null || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                //User didn't give the right credentials, so return error
                response.Success = false;
                response.Message = "Credentials are not valid!";
            }
            else
            {
                //User exists and password is correct, so set token
                response.Token = CreateToken(user);
                response.Message = "Successfully logged in!";
            }
            //return the filled response with token or error message
            return response;
        }

        public async Task<ServiceResponse<Guid>> Register(User user, string password)
        {
            //Create new empty response
            ServiceResponse<Guid> response = new ServiceResponse<Guid>();

            if (await UsernameExists(user.Username))
            {
                //Useraccount with Username already exists, so return error
                response.Success = false;
                response.Message = "Username already taken.";
                return response;
            }
            if (await EmailExists(user.Email))
            {
                //Useraccount with Email already exists, so return error
                response.Success = false;
                response.Message = "Email already taken.";
                return response;
            }


            //Create password hash with salt
            CreatePasswordHash(password, out byte[] passwordhash, out byte[] passwordSalt);

            user.PasswordHash = passwordhash;
            user.PasswordSalt = passwordSalt;

            //Add default role to new user
            Role defaultRole = _context.Roles.FirstOrDefault(x => x.Name.Equals("User"));

            //Set join-table for entity framework
            UserRole newPersonRole = new UserRole
            {
                UserID = user.ID,
                User = user,
                RoleID = defaultRole.ID,
                Role = defaultRole
            };

            //Add user and user-role to database
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            await _context.UserRoles.AddAsync(newPersonRole);
            await _context.SaveChangesAsync();

            //set return data
            response.Data = user.ID;
            response.Message = "User registered succesfully.";

            //return userID 
            return response;
        }

        public async Task<ServiceResponse<Guid>> RegisterAdmin(User user, string password)
        {
            //Create new empty response
            ServiceResponse<Guid> response = new ServiceResponse<Guid>();

            if (await UsernameExists(user.Username))
            {
                //Useraccount with Username already exists, so return error
                response.Success = false;
                response.Message = "Username already taken.";
                return response;
            }

            //Create password hash with salt
            CreatePasswordHash(password, out byte[] passwordhash, out byte[] passwordSalt);

            user.PasswordHash = passwordhash;
            user.PasswordSalt = passwordSalt;

            //Add default role to new user
            Role defaultRole = _context.Roles.FirstOrDefault(x => x.Name.Equals("Admin"));

            //Set join-table for entity framework
            UserRole newPersonRole = new UserRole
            {
                UserID = user.ID,
                User = user,
                RoleID = defaultRole.ID,
                Role = defaultRole
            };

            //Add user and user-role to database
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            await _context.UserRoles.AddAsync(newPersonRole);
            await _context.SaveChangesAsync();

            //set return data
            response.Data = user.ID;
            response.Message = "Admin registered succesfully.";

            //return AdminID 
            return response;
        }

        public async Task<bool> EmailExists(string email)
        {
            //Check if user with email already exists
            if (await _context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower())) return true;
            return false;
        }

        public async Task<bool> UsernameExists(string username)
        {
            //Check if user with username already exists
            if (await _context.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower())) return true;
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //Create password hash with salt
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            //Verify password hash with salt
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private string CreateToken(User user)
        {
            //Set claims for token
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString())
            };
            if (user.Username != null)
            {
                claims.Add(new Claim(ClaimTypes.Name, user.Username));
            }
            foreach (UserRole role in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Role.Name));
            }

            //Add security key to token
            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("Auth:Token").Value)
            );

            //Set token credentials
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //Fill token descriptor
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            //Create new tokenHandler and create token including tokenDescriptor
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            //Return JWT 
            return tokenHandler.WriteToken(token);
        }

        public async Task<User> Get(Guid id)
        {
            User retrievedUser = await _context.Users.FirstOrDefaultAsync(x => x.ID == id);
            if (retrievedUser != null)
            {
                User user = new User()
                {
                    ID = retrievedUser.ID,
                    Username = retrievedUser.Username,
                    Email = retrievedUser.Email,
                    PhoneNumber = retrievedUser.PhoneNumber
                };
                return user;
            }
            return null;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var entitiesToReturn = await _context.Users.ToListAsync();
            return entitiesToReturn;
        }

        public async Task<ServiceResponse<User>> DeleteUser(Guid id)
        {
            ServiceResponse<User> response = new ServiceResponse<User>();
            User user = new User()
            {
                ID = id
            };

            _context.Users.Attach(user);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            response.Message = "User has been removed!";
            response.Success = true;
            return response;
        }
    }
}
