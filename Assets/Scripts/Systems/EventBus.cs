using System;
using UnityEngine;

public static class EventBus
{
    public static Action<int, Vector2> OnPlayerDamaged; // (amount, hitPoint)
    public static Action<GameObject> OnEnemyKilled;     // enemy GO

    public static Action OnPlayerJump;
    public static Action OnPlayerAttack;
    public static Action OnPlayerDied;
    public static Action OnLevelWon;
    public static Action<Vector2> OnObstacleBroken; // hitPoint
}