using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    // function sem biður eftir collision
    private void OnTriggerStay2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
        RubyPlatformerControl controller2 = other.GetComponent<RubyPlatformerControl>();

        // sendir -1 a breytuna changeHealth
        if (controller != null)
        {
            controller.ChangeHealth(-1);

        }

        if (controller2 != null)
        {
            controller2.ChangeHealth(-1);
        }
    }
}
