using Marcplaats_Backend.BLL.Models.Entities;
using Marcplaats_Backend.BLL.RepositoryInterfaces;
using Marcplaats_Backend.DAL.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Marcplaats_Backend.BLL.Repositories
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
        private readonly DataContext _context;
        

        public AdvertisementRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<Guid>> Create(Advertisement advertisement, Guid categoryID)
        {
            //Create new empty response
            ServiceResponse<Guid> response = new ServiceResponse<Guid>();

            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.ID == categoryID);

            if(advertisement.Title == "" || advertisement.Description == "" || category == null)
            {
                response.Success = false;
                response.Message = "Ad could not be placed.";

                return response;
            }

            advertisement.Category = category;

            //Add advertisement to database
            await _context.Advertisements.AddAsync(advertisement);
            await _context.SaveChangesAsync();

            //set return data
            response.Data = advertisement.ID;
            response.Message = "Advertisement created succesfully.";

            return response;
        }

        public async Task<List<Advertisement>> GetAllAdvertisements()
        {
            var entityToReturn = await _context.Advertisements.ToListAsync();
            return entityToReturn;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            var entityToReturn = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            return entityToReturn;
        }
    }
}

