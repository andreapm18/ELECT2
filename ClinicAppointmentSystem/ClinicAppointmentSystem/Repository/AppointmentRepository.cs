using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointmentSystem;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ClinicContext _context;

    public AppointmentRepository(ClinicContext context) => _context = context;

    public async Task<Appointment> AddAsync(Appointment appointment)
    {
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
        return appointment;
    }

    public async Task<bool> UpdateAsync(Appointment appointment)
    {
        if (!await _context.Appointments.AnyAsync(a => a.AppointmentId == appointment.AppointmentId))
            return false;

        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int appointmentId)
    {
        var appt = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        if (appt is null) return false;

        _context.Appointments.Remove(appt);
        await _context.SaveChangesAsync();
        return true;
    }

    public Task<Appointment?> GetByIdAsync(int appointmentId) =>
        _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

    public Task<List<Appointment>> GetAllAsync() =>
        _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .ToListAsync();

    public Task<bool> ExistsAsync(int appointmentId) =>
        _context.Appointments.AnyAsync(a => a.AppointmentId == appointmentId);

    public Task<bool> DoctorIsBookedAsync(int doctorId, DateTime appointmentDateTime) =>
        _context.Appointments.AnyAsync(a =>
            a.DoctorId == doctorId &&
            a.AppointmentDateTime == appointmentDateTime &&
            a.Status != "Cancelled"); // allow booking if previous is cancelled (optional)

    public Task<bool> AnyForDoctorAsync(int doctorId) =>
        _context.Appointments.AnyAsync(a => a.DoctorId == doctorId);

    public Task<bool> AnyForPatientAsync(int patientId) =>
        _context.Appointments.AnyAsync(a => a.PatientId == patientId);
}