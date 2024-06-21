using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Services;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.Core.Dtos;


namespace Otus.Teaching.PromoCodeFactory.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _unitOfWork.CustomerRepository.GetAll().ToListAsync();
        }

        public async Task<Guid> CreateAsync(CreateOrEditCustomerDto customer)
        {
            var customerForCreate = _mapper.Map<Customer>(customer);
            customerForCreate.Preferences = await _unitOfWork.PreferencesRepository
                                                                .GetAll()
                                                                .Where(x => customer.PreferenceIds.Contains(x.Id))
                                                                .ToListAsync();

            await _unitOfWork.CustomerRepository.AddAsync(customerForCreate);
            await _unitOfWork.SaveChangesAsync();
            return customerForCreate.Id;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entityForRemove = await _unitOfWork
                                            .CustomerRepository.GetById(id)
                                            .Include(x => x.PromoCodes)
                                            .FirstOrDefaultAsync();
            if(entityForRemove is null)
                return false;

            await _unitOfWork.CustomerRepository.DeleteAsync(entityForRemove);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<Customer> GetAsync(Guid id)
        {
            return await _unitOfWork.CustomerRepository
                                        .GetById(id)
                                        .Include(x => x.Preferences)
                                        .Include(x => x.PromoCodes)
                                        .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(Guid id, CreateOrEditCustomerDto customer)
        {
            var currentCustomer = await _unitOfWork
                                            .CustomerRepository.GetById(id)
                                            .Include(x => x.Preferences)
                                            .FirstOrDefaultAsync();
            if (currentCustomer is null)
                return false;

            currentCustomer.Preferences = await _unitOfWork.PreferencesRepository
                                                                .GetAll()
                                                                .Where(x => customer.PreferenceIds.Contains(x.Id))
                                                                .ToListAsync();

            currentCustomer.FirstName = customer.FirstName;
            currentCustomer.LastName = customer.LastName;
            currentCustomer.Email = customer.Email;
            await _unitOfWork.CustomerRepository.UpdateAsync(currentCustomer);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
