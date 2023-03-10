using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Test.Models;
using System.Text.Json;

namespace Test.Controllers;

public class DirectoryController : Controller
{

    private readonly ILogger<DirectoryController> _logger;
    private readonly DirectoryContext _context;

    public DirectoryController(ILogger<DirectoryController> logger, DirectoryContext context)
    {
        _logger = logger;
        _context = context;
    }


    public IActionResult Main()
    {
        return View();
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult LoadFromSystem()
    {
        return View();
    }

    public IActionResult Load(DirectoryPath? directoryPath = null)
    {
        if (directoryPath?.Path == null)
        {
            return View();
        }
        else
        {
            try
            {
                List<Test.Models.Directory> directories = new List<Test.Models.Directory>();
                GetDirectories(ref directories, directoryPath.Path);
                DirectoryContext.Serialize(directories, directoryPath.Path);
                return RedirectToAction("Saved");
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }
    }

    public IActionResult Explorer(string? name = null)
    {
        ViewBag.Name = name;
        return View(_context.Directories);
    }


    public IActionResult Saved(string name)
    {
        if (name == null)
        {
            return View();
        }
        List<Test.Models.Directory> directories = Deserialize(name);
        ClearDb();
        _context.Directories.AddRange(directories);
        _context.SaveChanges();

        return RedirectToAction("Explorer");
    }

    public static void GetDirectories(ref List<Test.Models.Directory> directories, string path, Test.Models.Directory? parent = null)
    {
        Test.Models.Directory directory;
        try
        {
            List<string> dirs = System.IO.Directory.GetDirectories(path).ToList();

            foreach (var d in dirs)
            {
                directory = new Test.Models.Directory
                {
                    Name = d.Substring(d.LastIndexOf(@"\") + 1),
                    Parent = path == @"C:\" ? null : directories.FirstOrDefault(f => f.Name == path.Substring(path.LastIndexOf(@"\") + 1))
                };
                if (!directories.Contains(directory)) directories.Add(directory);
                GetDirectories(ref directories, d, parent);
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private List<Test.Models.Directory> Deserialize(string saveName = "")
    {
        List<Test.Models.Directory>? deserialized = new List<Test.Models.Directory>();
        using (FileStream fs = new FileStream($"{saveName}", FileMode.OpenOrCreate))
        {
            deserialized = JsonSerializer.Deserialize<IEnumerable<Test.Models.Directory>>(fs) as List<Test.Models.Directory>;
        }

        foreach (var directory in deserialized)
        {
            directory.Parent = deserialized.FirstOrDefault(f => f.Id == directory.ParentId);
        }
        return deserialized;
    }
    private void ClearDb()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}