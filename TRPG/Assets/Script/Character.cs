using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterData stats;
    bool _isDead;
    int _ownerId;
    int _charId;

    Vector3 _initialPosition;
    Vector3 _initialForward;
    Hitbox _hb;
    AttackingPosition _atPos;
    WeaponCollider _mainWeapon;

    public Hitbox Hitbox { get { return _hb; } }
    public AttackingPosition AttackingPosition { get { return _atPos; } }
    public WeaponCollider MainWeapon { get { return _mainWeapon; } }
    public bool IsDead
    {
        get { return _isDead; }
        private set { _isDead = value; }
    }

    void Start()
    {
        _hb = GetComponentInChildren<Hitbox>();
        _atPos = GetComponentInChildren<AttackingPosition>();
        _mainWeapon = GetComponentInChildren<WeaponCollider>();
    }

    public void Init(int owner, int id)
    {
        _ownerId = owner;
        _charId = id;
        AddEvents();
    }

    public void SetInitialPosition(Transform trn)
    {
        _initialPosition = trn.position;
        _initialForward = trn.right;
    }

    void AddEvents()
    {
        EventManager.AddEventListener(GameEvents.CharacterReachedTarget, OnReachedTarget);
    }

    void OnReachedTarget(object[] paramsContainer)
    {
        var sender = (Character)paramsContainer[0];

        if (sender == this)
        {
            var enemy = (Character)paramsContainer[1];
            enemy.TakeDamage(100);
            var target = enemy.GetComponentInChildren<AttackingPosition>();
            var lerpTime = .1f;
            StartCoroutine(LerpPosition(transform.position, target.transform.position, lerpTime));
            Invoke("Attack", lerpTime);
            Invoke("EndAttack", 1);
            Invoke("EndAssault", 1.1f + stats.moveTime);
        }
    }

    public void Execute(Character target)
    {
        _hb.Execute(target);
        Move(transform.position, target.AttackingPosition.transform.position);
    }

    void Attack()
    {

    }

    public void TakeDamage(int damage)
    {
        stats.hp -= damage;
        if (stats.hp <= 0)
        {
            IsDead = true;
            var graph = GetComponentsInChildren<Renderer>();
            foreach (var item in graph)
            {
                item.enabled = false;
            }
        }
    }

    void EndAttack()
    {
        Move(transform.position, _initialPosition);
    }

    void EndAssault()
    {
        transform.forward = _initialForward;
        EventManager.DispatchEvent(GameEvents.EndAssault, new object[] { this, _ownerId });
    }

    void Move(Vector3 from, Vector3 to)
    {
        StartCoroutine(LerpPosition(from, to, stats.moveTime));
    }

    IEnumerator LerpPosition(Vector3 startPos, Vector3 endPos, float maxTime)
    {
        var i = 0f;

        while (i <= 1)
        {
            i += Time.deltaTime / maxTime;
            transform.localPosition = Vector3.Lerp(startPos, endPos, i);
            yield return new WaitForEndOfFrame();
        }
    }
}
