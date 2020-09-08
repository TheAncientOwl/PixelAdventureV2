using UnityEngine;

namespace PixelAdventure.Traps.SpikedBalls
{
    public class SpikedBall180 : MonoBehaviour
    {
        [SerializeField] Transform m_Center = null;
        [SerializeField] Transform m_Ball = null;
        [SerializeField] float m_RotationRadius = 3.5f;
        [SerializeField] float m_AngularSpeed = -3.7f;
        [SerializeField] float m_MinAngle = -3f;
        [SerializeField] float m_MaxAngle = 0f;

        private float m_Angle = 0f;

        private void Update()
        {
            m_Ball.position = new Vector3
            (
                x: m_Center.position.x + Mathf.Cos(m_Angle) * m_RotationRadius,
                y: m_Center.position.y + Mathf.Sin(m_Angle) * m_RotationRadius,
                z: m_Ball.position.z
            );

            m_Angle += Time.deltaTime * m_AngularSpeed;

            if (m_Angle < m_MinAngle || m_Angle > m_MaxAngle)
                m_AngularSpeed = -m_AngularSpeed;

            Debug.Log("Angle: " + m_Angle + " | Angular Speed: " + m_AngularSpeed);

            Vector2 direction = m_Ball.position - m_Center.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            m_Center.rotation = rotation;
        }

    }
}
