using UnityEngine;

[RequireComponent(typeof(Transform))]
public class FixToGround : MonoBehaviour
{
    [Header("Ground Settings")]
    [Tooltip("Which layers count as ground")]
    public LayerMask groundLayer;

    [Tooltip("How far down to check for ground")]
    public float raycastDistance = 2f;

    [Tooltip("Offset above the ground")]
    public float heightOffset = 0.0f;

    [Tooltip("Should stick exactly to surface normal (rotate)?")]
    public bool alignToNormal = true;

    void LateUpdate()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f; // small offset up so ray starts clear
        Ray ray = new Ray(origin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, groundLayer))
        {
            // Move the IK target to the ground hit position
            transform.position = hit.point + Vector3.up * heightOffset;

            if (alignToNormal)
            {
                // Rotate to align with the ground normal
                transform.up = hit.normal;
            }
        }
    }
}

