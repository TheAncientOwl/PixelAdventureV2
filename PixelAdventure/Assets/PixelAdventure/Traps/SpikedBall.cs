using UnityEngine;
using System;

namespace PixelAdventure.Traps
{
    public class SpikedBall : MonoBehaviour
    {
        [SerializeField] RotationType m_RotationType = RotationType.Degrees360;
        [SerializeField] Transform m_Center = null;
        [SerializeField] Transform m_Ball = null;
        [SerializeField] float m_RotationRadius = 2f;
        [SerializeField] float m_AngularSpeed = -2f;

        private float m_Angle = 0f;

        private Action m_RotateFunction;

        private void Start() 
        {
            switch (m_RotationType)
            {
                case RotationType.Degrees360:
                    m_RotateFunction = Rotate360Degrees;
                    break;
                case RotationType.Degrees180:
                    m_RotateFunction = RotateDegrees180;
                    break;
                default:
                    Debug.LogError("Unsupported type SpikedBall.RotationType");
                    break;
            }
        }

        private void Update() => m_RotateFunction();

        private void Rotate360Degrees()
        {
            m_Ball.position = new Vector3
            (
                x: m_Center.position.x + Mathf.Cos(m_Angle) * m_RotationRadius,
                y: m_Center.position.y + Mathf.Sin(m_Angle) * m_RotationRadius,
                z: m_Ball.position.z
            );

            m_Angle += Time.deltaTime * m_AngularSpeed;
            
            if (m_Angle >= 360f) 
                m_Angle = 0f;

            Vector2 direction = m_Ball.position - m_Center.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            m_Center.rotation = rotation;
        }

        private void RotateDegrees180()
        {
            //if (!Input.GetKeyDown(KeyCode.S)) return;
            if (m_Ball.position.y >= m_Center.position.y + 0.1f)
            {
                m_AngularSpeed = -m_AngularSpeed;
            }

            m_Angle += Time.deltaTime * m_AngularSpeed;

            m_Ball.position = new Vector3
            (
                x: m_Center.position.x + Mathf.Cos(m_Angle) * m_RotationRadius,
                y: m_Center.position.y + Mathf.Sin(m_Angle) * m_RotationRadius,
                z: m_Ball.position.z
            );


            if (m_Angle <= -180f)
                m_Angle = 0f;

            // Debug.Log(m_Angle);

            Vector2 direction = m_Ball.position - m_Center.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            m_Center.rotation = rotation;

            
                
        }

        private enum RotationType
        {
            Degrees360,
            Degrees180
        }

    }
}
