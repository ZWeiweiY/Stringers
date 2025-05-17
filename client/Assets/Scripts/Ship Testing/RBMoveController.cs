using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

public class RBMoveController : MonoBehaviour
{
    public GameSettings gameSettings;
    public float maxSpeed = 10f; // Constant movement speed
    public float rotationSpeed = 3f; // Rotation smoothing factor
    public bool useController;
    public bool resetting;
    public Animator ghostAnimator;
    private TriggerAnimationEvent triggerAnimationEvent;
    public bool stopped;
    private Rigidbody rb;
    [SerializeField] private float rotationInput;

    public FaceDataSO faceData;
    [SerializeField] private float interpolationFactor = 0.1f;
    
    [Header("Dash")] 
    [SerializeField] private float dashStop = .5f;

    [Header("Shrink")] 
    public GameObject shrinkObject;
    [SerializeField] private float shrinkSpeed = 1f;
    [SerializeField] private float shrinkScale = .5f;
    public bool shrank { get; private set; }
    private Vector3 initialScale;
    [SerializeField] private bool shrinking;
    private float shrinkTimer;
    [SerializeField] private bool enlarging;
    private Vector3 shrinkTarget;

    public VisualEffect shirnkEffect;
    public VisualEffect enlargingEffect;
    public event Action DashActionTriggered;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 0f; // No natural slowing down
        rb.linearVelocity = transform.forward * maxSpeed;
        
        initialScale = shrinkObject.transform.localScale;
        shrinkTarget = initialScale * shrinkScale;

        shirnkEffect.enabled = false;
        enlargingEffect.enabled = false;

        if (ghostAnimator == null) ghostAnimator = GetComponentInChildren<Animator>();
        if (triggerAnimationEvent == null) triggerAnimationEvent = GetComponentInChildren<TriggerAnimationEvent>();
    }

    void FixedUpdate()
    {
        HandleInteraction();
        HandleMovement();
        HandleRotation();
        
    }

    void HandleMovement()
    {
        if (resetting)
        {
            //rb.linearVelocity = Vector3.Lerp(Vector3.zero, transform.forward * maxSpeed, resetSpeedTime);
            return;
        }

        if (stopped)
        {
            StopMovement();
        }
        else
        {
            rb.linearVelocity = transform.forward * maxSpeed;
        }
        // Ensure ship moves forward at max speed (except when braking)
        //rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }

    void HandleRotation()
    {
        if (useController)
        {
            rotationInput = Input.GetAxis("Horizontal");
            if (rotationInput != 0)
            {
                Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, rotationInput * rotationSpeed, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime);
            }

        }
        else
        {
            if (faceData.Roll != 0)
            {
                Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, faceData.Roll * rotationSpeed, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime);
            }
        }
    }


    public void StopMovement()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void HandleInteraction()
    {
        if (useController)
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    Debug.Log("Swallow");
            //    ghostAnimator.Play("Swallow");
            //}

            /*if (Input.GetKeyDown(KeyCode.LeftShift) && !stopped)
            {
                StartCoroutine(Dash());
            }*/

            if (Input.GetKeyDown(KeyCode.S) && !shrinking && !shrank)
            {
                Debug.Log("Shrinking");
                shrinking = true;
                shrinkTimer = 0;
                
            }
        }
        if (shrinking)
        {
            if (shrinkTimer >= shrinkSpeed)
            {
                shrank = true;
                shrinking = false;
                shrinkTimer = 0;
            }
            else
            {
                shrinkTimer += Time.deltaTime;
                shrinkObject.transform.localScale = 
                    Vector3.Lerp(initialScale, shrinkTarget, shrinkTimer/shrinkSpeed);
            }
        }

        if (shrank && enlarging)
        {
            if (shrinkTimer >= shrinkSpeed)
            {
                shrank = false;
                enlarging = false;
                shrinkTimer = 0;
            }
            else
            {
                shrinkTimer += Time.deltaTime;
                shrinkObject.transform.localScale =
                    Vector3.Lerp(shrinkTarget, initialScale, shrinkTimer / shrinkSpeed);
            }
        }
    }

    public IEnumerator Dash(Transform target)
    {
        Debug.Log("dash");
        stopped = true;
        SoundManager.Instance.PlayDashSound();
        transform.position = target.position;
        /*transform.position += transform.forward * dashDistance;*/
        DashActionTriggered?.Invoke();
        yield return new WaitForSeconds(dashStop);
        stopped = false;
    }

    public void LevelClear()
    {
        //stopped = true;
        ghostAnimator?.Play("Rotate&Shrink");
        SoundManager.Instance.PlayLevelFinishSound();
        //LevelManager.Instance.LoadScene(1);
    }
    public void Shrink()
    {
        shrinking = true;
        shrinkTimer = 0;
        shirnkEffect.enabled = true;
        shirnkEffect.Play();
        SoundManager.Instance.PlayShrinkSound();
    }

    public void Enlarge()
    {
        enlarging = true;
        shrinkTimer = 0;
        enlargingEffect.enabled = true;
        enlargingEffect.Play();
        SoundManager.Instance.PlayExpandSound();
    }

    public void SetSpeed(int i)
    {
        maxSpeed = gameSettings.GetLinearSpeed(i);
        rotationSpeed = gameSettings.GetAngularSpeed(i);
    }

}
