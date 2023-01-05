using Features.Camera;
using Features.Character;
using Features.CharacterModel;
using UnityEngine;

namespace Managers
{
    public class GameplayManager : SingletonManager<GameplayManager>
    {
        public Player Player;

        private void Start()
        {
            Player = GameObject.Find("Player").GetComponent<Player>();

            var cameraManager = GameObject.Find("camera_manager").GetComponent<CameraManager>();

            var characterModel = Player.GetComponentInChildren<CharacterModelController>();
            
            cameraManager.ChangeTarget(characterModel.CameraFollow, characterModel.CameraTarget);
        }
    }
}