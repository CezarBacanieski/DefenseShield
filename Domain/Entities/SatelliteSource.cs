namespace DefenseShield.Domain.Entities;

public sealed class SatelliteSource : MonitoringSource
{
    public SatelliteSource(string name, string description)
        : base(name, description)
    {
    }

    public override string GetSourceType() => "Satellite";
}
