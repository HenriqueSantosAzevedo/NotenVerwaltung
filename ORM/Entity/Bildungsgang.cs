using System.ComponentModel;
using JetBrains.Annotations;
using WebApplication1.Attribute;
using WebApplication1.ORM.Wrapper;

namespace WebApplication1.Entitiy;

[Table("bildungsgang")]
public class Bildungsgang : Entity
{
    [Column("bildungsgang_id")]
    [ID]
    [UsedImplicitly]
    public long ID { get; set; }
    
    [Column("name")]
    public string bezeichnung { get; set; }
}