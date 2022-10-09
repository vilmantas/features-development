using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContextMenuUIController : MonoBehaviour, IPointerExitHandler
{
    public ContextMenuOptionUI ContextMenuItemPrefab;

    private Action<string> Callback;

    private List<ContextMenuOptionUI> CurrentOptions = new();

    private CanvasGroup Group;

    private RectTransform Rect;

    private void Start()
    {
        Group = GetComponent<CanvasGroup>();

        Rect = GetComponent<RectTransform>();

        Hide();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var raycastObject = eventData.pointerCurrentRaycast.gameObject;

        if (raycastObject == null)
        {
            Hide();
            return;
        }

        if (raycastObject == gameObject) return;

        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (raycastObject == child.gameObject) return;
        }

        Hide();
    }

    public void Hide()
    {
        Group.alpha = 0;

        foreach (var currentOption in CurrentOptions)
        {
            Destroy(currentOption.gameObject);
        }

        CurrentOptions.Clear();

        Callback = null;
    }

    public void Show(Vector2 position, List<string> options, Action<string> selectionCallback)
    {
        if (Math.Abs(Group.alpha - 1) < 0.1) return;

        Callback = selectionCallback;

        position.y += 10;
        Group.alpha = 1;

        foreach (var option in options)
        {
            var instance = Instantiate(ContextMenuItemPrefab, transform);

            instance.SetData(option);

            instance.ContextOptionSelected.AddListener(ContextMenuDone);

            CurrentOptions.Add(instance);
        }

        position.x -= 5;
        position.y -= 5;

        transform.position = position;
    }

    private void ContextMenuDone(string option)
    {
        Callback.Invoke(option);

        Hide();
    }
}