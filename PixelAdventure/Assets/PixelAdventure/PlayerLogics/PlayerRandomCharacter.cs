using UnityEngine;

namespace PixelAdventure.PlayerLogics
{
    public class PlayerRandomCharacter : MonoBehaviour
    {
        [SerializeField] RuntimeAnimatorController[] m_Animators = null;

        private void Start() 
        => Player.Animator.runtimeAnimatorController = m_Animators[Random.Range(0, m_Animators.Length - 1)];
        
    }
}