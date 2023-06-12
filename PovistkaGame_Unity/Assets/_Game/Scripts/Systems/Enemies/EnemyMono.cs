using System.Collections;
using UnityEngine;

public class EnemyMono : MonoBehaviour
{
    [field: SerializeField]
    private float speed = 5f;
    
    public float Speed => speed;

    [field: SerializeField]
    private float castingDistance = 2f;
    
    public float CastingDistance => castingDistance;

    [field: SerializeField]
    private LayerMask layerToCastPlayer;
    
    public LayerMask LayerToCastPlayer => layerToCastPlayer;
    
    [field: SerializeField] 
    private LayerMask layerToCastWall;

    public LayerMask LayerToCastWall => layerToCastWall;

    [field: SerializeField]
    private PatrolRoute patrolRoute;
    
    public PatrolRoute PatrolRoute => patrolRoute;

    [field: SerializeField, Range(1, 179), Tooltip("A cone of vision for this enemy")]
    private float fieldOfVision = 45;
    
    public float FieldOfVision => fieldOfVision;
    
    [field: SerializeField, Tooltip("State machine for the ai of the enemy")]
    private Animator stateMachine;
    
    public Animator StateMachine => stateMachine;
    
    [field: SerializeField] 
    private Animator moveAnimator;

    public Animator MoveAnimator => moveAnimator;

    [field: SerializeField] 
    private SpriteRenderer sRenderer;

    public SpriteRenderer SRenderer => sRenderer;

    [field: SerializeField]
    private Transform enemyTransform;

    [field: SerializeField] 
    private GameObject allertSymbol;

    public Vector2 EnemyWorld2DPos => enemyTransform.position;

    private byte _visibilityScale;

    public byte VisibilityScale => _visibilityScale;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case"StreetLight":
                _visibilityScale ++;
                break;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case"StreetLight":
                _visibilityScale --;
                break;
        }
    }
    
    public void ShowAlertSymbol()
    {
        allertSymbol.SetActive(true);
        StartCoroutine(ShowAlertSymbolForSeconds());
    }

    private IEnumerator ShowAlertSymbolForSeconds()
    {
        yield return new WaitForSecondsRealtime(1);
        allertSymbol.SetActive(false);
    }
} 