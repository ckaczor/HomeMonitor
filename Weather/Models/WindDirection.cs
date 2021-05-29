using JetBrains.Annotations;

namespace ChrisKaczor.HomeMonitor.Weather.Models
{
    [PublicAPI]
    public enum WindDirection
    {
        None,
        North,
        East,
        South,
        West,
        NorthEast,
        SouthEast,
        SouthWest,
        NorthWest,
        NorthNorthEast,
        EastNorthEast,
        EastSouthEast,
        SouthSouthEast,
        SouthSouthWest,
        WestSouthWest,
        WestNorthWest,
        NorthNorthWest
    }
}
