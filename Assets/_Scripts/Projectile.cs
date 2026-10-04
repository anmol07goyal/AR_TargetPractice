using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float impactForce = 8f;

    private void Start()
    {
        // Auto-destroy if it misses and flies into the void
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Apply impulse to the hit cube if it has a Rigidbody
        Rigidbody hitRb = collision.rigidbody;
        if (hitRb != null)
        {
            Vector3 forceDirection = collision.relativeVelocity.normalized;
            hitRb.AddForce(forceDirection * impactForce, ForceMode.Impulse);
        }

        // Optional: Destroy projectile on impact or spawn impact particles
        //Destroy(gameObject);
    }
}