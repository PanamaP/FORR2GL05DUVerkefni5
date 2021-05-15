using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{

    public ParticleSystem HealthEffect;
    private ParticleSystem HealthCollisionEffect;

    public AudioClip collectedClip;

    // collission checker
    private void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
        RubyPlatformerControl controller2 = other.GetComponent<RubyPlatformerControl>();

        if (controller != null)
        {
            if (controller.health < controller.maxHealth)
            {
                // breytir health, ey�ir objectinu, synir effect og spilar hljo�i�
                controller.ChangeHealth(1);
                Destroy(gameObject);
                HealthCollisionEffect = Instantiate(HealthEffect, transform.position, Quaternion.identity) as ParticleSystem;
                HealthCollisionEffect.Play();

                controller.PlaySound(collectedClip);
            }

        }

        if (controller2 != null)
        {
            if (controller2.health < controller2.maxHealth)
            {
                // breytir health, ey�ir objectinu, synir effect og spilar hljo�i�
                controller2.ChangeHealth(1);
                Destroy(gameObject);
                HealthCollisionEffect = Instantiate(HealthEffect, transform.position, Quaternion.identity) as ParticleSystem;
                HealthCollisionEffect.Play();

                controller2.PlaySound(collectedClip);
            }

        }
    }
}
