using Marcplaats_Backend.BLL.DTOs;
using Marcplaats_Backend.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marcplaats_Backend.BLL.RepositoryInterfaces
{
    public interface IMessageRepository
    {
        Task<ServiceResponse<Guid>> SendMessage(MessageDTO messageDTO);
    }
}
