using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine
{
    private readonly Animator _stateController;

    private static readonly int
        IdleHash = Animator.StringToHash("Idle"),
        PatrolHash = Animator.StringToHash("Patrol"),
        KillHash = Animator.StringToHash("Kill"),
        ChaseHash = Animator.StringToHash("Chase"),
        AlertHash = Animator.StringToHash("Alert"),
        SpotHash = Animator.StringToHash("Spot"),
        ReturnHash = Animator.StringToHash("Return");

    private static readonly int
        CurrentIdleDurationHash = Animator.StringToHash("CurrentIdleDuration"),
        CurrentAlertDurationHash = Animator.StringToHash("CurrentAlertDuration"),
        HasReachedPatrolPointHash = Animator.StringToHash("HasReachedPatrolPoint"),
        SeesPlayerHash = Animator.StringToHash("SeesPlayer");


    private static readonly Dictionary<int, EnemyStates> HashToState = new Dictionary<int, EnemyStates>()
        {
            {IdleHash, EnemyStates.Idle },
            {PatrolHash, EnemyStates.Patrol },
            {KillHash, EnemyStates.Kill},
            {ChaseHash, EnemyStates.Chase },
            {AlertHash, EnemyStates.Alert },
            {SpotHash, EnemyStates.Spot },
            {ReturnHash, EnemyStates.Return }
        };

    public EnemyStates CurrentState()
    {
        var currentHash = _stateController.GetCurrentAnimatorStateInfo(0).shortNameHash;
        return HashToState[currentHash];
    }

    public AIStateMachine(Animator stateController)
    {
        _stateController = stateController;
    }

    public void SetCurrentIdleDuration(float duration)
    {
        _stateController.SetFloat(CurrentIdleDurationHash, duration);
    }
    
    public void SetCurrentAlertDuration(float duration) => _stateController.SetFloat(CurrentAlertDurationHash, duration);

    public void SetReachedPatrolPointTrigger() => _stateController.SetTrigger(HasReachedPatrolPointHash);

    public void SetPlayerVisibility(bool isVisible) => _stateController.SetBool(SeesPlayerHash, isVisible);
}
