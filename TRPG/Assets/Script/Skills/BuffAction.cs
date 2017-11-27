using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffAction : SkillAction
{
    Buff buffToApply;
    public Buff BuffToApply { get { return buffToApply; } set { buffToApply = value; } }

    public override void OnExecuteAction(Player performer, Player target)
    {
        EventManager.DispatchEvent(EventID.PLAYER_BUFFED, new object[] { target, buffToApply });
    }
}
