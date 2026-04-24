using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float speed = 12;
    [SerializeField] protected string _tagToAffect;
    //[SerializeField] protected LayerMask layersToDamage; //check by layer

    [SerializeField] protected AudioClip _soundShoot;
    [SerializeField] protected AudioClip _soundNoEffect;

    protected Rigidbody2D _rb;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Invoke(nameof(Vanish), 3);
    }

    public virtual void SetDirection(Vector2 dir)
    {
        var mouse = GameObject.Find("Mouse").GetComponent<MouseController>();
        dir.x = mouse.mouse_x;
        dir.y = mouse.mouse_y;
        _rb.velocity = (dir * speed) / 750;
        AudioSystem.Instance.PlaySound(_soundShoot, transform.position);
    }
    
    protected virtual void Vanish()
    {
        Destroy(gameObject);
    }
}