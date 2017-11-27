using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatDecorator : Stats
{
    protected Stats stats;

    public StatDecorator(Stats st)
    {
        stats = st;
    }
}
