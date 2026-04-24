using System;
using MassInteraction;
using UnityEngine;

public class StickyGround : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            other.gameObject.GetComponentInParent<IMass>().Red
        }
    }
}
