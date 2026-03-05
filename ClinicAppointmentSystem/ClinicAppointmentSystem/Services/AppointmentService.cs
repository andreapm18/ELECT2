using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Repositories.Interfaces;
using ClinicAppointmentSystem.Services.Interfaces;

namespace ClinicAppointmentSystem.Services;

public class AppointmentService : IAppointmentService
{
    private static readonly HashSet<string> AllowedStatuses =
        new(StringComparer.OrdinalIgnoreCase) { "Booked", "Completed", "Cancelled" };

    private readonly IPatientRepository _patients;
    private readonly IDoctorRepository _doctors;
    private readonly IAppointmentRepository _appointments;

    public AppointmentService(IPatientRepository patients, IDoctorRepository doctors, IAppointmentRepository appointments)
    {
        _patients = patients;
        _doctors = doctors;
        _appointments = appointments;
    }

    public async Task<(bool ok, string message, Appointment? appointment)> BookAsync(int patientId, int doctorId, DateTime dateTime)
    {
        if (dateTime <= DateTime.Now)
            return (false, "Appointment must be in the future.", null);

        if (!await _patients.ExistsAsync(patientId))
            return (false, "Patient not found.", null);

        if (!await _doctors.ExistsAsync(doctorId))
            return (false, "Doctor not found.", null);

        if (await _appointments.DoctorIsBookedAsync(doctorId, dateTime))
            return (false, "Doctor is already booked at that time.", null);

        var appt = new Appointment
        {
            PatientId = patientId,
            DoctorId = doctorId,
            AppointmentDateTime = dateTime,
            Status = "Booked"
        };

        var saved = await _appointments.AddAsync(appt);
        return (true, "Appointment booked.", saved);
    }

    public Task<(bool ok, string message)> CancelAsync(int appointmentId) =>
        UpdateStatusAsync(appointmentId, "Cancelled");

    public Task<(bool ok, string message)> CompleteAsync(int appointmentId) =>
        UpdateStatusAsync(appointmentId, "Completed");

    public async Task<(bool ok, string message)> UpdateStatusAsync(int appointmentId, string status)
    {
        if (!AllowedStatuses.Contains(status))
            return (false, "Invalid status. Allowed: Booked, Completed, Cancelled.");

        var appt = await _appointments.GetByIdAsync(appointmentId);
        if (appt is null)
            return (false, "Appointment not found.");

        appt.Status = status;
        await _appointments.UpdateAsync(appt);
        return (true, "Status updated.");
    }
}