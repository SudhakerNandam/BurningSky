using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    float GetHealth();
    void TakeDamage(float damageAmount);
    void SetHealth();
}
