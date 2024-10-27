using PHILOBM.Models.Base;

namespace PHILOBM.Models;

public class Client : BaseEntity
{
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string? Adress { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public List<Car> Cars { get; set; } = new List<Car>();
}
