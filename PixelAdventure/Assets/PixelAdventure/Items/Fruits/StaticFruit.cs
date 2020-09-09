using PixelAdventure.PlayerLogics;
using PixelAdventure.API;
using UnityEngine;

namespace PixelAdventure.Items.Fruits
{
    public class StaticFruit : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(Player.TAG))
            {
                GetComponent<Animator>().SetTrigger(AnimatorHashes.COLLECTED);
                Destroy(this.gameObject, 0.5f);
            }    
        }
    }
}