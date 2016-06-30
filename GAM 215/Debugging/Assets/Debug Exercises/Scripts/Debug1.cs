using UnityEngine;
using System.Collections;

public class Debug1 : MonoBehaviour {
    private CharacterController controller = null;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private float speed = 5.0f;

    // Use this for initialization
    void Start()
    {
        controller = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = Vector3.zero;
        if (Input.GetKey("w"))
        {
            velocity.z = speed;
        }
        controller.Move(transform.forward * velocity.z * Time.deltaTime);
    }
}
