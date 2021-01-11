using Google.Apis.Auth;
using Marcplaats_Backend.BLL.DTOs;
using Marcplaats_Backend.BLL.Models.Entities;
using Marcplaats_Backend.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marcplaats_Backend.BLL.RepositoryInterfaces
{
    public interface IAccountRepository
    {
        Task<ServiceResponse<Guid>> Register(User user, string password);
        Task<ServiceResponse<Guid>> RegisterAdmin(User user, string password);
        Task<ServiceResponse<User>> Login(string username, string password);
        Task<ServiceResponse<User>> GoogleLogin(GoogleJsonWebSignature.Payload payload);
        Task<bool> EmailExists(string email);
        Task<bool> UsernameExists(string username);
        Task<User> Get(Guid id);
        Task<List<User>> GetAllUsers();
        Task<ServiceResponse<User>> DeleteUser(Guid id);
    }
}
