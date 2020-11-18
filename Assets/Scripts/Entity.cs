using UnityEngine;
using System;

public class Entity : MonoBehaviour
{

    [SerializeField]
    protected int maxHP;

    protected int hp;

    protected virtual void Awake()
    {
        // initialise health point
        hp = maxHP;
    }

    public static Action<int> ScoreEnemy = delegate { };

}
