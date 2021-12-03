using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCollectible : MonoBehaviour
{
    public AudioClip collectedClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {

            controller.ammo += 4;
            controller.ammoText.text = "Gears: " + controller.ammo.ToString();
            Destroy(gameObject);

            controller.PlaySound(collectedClip);

        }

    }
}
