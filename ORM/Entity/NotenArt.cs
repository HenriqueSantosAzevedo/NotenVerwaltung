using WebApplication1.Attribute;
using WebApplication1.ORM.Wrapper;

namespace WebApplication1.Entitiy;

[Table("noteArt")]
public class NotenArt : Entity
{
    [Column("note_art_id")]
    [ID]
    public long ID { get; set; }
    
    [Column]
    public string Bezeichnung { get; set; }
}