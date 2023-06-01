using JetBrains.Annotations;
using WebApplication1.Attribute;
using WebApplication1.ORM.Entity;
using WebApplication1.ORM.Wrapper;

namespace WebApplication1.Entitiy;

[Table("schueler")]
public class Schueler : Entity
{
    [Column("schueler_id")]
    [ID]
    [UsedImplicitly]
    public long ID { get; set; }
    
    [OneToOne(primary: "anmeldung_id", foreign: "anmeldung", ReferencedBy.THIS)]
    [UsedImplicitly]
    [Attribute.NotNull]
    public Anmeldedatum Anmeldedatum { get; set; }   
    
    [Column("fullname")]
    [UsedImplicitly]
    public string? Name { get; set; }
    
    [ManyToMany("schueler_hat_klasse")]
    [UsedImplicitly]
    public List<Klasse> Klassen { get; set; }
    
    [OneToMany(primary: "schueler_id", foreign: "schueler_id")]
    [UsedImplicitly]
    public List<Leistung> Leistungen { get; set; }
}