using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{   
    public bool enableInputCapture = true;
    public bool lockAndHideCursor = false;
    public bool holdRightMouseCapture = false;

    public float lookSpeed = 5f;
    public float moveSpeed = 5f;
    public float sprintSpeed = 50f;
    public float mouseWheelScale = 1f;

    bool m_inputCaptured;
    float m_yaw;
    float m_pitch;
    
    // Start is called before the first frame update
    void Awake()
    {
        enabled = enableInputCapture;
    }

    void OnEnable()
    {
        // if(enableInputCapture && !holdRightMouseCapture)
        // {
        //     CaptureInput();
        // }
    }
    
    // void OnDisable()
    // {
    //     ReleaseInput();
    // }

    void OnValidate()
    {
        if(Application.isPlaying)
        {
            enabled = enableInputCapture;
        }
    }

    void CaptureInput(bool isRightMouseCapture)
    {
        if(!lockAndHideCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        m_inputCaptured = true;
        lockAndHideCursor = true;

        if(isRightMouseCapture)
        {
            holdRightMouseCapture = true;
        }

        m_yaw = transform.eulerAngles.y;
        m_pitch = transform.eulerAngles.x;
    }

    void ReleaseInput(bool isRightMouseCapture)
    {
        if(lockAndHideCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        m_inputCaptured = false;
        lockAndHideCursor = false;

        if(isRightMouseCapture)
        {
            holdRightMouseCapture = false;
        }
    }

    void ZoomInOut()
    {
        if(Input.mouseScrollDelta.magnitude > 0)
        {
            transform.position += transform.forward * (Input.mouseScrollDelta.y * mouseWheelScale);
        }
        else if(Input.mouseScrollDelta.magnitude < 0)
        {
            transform.position -= transform.forward * (Input.mouseScrollDelta.y * mouseWheelScale);
        }
    }

    void CameraMove()
    {
        var rotStrafe = Input.GetAxis("Mouse X");
        var rotFwd = Input.GetAxis("Mouse Y");

        m_yaw = (m_yaw + lookSpeed * rotStrafe) % 360f;
        m_pitch = (m_pitch - lookSpeed * rotFwd) % 360f;
        transform.rotation = Quaternion.AngleAxis(m_yaw, Vector3.up) * Quaternion.AngleAxis(m_pitch, Vector3.right);

        var speed = Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed);
        var forward = speed * Input.GetAxis("Vertical");
        var right = speed * Input.GetAxis("Horizontal");
        var up = speed * ((Input.GetKey(KeyCode.E) ? 1f : 0f) - (Input.GetKey(KeyCode.Q) ? 1f : 0f));
        transform.position += transform.forward * forward + transform.right * right + Vector3.up * up;
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_inputCaptured)
        {
            if(!holdRightMouseCapture && Input.GetMouseButtonDown(1))
            {
                CaptureInput(true);
            }
            else if(!holdRightMouseCapture && Input.mouseScrollDelta.magnitude != 0)
            {
                CaptureInput(false);
            }
        }

        if(!m_inputCaptured)
        {
            return;    
        }

        if(m_inputCaptured)
        {
            if(holdRightMouseCapture && Input.GetMouseButtonUp(1))
            {
                ReleaseInput(true);
            }
            else if(!holdRightMouseCapture && Input.mouseScrollDelta.magnitude == 0)
            {
                ReleaseInput(false);
            }
        }

        if(holdRightMouseCapture == true)
        {
            CameraMove();
        }
        else
        {
            ZoomInOut();
        }

    }
}
