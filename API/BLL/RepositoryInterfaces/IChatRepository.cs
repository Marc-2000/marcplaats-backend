using Marcplaats_Backend.BLL.DTOs;
using Marcplaats_Backend.BLL.Models.Entities;
using Marcplaats_Backend.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marcplaats_Backend.BLL.RepositoryInterfaces
{
    public interface IChatRepository
    {
        Task<List<Chat>> GetAllByUserID(Guid UserID);
    }
}
