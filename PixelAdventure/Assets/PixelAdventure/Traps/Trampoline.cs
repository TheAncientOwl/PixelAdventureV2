using PixelAdventure.PlayerLogics;
using PixelAdventure.API;
using UnityEngine;

namespace PixelAdventure.Traps
{
    public class Trampoline : MonoBehaviour
    {
        private static readonly Vector2 k_PUSH_FORCE = new Vector2(0f, 39.5f);
        private static readonly int k_PUSH_HASH = Animator.StringToHash("push");

        private Animator m_Animator = null;

        private void Start() => m_Animator = GetComponent<Animator>();

        private void OnTriggerEnter2D(Collider2D collider) 
        {
            if (collider.CompareTag(Player.TAG))
            {
                PlayerMovement.Instance.Velocity = k_PUSH_FORCE;
                CameraShaker.Instance.Shake();
                m_Animator.SetTrigger(k_PUSH_HASH);
            }
        }

    }
}