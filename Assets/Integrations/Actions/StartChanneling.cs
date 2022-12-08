using System;
using Features.Actions;
using Features.Combat;
using Features.Movement;
using UnityEngine;

namespace Integrations.Actions
{
    public static class StartChanneling
    {

        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(StartChanneling), OnActivation);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name,
                implementation);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            if (payload is not StartChannelingActionPayload startChannelingActionPayload)
            {
                throw new ArgumentException("Invalid type of payload passed to startChanneling action");
            }

            var channelingController =
                payload.Target.GetComponentInChildren<ChannelingController>();

            var channelCommand = new ChannelingCommand(startChannelingActionPayload.Data.Title,
                startChannelingActionPayload.Data.Duration);
            
            channelingController.StartChanneling(channelCommand);
        }

        public class StartChannelingActionPayload : ActionActivationPayload
        {
            public readonly ChannelingActionData Data;

            public StartChannelingActionPayload(ActionActivationPayload original,
                ChannelingActionData data) : base(original.Action,
                original.Source, original.Target, original.Data)
            {
                Data = data;
            }
        }

        public class ChannelingActionData
        {
            public readonly string Title;
            public readonly float Duration;

            public ChannelingActionData(string title, float duration)
            {
                Title = title;
                Duration = duration;
            }
        }
    }
}