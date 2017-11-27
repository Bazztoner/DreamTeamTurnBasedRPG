using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : Stats
{
    Stats stats;
    public Stats StoredStats { get { return stats; } set { stats = value; } }
    Stats buff;

    public Buff(Stats st, Stats statsToBuff)
    {
        stats = st;
        buff = statsToBuff;
    }

    public override int this[StatType i]
    {
        get
        {
            return stats[i] + buff[i];
        }
    }
    //public override int Strength { get { return Mathf.Max(0, stats.Strength + buff.Strength); } }
    //public override int Marksmanship { get { return Mathf.Max(0, stats.Marksmanship + buff.Marksmanship); } }
    //public override int Dexterity { get { return Mathf.Max(0, stats.Dexterity + buff.Dexterity); } }
    //public override int Luck { get { return Mathf.Max(0, stats.Luck + buff.Luck); } }
}
