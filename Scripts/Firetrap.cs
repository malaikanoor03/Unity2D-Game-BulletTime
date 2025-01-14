using System.Collections;
using UnityEngine;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private bool triggered;
    private bool active;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !triggered)
        {
            StartCoroutine(ActivateFiretrap());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && active)
        {
            // Apply damage to the player
            collision.GetComponent<Health>()?.TakeDamage(damage);
        }
    }

    private IEnumerator ActivateFiretrap()
    {
        triggered = true;

        // Change color to indicate the fire trap is about to activate
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(activationDelay);

        // Reset color and activate fire trap
        spriteRenderer.color = Color.white;
        active = true;
        anim.SetBool("activated", true); // Trigger the fire animation

        // Wait while the fire trap is active
        yield return new WaitForSeconds(activeTime);

        // Deactivate fire trap
        active = false;
        triggered = false;
        anim.SetBool("activated", false); // Reset the fire animation
    }
}
