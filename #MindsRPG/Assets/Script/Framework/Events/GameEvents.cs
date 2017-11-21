using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents
{
    /// <summary>
    /// Turn ID
    /// </summary>
    public const string ChangeTurn = "ChangeTurn";
    /// <summary>
    /// Turn ID
    /// </summary>
    public const string StartTurn = "StartTurn";

    /// <summary>
    /// 0 - Loser (PlayerTurn)
    /// </summary>
    public const string EndFight = "EndFight";

    /// <summary>
    ///  0 - Character (Character)
    ///  1 - Target (Character)
    ///  2 - PlayerID (int)
    /// </summary>
    public const string CharacterReachedTarget = "CharacterReachedTarget";
    /// <summary>
    /// 0 - Character (Character)
    /// 1 - PlayerID (int)
    /// </summary>
    public const string EndAssault = "EndAssault";
    public const string WeaponHit = "CharacterReachedTarget";
}
