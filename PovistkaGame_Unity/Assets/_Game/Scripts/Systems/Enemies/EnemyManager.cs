using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Linq;

public class EnemyManager
{
    private List<Enemy> _enemies;

    private float _visionRadius;
    
    public event Event<EventArgs> AttemptToLillPlayerEvent;
    public EnemyManager(PlayerMono playerMono)
    {
        _visionRadius = playerMono.VisionRadius;
        
        _enemies = new List<Enemy>();
    }

    public void FindAllEnemies()
    {
        var list = Object.FindObjectsOfType<EnemyMono>().ToList();

        for (int i = 0; i < list.Count; i++)
        {
            var enemy = new Enemy(list[i]);
            enemy.Init(TryToKillPlayer);
            _enemies.Add(enemy);
        }
    }
    
    public void FixedTick(float delta)
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            if (_enemies[i].VisibilityScale > 0)
                _enemies[i].SpriteRenderer.color = Color.white;
            else
                _enemies[i].SpriteRenderer.color = Color.clear;

            _enemies[i].FixedTick(delta);
        }
    }

    private void TryToKillPlayer()
    {
        AttemptToLillPlayerEvent(EventArgs.Empty);
    }
}
