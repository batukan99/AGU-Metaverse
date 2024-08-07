
using UnityEngine;
using Fusion;

[ScriptHelp(BackColor = EditorHeaderBackColor.Steel)]
public class ControllerPrototype : Fusion.NetworkBehaviour
{
    [SerializeField] private NetworkCharacterControllerPrototype _ncc;
    protected NetworkRigidbody _nrb;
    protected NetworkRigidbody2D _nrb2d;
    protected NetworkTransform _nt;

    /*[Networked]
    public Vector3 MovementDirection { get; set; }*/

    public bool TransformLocal = false;
    string emoteToPlay = "";

    [DrawIf(nameof(ShowSpeed), Hide = true)]
    public float Speed = 6f;
    [SerializeField] private GameObject emoteWheel;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject camBrain;
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private GameObject PlayerModel;
    [SerializeField] private ChatBubble chatBubble;
    [SerializeField] public State state = State.Walk;
    public enum State
    {
        Walk,
        Fly,
        Ride,
    };
    public static ControllerPrototype local { get; set; }
    public float smoothTurnTime = 0.1f;
    float smoothTurnVelocity;
    bool ShowSpeed => this && !TryGetComponent<NetworkCharacterControllerPrototype>(out _);

    private float targetAngle;
    private Vector3 moveDirection;
    public void Awake()
    {
        CacheComponents();
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            cam.gameObject.SetActive(true);
            camBrain.gameObject.SetActive(true);
            local = this;
        }
        else
        {
            cam.gameObject.SetActive(false);
        }
    }

    private void CacheComponents()
    {
        if (!_ncc) _ncc = GetComponent<NetworkCharacterControllerPrototype>();
        if (!_nrb) _nrb = GetComponent<NetworkRigidbody>();
        if (!_nrb2d) _nrb2d = GetComponent<NetworkRigidbody2D>();
        if (!_nt) _nt = GetComponent<NetworkTransform>();
    }

    bool JumpKeyLock = false;
    bool EmoteKeyLock = false;
    public override void FixedUpdateNetwork()
    {
        if (Runner.Config.PhysicsEngine == NetworkProjectConfig.PhysicsEngines.None)
        {
            return;
        }

        switch (state)
        {
            case State.Walk:
                Walk();
                break;
            case State.Fly:
                Fly();
                break;
            case State.Ride:
                Ride();
                break;
            default:
                Walk();
                break;
        }


    }
    void Walk()
    {
        CameraMovement.CameraMode camMode = camBrain.GetComponent<CameraMovement>().camMode;

        if (GetInput(out NetworkInputData networkInputData) && !chatBubble.isChatting)
        {
            switch (camMode)
            {
                case CameraMovement.CameraMode.ThirdPerson:
                    float smoothAngle;
                    if (networkInputData.movementInput.magnitude >= 0.1f)
                    {
                        moveDirection = networkInputData.movementInput;
                        targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg + networkInputData.camEulerY;
                        smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothTurnVelocity, smoothTurnTime * Time.deltaTime);
                        transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
                        moveDirection = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
                    }
                    else
                    {
                        Emote(networkInputData, _ncc.IsGrounded);
                    }
                    break;
                case CameraMovement.CameraMode.FirstPerson:
                    float smoothAngle1;
                    moveDirection = networkInputData.movementInput;
                    targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg + networkInputData.camEulerY;
                    smoothAngle1 = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothTurnVelocity, smoothTurnTime * Time.deltaTime);
                    transform.rotation = Quaternion.Euler(0f, smoothAngle1, 0f);
                    moveDirection = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;

                    if (networkInputData.movementInput.magnitude < 0.1f)
                    {
                        Emote(networkInputData, _ncc.IsGrounded);
                    }
            

                    break;
            }
            if (_ncc)
            {
                _ncc.Move(moveDirection, networkInputData.IsDown(NetworkInputData.BUTTON_RUN));
            }
            if (networkInputData.IsDown(NetworkInputData.BUTTON_JUMP) && !JumpKeyLock)
            {
                JumpKeyLock = true;
                if (_ncc)
                {
                    _ncc.Jump();
                }
            }
            else if (networkInputData.IsUp(NetworkInputData.BUTTON_JUMP) && JumpKeyLock)
            {
                JumpKeyLock = false;
            }
        }
    }

    void Fly()
    {
         CameraMovement.CameraMode camMode = camBrain.GetComponent<CameraMovement>().camMode;
        if (GetInput(out NetworkInputData networkInputData) && !chatBubble.isChatting)
        {

            switch (camMode)
            {
                case CameraMovement.CameraMode.ThirdPerson:
                    float smoothAngle;
                    if (networkInputData.movementInput.magnitude >= 0.1f)
                    {
                        moveDirection = networkInputData.movementInput;
                        targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg + networkInputData.camEulerY;
                        smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothTurnVelocity, smoothTurnTime * Time.deltaTime);
                        transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
                        moveDirection = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
                    }
                    else
                    {
                        Emote(networkInputData, _ncc.IsGrounded);
                    }
                    break;
                case CameraMovement.CameraMode.FirstPerson:
                    float smoothAngle1;
                    moveDirection = networkInputData.movementInput;
                    targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg + networkInputData.camEulerY;
                    smoothAngle1 = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothTurnVelocity, smoothTurnTime * Time.deltaTime);
                    transform.rotation = Quaternion.Euler(0f, smoothAngle1, 0f);
                    moveDirection = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;

                    if (networkInputData.movementInput.magnitude < 0.1f)
                    {
                        Emote(networkInputData, _ncc.IsGrounded);
                    }
                    break;
            }

            if (_ncc)
            {
                _ncc.Fly(moveDirection, networkInputData.IsDown(NetworkInputData.BUTTON_RUN));

                if (networkInputData.IsDown(NetworkInputData.BUTTON_JUMP)) _ncc.FlyUp();
                else if (networkInputData.IsDown(NetworkInputData.BUTTON_CROUCH)) _ncc.FlyDown();
                else _ncc.FlyNeutral();
            }
        }
    }

    void Ride()
    {
        if (GetInput(out NetworkInputData networkInputData) && !chatBubble.isChatting)
        {
            if (_ncc)
            { _ncc.Ride(); }
        }
    }

    void Emote(NetworkInputData networkInputData, bool grounded)
    {
        moveDirection = default;
        if (networkInputData.IsDown(NetworkInputData.BUTTON_GESTURE_LIST) && grounded && !EmoteKeyLock)
        {
            // open emote wheel
            cameraMovement.camLock = true;
            emoteWheel.SetActive(true);
            EmoteKeyLock = true;
        }
        else if ((networkInputData.IsUp(NetworkInputData.BUTTON_GESTURE_LIST) && emoteWheel.activeSelf && EmoteKeyLock))
        {
            // close emote wheel
            _ncc.RPC_SetEmote(emoteWheel.GetComponent<EmoteWheel>().currentEmote);
            emoteWheel.SetActive(false);
            cameraMovement.camLock = false;
            EmoteKeyLock = false;
        }
        else if (networkInputData.IsDown(NetworkInputData.BUTTON_FIRE) && emoteWheel.activeSelf && EmoteKeyLock)
        {

            _ncc.RPC_SetEmote(emoteWheel.GetComponent<EmoteWheel>().currentEmote);
            emoteWheel.SetActive(false);

            cameraMovement.camLock = false;
        }
        else if (networkInputData.IsUp(NetworkInputData.BUTTON_FIRE) && EmoteKeyLock == true && networkInputData.IsUp(NetworkInputData.BUTTON_GESTURE_LIST))
        {
            EmoteKeyLock = false;
        }
    }
}

