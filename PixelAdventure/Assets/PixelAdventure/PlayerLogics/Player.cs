using PixelAdventure.API;
using UnityEngine;

namespace PixelAdventure.PlayerLogics
{
    public class Player : MonoBehaviour
    {
        private static readonly int k_HIT_HASH = Animator.StringToHash("hit");

        public static Player Instance {get; private set;}
        public const string TAG = "Player";

        private void Awake() => Instance = this;

        public void Die()
        {
            CameraShaker.Instance.Shake();
            GetComponent<Animator>().SetTrigger(k_HIT_HASH);
            GetComponent<BoxCollider2D>().enabled = false;
            Rigidbody2D rigidbody2D = PlayerMovement.Instance.Rigidbody2D;
            rigidbody2D.constraints = RigidbodyConstraints2D.None;
            rigidbody2D.AddTorque(-PlayerMovement.Instance.LastDirection * 5f, ForceMode2D.Impulse);
            rigidbody2D.velocity = new Vector2
            (
                x: -PlayerMovement.Instance.LastDirection * 10f,
                y: 10f
            );
            GetComponent<PlayerMovement>().enabled = false;
        }
    }
}