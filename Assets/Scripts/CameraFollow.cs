using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] public Vector3 offset = new Vector3(0f, 5f, -10f); // Agora é pública e ajustável no Inspector
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] public Transform target;

    private void Update()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
