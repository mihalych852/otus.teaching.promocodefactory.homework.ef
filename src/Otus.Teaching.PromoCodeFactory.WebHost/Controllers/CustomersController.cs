using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Services;
using Otus.Teaching.PromoCodeFactory.Core.Dtos;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    ///     Покупатели
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        /// <summary>
        ///     Получение всех покупателей
        /// </summary>
        /// <returns>200 + список всех покупателей</returns>
        [HttpGet]
        public async Task<ActionResult<CustomerShortResponse>> GetCustomersAsync()
        {
            var result = await _customerService.GetAllAsync();
            return Ok(_mapper.Map<List<CustomerShortResponse>>(result));
        }
        
        /// <summary>
        ///     Получение покупателя по его Id
        /// </summary>
        /// <param name="id">Id искомого покупателя</param>
        /// <returns>
        ///     200 + покупатель или 404,
        ///     если покупателя по указанномому Id найти не удалось
        /// </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var result = await _customerService.GetAsync(id);
            if (result == null)
                return NotFound();
            
            return Ok(_mapper.Map<CustomerResponse>(result));
        }
        
        /// <summary>
        ///     Создание покупателя вместе с его предпочнениями
        /// </summary>
        /// <param name="request">Информация о покупателе и список id на препочтения</param>
        /// <returns>200 + Id новой записи</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync([FromBody]CreateOrEditCustomerRequest request)
        {
            var result = await _customerService.CreateAsync(_mapper.Map<CreateOrEditCustomerDto>(request));
            return Ok(result);
        }
        
        /// <summary>
        ///     Обновление покупателя и его предпочтений
        /// </summary>
        /// <param name="id">Id лица, которое надо обновить</param>
        /// <param name="request">Обновленные данные</param>
        /// <returns>
        ///     200 - если лицо удалось обновить
        ///     404 - лицо не найдено
        /// </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var result = await _customerService.UpdateAsync(id, _mapper.Map<CreateOrEditCustomerDto>(request));
            if (result)
                return Ok();
            return NotFound();
        }
        
        /// <summary>
        ///     Удаление покупателя и его промокодов
        /// </summary>
        /// <param name="id">Id лица, которого надо удалить</param>
        /// <returns>
        ///     200 - если удаление прошло успешно
        ///     404 - если лицо не найдено
        /// </returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var result = await _customerService.DeleteAsync(id);
            if(result)
               return Ok();
            return NotFound();
        }
    }
}