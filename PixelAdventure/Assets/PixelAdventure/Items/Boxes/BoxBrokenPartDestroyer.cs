using PixelAdventure.API;
using UnityEngine;

namespace PixelAdventure.Items.Boxes
{
    public class BoxBrokenPartDestroyer : MonoBehaviour
    {
        private bool m_Lock = false;
        
        private void OnCollisionEnter2D(Collision2D other) 
        {
            if (!m_Lock)
            {
                m_Lock = true;
                Invoke("DestroySelf", 2f);
            }
        }
                
        private void DestroySelf()
        {
            GetComponentInParent<ChildCounter>().Decrease();
            Destroy(this.gameObject);
        } 
    }
}
