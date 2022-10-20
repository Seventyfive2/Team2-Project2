using UnityEngine;
using UnityEngine.InputSystem;

public class LobbedProjectile : MonoBehaviour
{
    public Vector3 startingPosition;
    public Vector3 endingPosition;

    private float height;
    private float angle = 45f;
    public Rigidbody rb;

    public bool aimAtMouse = false;

    public void Start()
    {
        if(aimAtMouse)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            Vector3 input = Vector3.zero;

            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                input = raycastHit.point;
            }

            Activate(transform.position,input);
        }
        else
        {
            Activate(transform.position, endingPosition);
        }
    }

    public void Activate(Vector3 start, Vector3 end)
    {
        startingPosition = start;
        endingPosition = end;

        Vector3 dir = endingPosition - startingPosition; // get Target Direction
        height = dir.y; // get height difference
        dir.y = 0; // retain only the horizontal difference
        float dist = dir.magnitude; // get horizontal direction
        float a = angle * Mathf.Deg2Rad; // Convert angle to radians
        dir.y = dist * Mathf.Tan(a); // set dir to the elevation angle.
        dist += height / Mathf.Tan(a); // Correction for small height differences

        // Calculate the velocity magnitude
        float velocity = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        rb.velocity = velocity * dir.normalized; // Return a normalized vector.
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(startingPosition, .5f);
        Gizmos.DrawWireSphere(endingPosition, .5f);
    }
}
