using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    //Strength for blunt dmg, marksmanship for rng dmg, dex for slice dmg,
    //luck for crit chance and sudden energy recharge chance, speed for energy recharge speed.
    //int strength, marksmanship, dexterity, luck, speed;
    Dictionary<StatType, int> fullStats;
    public virtual int this[StatType i] { get { return fullStats[i]; } }

    public Stats()
    {
        fullStats = new Dictionary<StatType, int>()
        {
            { StatType.LEVEL, 1 },
            { StatType.EXPERIENCE_LEFT, 0},
            { StatType.STRENGTH, 0},
            { StatType.DEXTERITY, 0},
            { StatType.MARKSMANSHIP, 0},
            { StatType.LUCK, 0},
            { StatType.SPEED, 0}
        };
    }

    //This builder is here mainly to create the buffs and debuffs. REPLACE WITH BUILDER
    public Stats SetLevel(int lvl) { fullStats[StatType.LEVEL] = lvl; return this; }
    public Stats SetExperience(int exp) { fullStats[StatType.EXPERIENCE_LEFT] = exp; return this; }
    public Stats SetStrength(int str) { fullStats[StatType.STRENGTH] = str; return this; }
    public Stats SetMarksmanship(int marks) { fullStats[StatType.MARKSMANSHIP] = marks; return this; }
    public Stats SetDexterity(int dex) { fullStats[StatType.DEXTERITY] = dex; return this; }
    public Stats SetLuck(int l) { fullStats[StatType.LUCK] = l; return this; }
    public Stats SetSpeed(int sp) { fullStats[StatType.SPEED] = sp; return this; }

    //#region Getters
    //public virtual int Level { get { return level; } }
    //public virtual int Experience { get { return experience; } }
    //public virtual int Strength { get { return strength; } }
    //public virtual int Marksmanship { get { return marksmanship; } }
    //public virtual int Dexterity { get { return dexterity; } }
    //public virtual int Luck { get { return luck; } }
    //public virtual int Speed { get { return speed; } }
    //#endregion
}

public enum StatType
{
    LEVEL,
    EXPERIENCE_LEFT,
    STRENGTH,
    DEXTERITY,
    MARKSMANSHIP,
    LUCK,
    SPEED
}
