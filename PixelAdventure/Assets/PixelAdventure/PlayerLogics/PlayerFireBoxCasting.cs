using UnityEngine;

namespace PixelAdventure.PlayerLogics
{
    public class PlayerFireBoxCasting : MonoBehaviour
    {
        private static readonly Vector2 k_GROUND_BOX_CAST_SIZE = new Vector2(0.91f, 0.27f);
        private static readonly Vector2 k_SIDES_BOX_CAST_SIZE = new Vector2(0.75f, 1.31f);

        private const string k_FIRE_TAG = "Fire";
        private const float k_GROUND_OFFSET_Y = 1f;
        private const float k_SIDES_OFFSET_Y = 0.27f;

        [SerializeField] LayerMask m_FireLayerMask = 0;

        private void FixedUpdate() 
        {
            // * ground
            RaycastHit2D[] hits = Physics2D.BoxCastAll
            (
                origin    : Player.Transform.position + Vector3.down * k_GROUND_OFFSET_Y,
                size      : k_GROUND_BOX_CAST_SIZE,
                layerMask : m_FireLayerMask,
                direction : Vector2.down,
                distance  : 0f,
                angle     : 0f
            );

            foreach (var obj in hits)
                if (obj.transform.CompareTag(k_FIRE_TAG))
                    obj.transform.gameObject.GetComponent<Traps.Fire>().ApplyLogics();

            // * sides
            hits = Physics2D.BoxCastAll
            (
                origin    : Player.Transform.position + Vector3.down * k_SIDES_OFFSET_Y + new Vector3((Player.Movement.LastDirection * k_SIDES_BOX_CAST_SIZE.x / 2f), 0f, 0f),
                size      : k_SIDES_BOX_CAST_SIZE,
                layerMask : m_FireLayerMask,
                direction : Vector2.down,
                distance  : 0f,
                angle     : 0f
            );

            foreach (var obj in hits)
                if (obj.transform.CompareTag(k_FIRE_TAG))
                    obj.transform.gameObject.GetComponent<Traps.Fire>().ApplyLogics();
                    
        }

        [Header("Gizmos")]
        [SerializeField] bool m_DrawGizomos = true;
        [SerializeField] Color m_GroundColor = Color.yellow;
        [SerializeField] Color m_SidesColor = Color.blue;
        private void OnDrawGizmos() 
        {
            if (!m_DrawGizomos) return;

            Vector3 center = GetComponent<Transform>().position + Vector3.down * k_GROUND_OFFSET_Y;
            Vector2 halfSize = k_GROUND_BOX_CAST_SIZE / 2f;
            Draw(center, halfSize, m_GroundColor);

            center = GetComponent<Transform>().position + Vector3.down * k_SIDES_OFFSET_Y - new Vector3((-1f * k_SIDES_BOX_CAST_SIZE.x / 2f), 0f, 0f);
            halfSize = k_SIDES_BOX_CAST_SIZE / 2f;
            Draw(center, halfSize, m_SidesColor);
        }

        private void Draw(Vector3 center, Vector2 halfSize, Color color)
        {
            Vector3 topLeft = new Vector3
            (
                x: center.x - halfSize.x,
                y: center.y + halfSize.y,
                z: center.z
            );

            Vector3 topRight = new Vector3
            (
                x: center.x + halfSize.x,
                y: center.y + halfSize.y,
                z: center.z
            );

            Vector3 bottomLeft = new Vector3
            (
                x: center.x - halfSize.x,
                y: center.y - halfSize.y,
                z: center.z
            );

            Vector3 bottomRight = new Vector3
            (
                x: center.x + halfSize.x,
                y: center.y - halfSize.y,
                z: center.z
            );

            Debug.DrawLine(topLeft, topRight, color, 0f);
            Debug.DrawLine(topRight, bottomRight, color, 0f);
            Debug.DrawLine(bottomRight, bottomLeft, color, 0f);
            Debug.DrawLine(bottomLeft, topLeft, color, 0f);
        }
    }
}
