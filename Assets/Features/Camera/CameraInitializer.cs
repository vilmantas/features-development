using UnityEngine;

namespace Features.Camera
{
    public static class CameraInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitializeCameraScene()
        {
            var x = Resources.Load<GameObject>("Camera");

            var zz = GameObject.Instantiate(x);
        }
    }
}