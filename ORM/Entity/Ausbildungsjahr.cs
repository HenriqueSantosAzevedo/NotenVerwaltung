using JetBrains.Annotations;
using WebApplication1.Attribute;
using WebApplication1.ORM.Wrapper;

namespace WebApplication1.Entitiy;

[Table("ausbildungsjahr")]
public class Ausbildungsjahr : Entity
{
    [Column("ausbildungsjahr_id")]
    [ID]
    [UsedImplicitly]
    public long ID { get; set; }
    
    [Column]
    public string Bezeichnung { get; set; }
}