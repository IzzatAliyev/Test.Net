namespace Test.Models;
public class Directory
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? ParentId { get; set; }
    public virtual List<Directory>? Children { get; set; }
    public virtual Directory? Parent { get; set; }
}