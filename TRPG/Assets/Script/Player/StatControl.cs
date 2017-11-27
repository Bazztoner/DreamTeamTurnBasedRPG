using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatControl : MonoBehaviour
{
    Stats originalStats;
    Stats currentStats;
    public Stats CurrentStats { get { return currentStats; } }
    
    void Start ()
    {
        originalStats = new Stats().SetLevel(1).SetExperience(0).SetStrength(15).SetMarksmanship(12).SetDexterity(9).SetLuck(7).SetSpeed(14);
        currentStats = originalStats;
        EventManager.Subscribe(EventID.PLAYER_BUFFED, OnBuff);
    }
	
	void Update ()
    {
		
	}

    void OnBuff(params object[] info)
    {
        if (this == (StatControl)info[0])
        {
            currentStats = new Buff(currentStats, (Stats)info[1]);
            StartCoroutine(ResetStats((Buff)currentStats, (float)info[2]));
        }
    }

    IEnumerator ResetStats(Buff buff, float time)
    {
        var timeToClean = new WaitForSecondsRealtime(time);
        yield return timeToClean;

        Buff c = (Buff)currentStats;
        if (c == buff) currentStats = c.StoredStats;
        else
        {
            while (c.StoredStats != originalStats)
            {
                if (c.StoredStats == buff) c.StoredStats = ((Buff)c.StoredStats).StoredStats;
                else c = (Buff)c.StoredStats;
            }
        }
    }
}
