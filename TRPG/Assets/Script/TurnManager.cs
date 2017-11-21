using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    Transform _selectorGraphic;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    PlayerTurn[] _players = new PlayerTurn[2];
    public int _playerIndex = 0;

    public void Init(Tuple<List<Character>, List<Character>> party)
    {
        _selectorGraphic = GameObject.Find("Selector").transform;
        _selectorGraphic.GetComponent<Renderer>().enabled = false;

        _players[0] = GameObject.Find("Player1").GetComponent<PlayerTurn>();
        _players[0].Init(0, party.Item1);

        _players[1] = GameObject.Find("Player2").GetComponent<PlayerTurn>();
        _players[1].Init(1, party.Item2);

        _players[0].GetInitialTarget();

        _selectorGraphic.GetComponent<Renderer>().enabled = true;
        EventManager.AddEventListener(GameEvents.ChangeTurn, OnChangeTurn);
        EventManager.AddEventListener(GameEvents.EndFight, OnEndFight);
        EventManager.DispatchEvent(GameEvents.StartTurn, new object[] { _playerIndex });
    }

    public Tuple<Character, int> GetRandomTarget(int indx)
    {
        var playerTarget = indx == 0 ? 1 : 0;

        var rnd = UnityEngine.Random.Range(0, _players[playerTarget].Party.Count);

        var tuple = new Tuple<Character, int>(_players[playerTarget].Party[rnd], rnd);

        _selectorGraphic.position = new Vector3
                                    (_players[playerTarget].Party[rnd].transform.position.x,
                                    _selectorGraphic.position.y,
                                    _players[playerTarget].Party[rnd].transform.position.z);

        return tuple;
    }

    public Tuple<Character, int> GetTarget(int indx, int target)
    {
        var playerTarget = indx == 0 ? 1 : 0;
        var available = _players[playerTarget].Party;

        if (!available.Any()) return null;

        if (available.Count() == 1 || target < 0)
        {
            target = 0;
        }
        else if (target >= available.Count())
        {
            target = available.Count() - 1;
        }

        var tuple = new Tuple<Character, int>(available[target], target);

        _selectorGraphic.position = new Vector3
                                    (available[target].transform.position.x,
                                    _selectorGraphic.position.y,
                                   available[target].transform.position.z);

        return tuple;
    }

    public Tuple<Character, int> GetNewTarget(int indx, int target, bool direction)
    {
        var playerTarget = indx == 0 ? 1 : 0;
        var available = _players[playerTarget].Party;
        int newTarget = target;

        if (!available.Any()) return null;

        if (direction) newTarget++;
        else newTarget--;

        if (available.Count() == 1)
        {
            newTarget = 0;
        }
        else
        {
            if (newTarget >= available.Count())
            {
                newTarget = 0;
            }
            else if (newTarget < 0)
            {
                newTarget = available.Count() - 1;
            }
        }

        var tuple = new Tuple<Character, int>(available[newTarget], newTarget);

        _selectorGraphic.position = new Vector3
                                    (available[newTarget].transform.position.x,
                                    _selectorGraphic.position.y,
                                    available[newTarget].transform.position.z);

        return tuple;
    }

    void OnChangeTurn(object[] paramsContainer)
    {
        var indx = (int)paramsContainer[0];

        _playerIndex = indx == 0 ? 1 : 0;

        _selectorGraphic.GetComponent<Renderer>().enabled = false;

        StartCoroutine(WaitForNextTurn(_playerIndex, 0.1f));
    }

    void OnEndFight(object[] paramsContainer)
    {
        print((PlayerTurn)paramsContainer[0] + " has lost the match ");
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    void PassToNextTurn(int index)
    {
        _selectorGraphic.GetComponent<Renderer>().enabled = true;
        EventManager.DispatchEvent(GameEvents.StartTurn, new object[] { index });
    }

    IEnumerator WaitForNextTurn(int i, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        PassToNextTurn(i);
    }
}
