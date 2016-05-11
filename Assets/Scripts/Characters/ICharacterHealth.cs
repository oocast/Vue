using UnityEngine;
using System.Collections;

interface ICharacterHealth {

    void TakeDamage(int amount);
    /// <summary>
    /// Take damage, arrange
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="moveDistance"></param>
    /// <param name="staggerTime"></param>
    /// <param name="source"></param>
    void TakeDamage(int amount, float stunTime, float moveDistance, Transform source);
    void TakeHeal(int amount);

    int GetCurrentHealth();
}
