using UnityEngine;

public class Entity : MonoBehaviour
{

    [SerializeField]
    private int maxHP;

    protected int hp;

    public virtual void Awake()
    {
        // initialise health point
        hp = maxHP;
    }
}
