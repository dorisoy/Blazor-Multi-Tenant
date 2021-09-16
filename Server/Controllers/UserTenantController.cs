﻿using ExampleApp.Shared;
using ExampleApp.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleApp.Server.Controllers
{
    [Authorize]
    //[ServiceFilter(typeof(UserTenantAccess2Attribute))]
    [Route("api/[controller]")]
    public class UserTenantAccessController : ControllerBase
    {
        private readonly IUserTenantRepository _repository;

        public UserTenantAccessController(IUserTenantRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _repository.GetAll();
            return Ok(result);
        }

        [HttpGet]
        [Route("tenant/{tenantId}")]
        public async Task<IActionResult> GetAllByTenant(int tenantId)
        {
            var result = await _repository.Filter(x => x.TenantId == tenantId, new string[] { "Tenant", "User"});
            return Ok(result.ToList());
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserTenantAccess userTenantAccess)
        {
            if (ModelState.IsValid)
            {
                var result = await _repository.Insert(userTenantAccess);
                return Ok(result);
            }

            return BadRequest(ModelState.Values);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Details(int id)
        {


            var result = await _repository.GetById(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] UserTenantAccess userTenantAccess)
        {
            if (ModelState.IsValid)
            {
                var result = await _repository.Update(userTenantAccess);
                return Ok(result);
            }

            return BadRequest(ModelState.Values);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.Delete(id);
            return Ok();
        }
    }
}
