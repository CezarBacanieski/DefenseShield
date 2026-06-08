using System.Globalization;
using System.Text.Json.Serialization;
using DefenseShield.Domain.Exceptions;

namespace DefenseShield.Domain.ValueObjects;

public readonly struct GeoCoordinate
{
    [JsonConstructor]
    public GeoCoordinate(double latitude, double longitude)
    {
        if (latitude is < -90 or > 90)
        {
            throw new InvalidAlertDataException("Latitude deve estar entre -90 e 90.");
        }

        if (longitude is < -180 or > 180)
        {
            throw new InvalidAlertDataException("Longitude deve estar entre -180 e 180.");
        }

        Latitude = latitude;
        Longitude = longitude;
    }

    public double Latitude { get; }

    public double Longitude { get; }

    public override string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "{0:F4}, {1:F4}", Latitude, Longitude);
    }
}
