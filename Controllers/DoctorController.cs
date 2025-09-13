using Microsoft.AspNetCore.Mvc;
using salud_vital_proyecto1.Core.Entities;
using salud_vital_proyecto1.Core.Interfaces;
using salud_vital_proyecto1.Infraestructure.Repositories;

namespace salud_vital_proyecto1.Controllers
{
    public class DoctorController : ControllerBase
    {
        private readonly IUserService _doctorService;
        private readonly ILogService _logService;

        public DoctorController(IUserService doctorService, ILogService logService)
        {
            _doctorService = doctorService;
            _logService = logService;
        }

        // GET: api/doctor
        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();
            return Ok(doctors);
        }

        // GET: api/doctor/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _doctorService.GetDoctorAsync(id);
            if (doctor == null)
            {
                await _logService.LogAsync($"Doctor with id {id} not found", LogLevelType.Warning);
                return NotFound();
            }
            return Ok(doctor);
        }

        // POST: api/doctor
        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] Doctor doctor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdDoctor = await _doctorService.RegisterDoctorAsync(doctor);
            await _logService.LogAsync($"Doctor {doctor.Name} created", LogLevelType.Info);
            return CreatedAtAction(nameof(GetDoctorById), new { id = createdDoctor.Id }, createdDoctor);
        }

        // PUT: api/doctor/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] Doctor doctor)
        {
            if (id != doctor.Id)
                return BadRequest("Doctor ID mismatch");

            var existingDoctor = await _doctorService.GetDoctorAsync(id);
            if (existingDoctor == null)
            {
                await _logService.LogAsync($"Doctor with id {id} not found for update", LogLevelType.Warning);
                return NotFound();
            }

            await _doctorService.UpdateDoctorAsync(doctor);
            await _logService.LogAsync($"Doctor {doctor.Name} updated", LogLevelType.Info);

            return NoContent();
        }

        // DELETE: api/doctor/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var existingDoctor = await _doctorService.GetDoctorAsync(id);
            if (existingDoctor == null)
            {
                await _logService.LogAsync($"Doctor with id {id} not found for delete", LogLevelType.Warning);
                return NotFound();
            }

            await _doctorService.DeleteDoctorAsync(id);
            await _logService.LogAsync($"Doctor with id {id} deleted", LogLevelType.Info);

            return NoContent();
        }


    }
}
