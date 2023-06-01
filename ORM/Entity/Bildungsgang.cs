using JetBrains.Annotations;
using NotenAppConsoleSchueler.ORM.Wrapper.Attribute;

namespace NotenAppConsoleSchueler.ORM.Entity;

[Table("bildungsgang")]
public class Bildungsgang : Wrapper.Entity
{
    [Column("bildungsgang_id")]
    [ID]
    [UsedImplicitly]
    public long ID { get; set; }
    
    [Column("name")]
    public string bezeichnung { get; set; }
}