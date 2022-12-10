using System;
using UnityEngine;

namespace Features.Targeting
{
    public class TargetProvider : MonoBehaviour
    {
        public Action<Action<GameObject>> CharacterTargetProvider;

        public Func<Vector3> CurrentMousePositionProvider;

        public Action<Action<Vector3>> MousePositionSelectProvider;
        
        public void GetCharacterTarget(Action<GameObject> callback)
        {
            CharacterTargetProvider.Invoke(callback);
        }

        public Vector3 GetMousePosition()
        {
            return CurrentMousePositionProvider.Invoke();
        }

        public void PickMousePosition(Action<Vector3> callback)
        {
            MousePositionSelectProvider.Invoke(callback);
        }
    }
}