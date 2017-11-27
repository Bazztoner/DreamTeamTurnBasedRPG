using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OffensiveAction : SkillAction
{
    List<int> dmgPercentage;
    List<StatType> scalesWith;
    public List<int> DmgPercentage { get { return dmgPercentage; } set { dmgPercentage = value; } }
    public List<StatType> ScalesWith { get { return scalesWith; } set { scalesWith = value; } }
    int calculatedDamage = 0;

    public override void OnExecuteAction(Player performer, Player target)
    {
        for (int i = 0; i < scalesWith.Count; i++)
        {
            calculatedDamage += performer.SC.CurrentStats[scalesWith[i]] * dmgPercentage[i] / 100;
        }
        EventManager.DispatchEvent(EventID.PLAYER_ATTACKED, new object[] { target, calculatedDamage });
    }

    //Add a function to preview damage. It would calculate the damage each time you change targets, so that it can easily be
    //shown on the GUI.
}
