using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
    public class UserMouseInputController : SingletonManager<UserMouseInputController>
    {
        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (EventSystem.current != null &&
                    EventSystem.current.IsPointerOverGameObject()) return;


                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
                {
                    // var movePayload =
                    //     Move.MakePayload(RootGameObject, new MoveActionData(hit.point));
                    //
                    // PlayerInstance.m_ActionsController.DoAction(movePayload);
                }
            }
        }
    }
}