using System.Collections;
using UnityEngine;

namespace PixelAdventure.Traps
{
    public class Fire : MonoBehaviour
    {
        private static readonly int k_ON_HASH  = Animator.StringToHash("on");
        private static readonly int k_HIT_HASH = Animator.StringToHash("hit");
        private Animator m_Animator = null;

        private CircleCollider2D m_CircleCollider2D = null;

        private void Start() 
        {
            m_CircleCollider2D = GetComponent<CircleCollider2D>();
            m_CircleCollider2D.enabled = false;
            
            m_Animator = GetComponent<Animator>();
            m_Animator.SetBool(k_ON_HASH, false);
        }

        private static float k_HIT_TIME = 0.4f;
        private static float k_ON_TIME = 0.6f;

        private bool m_Lock = false;

        // MOVED TO PLAYER MOVEMENT RAYCASTS
        // private void OnCollisionEnter2D(Collision2D other) 
        // {
        //     if (!m_Lock)
        //         if (other.gameObject.CompareTag(Player.TAG))
        //             StartCoroutine(Logics());  
        // }

        public void ApplyLogics()
        {
            if (!m_Lock)
                StartCoroutine(Logics());
        }

        private IEnumerator Logics()
        {
            m_Lock = true;

            m_Animator.SetTrigger(k_HIT_HASH);

            yield return new WaitForSeconds(k_HIT_TIME);

            m_Animator.SetBool(k_ON_HASH, true);
            m_CircleCollider2D.enabled = true;

            yield return new WaitForSeconds(k_ON_TIME);
            
            m_Animator.SetBool(k_ON_HASH, false);
            m_CircleCollider2D.enabled = false;

            m_Lock = false;

            yield return 0;
        }
    }
}
