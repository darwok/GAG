using System;
using UnityEngine;

public static class EventBus
{
    public static Action<int, Vector2> OnPlayerDamaged; // (amount, hitPoint)
    public static Action<GameObject> OnEnemyKilled;     // enemy GO
}