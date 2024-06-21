using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Services;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.Services
{
    public class PreferenceService : IPreferenceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PreferenceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Preference>> GetAllAsync()
        {
            return await _unitOfWork.PreferencesRepository.GetAll().ToListAsync();
        }
    }
}
