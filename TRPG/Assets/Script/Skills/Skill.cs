using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Assets/Assistants/Skills/New Skill", menuName = "Game assets/Skill")]
public class Skill : ScriptableObject
{
    //MAKE A ***SkillAction*** SCRIPT, AND A NODE-BASED EDITOR WINDOW TO CREATE NEW SKILLS
    //AND ADD THEM SKILLACTIONS TO PERFORM
    int cost;
    public int Cost { get { return cost; } set { cost = value; } }

    bool selfOnly;
    public bool SelfOnly { get { return selfOnly; } set { selfOnly = value; } }

    bool includeSelf;
    public bool IncludeSelf { get { return includeSelf; } set { includeSelf = value; } }
    
    SkillAction[] actions;
    public SkillAction[] Actions { get { return actions; } set { actions = value; } }

    Player performer;
    Player[] targets;
    int targetID;

    public void OnActivate(Player performer)
    {
        //Si tiene target usa la query que se le diga, sino marca a self nomas
        if(selfOnly)
        {
            this.performer = performer;
            targets = new Player[] { performer };
        }
        else
        {
            //Query including self or not.

        }
    }

    public void Act()
    {
        //Hace cada acción necesaria
        //Debería pedir como parámetro los stats del jugador, y pasarlos a cada función necesaria.
        foreach (SkillAction act in actions)
        {
            act.OnExecuteAction(performer, targets[targetID]);
        }
    }

    public void OnDeactivate()
    {
        performer = null;
        targets = null;
    }
}
