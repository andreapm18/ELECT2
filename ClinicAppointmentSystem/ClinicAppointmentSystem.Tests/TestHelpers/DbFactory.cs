using ClinicAppointmentSystem.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointmentSystem.Tests.TestHelpers;

public static class DbFactory
{
    public static (ClinicContext ctx, SqliteConnection conn) Create()
    {
        var conn = new SqliteConnection("DataSource=:memory:");
        conn.Open();

        var options = new DbContextOptionsBuilder<ClinicContext>()
            .UseSqlite(conn)
            .EnableSensitiveDataLogging()
            .Options;

        var ctx = new ClinicContext(options);

        // important: creates tables in SQLite based on your EF model
        ctx.Database.EnsureCreated();

        return (ctx, conn);
    }
}