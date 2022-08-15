using UnityEngine;

namespace _SampleGames.Survivr
{
    public class EnemyController : MonoBehaviour
    {
        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        public virtual void Initialize(int health, CharacterController target)
        {
        }
    }
}