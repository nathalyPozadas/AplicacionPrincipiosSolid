using Microsoft.AspNetCore.Mvc;
using salud_vital_proyecto1.Core.Entities;
using salud_vital_proyecto1.Core.Interfaces;
using salud_vital_proyecto1.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace salud_vital_proyecto1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogService _loggerService;

        public PatientsController(IUserService userService, ILogService loggerService)
        {
            _userService = userService;
            _loggerService = loggerService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterPatient([FromBody] RegisterPatientRequest request)
        {
            try
            {
                var patient = new Patient
                {
                    Name = request.Name,
                    Email = request.Email,
                    Phone = request.Phone,
                    DateOfBirth = request.DateOfBirth,
                    Address = request.Address,
                    EmergencyContact = request.EmergencyContact,
                    EmergencyPhone = request.EmergencyPhone
                };

                var registeredPatient = await _userService.RegisterPatientAsync(patient);

                await _loggerService.LogAsync($"Paciente {registeredPatient.Id} registrado exitosamente", LogLevelType.Info);
                return CreatedAtAction(nameof(GetPatientById), new { id = registeredPatient.Id }, registeredPatient);
            }
            catch (Exception ex)
            {
                await _loggerService.LogAsync($"Error registrando paciente: {ex.Message}", LogLevelType.Error);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            try
            {
                var patient = await _userService.GetPatientAsync(id);
                if (patient == null) return NotFound();
                return Ok(patient);
            }
            catch (Exception ex)
            {
                await _loggerService.LogAsync($"Error obteniendo paciente {id}: {ex.Message}", LogLevelType.Error);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] UpdatePatientRequest request)
        {
            try
            {
                var patient = await _userService.GetPatientAsync(id);
                if (patient == null) return NotFound($"Paciente con ID {id} no encontrado");

                patient.Name = request.Name;
                patient.Phone = request.Phone;
                patient.DateOfBirth = request.DateOfBirth;
                patient.Address = request.Address;
                patient.EmergencyContact = request.EmergencyContact;
                patient.EmergencyPhone = request.EmergencyPhone;
                patient.UpdatedAt = DateTime.UtcNow; 

                await _userService.UpdatePatientAsync(patient);

                return Ok(patient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
