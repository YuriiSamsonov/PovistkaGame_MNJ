using System;
using System.Net.NetworkInformation;
using UnityEngine;

public class PlayerMono : MonoBehaviour
{
    [field: SerializeField]
    private float desiredSpeed = 5.0f;
    public float DesiredSpeed => desiredSpeed;
    
    [field: SerializeField] 
    private float visionRadius = 6.0f;

    public float VisionRadius => visionRadius;

    [field: SerializeField]
    private Rigidbody2D rBody;
    public Rigidbody2D RBody => rBody;

    [field: SerializeField]
    private Collider2D col;
    public Collider2D Col => col;

    [field: SerializeField] 
    private Animator playerAnimator;
    public Animator PlayerAnimator => playerAnimator;

    [field: SerializeField]
    private GameObject playerObject;

    public GameObject PlayerObject => playerObject;
    
    [field: SerializeField] 
    private Transform visionArea;

    public Transform VisionArea => visionArea;
    
    [field: SerializeField] 
    private Transform destination;

    public Transform Destination => destination;
    
    [field: SerializeField] 
    private GameObject waypointer;

    public GameObject Waypointer => waypointer;
    
    [field: SerializeField] 
    private float pointerDistance;

    public float PointerDistance => pointerDistance;
    
    [field: SerializeField] 
    private GameObject blueKey;

    public GameObject BlueKey => blueKey;
    
    [field: SerializeField] 
    private GameObject blueDoor;

    public GameObject BlueDoor => blueDoor;


    private Action _onFinishCallback, _onCutsceneCallback;
    private Action<bool> _onBlueKeyCallback, _onBlueDoorCallback;

    public void Init(Action onFinishCallback, Action<bool> onBlueKeyCallback, 
        Action<bool> onBlueDoorCallback, Action onCutsceneCallback)
    {
        _onFinishCallback = onFinishCallback;
        _onBlueKeyCallback = onBlueKeyCallback;
        _onBlueDoorCallback = onBlueDoorCallback;
        _onCutsceneCallback = onCutsceneCallback;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case"Finish":
                _onFinishCallback();
                break;
            case"BlueKey":
                _onBlueKeyCallback(true);
                break;
            case "BlueDoor":
                _onBlueDoorCallback(true);
                break;
            case "Cutscene":
                _onCutsceneCallback();
                break;
        }
    }
    
    public void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case"BlueKey":
                _onBlueKeyCallback(false);
                break;
            case"BlueDoor":
                _onBlueDoorCallback(false);
                break;
        }
    }
}
