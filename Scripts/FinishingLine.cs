using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishingLine : MonoBehaviour
{
    public ParticleSystem HealthEffect;
    private ParticleSystem HealthCollisionEffect;
    public AudioClip collectedClip;

    // collission checker
    private void OnTriggerEnter2D(Collider2D other)
    {
        RubyPlatformerControl player = other.GetComponent<RubyPlatformerControl>();

        // gerir effect, hljoð og loadar nytt scene
        if (player != null)
        {
            HealthCollisionEffect = Instantiate(HealthEffect, transform.position, Quaternion.identity) as ParticleSystem;
            HealthCollisionEffect.Play();
            player.PlaySound(collectedClip);
            StartCoroutine(WinLoad());
        }
    }

    IEnumerator WinLoad()
    {
        // loadar inn nytt scene eftir 1 sek
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(4);
    }
}
