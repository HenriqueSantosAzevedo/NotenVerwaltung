using JetBrains.Annotations;
using NotenAppConsoleSchueler.ORM.Wrapper.Attribute;

namespace NotenAppConsoleSchueler.ORM.Entity;

[Table("schueler")]
public class Schueler : Wrapper.Entity
{
    [Column("schueler_id")]
    [ID]
    [UsedImplicitly]
    public long ID { get; set; }
    
    [OneToOne(primary: "anmeldung_id", foreign: "anmeldung_id", ReferencedBy.THIS)]
    [UsedImplicitly]
    [NotenAppConsoleSchueler.ORM.Wrapper.Attribute.NotNull]
    public Anmeldedatum Anmeldedatum { get; set; }   
    
    [Column("fullname")]
    [UsedImplicitly]
    public string? Name { get; set; }
    
    [Column]
    [UsedImplicitly]
    public DateTime Geburtstag { get; set; }
    
    [ManyToMany("schueler_hat_klasse")]
    [UsedImplicitly]
    public List<Klasse> Klassen { get; set; }
    
    [OneToMany(primary: "schueler_id", foreign: "schueler_id")]
    [UsedImplicitly]
    public List<Leistung> Leistungen { get; set; }
}