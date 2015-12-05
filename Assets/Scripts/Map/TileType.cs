using UnityEngine;
using System.Collections;

public enum TileType {Grass, Sand, Rock, Inpassable, WaterShallow, WaterDeep, None
}

public enum AttackType
{
    Meelee, AreaOfEffect, MeeleeReach, Ranged
}

public enum TurnState
{
    Start,Move,Attack,End, Undecided
}

public enum Team
{
    Red,Blue
}

public enum OverlayType
{
    Path, Attack, Empty
}

public enum ModeType
{
    StartZone, Target, ChangeHeight, ChangeTileType, Remove, None
}

public enum Facing
{
    Left, Right, Up, Down
}