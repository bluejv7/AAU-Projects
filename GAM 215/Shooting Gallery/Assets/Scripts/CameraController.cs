using UnityEngine;
using System.Collections;

/// <summary>
/// Handle how the camera reacts to mouse events
/// </summary>
public class CameraController : MonoBehaviour {
    /// <summary>
    /// The target the camera will focus on
    /// </summary>
    [SerializeField] private Transform cameraTarget = null;

    [Header("--- Input Axis Names ---")]

    /// <summary>
    /// Input axis name for horizontal camera movement
    /// </summary>
    [SerializeField] private string horizontalCameraInputName = "Mouse X";

    /// <summary>
    /// Input axis name for vertical camera movement
    /// </summary>
    [SerializeField] private string verticalCameraInputName = "Mouse Y";

    [Header("--- Tilt Values ---")]

    /// <summary>
    /// Minimum tilt angle for the camera
    /// </summary>
    [SerializeField] private float minTilt = -10;

    /// <summary>
    /// Maximum tilt angle for the camera
    /// </summary>
    [SerializeField] private float maxTilt = 70;

    [Header("--- Mouse Sensitivity ---")]

    /// <summary>
    /// The speed we can rotate the camera horizontally
    /// </summary>
    [SerializeField] private float cameraSensitivityX = 2.5f;

    /// <summary>
    /// The speed of rotation for vertical camera movement
    /// </summary>
    [SerializeField] private float cameraSensitivityY = 2.0f;

    [Header("--- Misc Camera Controls")]

    /// <summary>
    /// Flag for inverting vertical camera controls
    /// </summary>
    [SerializeField] private bool shouldInvertCamera = false;

    /// <summary>
    /// The current camera tilt in degrees
    /// </summary>
    private float currentCameraTilt = 0;

    /// <summary>
    /// Modifier for mouse axis when inverted (-1 for normal, 1 for inverted)
    /// </summary>
    private int invertModifier = -1;

    /// <summary>
    /// Calculation variable for camera target rotation
    /// </summary>
    private Vector3 cameraTargetRotation = Vector3.zero;

    /// <summary>
    /// Rotate the camera
    /// </summary>
    /// <param name="rotateX">The magnitude/direction for rotating camera horizontally</param>
    /// <param name="rotateY">The magnitude/direction for rotating camera vertically</param>
    private void RotateCamera(float rotateX, float rotateY)
    {
        // Look left/right
        this.transform.Rotate(0, rotateX * cameraSensitivityX, 0);

        // Look up/down
        currentCameraTilt = Mathf.Clamp(currentCameraTilt + invertModifier * rotateY * cameraSensitivityY, minTilt, maxTilt);
        cameraTargetRotation = Vector3.zero;
        cameraTargetRotation.x = currentCameraTilt;
        cameraTarget.localEulerAngles = cameraTargetRotation;
    }

	/// <summary>
    /// Initialize variables and configs
    /// </summary>
	private void Start()
    {
        // Initialize invertModifier
        invertModifier = (shouldInvertCamera) ? 1 : -1;

        // Initialize cameraTarget (assuming target is the parent)
        cameraTarget = this.transform.FindChild("Camera Target").transform;

        Cursor.lockState = CursorLockMode.Locked;
	}
	
	/// <summary>
    /// Handle camera movements every frame
    /// </summary>
	private void Update()
    {
        // Handle camera rotations
        float rotateX = Input.GetAxis(horizontalCameraInputName);
        float rotateY = Input.GetAxis(verticalCameraInputName);
        RotateCamera(rotateX, rotateY);
	}
}
