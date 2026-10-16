
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Models;

public class Event
{
    public int Event_ID { get; set; }

    public string Event_Name { get; set; } = "";

    [Column(TypeName = "date")]
    public DateTime Event_Date { get; set; }

    [Column("Locationn")]
    public string Location { get; set; } = "";

    [Column("Descriptionn")]
    public string? Description { get; set; }

    public int Organiser_ID { get; set; }

    public decimal Distance { get; set; }

    public string Event_Type { get; set; } = "";
}

using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Models;

public class Event
{
    public int Event_ID { get; set; }

    public string Event_Name { get; set; } = "";

    [Column(TypeName = "date")]
    public DateTime Event_Date { get; set; }

    [Column("Locationn")]
    public string Location { get; set; } = "";

    [Column("Descriptionn")]
    public string? Description { get; set; }

    public int Organiser_ID { get; set; }

    public decimal Distance { get; set; }

    public string Event_Type { get; set; } = "";
}


