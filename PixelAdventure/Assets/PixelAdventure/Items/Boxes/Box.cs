using PixelAdventure.PlayerLogics;
using PixelAdventure.API;
using UnityEngine;

namespace PixelAdventure.Items.Boxes
{
    public class Box : MonoBehaviour
    {
        private static readonly Vector2 k_PUSH_VELOCITY = new Vector2(0f, 24f);
        private static CameraShaker m_CameraShaker = null;

        [SerializeField] GameObject[] m_BrokenParts = null;
        [SerializeField] GameObject[] m_Fruits = null;
        [SerializeField] int m_JumpTimes = 4;
        [SerializeField] bool m_ThrowAllFruitsOnBreak = false;
        
        private int m_FruitsPerJump = 0;
        private int m_LastFruit = 0;

        private Animator m_Animator = null;

        private void Start()
        {
            m_CameraShaker = CameraShaker.Instance;
            m_Animator = GetComponent<Animator>();
            if (m_JumpTimes > 0) m_FruitsPerJump = m_Fruits.Length / m_JumpTimes;
            m_LastFruit = m_Fruits.Length - 1;

            foreach (var fruit in m_Fruits)
                fruit.GetComponent<Animator>().runtimeAnimatorController = FruitAnimatorsManager.Instance.GetRandomController;
        }

        private void OnCollisionEnter2D(Collision2D other) 
        {
            float y = other.contacts[0].normal.y;
            if (other.gameObject.CompareTag(Player.TAG))
            {
                if (y < 0f)
                {
                    Hit();
                    PlayerMovement.Instance.Velocity = k_PUSH_VELOCITY;
                }
                else if (y > 0f) 
                    Hit();    
            }
        }

        private void Hit()
        {
            m_CameraShaker.Shake();
            m_Animator.SetTrigger("hit");

            if (!m_ThrowAllFruitsOnBreak)
                for (int i = 0; i < m_FruitsPerJump; ++i)
                {
                    m_Fruits[m_LastFruit].SetActive(true);
                    m_Fruits[m_LastFruit].GetComponent<Rigidbody2D>().velocity = new Vector2
                    (
                        x: Random.Range(-7f, 7f),
                        y: Random.Range(0f, 2f)
                    );

                    m_LastFruit--;
                }

            m_JumpTimes--;

            if (m_JumpTimes <= 0)
            {
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;

                if (m_ThrowAllFruitsOnBreak)
                    foreach (var fruit in m_Fruits)
                    {
                        fruit.SetActive(true);
                        fruit.GetComponent<Rigidbody2D>().velocity = new Vector2
                        (
                            x: Random.Range(-7f, 7f),
                            y: Random.Range(0f, 2f)
                        );
                    }

                foreach (var brokenPart in m_BrokenParts)
                {
                    brokenPart.SetActive(true);
                    brokenPart.GetComponent<Rigidbody2D>().velocity = new Vector2
                    (
                        x: Random.Range(-7f, 7f),
                        y: Random.Range(2f, 10f)
                    );
                }

                Destroy(this.gameObject);
            }
        }

    }
}