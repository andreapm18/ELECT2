using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Repositories.Interfaces;
using ClinicAppointmentSystem.Services.Interfaces;

namespace ClinicAppointmentSystem.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patients;
    private readonly IAppointmentRepository _appointments;

    public PatientService(IPatientRepository patients, IAppointmentRepository appointments)
    {
        _patients = patients;
        _appointments = appointments;
    }

    public async Task<(bool ok, string message, Patient? patient)> RegisterAsync(Patient patient)
    {
        if (string.IsNullOrWhiteSpace(patient.PatientNumber))
            return (false, "PatientNumber is required.", null);
        if (string.IsNullOrWhiteSpace(patient.FirstName) || string.IsNullOrWhiteSpace(patient.LastName))
            return (false, "FirstName and LastName are required.", null);

        if (await _patients.PatientNumberExistsAsync(patient.PatientNumber))
            return (false, "PatientNumber already exists.", null);

        var saved = await _patients.AddAsync(patient);
        return (true, "Patient registered.", saved);
    }

    public async Task<(bool ok, string message)> DeleteAsync(int patientId)
    {
        if (!await _patients.ExistsAsync(patientId))
            return (false, "Patient not found.");

        if (await _appointments.AnyForPatientAsync(patientId))
            return (false, "Cannot delete patient with existing appointments.");

        await _patients.DeleteAsync(patientId);
        return (true, "Patient deleted.");
    }
}