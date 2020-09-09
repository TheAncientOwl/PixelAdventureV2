using PixelAdventure.PlayerLogics;
using PixelAdventure.API;
using UnityEngine;

namespace PixelAdventure.Traps
{
    public class Arrow : MonoBehaviour
    {
        private static readonly Vector2 k_PUSH_FORCE = new Vector2(0f, 27f);

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(Player.TAG))
            {
                Player.Velocity = k_PUSH_FORCE;
                CameraShaker.Instance.Shake();
                GetComponent<Animator>().SetTrigger(AnimatorHashes.HIT);
                Destroy(this.gameObject, 0.2f);
            }
        }
    }
}