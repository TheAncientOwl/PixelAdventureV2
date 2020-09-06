using PixelAdventure.PlayerLogics;
using UnityEngine;

namespace PixelAdventure.Items.Fruits
{
    public class StaticFruit : MonoBehaviour
    {
        private static readonly int k_COLLECTED_HASH = Animator.StringToHash("collected");

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(Player.TAG))
            {
                GetComponent<Animator>().SetTrigger(k_COLLECTED_HASH);
                Destroy(this.gameObject, 0.5f);
            }    
        }
    }
}