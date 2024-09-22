namespace NRules.Samples.JsonRules.Domain;

public class Position
{
    public string Currency { get; set; }
    public string AssetClass { get; set; }
    public int YearMaturityFrom { get; set; }
    public int YearMaturityTo { get; set; }
    public decimal Result { get; set; } 
}