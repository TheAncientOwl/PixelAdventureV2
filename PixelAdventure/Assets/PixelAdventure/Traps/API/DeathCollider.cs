using PixelAdventure.PlayerLogics;
using UnityEngine;

namespace PixelAdventure.Traps.API
{
    public class DeathCollider : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other) 
        {
            if (other.CompareTag(Player.TAG))
                Player.Die();    
        }
    }
}
