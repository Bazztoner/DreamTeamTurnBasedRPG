using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatControl))]
public class Player : MonoBehaviour
{
    public int maxHP = 100;
    public int maxEnergy = 5;
    public float rechargeTime = 5;
    float timeLeftToRecharge;
    int hp;
    int energy;
    public int Energy { get { return energy; } set { energy = Mathf.Max(Mathf.Min(value, maxEnergy), 0); } }
    StatControl sC;
    public StatControl SC { get { return sC; } }

	void Start ()
    {
        sC = GetComponent<StatControl>();
        hp = maxHP;
        energy = maxEnergy;

        rechargeTime -= rechargeTime * sC.CurrentStats[StatType.SPEED] / 100;
        timeLeftToRecharge = rechargeTime;
	}
	
	void Update ()
    {
		if(energy < maxEnergy)
        {
            timeLeftToRecharge -= Time.deltaTime;
            if(timeLeftToRecharge <= 0)
            {
                float criticalRecharge = Random.Range(0,1);
                // Divide by 200: 100 for percentage,
                //multiplied by 2 to make sure that a 100 luck stat gives 50% chance of critical restoration
                if (criticalRecharge > sC.CurrentStats[StatType.LUCK] / 200) energy = Mathf.Min(maxEnergy, energy + 2);
                else energy++;
                timeLeftToRecharge = rechargeTime;
            }
        }
	}
}
