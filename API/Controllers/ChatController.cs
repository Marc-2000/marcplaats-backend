using Marcplaats_Backend.BLL.DTOs;
using Marcplaats_Backend.BLL.Models.Entities;
using Marcplaats_Backend.BLL.RepositoryInterfaces;
using Marcplaats_Backend.DAL.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marcplaats_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMessageRepository _messageRepository;

        public ChatController(IChatRepository chatrepository, IMessageRepository messagerepository)
        {
            _chatRepository = chatrepository;
            _messageRepository = messagerepository;
        }

        [Authorize]
        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] MessageDTO messageDTO)
        {
            ServiceResponse<Guid> response = await _messageRepository.SendMessage(messageDTO);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("GetAllByUserID")]
        public async Task<IActionResult> GetAllByUserID(Guid UserID)
        {
            List<Chat> chats = await _chatRepository.GetAllByUserID(UserID);
            return Ok(chats);
        }
    }
}
