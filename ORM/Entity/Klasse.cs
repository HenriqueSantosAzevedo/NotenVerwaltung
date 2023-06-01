using JetBrains.Annotations;
using NotenAppConsoleSchueler.ORM.Wrapper.Attribute;

namespace NotenAppConsoleSchueler.ORM.Entity;

[Table("klasse")]
public class Klasse : Wrapper.Entity
{
    [Column("klasse_id")]
    [UsedImplicitly]
    [ID]
    public long? ID { get; set; }
    
    [Column("klassen_name")]
    [UsedImplicitly]
    public string? KlassenName { get; set; }

    [ManyToMany("klasse_hat_kurs")]
    [UsedImplicitly]
    public List<Kurs> Kurse { get; set; }
    
    [OneToOne("ausbildungsjahr_id", "ausbildungsjahr_id")]
    public Ausbildungsjahr Ausbildungsjahr { get; set; }
    
    [OneToOne("bildungsgang_id", "bildungsgang_id")]
    public Bildungsgang Bildungsgang { get; set; }
    
    [OneToOne("lehrer_id", "lehrer_id")]
    public Lehrer Lehrer { get; set; }
}