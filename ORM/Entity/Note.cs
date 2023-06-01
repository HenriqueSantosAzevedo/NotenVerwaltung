using JetBrains.Annotations;
using NotenAppConsoleSchueler.ORM.Wrapper.Attribute;

namespace NotenAppConsoleSchueler.ORM.Entity;

[Table("note")]
public class Note : Wrapper.Entity
{
    [Column("note_id")]
    [ID]
    [UsedImplicitly]
    public long ID { get; set; }
    
    [Column("note")]
    [UsedImplicitly]
    public long NotenWert { get; set; }
    
    [Column]
    [UsedImplicitly]
    public string Bezeichnung { get; set; }
}