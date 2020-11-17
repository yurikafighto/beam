using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Entity
{

    public int GetMaxHP()
    {
        return maxHP;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public static Action<int> OnBossHPChange = delegate { };
    public static Action<bool> OnBossAppear = delegate { };

}
