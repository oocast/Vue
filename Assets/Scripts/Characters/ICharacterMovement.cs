using UnityEngine;
using System.Collections;

interface ICharacterMovement {
    /// <summary>
    /// Get velocity of the moving character
    /// </summary>
    /// <returns></returns>
    Vector3 GetVelocity();
}
