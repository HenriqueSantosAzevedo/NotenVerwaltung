using JetBrains.Annotations;
using WebApplication1.Attribute;
using WebApplication1.ORM.Wrapper;

namespace WebApplication1.Entitiy;

[Table("anmeldung")]
public class Anmeldedatum : Entity
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