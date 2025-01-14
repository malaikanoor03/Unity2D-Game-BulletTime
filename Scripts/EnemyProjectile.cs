using UnityEngine;

public class EnemyProjectile : EnemyDamage
{ [SerializeField] private float speed;
[SerializeField] private float resetTime;
private float lifetime;



    // Method to set the direction of the projectile
    public void activateProjectile()
    {
       lifetime = 0;
       gameObject.SetActive(true);
    }

    private void Update()
    {
        float movementSpeed = speed * Time.deltaTime; 
        transform.Translate (movementSpeed, 0, 0);
        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
        gameObject.SetActive(false);
       
    }
    private void OntriggerEnter2D (Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        gameObject.SetActive(false);
    }
    
        
        
}
