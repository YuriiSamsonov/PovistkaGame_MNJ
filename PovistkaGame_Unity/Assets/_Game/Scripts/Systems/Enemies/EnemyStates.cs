using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum EnemyStates
    {
        Idle, //the enemy in this state should just stay in one place, and continue looking in the direction he is facing
        Patrol, //the enemy moves from his previous point to the next
        Alert, //when an enemy spots the player, he will idle for (x) seconds before giving chase
        Kill,
        Chase, //the enemy moves to the last position he saw the player at
        Spot,
        Return, //in this state the enemy will return to the last point from which he started to chasing the player
    }

