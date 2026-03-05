using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointmentSystem.Models;

[Index("Email", Name = "UQ_Doctors_Email", IsUnique = true)]
[Index("LicenseNumber", Name = "UQ_Doctors_LicenseNumber", IsUnique = true)]
public partial class Doctor
{
    [Key]
    public int DoctorId { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string LicenseNumber { get; set; } = null!;

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(80)]
    public string? Specialty { get; set; }

    [InverseProperty("Doctor")]
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
