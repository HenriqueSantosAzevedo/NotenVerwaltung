using NotenAppConsoleSchueler.ORM.Wrapper.Attribute;

namespace NotenAppConsoleSchueler.ORM.Entity;

[Table("noteArt")]
public class NotenArt : Wrapper.Entity
{
    [Column("note_art_id")]
    [ID]
    public long ID { get; set; }
    
    [Column]
    public string Bezeichnung { get; set; }
}