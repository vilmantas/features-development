using System;
using UnityEngine;

namespace Managers
{
    public class UserKeyboardInputController : SingletonManager<UserKeyboardInputController>
    {
        public Action<int> OnSkillActivationRequested;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0))
            {
                OnSkillActivationRequested?.Invoke(0);
            }
            
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                OnSkillActivationRequested?.Invoke(1);
            }
            
            if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            {
                OnSkillActivationRequested?.Invoke(2);
            }
        }
    }
}