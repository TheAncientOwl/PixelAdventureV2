using PixelAdventure.PlayerLogics;
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
                Destroy(this.gameObject, 0.5f);
            }    
        }

    }
}