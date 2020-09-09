using PixelAdventure.PlayerLogics;
using PixelAdventure.API;
using UnityEngine;

namespace PixelAdventure.Items.Fruits
{
    public class DynamicFruit : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other) 
        {
            if (other.gameObject.CompareTag(Player.TAG))
            {
                GetComponent<Animator>().SetTrigger(AnimatorHashes.COLLECTED);
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