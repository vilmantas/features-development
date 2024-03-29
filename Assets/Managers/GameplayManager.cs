using Features.Actions;
using Features.Camera;
using Features.Character;
using Features.CharacterModel;
using Features.DestinationPointer;
using Features.Movement;
using Features.Skills;
using Features.Targeting;
using Integrations.Actions;
using Integrations.Skills.Actions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class GameplayManager : SingletonManager<GameplayManager>
    { 
        private Player m_Player;

        public bool DisableInput;

        public bool DisableMovement;

        private UserMouseInputController m_MouseInputController;

        private UserKeyboardInputController m_KeyboardInputController;

        private void Start()
        {
            var cameraManager = CameraManager.Instance;

            var characterModel = m_Player.GetComponentInChildren<CharacterModelController>();

            cameraManager.ChangeTarget(characterModel.CameraFollow, characterModel.CameraTarget);

            m_MouseInputController = UserMouseInputController.Instance;

            m_KeyboardInputController = UserKeyboardInputController.Instance;

            m_MouseInputController.OnGameGroundClicked += OnGameGroundClicked;

            m_MouseInputController.OnCharacterLocationClicked += OnCharacterLocationClicked;
            
            m_KeyboardInputController.OnSkillActivationRequested += OnSkillActivationRequested;
            
            m_KeyboardInputController.OnAttackActivationRequested += OnAttackActivationRequested;
            
            m_KeyboardInputController.OnRunningToggled += OnRunningToggled;

            LocationProvider.OnOverlayActivated += OnOverlayActivated;

            LocationProvider.OnOverlayDisabled += OnOverlayDisabled;
        }

        private void OnAttackActivationRequested()
        {
            var metadata = SkillMetadataRegistry.Implementations["Basic Attack"];
            
            var skillActivation = ActivateSkill.MakePayload(m_Player.gameObject, metadata);

            m_Player.m_ActionsController.DoAction(skillActivation);
        }

        private void OnRunningToggled(bool isRunning)
        {
            ActionActivationPayload payload;
            
            if (isRunning)
            {
                payload = StartRunning.MakePayload(m_Player.gameObject);
            }
            else
            {
                payload = StartWalking.MakePayload(m_Player.gameObject);
            }

            m_Player.m_ActionsController.DoAction(payload);
        }

        private void OnSkillActivationRequested(int obj)
        {
            var skillActivation = ActivateSkill.MakePayload(m_Player.gameObject, obj);

            m_Player.m_ActionsController.DoAction(skillActivation);
        }

        public static Player Player => Instance.m_Player;

        protected override void DoSetup()
        {
            m_Player = GameObject.Find("Player").GetComponent<Player>();
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
                Move.MakePayload(m_Player.gameObject, new MoveActionData(point));

            m_Player.m_ActionsController.DoAction(movePayload);
        }

        private void OnCharacterLocationClicked(Vector3 characterLocation)
        {
            if (DisableInput) return;

            var movePayload =
                Move.MakePayload(m_Player.gameObject, new MoveActionData(characterLocation));

            m_Player.m_ActionsController.DoAction(movePayload);
        }
    }
}