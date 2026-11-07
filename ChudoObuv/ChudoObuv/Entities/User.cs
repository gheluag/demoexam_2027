namespace ChudoObuv.Entities;

public partial class User
{
    public int IdUser { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? Login { get; set; }

    public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();

    public int? RoleId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role? Role { get; set; }
}
