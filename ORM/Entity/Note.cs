using JetBrains.Annotations;
using WebApplication1.Attribute;
using WebApplication1.ORM.Wrapper;

namespace WebApplication1.Entitiy;

[Table("note")]
public class Note : Entity
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