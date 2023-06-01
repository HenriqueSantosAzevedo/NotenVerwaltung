using JetBrains.Annotations;
using WebApplication1.Attribute;
using WebApplication1.ORM.Entity;
using WebApplication1.ORM.Wrapper;

namespace WebApplication1.Entitiy;

[Table("lehrer")]
public class Lehrer : Entity
{
    [Column("lehrer_id")]
    [ID]
    [UsedImplicitly]
    public long ID { get; set; }
    
    [Column("fullname")]
    [UsedImplicitly]
    public string? Name { get; set; }
    
    [Column]
    [UsedImplicitly]
    public DateTime Geburtstag { get; set; }
    
    [OneToMany(foreign: "lehrer_id")]
    [UsedImplicitly]
    public List<Kurs>? Kurse { get; set; }
    
    [OneToOne(primary: "anmeldung_id", foreign: "anmeldung_id")]
    [UsedImplicitly]
    [Attribute.NotNull]
    public Anmeldedatum Anmeldedatum { get; set; }
    
    [Column("hat_administrative_rechte")]
    public Boolean HatAdministrativeRechte { get; set; }
}