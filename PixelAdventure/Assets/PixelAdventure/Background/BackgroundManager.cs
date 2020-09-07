using System.Collections.Generic;
using UnityEngine;

namespace PixelAdventure.Background
{
    public class BackgroundManager : MonoBehaviour
    {

        const float k_MOVE_TIME = 0.7f;
        const float k_MOVE_SPEED = -2f;

        [SerializeField] Transform[] m_BackgroundObjects = null;
        [SerializeField] Sprite[] m_Sprites = null;

        private BackgroundRoll m_BackgroundRoll;

        private void Start()
        {
            Sprite sprite = m_Sprites[Random.Range(0, m_Sprites.Length - 1)];
            foreach (var obj in m_BackgroundObjects)
                obj.GetComponent<SpriteRenderer>().sprite = sprite;

            m_BackgroundRoll = new BackgroundRoll(m_BackgroundObjects);
        }

        private void Update() => m_BackgroundRoll.Roll(k_MOVE_SPEED * Time.deltaTime, k_MOVE_TIME);

    }
}