using PixelAdventure.PlayerLogics;
using PixelAdventure.API;
using UnityEngine;

namespace PixelAdventure.Traps
{
    public class Arrow : MonoBehaviour
    {
        private static readonly Vector2 k_PUSH_FORCE = new Vector2(0f, 27f);
        private static readonly int k_HIT_HASH = Animator.StringToHash("hit");

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(Player.TAG))
            {
                PlayerMovement.Instance.Velocity = k_PUSH_FORCE;
                CameraShaker.Instance.Shake();
                GetComponent<Animator>().SetTrigger(k_HIT_HASH);
                Destroy(this.gameObject, 0.2f);
            }
        }
    }
}