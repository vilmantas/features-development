using System;
using _SampleGames.Survivr.SurvivrFeatures.Actions;
using Features.Actions;
using Features.Combat;
using Features.Stats.Base;
using UnityEngine;

namespace _SampleGames.Survivr.SurvivrFeatures.Combat
{
    public class CombatManager : MonoBehaviour
    {
        private CombatController m_CombatController;

        private StatsController m_StatsController;

        private ActionsController m_ActionsController;
        
        private void Awake()
        {
            var root = transform.root;
            
            m_CombatController = root.GetComponentInChildren<CombatController>();

            m_StatsController = root.GetComponentInChildren<StatsController>();
            
            m_ActionsController = root.GetComponentInChildren<ActionsController>();
            
            m_CombatController.OnHit += OnHit;
        }

        private void OnHit(AttackMetadataBase attack, Action<HitMetadataBase> callback)
        {
            var attackData = attack as AttackData;

            var actualDamage = attackData.Damage - m_StatsController.CurrentStats["Defence"].Value;

            var payload = new ActionActivationPayload(new(nameof(Damage)), attackData.Source, transform.root.gameObject);

            m_ActionsController.DoAction(new DamageActionPayload(payload, actualDamage));
            
            callback.Invoke(new HitData(actualDamage));
        }
    }
}