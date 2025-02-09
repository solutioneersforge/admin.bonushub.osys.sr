namespace Data.Models;

public class AllowedAddsRange
{
    public int Id { get; set; }
    public string Slot { get; set; }
    public int MinimumAdds { get; set; }
    public int MaximumAdds { get; set; }
    public string User { get; set; }
}
