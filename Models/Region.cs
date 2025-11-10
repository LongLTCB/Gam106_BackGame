namespace WebApplication1.Models;
public class Region
{
    public int regionId { get; set; }
    public required string Name { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
    public Region()
    {
        Users = new List<User>();
    }
    public Region(int id, string name)
    {
        regionId = id;
        Name = name;
    }
}