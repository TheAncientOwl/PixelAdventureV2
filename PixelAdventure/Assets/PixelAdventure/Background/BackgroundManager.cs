using System.Collections.Generic;
using UnityEngine;

namespace PixelAdventure.Background
{
    public class BackgroundManager : MonoBehaviour
    {
        [Range(0, 1)]
        [SerializeField] float m_MoveTime = 0.7f;
        [SerializeField] float m_MoveSpeed = 2f;

        [SerializeField] Transform[] m_BackgroundObjects = null;
        [SerializeField] Sprite[] m_Sprites = null;

        private BackgroundRoll m_BackgroundRoll;

        private void Start()
        {
            Sprite sprite = m_Sprites[Random.Range(0, m_Sprites.Length - 1)];
            foreach (var obj in m_BackgroundObjects)
                obj.GetComponent<SpriteRenderer>().sprite = sprite;

            m_BackgroundRoll = new BackgroundRoll(m_BackgroundObjects);
            
            m_MoveSpeed = -m_MoveSpeed;
        }

        private void Update() => m_BackgroundRoll.Roll(m_MoveSpeed * Time.deltaTime, m_MoveTime);

    }
}