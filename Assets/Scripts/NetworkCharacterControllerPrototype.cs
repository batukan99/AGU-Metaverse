using System;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[OrderBefore(typeof(NetworkTransform))]
[DisallowMultipleComponent]
// ReSharper disable once CheckNamespace
public class NetworkCharacterControllerPrototype : NetworkTransform {

  [SerializeField] private Animator animator;

  [Header("Character Controller Settings")]
  public float gravity       = 20.0f;
  public float flyVerticalSpeed = 20.0f;
  public float flyHorizontalSpeed = 40.0f;
  public float flyAccel = 20.0f;
  public float flyFastAccel = 40.0f;
  public float flyFastSpeed = 70.0f;
  public float jumpImpulse   = 8.0f;
  public float acceleration  = 10.0f;
  public float maxSpeed      = 2.0f;
  public float runningAcceleration  = 20.0f;
  public float runningMaxSpeed = 4.0f;
  public float rotationSpeed = 15.0f;

[Networked]
  private Boolean isRunningState {get; set;}
  [Networked]
  private Boolean isWalkingState {get; set;}
  [Networked]
  private Boolean isIdleState {get; set;}

  [Networked]
  [HideInInspector]
  public bool IsGrounded { get; set; }

  [Networked]
  [HideInInspector]
  public Vector3 Velocity { get; set; }

  /// <summary>
  /// Sets the default teleport interpolation velocity to be the CC's current velocity.
  /// For more details on how this field is used, see <see cref="NetworkTransform.TeleportToPosition"/>.
  /// </summary>
  protected override Vector3 DefaultTeleportInterpolationVelocity => Velocity;

  /// <summary>
  /// Sets the default teleport interpolation angular velocity to be the CC's rotation speed on the Z axis.
  /// For more details on how this field is used, see <see cref="NetworkTransform.TeleportToRotation"/>.
  /// </summary>
  protected override Vector3 DefaultTeleportInterpolationAngularVelocity => new Vector3(0f, 0f, rotationSpeed);

  public CharacterController Controller { get; private set; }
  public static NetworkCharacterControllerPrototype localPlayer {get; set;}
  [SerializeField] private GameObject PlayerModel;

  [Networked]
  private bool velocityCheck {get; set;}
  [Networked]
  private bool isRun {get; set;}
  [Networked]
  public NetworkString<_16> currentEmote {get; set;}

  public bool moveLock = false;
  
  protected override void Awake() {
    base.Awake();
    CacheController();
  }

  public override void Spawned() {
    base.Spawned();
    CacheController();
    isIdleState = true;

  }

  private void CacheController() {
    if (Controller == null) {
      Controller = GetComponent<CharacterController>();

      Assert.Check(Controller != null, $"An object with {nameof(NetworkCharacterControllerPrototype)} must also have a {nameof(CharacterController)} component.");
    }
  }

  protected override void CopyFromBufferToEngine() {
    // Trick: CC must be disabled before resetting the transform state
    Controller.enabled = false;

    // Pull base (NetworkTransform) state from networked data buffer
    base.CopyFromBufferToEngine();

    // Re-enable CC
    Controller.enabled = true;
  }

  /// <summary>
  /// Basic implementation of a jump impulse (immediately integrates a vertical component to Velocity).
  /// <param name="ignoreGrounded">Jump even if not in a grounded state.</param>
  /// <param name="overrideImpulse">Optional field to override the jump impulse. If null, <see cref="jumpImpulse"/> is used.</param>
  /// </summary>
  public virtual void Jump(bool ignoreGrounded = false, float? overrideImpulse = null) {
    if (IsGrounded || ignoreGrounded) {
      var newVel = Velocity;
      newVel.y += overrideImpulse ?? jumpImpulse;
      Velocity =  newVel;
    }
  }

