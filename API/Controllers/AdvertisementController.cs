using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Marcplaats_Backend.BLL.DTOs;
using Marcplaats_Backend.BLL.Models.Entities;
using Marcplaats_Backend.BLL.RepositoryInterfaces;
using Marcplaats_Backend.DAL.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Marcplaats_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementRepository _advertisementRepository;

        public AdvertisementController(IAdvertisementRepository advertisementRepository)
        {
            _advertisementRepository = advertisementRepository;
        }

        [Authorize]
        [HttpPost("Create"), DisableRequestSizeLimit]
        public async Task<IActionResult> Create([FromBody] CreateAdDTO advertisement)
        {
            ServiceResponse<Guid> response = await _advertisementRepository.Create(
                new Advertisement
                { Title = advertisement.Title, Description = advertisement.Description, PicturePath = advertisement.PicturePath, Price = advertisement.Price, Bidding = advertisement.Bidding },
                advertisement.CategoryID
                );
            return Ok(response);
        }

        [HttpGet("GetAllAdvertisements")]
        public async Task<IActionResult> GetAllAdvertisements()
        {
            List<Advertisement> advertisements = await _advertisementRepository.GetAllAdvertisements();
            return Ok(advertisements);
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            List<Category> categories = await _advertisementRepository.GetAllCategories();
            return Ok(categories);
        }
    }
}
