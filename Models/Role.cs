namespace WebApplication1.Models;
public class Role
{
    public int roleId { get; set; }
    public required string Name { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
    public Role()
    {
        Users = new List<User>();
    }
    public Role(int id, string name)
    {
        roleId = id;
        Name = name;
    }
}