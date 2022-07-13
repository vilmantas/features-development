using UnityEngine;

namespace Features.Character
{
    public class PlayerScript : MonoBehaviour
    {
        private void Start()
        {
            GetComponentInChildren<CharacterManager>().DoSetup();
        }
    }
}