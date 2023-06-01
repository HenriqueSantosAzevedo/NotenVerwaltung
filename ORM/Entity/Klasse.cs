using JetBrains.Annotations;
using WebApplication1.Attribute;
using WebApplication1.ORM.Entity;
using WebApplication1.ORM.Wrapper;

namespace WebApplication1.Entitiy;

[Table("klasse")]
public class Klasse : Entity
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