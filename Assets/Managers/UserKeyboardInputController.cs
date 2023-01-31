using System;
using UnityEngine;

namespace Managers
{
    public class UserKeyboardInputController : SingletonManager<UserKeyboardInputController>
    {
        public Action<int> OnSkillActivationRequested;

        public Action OnAttackActivationRequested;

        public Action<bool> OnRunningToggled;
        
        private void Update()
        {
            for (var i = 1; i < 10; i++)
            {
                var alpha = Enum.Parse<KeyCode>($"Alpha{i}");
                var keypad = Enum.Parse<KeyCode>($"Keypad{i}");  
                
                if (Input.GetKeyDown(alpha) || Input.GetKeyDown(keypad))
                {
                    OnSkillActivationRequested?.Invoke(i - 1);
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                OnRunningToggled?.Invoke(true);
            }
            
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                OnRunningToggled?.Invoke(false);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnAttackActivationRequested?.Invoke();
            }
        }
    }
}