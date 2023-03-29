using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private IMapper _mapper;
        private IRepository<Customer> _repo;
        public CustomersController (IMapper mapper, IRepository<Customer> repo)
        {
            _mapper = mapper;
            _repo = repo;
        }
        /// <summary>
        /// Получение списка клиентов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<CustomerShortResponse>> GetCustomersAsync()
        {
            var customers = await _repo.GetAllAsync();
            var customersDto = _mapper.Map<IEnumerable<CustomerShortResponse>>(customers);
            return Ok(customersDto);
        }


        /// <summary>
        /// получение клиента по его уникальному ИД
        /// </summary>
        /// <param name="id">ИД записи клиента</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            //TODO: Добавить получение клиента вместе с выданными ему промомкодами
            var entitySet = await _repo.GetByIdAsync(id);
            return Ok(entitySet);
        }
        
        /// <summary>
        /// Создание записи в таблице клиентов
        /// </summary>
        /// <param name="request">заполненная сущность</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var model = _mapper.Map<CreateOrEditCustomerRequest, Customer>(request);
            var result = await _repo.CreateAsync(model);
            return Ok(result);
        }
        
        /// <summary>
        /// изменение записи в таблице клиентов
        /// </summary>
        /// <param name="id">ИД клиента</param>
        /// <param name="request">сущность клиента</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var editedCustomer = _mapper.Map<Customer>(request);
            var customer = await _repo.GetByIdAsync(id);
            editedCustomer.PromoCodes = customer.PromoCodes;
            await _repo.UpdateAsync(editedCustomer);
            return Ok();

        }

        /// <summary>
        /// удаление записи клиента 
        /// </summary>
        /// <param name="id">ИД записи клиента в таблице</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var result = await _repo.DeleteAsync(id);
            return result ? Ok() : BadRequest();
        }
    }
}