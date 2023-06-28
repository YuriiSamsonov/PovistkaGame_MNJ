using _Game.Scripts.Systems.Player;
using _Game.Scripts.Utils;
using UnityEngine;

namespace _Game.Scripts.PovistkaGame
{
    public class Core : MonoBehaviour
    {
        #region SceneReferences

    [field: SerializeField]
    private PlayerMono playerMono;

    [field: SerializeField]
    private Camera mainCamera;
    
    [field: SerializeField] 
    private UIManager uiManager;

    [field: SerializeField] 
    private SceneLoader sceneLoader;

    [field: SerializeField] 
    private EndGameCutscene cutscene;
    
    [field: SerializeField] 
    private SoundManager soundManager;
    
    #endregion

    
    #region ScriptableObjects

    [field: SerializeField]
    private CameraSettings cameraSettings;
    
    #endregion


    #region Variables

    private CameraController _cameraController;
    private Player _player;
    private MyInputHandler _handleInput;
    private EnemyManager _enemyManager;
    private InteractionEventArgs _interactionEventArgs;

    #endregion


    #region UnityEvemtFunctions

    private void Awake()
    {
        ConstructComponents();
        EstablishEventConnections();
    }

    private void Start()
    {
        _cameraController.SetTarget(playerMono.transform);
        _enemyManager.FindAllEnemies();
        _player.StartLevel();
    }

    private void Update()
    {
        _cameraController.UpdateCamera();

        _handleInput.HandleKeyboardInput();
        _player.Execute();
    }

    private void FixedUpdate()
    {
        var fDT = Time.deltaTime;

        _player.CalculateMovement(fDT);
        _enemyManager.FixedTick(fDT);
    }

    #endregion


    #region Components Initialization

    private void ConstructComponents()
    {
        _cameraController = new CameraController(cameraSettings, mainCamera);
        _player = new Player(playerMono);
        _handleInput = new MyInputHandler(_player);
        _enemyManager = new EnemyManager(playerMono);
        _interactionEventArgs = new InteractionEventArgs(false);
    }

    private void EstablishEventConnections()
    {
        _player.PlayerFinishedLevelEvent += sceneLoader.OnPlayerFinishedLevelEventHandler;
        _player.CutsceneEvent += cutscene.OnPlayerStartCutscene;
        _player.CutsceneEvent += _handleInput.OnPlayerStartCutsceneEventHandler;
        _handleInput.PlayerStepsEvent += soundManager.StepsSoundEventHandler;

        uiManager.SubscribeToEvents(_player, 
            _enemyManager);
        _player.SubscribeToEvents(_enemyManager);
    }

    #endregion
}
}
