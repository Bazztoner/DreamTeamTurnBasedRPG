using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerTurn : MonoBehaviour
{
    bool _myTurn;
    List<Character> _allChars;
    List<Character> _availableChars;
    Queue<Character> _turns = new Queue<Character>();
    Character _actualChar;
    Character _actualTarget;
    string _playerName;
    int _playerId;
    int _targetId;
    bool _executing;

    public List<Character> Party
    {
        get { return _availableChars.Where(x => !x.IsDead).ToList(); }
    }

    public int PlayerID
    {
        get { return _playerId; }
        set { _playerId = value; }
    }

    public int TargetID
    {
        get { return _targetId; }
        private set { _targetId = value; }
    }

    public Character ActualTarget
    {
        get { return _actualTarget; }
        private set { _actualTarget = value; }
    }

    public void Init(int id, List<Character> characters)
    {
        PlayerID = id;
        EventManager.AddEventListener(GameEvents.StartTurn, OnStartTurn);
        EventManager.AddEventListener(GameEvents.EndAssault, OnEndAssault);
        _allChars = characters;
        _availableChars = characters;

        for (int i = 0; i < _availableChars.Count; i++)
        {
            _turns.Enqueue(_availableChars[i]);
            _availableChars[i].Init(PlayerID, i);
        }
    }

    public void GetInitialTarget()
    {
        var targetTuple = TurnManager.instance.GetTarget(PlayerID, 0);
        ActualTarget = targetTuple.Item1;
        TargetID = targetTuple.Item2;
    }

    void ChangeTarget(bool direction)
    {
        var targetTuple = TurnManager.instance.GetNewTarget(PlayerID, TargetID, direction);
        ActualTarget = targetTuple.Item1;
        TargetID = targetTuple.Item2;
    }

    void OnEndAssault(object[] paramsContainer)
    {
        if ((int)paramsContainer[1] == PlayerID)
        {
            EndTurn();
        }
    }

    void OnStartTurn(object[] paramsContainer)
    {
        if ((int)paramsContainer[0] == PlayerID)
        {
            _myTurn = true;

            CheckAvailableChars();

            CheckState();
            
            var tuple = TurnManager.instance.GetTarget(PlayerID, TargetID);
            ActualTarget = tuple.Item1;
            TargetID = tuple.Item2;
        }
    }

    void CheckAvailableChars()
    {
        var qiu = new Queue<Character>();

        while (_turns.Any())
        {
            var temp = _turns.Dequeue();
            if (!temp.IsDead)
            {
                qiu.Enqueue(temp);
            }
        }

        _turns = qiu;
    }

    void CheckState()
    {
        if (!_turns.Any())
        {
            EventManager.DispatchEvent(GameEvents.EndFight, this);
            return;
        }

        _actualChar = _turns.Dequeue();
    }

    void Update()
    {
        if (_myTurn && !_executing)
        {
            CheckInput();
        }
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _actualChar.Execute(ActualTarget);
            _executing = true;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeTarget(false);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeTarget(true);
        }
    }

    void EndTurn()
    {
        _myTurn = false;
        _executing = false;
        _turns.Enqueue(_actualChar);
        _actualChar = null;
        EventManager.DispatchEvent(GameEvents.ChangeTurn, new object[] { PlayerID });
    }
}
