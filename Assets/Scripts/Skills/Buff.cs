using UnityEngine;
using System.Collections;

public enum BuffOperator
{
    ADD,
    MULTIPLY
}

public class Buff {
    public string buffName;
    public int buffIndex;
    public float duration;
    public BuffOperator buffOperator;
    public float health;
    public float damage;
    public float attackSpeed;
    public float moveSpeed;

}
