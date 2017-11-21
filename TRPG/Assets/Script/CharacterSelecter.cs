using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterSelecter : MonoBehaviour
{
	void Start ()
    {
        var chars = GameObject.FindObjectsOfType<Character>().ToList();
        var player1Spawns = GameObject.Find("Team1Spawns");
        var team1Spawns = player1Spawns.GetComponentsInChildren<Transform>().Where(x => x != player1Spawns.transform).ToArray();
        var player2Spawns = GameObject.Find("Team2Spawns");
        var team2Spawns = player2Spawns.GetComponentsInChildren<Transform>().Where(x => x != player2Spawns.transform).ToArray();

        Utilities.Utility.KnuthShuffle<Character>(chars);

        var player1Party = chars.Take(3).ToList();
        for (int i = 0; i < player1Party.Count(); i++)
        {
            player1Party[i].transform.localPosition = team1Spawns[i].position;
            player1Party[i].transform.forward = team1Spawns[i].right;
            player1Party[i].SetInitialPosition(team1Spawns[i]);
        }
        var player2Party = chars.Skip(3).ToList();
        for (int i = 0; i < player2Party.Count(); i++)
        {
            player2Party[i].transform.localPosition = team2Spawns[i].position;
            player2Party[i].transform.forward = team2Spawns[i].right;
            player2Party[i].SetInitialPosition(team2Spawns[i]);
        }

        GetComponent<TurnManager>().Init(new Tuple<List<Character>, List<Character>>(player1Party, player2Party));
    }
}
