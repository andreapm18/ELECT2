using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

using ClinicAppointmentSystem;

public class PatientRepository : IPatientRepository
{
    private readonly ClinicContext _context;

    public PatientRepository(ClinicContext context) => _context = context;

    public async Task<Patient> AddAsync(Patient patient)
    {
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return patient;
    }

    public async Task<bool> UpdateAsync(Patient patient)
    {
        if (!await _context.Patients.AnyAsync(p => p.PatientId == patient.PatientId))
            return false;

        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int patientId)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientId == patientId);
        if (patient is null) return false;

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
        return true;
    }

    public Task<Patient?> GetByIdAsync(int patientId) =>
        _context.Patients.FirstOrDefaultAsync(p => p.PatientId == patientId);

    public Task<List<Patient>> GetAllAsync() =>
        _context.Patients.ToListAsync();

    public Task<bool> ExistsAsync(int patientId) =>
        _context.Patients.AnyAsync(p => p.PatientId == patientId);

    public Task<bool> PatientNumberExistsAsync(string patientNumber) =>
        _context.Patients.AnyAsync(p => p.PatientNumber == patientNumber);
}