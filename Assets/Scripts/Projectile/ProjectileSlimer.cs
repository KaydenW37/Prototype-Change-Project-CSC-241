using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProjectileSlimer : Projectile
{
    [SerializeField] private AudioClip _soundSlimed;
    [SerializeField] private GameObject slimePrefab;

    
    //[SerializeField] private LayerMask layersToDamage; //check by layer
    
    private void OnCollisionEnter2D(Collision2D other)    
    {
        //this projectile goes away when hitting anything
        Destroy(gameObject);
        
        //Check by layer 
        //if ((layersToDamage.value & (1 << other.gameObject.layer)) > 0)
        
        //Check by tag
        if (other.transform.CompareTag(_tagToAffect))
        {
            //Eggs are enemies that can't be changed into slimes
            if (other.transform.GetComponent<EggController>())
            {
                AudioSystem.Instance.PlaySound(_soundNoEffect, transform.position);
                return;
            }
            
            AudioSystem.Instance.PlaySound(_soundSlimed, transform.position);
            
           

            //UPDATED HERE, made it so if it is a tile it DOES NOT MAKE A GREEN THING. DID NOT ADD NEW SCRIPT SO KINDA BAD??
            GameObject slime = null;

            var collector = GameObject.Find("Player").GetComponent<PlayerCollector>();
            Debug.Log(collector.SlimeCount);
            if (other.gameObject.GetComponent<TilemapCollider2D>() != null && collector.SlimeCount > 0)
            {
                //destroy enemy hit (it will be around for the rest of this method)
                Destroy(other.gameObject);
                collector.SpendSlimes(1);
                GameEventDispatcher.TriggerSlimeSpent();

            }
            else if (other.gameObject.GetComponent<TilemapCollider2D>() == null)
            {
                Destroy(other.gameObject);
                slime = Instantiate(slimePrefab, other.transform.position, Quaternion.identity);
            }
            //create a slime object (instantiate)
            if (!slime) return;
            
            //Transfer the velocity from the enemy hit to the new slime object
            var slimeRb = slime.transform.GetComponent<Rigidbody2D>();
            if (!slimeRb) return;
            
            var otherRb = other.transform.GetComponent<Rigidbody2D>();
            if (!otherRb) return; 

            slimeRb.velocity = otherRb.velocity;
            slimeRb.gravityScale = otherRb.gravityScale;
        }
        else
        {
            //AudioSystem.Instance.PlaySound(_soundNoEffect, transform.position);
        }
    }
}