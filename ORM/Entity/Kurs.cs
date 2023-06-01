using JetBrains.Annotations;
using WebApplication1.Attribute;
using WebApplication1.Entitiy;

namespace WebApplication1.ORM.Entity;

[Table("kurs")]
public class Kurs : Wrapper.Entity
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
}