using ClinicAppointmentSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAppointmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _service;

    public AppointmentsController(IAppointmentService service) => _service = service;

    [HttpPost("book")]
    public async Task<IActionResult> Book(int patientId, int doctorId, DateTime dateTime)
    {
        var (ok, message, appointment) = await _service.BookAsync(patientId, doctorId, dateTime);
        if (!ok) return BadRequest(new { message });

        return Ok(new { message, appointment });
    }

    [HttpPatch("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var (ok, message) = await _service.CancelAsync(id);
        return ok ? Ok(new { message }) : BadRequest(new { message });
    }

    [HttpPatch("{id}/complete")]
    public async Task<IActionResult> Complete(int id)
    {
        var (ok, message) = await _service.CompleteAsync(id);
        return ok ? Ok(new { message }) : BadRequest(new { message });
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        var (ok, message) = await _service.UpdateStatusAsync(id, status);
        return ok ? Ok(new { message }) : BadRequest(new { message });
    }
}