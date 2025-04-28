using UnityEngine;
using PController;
using UnityEngine.InputSystem;

public class PandaGripperArticulation : MonoBehaviour
{
    //public ArticulationBody rightFinger;  // Assign panda_rightfinger in Inspector
    //public ArticulationBody leftFinger;   // Assign panda_leftfinger in Inspector
    public PandaController controller;

    public float openPosition = 0.05f;  // Adjust based on URDF constraints
    public float closePosition = -0.05f; //0.02
    public float speed = 10f;

    public InputActionReference gripActionReference;

    private bool isClosed = false;

    private bool isAttached = false;
    private Transform parentBeforeAttach;
    private Transform collidedObject;
    private Rigidbody grabbedObjectRB;

    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // Press 'G' to toggle
        {
            UnityEngine.Debug.Log("gripper triggered");
            isClosed = !isClosed;
            MoveGripper(isClosed ? closePosition : openPosition);
        }
    }
    */

    void MoveGripper(float target)
    {
        controller.rightfingertarget = target;
        controller.leftfingertarget = target;
    }

    void OnEnable()
    {
        // Enable the input action for gripping
        if (gripActionReference != null && gripActionReference.action != null)
        {
            gripActionReference.action.performed += OnGripActionPerformed;
            //gripActionReference.action.performed += ToggleAttachment;
            gripActionReference.action.Enable();
        }
    }

    void OnDisable()
    {
        // Disable the input action for gripping
        if (gripActionReference != null && gripActionReference.action != null)
        {
            gripActionReference.action.performed -= OnGripActionPerformed;
            //gripActionReference.action.performed -= ToggleAttachment;
            gripActionReference.action.Disable();
        }
    }

    
    private void OnGripActionPerformed(InputAction.CallbackContext context)
    {
        // Call MoveGripper when the new input action is performed
        isClosed = !isClosed;
        MoveGripper(isClosed ? closePosition : openPosition);
        /*
        if (isClosed == false)
        {
            ReleaseObject();
        }
        */
    }

    /*

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            if (!isClosed)
            {
                if (!isAttached)
                {
                    parentBeforeAttach = transform.parent;
                    transform.SetParent(collision.transform);
                    isAttached = true;
                }
                else
                {
                    transform.SetParent(parentBeforeAttach);
                    isAttached = false;
                }
            }

            Rigidbody objectRB = collision.rigidbody;

            if (objectRB != null && GetComponent<FixedJoint>() == null)
            {
                FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = objectRB;
                joint.breakForce = 500; // Adjust to make grasp breakable
                joint.breakTorque = 500;

                Debug.Log("Object grabbed: " + collision.gameObject.name);
            }
        }

    }
    */
    
    /*

    void OnCollisionEnter(Collision collision)
    {
        
        //if (collision.gameObject.CompareTag("Grabbable"))
        //{
            //collidedObject = collision.transform;
        //}
        
        
        if (collision.gameObject.CompareTag("Grabbable"))
        {
            if (!isClosed)
            {
                if (!isAttached)
                {
                    parentBeforeAttach = transform.parent;
                    transform.SetParent(collision.transform);
                    isAttached = true;
                }
                else
                {
                    transform.SetParent(parentBeforeAttach);
                    isAttached = false;
                }
            }

            Rigidbody objectRB = collision.rigidbody;

            if (objectRB != null && GetComponent<FixedJoint>() == null)
            {
                FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = objectRB;
                joint.breakForce = 500; // Adjust to make grasp breakable
                joint.breakTorque = 500;

                Debug.Log("Object grabbed: " + collision.gameObject.name);
            }
        }
        
    }
    */
    

    /*
    private void ToggleAttachment(InputAction.CallbackContext context)
    {
        if (collidedObject == null)
        {
            return;
        }

        if (!isAttached)
        {
            parentBeforeAttach = transform.parent;
            transform.SetParent(collidedObject);
            isAttached = true;
        }
        else
        {
            transform.SetParent(parentBeforeAttach);
            isAttached = false;
        }
    }
    */

    /*
    void ReleaseObject()
    {
        FixedJoint joint = GetComponent<FixedJoint>();
        if (joint != null)
        {
            Destroy(joint);
            Debug.Log("Object released!");
        }
    }
    */
}