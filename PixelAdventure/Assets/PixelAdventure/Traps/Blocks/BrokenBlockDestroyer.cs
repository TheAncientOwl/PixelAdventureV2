using System.Collections;
using UnityEngine;

namespace PixelAdventure.Traps.Blocks
{
    public class BrokenBlockDestroyer : MonoBehaviour
    {
        private static readonly int k_HIT_HASH = Animator.StringToHash("hit");

        private void OnCollisionEnter2D(Collision2D other) 
        {
            if (other.collider.gameObject.CompareTag("Ground"))
                StartCoroutine(DestroyCoroutine());
        } 

        private IEnumerator DestroyCoroutine()
        {
            GetComponent<Animator>().SetTrigger(k_HIT_HASH);
            yield return new WaitForSeconds(0.2f);

            Destroy(this.gameObject);

            yield return 0;
        }
    }
}
