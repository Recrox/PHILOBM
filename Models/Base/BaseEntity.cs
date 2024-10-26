using System.ComponentModel.DataAnnotations;

namespace PHILOBM.Models.Base;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
}
