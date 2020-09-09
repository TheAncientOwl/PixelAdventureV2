using UnityEngine;

namespace PixelAdventure.API
{
    public class ChildCounter : MonoBehaviour
    {
        [SerializeField] int m_Count = 0;

        public void Decrease()
        {
            int id = this.gameObject.GetInstanceID();
            Debug.Log(id + " : " + m_Count);
            m_Count--;
            if (m_Count <= 0)
                Invoke("DestroySelf", 1.5f);
        }

        private void DestroySelf() => Destroy(this.gameObject);

    }
}
