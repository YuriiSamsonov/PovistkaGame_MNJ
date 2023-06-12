using System;
using _Game.Scripts.Systems.Player;
using UnityEngine;

public class Player
{
    private PlayerMono _mono;
    private Vector2 _desiredDirection;
    //public AudioSource Steps;
    private float _pointerDistance;
    private bool _isWaypointerActive;
    private bool _isBlueDoorOpen;
    private bool _nearTheKey;
    private bool _nearTheDoor;

    private Vector2 _lastMoveDirection;

    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int LastHorizontal = Animator.StringToHash("LastHorizontal");
    private static readonly int LastVertical = Animator.StringToHash("LastVertical");
    private static readonly int IsStay = Animator.StringToHash("IsStay");

    public event Event<EventArgs> PlayerFinishedLevelEvent, CutsceneEvent, StartLevelEvent;
    public event Event<InteractionEventArgs> PlayerNearTheKeyEvent, PlayerNearClosedDoorEvent;
    public Player(PlayerMono playerMono)
    {
        _mono = playerMono;
        _pointerDistance = playerMono.PointerDistance;
        _mono.Init(OnFinishLevelBoxTrigger, OnKeyPickUpBoxTrigger, OnDoorBoxTrigger, OnCutsceneBoxTrigger);
    }

    public void SetDesiredMoveDirection(Vector2 desiredDirection)
        => _desiredDirection = desiredDirection;

    public void Execute()
    {
        ConstructWaypointer();
    }

    private void ConstructWaypointer()
    {
        var pPos = _mono.transform.position;
        var dirToDestination = _mono.Destination.position - pPos;
        var distanceToDestinationCurrent = Vector3.Distance(pPos, _mono.Destination.transform.position);
        var rotation = Mathf.Rad2Deg * Mathf.Atan2(dirToDestination.y, dirToDestination.x);
        var distanceToHideWaypoint = 2;
        
        _mono.Waypointer.transform.localRotation = Quaternion.Euler(0,0,rotation);

        var clampedDistance = Mathf.Min(_pointerDistance, distanceToDestinationCurrent);
        _mono.Waypointer.transform.localPosition = dirToDestination.normalized * clampedDistance;

        if (distanceToDestinationCurrent < _pointerDistance)
        {
            var waypointerSize = (distanceToDestinationCurrent - distanceToHideWaypoint) / 4;

            if (waypointerSize > 0 && waypointerSize < 1)
                _mono.Waypointer.transform.localScale = new Vector3(waypointerSize, waypointerSize, waypointerSize);
            else if(waypointerSize > 1)
                _mono.Waypointer.transform.localScale = new Vector3(1, 1, 1);
            else
                _mono.Waypointer.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    public void ShowWaypointer()
    {
        if (!_isWaypointerActive && Input.GetKeyDown(KeyCode.F))
        {
            _mono.Waypointer.SetActive(true);
            _isWaypointerActive = true;
        }
        else if (_isWaypointerActive && Input.GetKeyDown(KeyCode.F))
        {
            _mono.Waypointer.SetActive(false);
            _isWaypointerActive = false;
        }
    }

    private void OnKeyPickUpBoxTrigger(bool state)
    {
        PlayerNearTheKeyEvent(new InteractionEventArgs(state));
        _nearTheKey = state;
    }

    public void ActivateKey()
    {
        if (_nearTheKey)
        {
            _mono.BlueKey.SetActive(false);
            _isBlueDoorOpen = true;
            _nearTheKey = false;
            PlayerNearTheKeyEvent(new InteractionEventArgs(false));
        }
    }

    private void OnDoorBoxTrigger(bool state)
    {
        _nearTheDoor = state;

        if (!_isBlueDoorOpen)
        {
            PlayerNearClosedDoorEvent(new InteractionEventArgs(state));
        }
    }

    public void TryToOpenDoor()
    {
        if (_isBlueDoorOpen && _nearTheDoor)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayCutscene();
            }
        }
    }
    
    private void PlayCutscene()
    {
        _mono.BlueDoor.SetActive(false);
    }
    
    
    public void CalculateMovement(float time)
    {
        if (_desiredDirection.magnitude > 0.01)
            _lastMoveDirection = _desiredDirection;
        
        var move = time * _mono.DesiredSpeed * _desiredDirection;
        
        _mono.RBody.MovePosition(_mono.RBody.position + move);
        MovementAnimation(_desiredDirection, _lastMoveDirection);

        _mono.VisionArea.position = _mono.transform.position;
    }

    private void MovementAnimation(Vector3 currentMovementDir, Vector3 lastMovementDir)
    {
        _mono.PlayerAnimator.SetFloat(Horizontal, currentMovementDir.x);
        _mono.PlayerAnimator.SetFloat(Vertical, currentMovementDir.y);
        _mono.PlayerAnimator.SetFloat(LastHorizontal, lastMovementDir.x);
        _mono.PlayerAnimator.SetFloat(LastVertical, lastMovementDir.y);

        if(_desiredDirection.magnitude > 0.01)
            _mono.PlayerAnimator.SetBool(IsStay, false);
        else
            _mono.PlayerAnimator.SetBool(IsStay, true);
    }

    private void Die(EventArgs _)
    {
        _mono.PlayerObject.SetActive(false);
    }

    private void OnFinishLevelBoxTrigger()
    {
        PlayerFinishedLevelEvent(EventArgs.Empty);
        _mono.PlayerObject.SetActive(false);
    }

    private void OnCutsceneBoxTrigger()
    {
        CutsceneEvent(EventArgs.Empty);
    }

    public void StartLevel()
    {
        StartLevelEvent(EventArgs.Empty);
    }

    public void SubscribeToEvents(EnemyManager enemyManager)
    {
        enemyManager.AttemptToLillPlayerEvent += Die;
    }
}