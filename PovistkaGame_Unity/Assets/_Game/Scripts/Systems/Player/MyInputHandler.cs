using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using _Game.Scripts.Systems.Player;
using UnityEngine;

public class MyInputHandler
{
    private Player _player;
    private bool _allowMoving = true;

    public event Event<InteractionEventArgs> PlayerStepsEvent;

    public MyInputHandler(Player player) 
    {
        _player = player;
    }

    public void HandleKeyboardInput()
    {
        if (_allowMoving)
        {
            HandleMovement();
            HandleInteraction();
            _player.ShowWaypointer();
        }

        if (!_allowMoving)
        {
            _player.SetDesiredMoveDirection(Vector2.zero);
        }
    }

    private void HandleMovement()
    {
        Vector2 desiredMoveDir = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            desiredMoveDir += Vector2.up;

        if (Input.GetKey(KeyCode.S))
            desiredMoveDir += Vector2.down;

        if (Input.GetKey(KeyCode.A))
            desiredMoveDir += Vector2.left;

        if (Input.GetKey(KeyCode.D))
            desiredMoveDir += Vector2.right;

        _player.SetDesiredMoveDirection(desiredMoveDir.normalized);

        if (desiredMoveDir == Vector2.zero)
            PlayerStepsEvent(new InteractionEventArgs(false));
        else
            PlayerStepsEvent(new InteractionEventArgs(true));
    }

    private void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _player.ActivateKey();
            _player.TryToOpenDoor();
        }
    }

    public void OnPlayerStartCutsceneEventHandler(EventArgs _)
    {
        _allowMoving = false;
        PlayerStepsEvent(new InteractionEventArgs(false));
    }
}
