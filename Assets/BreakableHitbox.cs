using UnityEngine;

public class BreakableHitBox : MonoBehaviour
{
    [SerializeField] private int hitsToBreak = 10;
    [SerializeField] private AudioClip _breakSound;
    [SerializeField] private GameObject breakEffect;

    private int hitCount = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only count hits from slimer projectiles
        ProjectileSlimer slimer = collision.gameObject.GetComponent<ProjectileSlimer>();

        if (slimer == null) return;

        hitCount++;

        if (hitCount >= hitsToBreak)
        {
            Break();
        }
    }

    private void Break()
    {
        if (_breakSound)
            AudioSystem.Instance.PlaySound(_breakSound, transform.position);

        if (breakEffect)
            Instantiate(breakEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}