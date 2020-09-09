using PixelAdventure.PlayerLogics;
using PixelAdventure.API;
using System.Collections;
using UnityEngine;

namespace PixelAdventure.Traps.Blocks
{
    public class Block : MonoBehaviour
    {
        private static readonly Vector2 k_PUSH_VELOCITY = new Vector2(0f, 24f);
        private static CameraShaker m_CameraShaker = null;

        [SerializeField] GameObject[] m_BrokenParts = null;
        private Animator m_Animator = null;

        private void Start() 
        {
            m_CameraShaker = CameraShaker.Instance;
            m_Animator = GetComponent<Animator>();    
        }

        private void OnCollisionEnter2D(Collision2D other) 
        {
            float y = other.contacts[0].normal.y;
            if (other.gameObject.CompareTag(Player.TAG))
            {
                if (y < 0f)
                {
                    StartCoroutine(BreakCoroutine());
                    Player.Velocity = k_PUSH_VELOCITY;
                }
                else if (y > 0f)
                    StartCoroutine(BreakCoroutine());
            }    
        }

        private IEnumerator BreakCoroutine()
        {
            m_CameraShaker.Shake();
            m_Animator.SetTrigger(AnimatorHashes.HIT);

            yield return new WaitForSeconds(0.15f);

            foreach (var brokenPart in m_BrokenParts)
            {
                brokenPart.SetActive(true);
                Rigidbody2D rigidbody2D = brokenPart.GetComponent<Rigidbody2D>();
                rigidbody2D.velocity = new Vector2
                (
                    x: Random.Range(-3f, 3f),
                    y: Random.Range(2f, 6f)
                );
                rigidbody2D.AddTorque(Random.Range(-5f, 5f));
            }

            GetComponentInParent<ChildCounter>().Decrease();

            Destroy(this.gameObject);

            yield return 0;
        }

    }
}
