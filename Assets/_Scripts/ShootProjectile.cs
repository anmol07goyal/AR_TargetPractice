using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    private Camera arCamera;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float launchVelocity = 15f;

    private void Awake()
    {
        if (arCamera == null)
            arCamera = Camera.main;
    }

    public void ShootFromScreenCenter()
    {
        Ray centerRay = arCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 targetPoint;
        if (Physics.Raycast(centerRay, out RaycastHit hitInfo, 100f))
            targetPoint = hitInfo.point;
        else
            targetPoint = centerRay.GetPoint(50f);

        Vector3 origin = arCamera.transform.position + (arCamera.transform.forward * 0.2f);
        Vector3 launchDirection = (targetPoint - origin).normalized;

        GameObject projectile = Instantiate(projectilePrefab, origin, Quaternion.LookRotation(launchDirection));

        if (projectile.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = launchDirection * launchVelocity;
        }
    }
}
