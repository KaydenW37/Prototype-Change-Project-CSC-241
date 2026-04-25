using System;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(HealthSystem))]
[RequireComponent(typeof(Rigidbody2D))]


public class EightLegDoor: MonoBehaviour
{
    
    
    private Rigidbody2D _rb;
    private HealthSystem _healthSystem;
    private BoxCollider2D _collider;



    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _healthSystem = GetComponent<HealthSystem>();
        _collider = GetComponent<BoxCollider2D>();
    }

    public void AcceptDefeat()
    {
        GameEventDispatcher.TriggerEnemyDefeated();
        Destroy(gameObject);
        
    }
}