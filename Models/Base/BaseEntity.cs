using System.ComponentModel.DataAnnotations;

namespace PHILOBM.Models.Base;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
}
