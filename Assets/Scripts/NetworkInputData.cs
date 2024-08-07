using UnityEngine;
using Fusion;

public struct NetworkInputData : INetworkInput
{
    public Vector2 movementInput;
    public Vector3 aimForwardVector;
    public float camEulerY;
    public NetworkBool isJumpPressed;
    
  public const int BUTTON_USE      = 0;
  public const int BUTTON_FIRE     = 1;
  public const int BUTTON_FIRE_ALT = 2;

  public const int BUTTON_FORWARD  = 3;
  public const int BUTTON_BACKWARD = 4;
  public const int BUTTON_LEFT     = 5;
  public const int BUTTON_RIGHT    = 6;

  public const int BUTTON_JUMP     = 7;
  public const int BUTTON_CROUCH   = 8;
  public const int BUTTON_WALK     = 9;

  public const int BUTTON_ACTION1  = 10;
  public const int BUTTON_ACTION2  = 11;
  public const int BUTTON_ACTION3  = 12;
  public const int BUTTON_GESTURE_LIST  = 13;
  public const int BUTTON_RELOAD   = 14;  
  public const int BUTTON_RUN  = 15;  
  public const int BUTTON_CHAT = 16;
  public const int BUTTON_VOICE_CHAT = 17;
  public const int BUTTON_CHAT_SEND = 18;
  
  public NetworkButtons Buttons;
  public byte Weapon;
  public Angle Yaw;
  public Angle Pitch;

  public bool IsUp(int button) {
    return Buttons.IsSet(button) == false;
  }

  public bool IsDown(int button) {
    return Buttons.IsSet(button);
  }
}
