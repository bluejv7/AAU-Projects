using UnityEngine;
using System.Collections;

/// <summary>
/// Handle camera controls from mouse movement
/// </summary>
public class CameraController : MonoBehaviour {
    // Header declaration
    [Header("Input Axis Names")]

    // Serialized private variables

    /// <summary>
    /// Input name for moving the camera in the x axis
    /// </summary>
    [SerializeField] private string moveCameraInputX = "Mouse X";

    /// <summary>
    /// Input name for moving the camera in the y axis
    /// </summary>
    [SerializeField] private string moveCameraInputY = "Mouse Y";

    /// <summary>
    /// Object used for vertical tilting of the camera
    /// </summary>
    [SerializeField] private Transform cameraTarget = null;

    /// <summary>
    /// Minimum tilt value for the x-rotation of the camera
    /// </summary>
    [SerializeField] private float minTilt = -50.0f;

    /// <summary>
    /// Maximum tilt value fo the x-rotation of the camera
    /// </summary>
    [SerializeField] private float maxTilt = 30.0f;

    /// <summary>
    /// How sensitive the camera is when looking left/right
    /// </summary>
    [SerializeField] private float cameraSensitivityX = 2.5f;

    /// <summary>
    /// How sensitive the camera is when looking up/down
    /// </summary>
    [SerializeField] private float cameraSensitivityY = 2.0f;

    /// <summary>
    /// Decide whether to invert the camera or not (I personally hate inverted)
    /// </summary>
    [SerializeField] private bool shouldInvertCamera = false;

    // Private vars

    /// <summary>
    /// Current x-rotation of the camera
    /// </summary>
    private float currentTilt = 0;

    /// <summary>
    /// Modifier for mouse axis when inverted is on/off (This value should only be 1 or -1... never 0)
    /// </summary>
    private int invertModifier = -1;

    // Private functions

    /// <summary>
    /// Handle rotation of the camera
    /// </summary>
    private void RotateCamera(float rotateX, float rotateY)
    {
        // Rotate camera left/right
        this.transform.Rotate(0, rotateX * cameraSensitivityX, 0);

        // Rotate camera up/down
        currentTilt = Mathf.Clamp(currentTilt + invertModifier * rotateY * cameraSensitivityY, minTilt, maxTilt);
        cameraTarget.localEulerAngles = new Vector3(currentTilt, 0, 0);
    }

    /// <summary>
    /// Initialize variables
    /// </summary>
    private void Start()
    {
        // Set up whether the camera should be inverted based on initial settings
        invertModifier = (shouldInvertCamera ? 1 : -1);
    }

    /// <summary>
    /// Handle camera rotations each frame
    /// </summary>
    private void Update () {
        // Handle player's camera rotation
        float rotateX = Input.GetAxis(moveCameraInputX);
        float rotateY = Input.GetAxis(moveCameraInputY);
        RotateCamera(rotateX, rotateY);
    }
}