  /// <summary>
  /// Basic implementation of a character controller's movement function based on an intended direction.
  /// <param name="direction">Intended movement direction, subject to movement query, acceleration and max speed values.</param>
  /// </summary>
  public virtual void Move(Vector3 direction, bool isRunning) 
  {
    if (moveLock)
    {
      if(!isIdleState) 
      {
        isIdleState = true;
        isRunningState = false;
        isWalkingState = false;
        animator.ResetTrigger("Walk");
        animator.ResetTrigger("Run");
        animator.SetTrigger("Idle");
      }

      return;
    }
    
    var deltaTime    = Runner.DeltaTime;
    var previousPos  = transform.position;
    var moveVelocity = Velocity;

    direction = direction.normalized;

    if (IsGrounded && moveVelocity.y < 0) {
      moveVelocity.y = 0f;
    }

    moveVelocity.y -= gravity * deltaTime;

    var horizontalVel = default(Vector3);
    horizontalVel.x = moveVelocity.x;
    horizontalVel.z = moveVelocity.z;
    
    if (direction == default) {
      horizontalVel = Vector3.Lerp(horizontalVel, default, Mathf.Infinity * deltaTime);
    } else if (!isRunning) {
      horizontalVel = Vector3.ClampMagnitude(horizontalVel + direction * acceleration * deltaTime, maxSpeed);
    } else {
      horizontalVel = Vector3.ClampMagnitude(horizontalVel + direction * runningAcceleration * deltaTime, runningMaxSpeed);
    }

    moveVelocity.x = horizontalVel.x;
    moveVelocity.z = horizontalVel.z;

    Controller.Move(moveVelocity * deltaTime);

    Velocity   = (transform.position - previousPos) * Runner.Simulation.Config.TickRate;
    IsGrounded = Controller.isGrounded;

    if (horizontalVel.magnitude > 0) {
      velocityCheck = true;
    } else {
      velocityCheck = false;
      }
      isRun = isRunning;
    }

    public virtual void Fly(Vector3 direction, bool isRunning) {
    var deltaTime    = Runner.DeltaTime;
    var previousPos  = transform.position;
    var moveVelocity = Velocity;

    direction = direction.normalized;

    var horizontalVel = default(Vector3);
    horizontalVel.x = moveVelocity.x;
    horizontalVel.z = moveVelocity.z;
    
    if (direction == default) {
      horizontalVel = Vector3.Lerp(horizontalVel, default, Mathf.Infinity * deltaTime);
    } else if (!isRunning) {
      horizontalVel = Vector3.ClampMagnitude(horizontalVel + direction * flyAccel * deltaTime, flyHorizontalSpeed);
    } else {
      horizontalVel = Vector3.ClampMagnitude(horizontalVel + direction * flyFastAccel * deltaTime, flyFastSpeed);
    }

    moveVelocity.x = horizontalVel.x;
    moveVelocity.z = horizontalVel.z;

    Controller.Move(moveVelocity * deltaTime);

    Velocity   = (transform.position - previousPos) * Runner.Simulation.Config.TickRate;
    IsGrounded = Controller.isGrounded;

    if (horizontalVel.magnitude > 0) {
      velocityCheck = true;
    } else {
      velocityCheck = false;
      }
      isRun = isRunning;
    }

    public void FlyUp() { //fly upwards via applying velocity towards up
      var newVel = Velocity;
      newVel.y = flyVerticalSpeed;
      Velocity =  newVel;
    }

    public void FlyDown() { //fly downwards via applying velocity towards down
      var newVel = Velocity;
      newVel.y = -flyVerticalSpeed;
      Velocity =  newVel;
    }

    public void FlyNeutral() { //reset vertical velocity
      var newVel = Velocity;
      newVel.y = 0;
      Velocity =  newVel;
    }

    public void Ride() {
        currentEmote = "";
        isIdleState = false;
        isRunningState = false;
        isWalkingState = false;
        velocityCheck = false;
        isRun = false;
    }

    public override void Render()
    {
        base.Render();
        if(currentEmote != "")
        {
          animator.ResetTrigger("Walk");
          animator.ResetTrigger("Run");
          isRunningState = false;
          isWalkingState = false;
          animator.SetTrigger(currentEmote.ToString());
          currentEmote = "";
        }
        if(velocityCheck){
          if (isIdleState) {animator.ResetTrigger("Idle"); isIdleState = false;}
          if (!isRun && !isWalkingState) { //NOT WALKING, WALK
          animator.SetTrigger("Walk");
          animator.ResetTrigger("Run");
          isRunningState = false;
          isWalkingState = true;
        } else if (isRun && !isRunningState) { //NOT RUNNING, RUN
          animator.SetTrigger("Run");
          animator.ResetTrigger("Walk");
          isRunningState = true;
          isWalkingState = false;
      }
        } 
        else if (!isIdleState) {           
          animator.ResetTrigger("Walk");
          animator.ResetTrigger("Run");
          animator.SetTrigger("Idle");
          isIdleState = true;
          isRunningState = false;
          isWalkingState = false;
        }
    }



    public void playEmote(String emote) {
      currentEmote = emote;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetEmote(string emote, RpcInfo info = default)
    {
        this.currentEmote = emote;
    }
}