using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig
{
    public const int mapWidth = 32;
    public const int mapHeight = 18;

    public const int maxLevelIndex = 122;
    public static readonly BoundsInt allTilesBound = new BoundsInt(-16, -9, 0, 32, 18, 1);
}
