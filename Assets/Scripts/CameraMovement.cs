using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivity = 1f;
    public float camDistance = 75f;
    public GameObject Camera;
    float camMax = 75;
    float camMin = 15;
    public float camCollisionOffsetX = 0.01f; //controls how much camera will offset from raycasted object to prevent collision, X axis
    public float camCollisionOffsetY = 0.01f; //controls how much camera will offset from raycasted object to prevent collision, Y axis
    public float zoomSpeed = 5; //controls how fast camera will move towards player via scrolling
    float x;
    float y;
    public bool invert;
    public Transform player;
    public Transform thirdPersonAnchorPoint;
    public Transform firstPersonAnchorPoint;
    public bool camLock = false;
    [SerializeField] float tp_camY = 0;
    [SerializeField] float tp_camX = 0;
    [SerializeField] float tp_camZ = 0;
    [SerializeField] float fp_camZ = 0;
    [SerializeField] float fp_camY = 0;
    [SerializeField] float fp_camX = 0;
    public enum CameraMode
    {
        ThirdPerson,
        FirstPerson,
    }
    public CameraMode camMode = CameraMode.ThirdPerson;

    // Start is called before the first frame update
    void Start()
    {

        //Cursor.lockState = CursorLockMode.Locked;

        //Detach camBrain
        transform.parent = null;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null)
        {
            if (camMode == CameraMode.ThirdPerson)
            {
                transform.position = thirdPersonAnchorPoint.position + new Vector3(tp_camX, tp_camY, tp_camZ);
            }
            else
            {
                transform.position = firstPersonAnchorPoint.position + new Vector3(fp_camX, fp_camY, fp_camZ);
            }
        }

        x = Input.GetAxis("Mouse X") * (sensitivity + 0.7f); //0.7 makes the sensitivity reasonable, while keeping visible sensitivity 1 which is more understandable and user friendly
        y = Input.GetAxis("Mouse Y") * (invert ? -1 : 1) * -(sensitivity + 0.7f);

        float rotateZ = transform.eulerAngles.z;

        if (rotateZ > 180)
        {
            rotateZ = rotateZ - 360; //without these few lines, the code cannot comprehend angles correctly via Mathf.clamp, the angles are 0-360 this code makes it so that 359 = -1 
        }

        switch (camMode)
        {
            case CameraMode.ThirdPerson:
                ThirdPersonCamera(rotateZ);
                break;
            case CameraMode.FirstPerson:
                FirstPersonCamera(rotateZ);
                break;
        }

    }
    void ThirdPersonCamera(float rotateZ)
    {
        transform.eulerAngles = camLock ? transform.eulerAngles : new Vector3(0, transform.eulerAngles.y + x, Mathf.Clamp(rotateZ + y, -10, 89)); //ensure camera cant continute above/beyond character

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        { //scroll to zoom in/out
            camDistance -= zoomSpeed;
        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                camDistance += zoomSpeed;
            }
        }

        camDistance = Mathf.Clamp(camDistance, camMin, camMax);
    }

    public void FocusToQuiz(Transform quiz) 
    {
        Camera.transform.LookAt(quiz);
    }
    void FirstPersonCamera(float rotateZ)
    {
        transform.eulerAngles = camLock ? transform.eulerAngles : new Vector3(0, transform.eulerAngles.y + x, Mathf.Clamp(rotateZ + y, -10, 89)); //ensure camera cant continute above/beyond character
        camDistance = 0;
    }


    void FixedUpdate()
    {
        switch (camMode)
        {
            case CameraMode.ThirdPerson:
                RaycastHit hit;

                if (Physics.Raycast(transform.position, -Camera.transform.TransformDirection(Vector3.forward), out hit, 23.25f)) //75 local = 23.25
                {   //send a ray from brain towards camera
                    Debug.DrawRay(transform.position, -Camera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow); //DEBUG ONLY
                    if (hit.transform.root != gameObject.transform.root)
                    {
                        Vector3 localPoint = transform.InverseTransformPoint(hit.point.x, hit.point.y, hit.point.z); //world to local
                        Camera.transform.localPosition = new Vector3(Mathf.Clamp(camDistance, camMin, localPoint.x + Mathf.Sign(transform.rotation.x) * camCollisionOffsetX), localPoint.y + Mathf.Sign(transform.rotation.y) * camCollisionOffsetY, localPoint.z); // if a collision happens pan camera to collision point + offsets Mathf.Clamp allows scrolling
                    }   //Mathf.Sign(transform.rotation) ensures to correctly offset the camera according to left/right up/down
                }
                else
                {
                    Camera.transform.localPosition = new Vector3(camDistance, 0, 0); //recover from collision/execute scroll
                }
                break;

            case CameraMode.FirstPerson:
                Camera.transform.localPosition = new Vector3(0, 0, 0);
                break;
        }
    }
}
