using UnityEngine;

namespace PixelAdventure.Background
{
    public class BackgroundPieceOutline : MonoBehaviour
    {
        [SerializeField] Color color = Color.green;
        private void OnDrawGizmos()
        {
            Vector3 itemSize = GetComponent<SpriteRenderer>().bounds.size;

            Vector3 topLeft = new Vector3
            (
                x: this.gameObject.transform.position.x - itemSize.x / 2,
                y: this.gameObject.transform.position.y + itemSize.y / 2,
                z: 20f
            );

            Vector3 topRight = new Vector3
            (
                x: this.gameObject.transform.position.x + itemSize.x / 2,
                y: this.gameObject.transform.position.y + itemSize.y / 2,
                z: 20f
            );

            Vector3 bottomLeft = new Vector3
            (
                x: this.gameObject.transform.position.x - itemSize.x / 2,
                y: this.gameObject.transform.position.y - itemSize.y / 2,
                z: 20f
            );

            Vector3 bottomRight = new Vector3
            (
                x: this.gameObject.transform.position.x + itemSize.x / 2,
                y: this.gameObject.transform.position.y - itemSize.y / 2,
                z: 20f
            );
            
            float duration = 0f;

            Debug.DrawLine(topLeft, topRight, color, duration);
            Debug.DrawLine(topLeft, bottomLeft, color, duration);
            Debug.DrawLine(bottomLeft, bottomRight, color, duration);
            Debug.DrawLine(bottomRight, topRight, color, duration);
        }
    }
}