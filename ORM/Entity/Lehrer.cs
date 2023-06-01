using JetBrains.Annotations;
using NotenAppConsoleSchueler.ORM.Wrapper.Attribute;

namespace NotenAppConsoleSchueler.ORM.Entity;

[Table("lehrer")]
public class Lehrer : Wrapper.Entity
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
    [NotenAppConsoleSchueler.ORM.Wrapper.Attribute.NotNull]
    public Anmeldedatum Anmeldedatum { get; set; }
    
    [Column("hat_administrative_rechte")]
    public Boolean HatAdministrativeRechte { get; set; }
}