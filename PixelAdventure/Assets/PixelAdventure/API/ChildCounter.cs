using UnityEngine;

namespace PixelAdventure.API
{
    public class ChildCounter : MonoBehaviour
    {
        [SerializeField] int m_Count = 0;

        public void Decrease()
        {
            m_Count--;
            if (m_Count <= 0)
                Destroy(this.gameObject);
        }

    }
}
