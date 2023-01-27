using Features.Buffs;
using UnityEngine;

namespace Features.Character.Configurations
{
    [CreateAssetMenu(fileName = "RENAME ME", menuName = "Configurations/Character Buffs Configuration", order = 0)]
    public class CharacterBuffConfiguration_SO : ScriptableObject
    {
        public BuffGroup_SO NegativeBuffs;

        public BuffGroup_SO PositiveBuffs;
    }
}