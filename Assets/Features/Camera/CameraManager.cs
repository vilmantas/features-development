using Managers;

namespace Features.Camera
{
    public class CameraManager : SingletonManager<CameraManager>
    {
        protected override void DoSetup()
        {
            if (UnityEngine.Camera.allCameras.Length > 1)
            {
                Destroy(gameObject);

                return;
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}