using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float impactForce = 8f;

    [SerializeField] private Rigidbody rb;

    private void Start()
    {
        // Auto-destroy if it misses and flies into the void
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Target"))
            return;

        if (collision.gameObject.TryGetComponent<MovingTarget>(out MovingTarget target))
            target.ShowParticles();

        GameManager.Instance.UpdateScore();
    }
}