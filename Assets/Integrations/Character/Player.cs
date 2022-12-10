using Features.Targeting;
using UnityEngine;

namespace Features.Character
{
    public class Player : Modules.Character
    {
        protected override void ConfigureTargetingSystem(TargetProvider provider)
        {
            provider.CurrentMousePositionProvider = LocationProvider.MousePositionProvider;

            provider.CharacterTargetProvider = LocationProvider.StartCharacterSelect;
        }

        protected override void SetupGameHooks()
        {
            
        }
    }
}