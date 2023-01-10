using System;
using Features.Camera;
using Features.Character;
using Features.CharacterModel;
using UnityEngine;

namespace Managers
{
    public class GameplayManager : SingletonManager<GameplayManager>
    {
        [HideInInspector] public Player Player;

        protected override void DoSetup()
        {
            Player = GameObject.Find("Player").GetComponent<Player>();
        }

        private void Start()
        {
            var cameraManager = CameraManager.Instance;

            var characterModel = Player.GetComponentInChildren<CharacterModelController>();
            
            cameraManager.ChangeTarget(characterModel.CameraFollow, characterModel.CameraTarget);
        }
    }
}