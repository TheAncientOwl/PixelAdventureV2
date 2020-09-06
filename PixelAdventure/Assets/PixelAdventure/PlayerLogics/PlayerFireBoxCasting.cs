using UnityEngine;

namespace PixelAdventure.PlayerLogics
{
    public class PlayerFireBoxCasting : MonoBehaviour
    {
        [Header("Ground")]
        [SerializeField] float m_GroundOffsetY = 1f;
        [SerializeField] Vector2 m_GroundBoxCastSize = new Vector2(0.91f, 0.27f);

        [Header("Sides")]
        [SerializeField] float m_SidesOffsetY = 0.27f;
        [SerializeField] Vector2 m_SidesBoxCastSize = new Vector2(0.75f, 1.31f);

        [Header("Misc")]
        [SerializeField] LayerMask m_FireLayerMask = 0;

        private Transform m_Transform = null;
        private PlayerMovement m_PlayerMovement = null;

        private void Start() 
        {
            m_Transform = GetComponent<Transform>();
            m_PlayerMovement = PlayerMovement.Instance;
        }

        private void FixedUpdate() 
        {
            // * ground
            RaycastHit2D[] hits = Physics2D.BoxCastAll
            (
                origin    : m_Transform.position + Vector3.down * m_GroundOffsetY,
                size      : m_GroundBoxCastSize,
                layerMask : m_FireLayerMask,
                direction : Vector2.down,
                distance  : 0f,
                angle     : 0f
            );

            foreach (var obj in hits)
                if (obj.transform.CompareTag("Fire"))
                    obj.transform.gameObject.GetComponent<Traps.Fire>().ApplyLogics();

            // * sides
            hits = Physics2D.BoxCastAll
            (
                origin    : m_Transform.position + Vector3.down * m_SidesOffsetY + new Vector3((m_PlayerMovement.LastDirection * m_SidesBoxCastSize.x / 2f), 0f, 0f),
                size      : m_SidesBoxCastSize,
                layerMask : m_FireLayerMask,
                direction : Vector2.down,
                distance  : 0f,
                angle     : 0f
            );

            foreach (var obj in hits)
                if (obj.transform.CompareTag("Fire"))
                    obj.transform.gameObject.GetComponent<Traps.Fire>().ApplyLogics();
                    
        }

        [Header("Gizmos")]
        [SerializeField] bool m_DrawGizomos = true;
        [SerializeField] Color m_GroundColor = Color.yellow;
        [SerializeField] Color m_SidesColor = Color.blue;
        private void OnDrawGizmos() 
        {
            if (!m_DrawGizomos) return;

            Vector3 center = GetComponent<Transform>().position + Vector3.down * m_GroundOffsetY;
            Vector2 halfSize = m_GroundBoxCastSize / 2f;
            Draw(center, halfSize, m_GroundColor);

            center = GetComponent<Transform>().position + Vector3.down * m_SidesOffsetY - new Vector3((-1f * m_SidesBoxCastSize.x / 2f), 0f, 0f);
            halfSize = m_SidesBoxCastSize / 2f;
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
