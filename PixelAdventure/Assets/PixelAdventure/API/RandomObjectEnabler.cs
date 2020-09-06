using System.Collections;
using UnityEngine;

namespace PixelAdventure.API
{
    public class RandomObjectEnabler : MonoBehaviour
    {
        [SerializeField] GameObject[] m_Objects = null;

        private void Start() 
        {
            foreach (var obj in m_Objects)
                obj.SetActive(false); 

            Shuffle();

            StartCoroutine(EnableCoroutine());
        }

        private IEnumerator EnableCoroutine()
        {
            foreach (var obj in m_Objects)
            {
                obj.SetActive(true);
                yield return null;
            }

            Destroy(this.gameObject);
            yield return 0;
        }

        private void Shuffle()
        {
            System.Random rng = new System.Random();
            int n = m_Objects.Length;
            while (n > 1)
            {
                --n;
                int k = rng.Next(n + 1);
                GameObject temp = m_Objects[k];
                m_Objects[k] = m_Objects[n];
                m_Objects[n] = temp;
            }
        }
    }
}