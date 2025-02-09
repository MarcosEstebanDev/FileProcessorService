using Microsoft.EntityFrameworkCore;
using FileProcessorService.Models;

namespace FileProcessorService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<FileRecord> FileRecords { get; set; }
}
