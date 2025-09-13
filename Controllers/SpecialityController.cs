using Microsoft.AspNetCore.Mvc;
using salud_vital_proyecto1.Core.Entities;
using salud_vital_proyecto1.Core.Interfaces;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace salud_vital_proyecto1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialityController : ControllerBase
    {
        private readonly ISpecialityService _specialityService;

        public SpecialityController(ISpecialityService specialityService)
        {
            _specialityService = specialityService;
        }
        // GET: api/<SpecialityController>
        [HttpGet]
        public async Task<IEnumerable<Speciality>> Get()
        {
            IEnumerable<Speciality> specialities = await _specialityService.GetSpecialitiesAsync();
            return specialities;
        }

        // GET api/<SpecialityController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SpecialityController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SpecialityController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SpecialityController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
