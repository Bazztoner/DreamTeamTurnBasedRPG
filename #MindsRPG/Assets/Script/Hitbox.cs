using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    Character _target;
    bool _isActive;

    public void Execute(Character target)
    {
        _target = target;
        _isActive = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (_isActive)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("AttackingPosition"))
            {
                var enemy = other.GetComponentInParent<Character>();
                if (enemy != null && enemy == _target)
                {
                    ReachTarget(enemy);
                }
            }
        }
    }

    void ReachTarget(Character enemy)
    {
        _isActive = false;
        EventManager.DispatchEvent(GameEvents.CharacterReachedTarget, new object[] { GetComponentInParent<Character>(), enemy });
    }
}
