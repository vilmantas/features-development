using Features.Camera;
using Features.Character;
using Features.CharacterModel;
using Features.DestinationPointer;
using Features.Movement;
using Features.Targeting;
using Integrations.Actions;
using UnityEngine;

namespace Managers
{
    public class GameplayManager : SingletonManager<GameplayManager>
    {
        [HideInInspector] public Player Player;

        public bool DisableInput;

        public bool DisableMovement;

        private UserMouseInputController m_MouseInputController;

        private void Start()
        {
            var cameraManager = CameraManager.Instance;

            var characterModel = Player.GetComponentInChildren<CharacterModelController>();

            cameraManager.ChangeTarget(characterModel.CameraFollow, characterModel.CameraTarget);

            m_MouseInputController = UserMouseInputController.Instance;

            m_MouseInputController.OnGameGroundClicked += OnGameGroundClicked;

            m_MouseInputController.OnCharacterLocationClicked += OnCharacterLocationClicked;

            LocationProvider.OnOverlayActivated += OnOverlayActivated;

            LocationProvider.OnOverlayDisabled += OnOverlayDisabled;
        }

        protected override void DoSetup()
        {
            Player = GameObject.Find("Player").GetComponent<Player>();
        }

        private void OnOverlayDisabled()
        {
            DisableInput = false;
            DisableMovement = false;
        }

        private void OnOverlayActivated(OverlayInfo info)
        {
            DisableInput = true;

            if (info.BlockMovementActions)
            {
                DisableMovement = true;
            }
        }

        private void OnGameGroundClicked(Vector3 point)
        {
            if (DisableInput && DisableMovement) return;

            DestinationPointerManager.Show(point, 1f);

            var movePayload =
                Move.MakePayload(Player.gameObject, new MoveActionData(point));

            Player.m_ActionsController.DoAction(movePayload);
        }

        private void OnCharacterLocationClicked(Vector3 characterLocation)
        {
            if (DisableInput) return;

            var movePayload =
                Move.MakePayload(Player.gameObject, new MoveActionData(characterLocation));

            Player.m_ActionsController.DoAction(movePayload);
        }
    }
}