using PixelAdventure.Items.Fruits;
using PixelAdventure.PlayerLogics;
using System.Collections;
using UnityEngine;

namespace PixelAdventure.Items.Boxes
{
    public class Box1 : MonoBehaviour
    {
        private static Vector2 k_PUSH_VELOCITY = new Vector2(0f, 24f);

        private ParticleSystem m_ParicleSystem = null;

        private void Awake() 
        {
            m_ParicleSystem = GetComponentInChildren<ParticleSystem>();    
        }

        private void OnCollisionEnter2D(Collision2D other) 
        {
            float y = other.contacts[0].normal.y;
            if (other.gameObject.CompareTag(Player.TAG))
            {
                if (y == -1)
                {
                    PlayerMovement.Instance.Velocity = k_PUSH_VELOCITY;
                    StartCoroutine(BreakCoroutine());
                }
                else if (y == 1)
                    StartCoroutine(BreakCoroutine());
            }
        }

        private IEnumerator BreakCoroutine()
        {
            GetComponent<Animator>().SetTrigger("hit");
            GetComponent<BoxCollider2D>().enabled = false;

            yield return new WaitForSeconds(0.2f);
            GetComponent<SpriteRenderer>().enabled = false;
            m_ParicleSystem.Play();

            Destroy(this.gameObject, m_ParicleSystem.main.startLifetime.constantMax);

            yield return 0;
        }

    }
}