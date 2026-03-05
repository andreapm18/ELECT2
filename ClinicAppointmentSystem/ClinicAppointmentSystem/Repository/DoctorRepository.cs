using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointmentSystem;

public class DoctorRepository : IDoctorRepository
{
    private readonly ClinicContext _context;

    public DoctorRepository(ClinicContext context) => _context = context;

    public async Task<Doctor> AddAsync(Doctor doctor)
    {
        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();
        return doctor;
    }

    public async Task<bool> UpdateAsync(Doctor doctor)
    {
        if (!await _context.Doctors.AnyAsync(d => d.DoctorId == doctor.DoctorId))
            return false;

        _context.Doctors.Update(doctor);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int doctorId)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorId == doctorId);
        if (doctor is null) return false;

        _context.Doctors.Remove(doctor);
        await _context.SaveChangesAsync();
        return true;
    }

    public Task<Doctor?> GetByIdAsync(int doctorId) =>
        _context.Doctors.FirstOrDefaultAsync(d => d.DoctorId == doctorId);

    public Task<List<Doctor>> GetAllAsync() =>
        _context.Doctors.ToListAsync();

    public Task<bool> ExistsAsync(int doctorId) =>
        _context.Doctors.AnyAsync(d => d.DoctorId == doctorId);

    public Task<bool> EmailExistsAsync(string email) =>
        _context.Doctors.AnyAsync(d => d.Email == email);

    public Task<bool> LicenseNumberExistsAsync(string licenseNumber) =>
        _context.Doctors.AnyAsync(d => d.LicenseNumber == licenseNumber);
}