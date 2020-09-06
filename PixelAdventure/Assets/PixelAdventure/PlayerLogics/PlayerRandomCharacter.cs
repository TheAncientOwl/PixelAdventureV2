using UnityEngine;

namespace PixelAdventure.PlayerLogics
{
    public class PlayerRandomCharacter : MonoBehaviour
    {
        [SerializeField] RuntimeAnimatorController[] m_Animators = null;

        private void Start()
        {
            Animator animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = m_Animators[Random.Range(0, m_Animators.Length - 1)];
        }
    }
}