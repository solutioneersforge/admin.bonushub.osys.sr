using System;

namespace Data.Models;

public class Changelog
{
    public int Id { get; set; }
    public string TableName { get; set; }
    public string Operation { get; set; }
    public string ChangedBy { get; set; }
    public DateTime? ChangeDate { get; set; }
    public int RecordId { get; set; }
    public string OldValues { get; set; }
    public string NewValues { get; set; }
}
