using JetBrains.Annotations;
using NotenAppConsoleSchueler.ORM.Wrapper.Attribute;

namespace NotenAppConsoleSchueler.ORM.Entity;

[Table("kurs")]
public class Kurs : NotenAppConsoleSchueler.ORM.Wrapper.Entity
{
    [OneToOne(primary: "lehrer_id", foreign: "lehrer_id")]
    [UsedImplicitly]
    public Lehrer Lehrer { get; set; }

    [Column("kurs_id")]
    [ID]
    [UsedImplicitly] 
    public long ID { get; set; }
    
    [Column("kurs_name")]
    [UsedImplicitly]
    public string KursName { get; set; }
    
    [OneToMany("kurs_id", "kurs_id")]
    public List<Leistung> Leistungen { get; set; }
}