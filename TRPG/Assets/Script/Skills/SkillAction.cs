using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class SkillAction
{
    public abstract void OnExecuteAction(Player performer, Player target);
}
