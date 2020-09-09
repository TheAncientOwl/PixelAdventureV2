﻿using PixelAdventure.PlayerLogics;
using PixelAdventure.API;
using UnityEngine;

namespace PixelAdventure.Items.Fruits
{
    public class DynamicFruit : MonoBehaviour
    {
        private static readonly int k_COLLECTED_HASH = Animator.StringToHash("collected");

        private void OnCollisionEnter2D(Collision2D other) 
        {
            if (other.gameObject.CompareTag(Player.TAG))
            {
                GetComponent<Animator>().SetTrigger(k_COLLECTED_HASH);
                Invoke("DestroySelf", 0.5f);
            }    
        }

        private void DestroySelf()
        {
            Debug.Log("Fruit destroy!");
            GetComponentInParent<ChildCounter>().Decrease();
            Destroy(this.gameObject);
        }

    }
}