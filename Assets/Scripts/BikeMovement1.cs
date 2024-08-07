using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class BikeMovement1 : NetworkBehaviour
{
    [SerializeField] GameObject Gidon;
    [SerializeField] Transform gearModel;
    [SerializeField] Transform frontWheelModel;
    [SerializeField] Transform backWheelModel;
    [SerializeField] float steerAngle;
    [SerializeField] float steerSpeed = 1 / 5;
    [SerializeField] float wheelSpinMultiplier;
    [SerializeField] float smoothTurnTime;
    [SerializeField] Rigidbody rb;
    [SerializeField] float gravity;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    [SerializeField] float maxSpeed;
    [SerializeField] float minusMaxSpeed;
    [SerializeField] bool isGrounded;
    [SerializeField] float riderHeight;
    public bool getInput = false;
    public Transform rider = null;
    float currentBrakeForce;
    bool Brake;
    float Horizontal;
    float Vertical;
    float smoothTurnVelocity;

    public override void Spawned() {
        base.Spawned();
        deltaTime = Time.deltaTime;
    }

    void Update()
    {
        if (getInput)
        {
            rider.position = transform.position + new Vector3(0, riderHeight, 0);
            rider.rotation = transform.rotation;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (getInput) { GetInput(); }
        else
        {
            Vertical = 0;
            Horizontal = 0;
        }
        Cycle(Gidon.transform.forward);
        ApplySteer();
    }

    void GetInput()
    {
        if(rider == null) return;

        Horizontal = rider.GetComponent<InputHandler>().networkInputData.movementInput.x;
        Vertical = rider.GetComponent<InputHandler>().networkInputData.movementInput.y;
        Brake =  rider.GetComponent<InputHandler>().networkInputData.IsDown(NetworkInputData.BUTTON_JUMP);
    }

    void ApplySteer()
    {
        float currentSteerAngle = steerAngle * Horizontal;
        Gidon.transform.localEulerAngles = new Vector3(0, currentSteerAngle, 0);
        if (rb.velocity.sqrMagnitude > 0.005)
        {
            float rotY = transform.eulerAngles.y;
            transform.eulerAngles = new Vector3(0, rotY += steerSpeed * Horizontal * Mathf.Sign(Vector3.Dot(horizontalVelocity, Gidon.transform.forward)), 0);
        }
    }

    public void LeaveBike()
    {
        if (rider != null)
        {
            rider.position = transform.position + transform.right * 3 + new Vector3(0, 2, 0);
            rider = null;
            getInput = false;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        isGrounded = true;
    }

    void OnTriggerExit(Collider collider)
    {
        isGrounded = false;
    }

    static Vector3 Velocity = new Vector3(0, 0, 0);
    [Networked] float wheelX {get; set;}//the wheels gimbal lock with normal angles
    [Networked] float gearX  {get; set;}
    [Networked] float deltaTime {get; set;}
    
    [Networked] Vector3 newDirection {get; set;}
    [Networked] Vector3 horizontalVelocity {get; set;}
    [Networked] Vector3 verticalVelocity {get; set;}
    [Networked] Vector3 currentVelocity {get; set;}
    [Networked] float Sign {get; set;}

    void Cycle(Vector3 direction)
    {
        verticalVelocity = new Vector3(0, rb.velocity.y, 0);
        horizontalVelocity = rb.velocity - verticalVelocity;
        Sign = Mathf.Sign(Vector3.Dot(horizontalVelocity, Gidon.transform.forward));

        horizontalVelocity = direction * Sign * Mathf.Clamp(horizontalVelocity.magnitude - Mathf.Clamp(Mathf.Abs(1-Vertical) * deceleration, 0, deceleration) * deltaTime, 0, maxSpeed);
        newDirection = direction * Mathf.Clamp(Sign * horizontalVelocity.magnitude + Vertical * acceleration * deltaTime, minusMaxSpeed, maxSpeed);

        verticalVelocity = isGrounded ? new Vector3(verticalVelocity.x, 0, verticalVelocity.z) 
        : new Vector3(verticalVelocity.x, verticalVelocity.y - gravity * deltaTime, verticalVelocity.z) ;
        
        if (isGrounded && horizontalVelocity.y <= 0.001)
        {
            verticalVelocity = new Vector3(verticalVelocity.x, 0, verticalVelocity.z);
        }
        
        rb.velocity = newDirection + verticalVelocity;
        
    }
    public override void Render()
    {
        base.Render();
        //update models
        float targetAngle = Mathf.Clamp(newDirection.magnitude / maxSpeed, 0, 1) * wheelSpinMultiplier;
        float gearTargetAngle = Vertical * Mathf.Clamp(newDirection.magnitude / maxSpeed, 0, 1) / 2 * wheelSpinMultiplier;
        backWheelModel.Rotate(targetAngle * Mathf.Sign(Vector3.Dot(horizontalVelocity, Gidon.transform.forward)) , 0f, 0f, Space.Self);
        frontWheelModel.Rotate(targetAngle * Mathf.Sign(Vector3.Dot(horizontalVelocity, Gidon.transform.forward)) , 0f, 0f, Space.Self);
        gearModel.Rotate(gearTargetAngle, 0f, 0f, Space.Self);
    }
}