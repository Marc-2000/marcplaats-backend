using Marcplaats_Backend.BLL.DTOs;
using Marcplaats_Backend.BLL.Models.Entities;
using Marcplaats_Backend.BLL.RepositoryInterfaces;
using Marcplaats_Backend.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marcplaats_Backend.BLL.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        public MessageRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<Guid>> SendMessage(MessageDTO messageDTO)
        {
            ServiceResponse<Guid> response = new ServiceResponse<Guid>();

            Message message = new Message
            {
                Text = messageDTO.message,
                UserID = messageDTO.UserID,
                ChatID = messageDTO.ChatID,
                Time = messageDTO.DateTime
            };

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            return response;
        }
    }
}
