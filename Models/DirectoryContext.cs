using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Test.Models;

public class DirectoryContext : DbContext
{
    public DbSet<Directory> Directories { get; set; }

    public DirectoryContext(DbContextOptions<DirectoryContext> options)
            : base(options)
        {
            Database.EnsureCreated(); 
        }

        public static void Serialize(List<Directory> directories, string saveName)
        {
            var json = JsonSerializer.Serialize(directories, new JsonSerializerOptions()
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });
            File.WriteAllText($"Saved/{saveName.Substring(saveName.LastIndexOf(@"\") + 1)}.json", json);
        }
}