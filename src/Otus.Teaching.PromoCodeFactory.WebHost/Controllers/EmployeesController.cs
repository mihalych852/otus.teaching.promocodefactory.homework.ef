using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController
        : ControllerBase
    {
        private readonly IRepository<Employee> _empRepository;

        public EmployeesController(IRepository<Employee> empRepository)
        {
            _empRepository = empRepository;
        }
        
        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortDTO>> GetEmployeesAsync()
        {
            var employees = await _empRepository.GetAllAsync();

            var employeesModelList = employees.Select(x => 
                new EmployeeShortDTO()
                    {
                        Id = x.Id,
                        Email = x.Email,
                        FullName = x.FullName,
                    }).ToList();

            return employeesModelList;
        }
        
        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _empRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeDTO()
            {
                Id = employee.Id,
                Email = employee.Email,
                Role = new RoleItemDTO()
                {
                    Id = employee.Id,
                    Name = employee.Role.Name,
                    Description = employee.Role.Description
                },
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        /// <summary>
        /// Создать сотрудника
        /// </summary>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<ActionResult> AddEmployeeAsync(Employee emp)
        {
            await _empRepository.AddAsync(emp);

            return Ok();
        }

        /// <summary>
        /// Удалить сотрудника
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete")]
        public async Task<ActionResult> DeleteEmployeeAsync(Employee emp)
        {
            await _empRepository.DeleteAsync(emp);

            return Ok();
        }

        /// <summary>
        /// Обновить сотрудника
        /// </summary>
        /// <returns></returns>
        [HttpPost("edit")]
        public async Task<ActionResult> UpdateEmployeeAsync(Employee emp)
        {
            await _empRepository.UpdateAsync(emp);

            return Ok();
        }
    }
}