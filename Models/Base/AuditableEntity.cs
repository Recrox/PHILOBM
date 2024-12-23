﻿namespace PHILOBM.Models.Base;

public abstract class AuditableEntity : BaseEntity
{
    public string? CreatedBy { get; set; } // Utilisateur qui a créé l'entité
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string? ModifiedBy { get; set; } // Utilisateur qui a modifié l'entité
    public DateTime? ModifiedDate { get; set; } // Date de la dernière modification

}