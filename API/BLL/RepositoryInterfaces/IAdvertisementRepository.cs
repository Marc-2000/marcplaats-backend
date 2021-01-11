using Marcplaats_Backend.BLL.Models.Entities;
using Marcplaats_Backend.DAL.Context;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marcplaats_Backend.BLL.RepositoryInterfaces
{
    public interface IAdvertisementRepository
    {
        Task<ServiceResponse<Guid>> Create(Advertisement advertisement, Guid categoryID);
        Task<List<Advertisement>> GetAllAdvertisements();
        Task<List<Category>> GetAllCategories();
    }
}
