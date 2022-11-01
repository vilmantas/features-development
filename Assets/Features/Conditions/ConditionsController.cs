using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.Conditions
{
    public class ConditionsController : MonoBehaviour
    {
        public Action<StatusCondition> OnConditionAdded;

        public Action<StatusCondition> OnConditionRemoved;
        
        public List<StatusCondition> Conditions = new List<StatusCondition>();

        public void AddCondition(StatusCondition condition)
        {
            Conditions.Add(condition);
            
            OnConditionAdded?.Invoke(condition);
        }
    }
}