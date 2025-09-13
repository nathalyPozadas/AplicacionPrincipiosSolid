using Microsoft.AspNetCore.Mvc;
using salud_vital_proyecto1.Core.Interfaces;
using salud_vital_proyecto1.DTOs;
using salud_vital_proyecto1.Core.Entities;

namespace salud_vital_proyecto1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            var appointment = await _appointmentService.CreateAppointmentAsync(
                request.PatientId, request.DoctorId, request.AppointmentDate
            );
            return CreatedAtAction(nameof(GetAppointmentsByDoctor), new { doctorId = appointment.DoctorId, date = appointment.AppointmentDate.Date }, appointment);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var result = await _appointmentService.CancelAppointmentAsync(id);
            return result ? Ok() : BadRequest("No se pudo cancelar la cita");
        }

        [HttpPost("{id}/reschedule")]
        public async Task<IActionResult> RescheduleAppointment(int id, [FromBody] RescheduleAppointmentRequest request)
        {
            var result = await _appointmentService.RescheduleAppointmentAsync(id, request.NewDate);
            return result ? Ok() : BadRequest("No se pudo reprogramar la cita");
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveAppointment(int id)
        {
            var result = await _appointmentService.ApproveAppointmentAsync(id);
            return result ? Ok() : BadRequest("No se pudo aprobar la cita");
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> RejectAppointment(int id)
        {
            var result = await _appointmentService.RejectAppointmentAsync(id);
            return result ? Ok() : BadRequest("No se pudo rechazar la cita");
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsByDoctor(int doctorId, [FromQuery] DateOnly date)
        {
            var appointments = await _appointmentService.GetDoctorDailyAppointmentsAsync(doctorId, date);
            return Ok(appointments);
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> CompleteAppointment(int id)
        {
            var result = await _appointmentService.CompleteAppointmentAsync(id);
            return result ? Ok() : BadRequest("No se pudo completar la cita");
        }

        [HttpPost("{id}/add-summary")]
        public async Task<IActionResult> AddMedicalSummary(int id, [FromBody] AddMedicalSummaryRequest request)
        {
            var result = await _appointmentService.AddMedicalSummaryAsync(id, request.MedicalSummary);
            return result ? Ok() : BadRequest("No se pudo agregar el resumen médico");
        }
    }
}
