using UnityEngine;

public class Shrinker : MonoBehaviour
{
    public GameSettings gameSettings;
    [SerializeField] private GhostFaceController ghostFace;
    private RBMoveController rbMoveController;

    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rbMoveController = GetComponent<RBMoveController>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        ExpressionTrigger trigger = other.GetComponent<ExpressionTrigger>();
        if (trigger == null) return;

        if(gameSettings.autoExpression 
           && trigger.inTrigger 
           && ghostFace.expression.shrink
           || (ghostFace.IsMouthRoll &&
            ghostFace.expression.shrink &&
            trigger.inTrigger))
        {
            trigger.MarkTriggered();
            rbMoveController.Shrink();
        }

        if(trigger.expression.enlarge 
            && trigger.inTrigger 
            && rbMoveController.shrank)
        {
            trigger.MarkTriggered();
            rbMoveController.Enlarge();
        }
    }

}
