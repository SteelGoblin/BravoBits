using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KeyBoardMoveCamera : MonoBehaviour {

	public float CameraSpeed = 5f;
	public float dragSpeed = 2;
    public float zoomSpeed = 5f;
    public float zoomHeight = 100;
    public float maxWidth = 100;
    public float maxLength = 100;

    public int CAMERA_MOVEMENT = 2;
    
    
    // Only autoscroll when at the edges of the screen
    const float screenEdgeBuffer = 0.2f;
    
    float left = Screen.width * screenEdgeBuffer;
	float right = Screen.width - (Screen.width * screenEdgeBuffer);
    float bottom = Screen.height * screenEdgeBuffer;
	float top = Screen.height - (Screen.height * screenEdgeBuffer);


	bool cameraDragging = true;
	Vector3 dragOrigin;
	bool rotating = false;
	bool RightClick = false;

    public float MAX_TERRAIN_DISTANCE = 30f;

    private float rotationY = 0f;
    

    void Start()
    {
        rotationY = transform.eulerAngles.y;

    }

    void Update()
    {
        Vector3 cameraDirection;

        cameraDirection.x = 0;
        cameraDirection.y = 0;
        cameraDirection.z = 0;

        if (Input.GetKey(KeyCode.A))
            cameraDirection.x -= CAMERA_MOVEMENT;
        else if (Input.GetKey(KeyCode.D))
            cameraDirection.x = CAMERA_MOVEMENT;
        else if (Input.GetKey(KeyCode.W))
            cameraDirection.z = CAMERA_MOVEMENT;
        else if (Input.GetKey(KeyCode.S))
            cameraDirection.z -= CAMERA_MOVEMENT;
        else if (Input.GetKey(KeyCode.Minus) && GetGroundHeight() >= MAX_TERRAIN_DISTANCE)
            cameraDirection.y -= CAMERA_MOVEMENT;
        else if (Input.GetKey(KeyCode.Equals))
            cameraDirection.y = CAMERA_MOVEMENT;

        if (Input.GetKey(KeyCode.Q))
        {             
            rotationY += 2f;           
            transform.localEulerAngles = new Vector3(0,rotationY, 0);        
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotationY -= 2f;
            transform.localEulerAngles = new Vector3(0, rotationY, 0);
        }

        transform.Translate(cameraDirection,Space.Self);
    }


    public float GetGroundHeight()
    {
        RaycastHit hit;
        int layerMask = 1 << 8; //bit shift the index of the 8th layer to get its bitmask so only terrain is considered the ground

        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out hit, 10000f, layerMask))
        {

            return hit.distance;
        }
        else if (Physics.Raycast(new Ray(transform.position, Vector3.up), out hit, 10000f, layerMask))
        {
            return hit.distance;
        }

        //No hit? What the hell happened?! Throw an exception!
        throw new UnityException("Camera could not find any ground beneath it. Mark terrain layer 8");
    }




    void MouseMovement()
    {

        // Get current mouse and world locations
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        /*Vector2 mousePosition;
        
        mousePosition.x = Input.GetAxis("Horizontal");
        mousePosition.y = Input.GetAxis("Vertical");*/

        Vector3 currPosition = transform.position;
        float mouseAxis = Input.GetAxis("Mouse ScrollWheel");

        float Y;


        //        Debug.Log(transform.position);

        // Zoom up / down
        // TODO: floor is hardcoded  - will cause problems with varied terrain height        
        // Zoom down
        if (mouseAxis > 0f)
        {
            if (currPosition.y > 2f)
            {
                transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed);
            }
        }
        else if (mouseAxis < 0f)
        { // Zoom up
            if (currPosition.y <= zoomHeight)
            {
                transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed);
            }
        }

        // Check if mouse is left / right / top / bottom screen edge

        if (mousePosition.x < left || mousePosition.x > right || mousePosition.y > top || mousePosition.y < bottom)
        {
            cameraDragging = true;
        }
        else
            cameraDragging = false;



        if (cameraDragging)
        {
            Y = transform.position.y;
            if (mousePosition.x < left && currPosition.x >= (maxWidth * -1))
            {
                transform.Translate(Vector3.left * Time.deltaTime * CameraSpeed);
            }
            else if (mousePosition.x > right && currPosition.x <= maxWidth)
            {
                transform.Translate(Vector3.right * Time.deltaTime * CameraSpeed);
            }
            else if (mousePosition.y > top && currPosition.z <= maxLength)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * CameraSpeed);
            }
            else if (mousePosition.y < bottom && currPosition.z >= (maxWidth * -1))
            {
                transform.Translate(Vector3.back * Time.deltaTime * CameraSpeed);
            }

            // Reset Height else camera moved up or down because it's tilting
            Vector3 pos;
            pos = transform.position;
            pos.y = Y;
            transform.position = pos;
        }

        RightClick = Input.GetMouseButtonDown(1);
        if (RightClick && !rotating)
        {
            rotating = true;
            dragOrigin = mousePosition;
        }

        if (rotating)
        {
            if (dragOrigin.x < mousePosition.x)
            {
                transform.RotateAround(transform.position, Vector3.up, .5f);
            }
            else if (dragOrigin.x > mousePosition.x)
            {
                transform.RotateAround(transform.position, Vector3.up, -.5f);
            }
        }

        if (!Input.GetMouseButton(1))
        {
            rotating = false;
        }
    }
}
