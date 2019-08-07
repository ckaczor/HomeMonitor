using JetBrains.Annotations;

namespace ChrisKaczor.HomeMonitor.Weather.Models
{
    [PublicAPI]
    public enum WindDirection
    {
        None = -1,
        North = 0,
        East = 90,
        South = 180,
        West = 270,
        NorthEast = 45,
        SouthEast = 135,
        SouthWest = 225,
        NorthWest = 315,
        NorthNorthEast = 23,
        EastNorthEast = 68,
        EastSouthEast = 113,
        SouthSouthEast = 158,
        SouthSouthWest = 203,
        WestSouthWest = 248,
        WestNorthWest = 293,
        NorthNorthWest = 338
    }
}
