namespace Test.Models
{
    public class DirectoryPath
    {
        public string? Path { get; set; } = null;
        public static string? path = null; 

        public void SetPath()
        {
            path = Path;
        }
    }
}