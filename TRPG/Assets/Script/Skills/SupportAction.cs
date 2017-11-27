using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SupportAction : SkillAction
{
    [Range(0, 100)]
    int healAmount;
    public int HealAmount { get { return healAmount; } set { healAmount = value; } }

    public override void OnExecuteAction(Player performer, Player target)
    {
        EventManager.DispatchEvent(EventID.PLAYER_HEALED, new object[] { target, healAmount });
    }
}
