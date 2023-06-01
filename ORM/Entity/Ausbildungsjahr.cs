using JetBrains.Annotations;
using NotenAppConsoleSchueler.ORM.Wrapper.Attribute;

namespace NotenAppConsoleSchueler.ORM.Entity;

[Table("ausbildungsjahr")]
public class Ausbildungsjahr : Wrapper.Entity
{
    [Column("ausbildungsjahr_id")]
    [ID]
    [UsedImplicitly]
    public long ID { get; set; }
    
    [Column]
    public string Bezeichnung { get; set; }
}