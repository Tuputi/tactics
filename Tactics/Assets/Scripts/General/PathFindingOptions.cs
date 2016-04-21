using UnityEngine;
using System.Collections;

public class PathFindingOptions
{
    public static readonly PathFindingOptions Default = new PathFindingOptions { IgnoreHeight = false, IgnoreWater = false, IgnoreOccupied = false };
    public static readonly PathFindingOptions Flying = new PathFindingOptions { IgnoreHeight = true, IgnoreWater = true, IgnoreOccupied = true };

    public bool IgnoreWater { get; set; }
    public bool IgnoreHeight { get; set; }
    public bool IgnoreOccupied { get; set; }
}
