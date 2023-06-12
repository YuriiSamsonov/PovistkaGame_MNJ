using System;
using UnityEngine;

public class Enemy
{
    private RaycastHit2D[] _results = new RaycastHit2D[1];
    private Transform _enemyTransform;
    private EnemyMono _mono;
    private AIStateMachine _stateMachine;
    private Animator _moveAnimator;
    public SpriteRenderer SpriteRenderer;

    private int _currentPatrolPointIndex;
    private bool _isMoving;

    public int VisibilityScale => _mono.VisibilityScale;

    private Vector2[] _patrolPoints;
    private Vector3 _castingOffset = new Vector3(0, 1, 0);

    private float _currentIdleDuration, _currentAlertDuration, _currentSpotDuration;

    private Vector2 _lastSeenPlayerPos = Vector2.zero, _lastAlertOrigin = Vector2.zero;

    private const float RayPrecisionInDegrees = 4.0f;
    private const float RayPrecisionInRadius = RayPrecisionInDegrees * Mathf.Deg2Rad;

    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    
    private Action _onDeathCallback;

    private int NextWaypoint =>
        _currentPatrolPointIndex < _patrolPoints.Length - 1
        ? _currentPatrolPointIndex + 1
        : 0;

    private Vector2 NextWaypointPosition => 
        _currentPatrolPointIndex < _patrolPoints.Length - 1
        ? _patrolPoints[_currentPatrolPointIndex + 1]
        : _patrolPoints[0];

    public void Init(Action onDeathCallback)
    {
        _onDeathCallback = onDeathCallback;
    }
    
    public Enemy(EnemyMono enemyMono)
    {
        _mono = enemyMono;
        _stateMachine = new AIStateMachine(_mono.StateMachine);

        _moveAnimator = enemyMono.MoveAnimator; 
        SpriteRenderer = enemyMono.SRenderer;
        _enemyTransform = enemyMono.transform;
        _patrolPoints = _mono.PatrolRoute.GetWaypoints();

        _currentPatrolPointIndex = 0;
        float minDistance = Vector2.Distance(_patrolPoints[0], _mono.transform.position);
        
        for (int i = 1; i < _patrolPoints.Length; i++)
        {
            var distance = Vector2.Distance(_patrolPoints[i], _mono.transform.position);
            
            if (distance < minDistance)
            {
                minDistance = distance;
                _currentPatrolPointIndex = i;
            }
        }
        _mono.transform.position = _patrolPoints[_currentPatrolPointIndex];
    }

    public void FixedTick(float delta)
    {
        Execute(delta);
    }

    private void Execute(float delta)
    {
        _isMoving = false;
        
        var currentState = _stateMachine.CurrentState();

        if (currentState != EnemyStates.Idle)
            _currentIdleDuration = 0;

        if(currentState != EnemyStates.Alert)
            _currentAlertDuration = 0;

        if(currentState != EnemyStates.Spot)
            _currentSpotDuration = 0;


        var castDir = Vector2.zero;
        switch (_stateMachine.CurrentState())
        {
            case EnemyStates.Idle:
                castDir = NextWaypointPosition - _mono.EnemyWorld2DPos;
                _currentIdleDuration += delta;
                _stateMachine.SetCurrentIdleDuration(_currentIdleDuration);
                break;
            case EnemyStates.Patrol:
                castDir = NextWaypointPosition - _mono.EnemyWorld2DPos;
                Move(NextWaypointPosition, _mono.Speed, delta, ReachedPatrolPoint);
                break;
            case EnemyStates.Alert:
                castDir = _lastSeenPlayerPos - _mono.EnemyWorld2DPos;
                _isMoving = false;
                _currentAlertDuration += delta;
                _mono.ShowAlertSymbol();
                _stateMachine.SetCurrentAlertDuration(_currentAlertDuration);
                break;
            case EnemyStates.Kill:
                _onDeathCallback();
                break;
            default:
                throw new Exception();
        }
        
        ResolveMovementAnimation(castDir);
        CastRay(castDir);
    }

    private void ReachedPatrolPoint()
    {
         _currentPatrolPointIndex = NextWaypoint;
         _stateMachine.SetReachedPatrolPointTrigger();
    }

    private void Move(Vector2 targetPos, float speed, float delta, Action onTriggerReachedCallback)
    {
        _isMoving = true;
        
        var enemy2DPos = (Vector2)_mono.transform.position;
        var direction = targetPos - enemy2DPos;
        
        _mono.transform.Translate(direction.normalized * (delta * speed));

        const float distanceTolerance = 0.05f;
        if (Vector2.Distance(enemy2DPos, targetPos) < distanceTolerance)
        {
            onTriggerReachedCallback();
        }
    }
    
    private void ResolveMovementAnimation(Vector3 movementDir)
    {
        var moveDir = movementDir.normalized;
        
        _moveAnimator.SetBool(IsMoving, _isMoving);

        if (Mathf.Abs(moveDir.x) > Mathf.Abs(moveDir.y))
        {
            _moveAnimator.SetFloat(Horizontal, moveDir.x);
            _moveAnimator.SetFloat(Vertical, 0);
        }

        if (Mathf.Abs(moveDir.x) < Mathf.Abs(moveDir.y))
        {
            _moveAnimator.SetFloat(Horizontal, 0);
            _moveAnimator.SetFloat(Vertical, moveDir.y);
        }
    }

    private bool TryCastForPlayer(Vector3 castDir, float distance)
    {
        var castingPos = _enemyTransform.position + _castingOffset;
        var hits = Physics2D.RaycastNonAlloc(castingPos,
            castDir,
            _results,
            distance,
            _mono.LayerToCastWall.value + _mono.LayerToCastPlayer.value);
        
        
        var hitPlayer = hits > 0 && 1 << _results[0].transform.gameObject.layer == _mono.LayerToCastPlayer.value;
        
        Debug.DrawRay(castingPos, castDir * distance,
            hitPlayer ? Color.green : Color.red, .03f);
        
        return hitPlayer;
    }

    public void CastRay(Vector2 castDir)
    {
        var rayDir = castDir.normalized * _mono.CastingDistance;
        float initialAngleRads = Mathf.Atan2(rayDir.y, rayDir.x);

        var halfCone =_mono.FieldOfVision / 2.0f;

        int raysToShoot = Mathf.CeilToInt(halfCone / RayPrecisionInDegrees);

        var foundPlayer = false;
        for(int i = 0; i < raysToShoot; i++)
        {
            float newAngleRadsPos = RayPrecisionInRadius * i + initialAngleRads;
            float x1 = Mathf.Cos(newAngleRadsPos);
            float y1 = Mathf.Sin(newAngleRadsPos);
            Vector2 posDir = new Vector2(x1, y1);
            foundPlayer = TryCastForPlayer(posDir, _mono.CastingDistance);
            if (foundPlayer)
                break;

            float newAngleRadsNeg = RayPrecisionInRadius * -i + initialAngleRads;
            float x2 = Mathf.Cos(newAngleRadsNeg);
            float y2 = Mathf.Sin(newAngleRadsNeg);
            Vector2 negDir = new Vector2(x2, y2);
            foundPlayer = TryCastForPlayer(negDir, _mono.CastingDistance);
            if(foundPlayer)
                break;
        }

        if (foundPlayer)
            _lastSeenPlayerPos = _results[0].transform.position;
        
        _stateMachine.SetPlayerVisibility(foundPlayer);
    }

}
