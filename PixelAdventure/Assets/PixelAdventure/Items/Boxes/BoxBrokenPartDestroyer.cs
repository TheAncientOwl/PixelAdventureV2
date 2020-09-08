using UnityEngine;

namespace PixelAdventure.Items.Boxes
{
    public class BoxBrokenPartDestroyer : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other) => Invoke("DestroySelf", 0.6f);
                
        private void DestroySelf() => Destroy(this.gameObject);
    }
}
