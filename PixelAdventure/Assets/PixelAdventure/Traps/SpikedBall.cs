using UnityEngine;

namespace PixelAdventure.Traps
{
    public class SpikedBall : MonoBehaviour
    {
        [SerializeField] Transform m_RotationCenter = null;
        [SerializeField] Transform m_Collider = null;
        [SerializeField] Transform m_Chain = null;
        [SerializeField] float m_RotationRadius = 2f;
        [SerializeField] float m_AngularSpeed = -2f;
        [SerializeField] float m_ChainAngularSpeed = -2f;

        private float m_Angle = 0f;
        private float m_ChainAngle = 0f;

        private void Update() 
        {
            m_Collider.position = new Vector3
            (
                x: m_RotationCenter.position.x + Mathf.Cos(m_Angle) * m_RotationRadius,
                y: m_RotationCenter.position.y + Mathf.Sin(m_Angle) * m_RotationRadius,
                z: m_Collider.position.z
            );

            m_Angle += Time.deltaTime * m_AngularSpeed;
            
            if (m_Angle >= 360f)
                m_Angle = 0f;


            m_RotationCenter.eulerAngles = new Vector3(0f, 0f, m_ChainAngle);

            m_ChainAngle += Time.deltaTime * m_ChainAngularSpeed;

            if (m_ChainAngle >= 360f)
                m_ChainAngle = 0f;
        }

    }
}
