using WebApplication1.Attribute;
using WebApplication1.Entitiy;

namespace WebApplication1.ORM.Entity;

[Table("leistung")]
public class Leistung : Wrapper.Entity
{
    [Column("leistungId")]
    [ID]
    public long ID { get; set; }

    [Column]
    public DateTime Datum { get; set; }

    [Column]
    public String Notiz { get; set; }
    
    [OneToOne("note_id")]
    public Note Note { get; set; }

    [OneToOne("schueler_id")]
    public Schueler Schueler { get; set; }
    
    [OneToOne("ausbildungsjahr_id")]
    public Ausbildungsjahr Ausbildungsjahr { get; set; }
    
    [OneToOne("note_art_id", "noten_art_id")]
    public NotenArt NotenArt { get; set; }
}
