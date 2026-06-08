namespace DefenseShield.Domain.Entities;

public sealed class SensorSource : MonitoringSource
{
    public SensorSource(string name, string description)
        : base(name, description)
    {
    }

    public override string GetSourceType() => "IoT Sensor";
}
