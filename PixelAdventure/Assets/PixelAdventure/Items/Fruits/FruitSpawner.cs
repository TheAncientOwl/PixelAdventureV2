using System.Collections.Generic;
using UnityEngine;

namespace PixelAdventure.Items.Fruits
{
    public class FruitSpawner : MonoBehaviour
    {
        public static FruitSpawner Instance {get; private set;}
        private void Awake() => Instance = this;

        [SerializeField] RuntimeAnimatorController[] m_FruitAnimators = null;
        [SerializeField] GameObject m_FruitBase = null;
        [SerializeField] int m_MaxFruits = 20;

        private Queue<GameObject> m_ObjectPool = null;

        private void Start() 
        {
            m_ObjectPool = new Queue<GameObject>();
            while (m_MaxFruits > 0)
            {
                GameObject obj = Instantiate(m_FruitBase, Vector3.zero, Quaternion.identity);
                obj.SetActive(false);
                m_ObjectPool.Enqueue(obj);
                --m_MaxFruits;
            }    
        }

        public void Cache(GameObject obj)
        {
            Debug.Log("Object cached!");
            obj.SetActive(false);
            m_ObjectPool.Enqueue(obj);
        }

        public GameObject SpawnAt(Vector3 position)
        {
            GameObject obj = m_ObjectPool.Dequeue();
            obj.GetComponent<Animator>().runtimeAnimatorController = m_FruitAnimators[Random.Range(0, m_FruitAnimators.Length - 1)];
            obj.transform.position = position;
            obj.SetActive(true);
            return obj;
        }


    }
}