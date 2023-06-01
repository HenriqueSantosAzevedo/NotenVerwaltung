using JetBrains.Annotations;
using NotenAppConsoleSchueler.ORM.Wrapper.Attribute;

namespace NotenAppConsoleSchueler.ORM.Entity;

[Table("anmeldung")]
public class Anmeldedatum : Wrapper.Entity
{
    [Column("anmeldung_id")]
    [ID]
    [UsedImplicitly]
    public long ID { get; set; }
    
    [Column]
    [UsedImplicitly]
    public string? Benutzername { get; set; }
    
    [Column("password_hash")]
    [UsedImplicitly]
    public string? Password { get; set; }
}