using System;
using System.Text;
using Features.Buffs;
using TMPro;
using UnityEngine;

namespace DebugScripts.Buffs
{
    public class BuffDisplayDebug : MonoBehaviour
    {
        public BuffController BuffController;

        public TextMeshProUGUI Text;

        private void FixedUpdate()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(BuffController.ActiveBuffs.Count);
            sb.Append(Environment.NewLine);

            foreach (var buffControllerActiveBuff in BuffController.ActiveBuffs)
            {
                sb.Append(
                    $"{buffControllerActiveBuff.Metadata.Name}({buffControllerActiveBuff.Stacks}): {buffControllerActiveBuff.DurationLeft:F}");
                sb.Append(Environment.NewLine);
            }

            Text.text = sb.ToString();
        }
    }
}