using Marcplaats_Backend.BLL.Models.Entities;
using Marcplaats_Backend.BLL.RepositoryInterfaces;
using Marcplaats_Backend.DAL.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marcplaats_Backend.BLL.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly DataContext _context;
        public ChatRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Chat>> GetAllByUserID(Guid UserID)
        {
            try
            {
                User user = await _context.Users.Include(u => u.ChatUsers).ThenInclude(cu => cu.Chat).ThenInclude(ms => ms.Messages).FirstOrDefaultAsync(u => u.ID == UserID);
                List<Chat> chats = new List<Chat>();

                for (int i = 0; i < user.ChatUsers.Count; i++)
                {
                    chats.Add(user.ChatUsers[i].Chat);
                }

                return chats;
            }
            catch (Exception error)
            {
                return null;
            }
        }
    }
}
