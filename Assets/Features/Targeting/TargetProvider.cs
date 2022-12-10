using System;
using UnityEngine;

namespace Features.Targeting
{
    public class TargetProvider : MonoBehaviour
    {
        public Func<Action<GameObject>, GameObject> CharacterTargetProvider;

        public Func<Vector3> MousePositionProvider;
        
        public void GetCharacterTarget(Action<GameObject> callback)
        {
            CharacterTargetProvider.Invoke(callback);
        }

        public Vector3 GetMousePosition()
        {
            return MousePositionProvider.Invoke();
        }
    }
}