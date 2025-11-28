using System;
using UnityEngine;

public static class EventBus
{
    // Player
    public static Action OnPlayerJump;
    public static Action OnPlayerAttack;
    public static Action<int, Vector2> OnPlayerDamaged;   // (amount, hitPoint)
    public static Action OnPlayerDied;

    // Enemigos / objetos
    public static Action<GameObject> OnEnemyKilled;       // boss / enemigos normales
    public static Action<Vector2> OnObstacleBroken;       // breakables

    // Flujo de nivel
    public static Action OnLevelWon;                      // <-- el que usa Goal
}