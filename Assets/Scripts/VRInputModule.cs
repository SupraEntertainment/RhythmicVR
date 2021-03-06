﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRInputModule : BaseInputModule
{
    [SerializeField] private List<Pointer> pointers = new List<Pointer>();
    private Pointer _currentPointer = null;

    public PointerEventData Data { get; private set; } = null;

    protected override void Start() {
        Data = new PointerEventData(eventSystem);
        SetPointer(0);
    }

    protected void SetPointer(int id) {
        foreach (var pointer in pointers) {
            pointer.ToggleLaser(false);
        }
        _currentPointer = pointers[id];
        _currentPointer.ToggleLaser(true);
        Data.position = new Vector2(_currentPointer.Camera.pixelWidth / 2f, _currentPointer.Camera.pixelHeight / 2f);
        Canvas[] canvases = Resources.FindObjectsOfTypeAll<Canvas>();
        foreach (var canvas in canvases) {
            canvas.worldCamera = _currentPointer.Camera;
        }
    }

    public override void Process()
    {
        eventSystem.RaycastAll(Data, m_RaycastResultCache);
        Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);

        HandlePointerExitAndEnter(Data, Data.pointerCurrentRaycast.gameObject);

        ExecuteEvents.Execute(Data.pointerDrag, Data, ExecuteEvents.dragHandler);
    }

    public void Press()
    {
        Data.pointerPressRaycast = Data.pointerCurrentRaycast;

        Data.pointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(Data.pointerPressRaycast.gameObject);
        Data.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(Data.pointerPressRaycast.gameObject);

        ExecuteEvents.Execute(Data.pointerPress, Data, ExecuteEvents.pointerDownHandler);
        ExecuteEvents.Execute(Data.pointerDrag, Data, ExecuteEvents.beginDragHandler);
    }

    public void Release()
    {
        GameObject pointerRelease = ExecuteEvents.GetEventHandler<IPointerClickHandler>(Data.pointerCurrentRaycast.gameObject);

        if (Data.pointerPress == pointerRelease)
            ExecuteEvents.Execute(Data.pointerPress, Data, ExecuteEvents.pointerClickHandler);

        ExecuteEvents.Execute(Data.pointerPress, Data, ExecuteEvents.pointerUpHandler);
        ExecuteEvents.Execute(Data.pointerDrag, Data, ExecuteEvents.endDragHandler);

        Data.pointerPress = null;
        Data.pointerDrag = null;

        Data.pointerCurrentRaycast.Clear();
    }
    
    public void Scroll()
    {
        var scrollDelta = Data.scrollDelta;
        if (!Mathf.Approximately(scrollDelta.sqrMagnitude, 0.0f))
        {
            var scrollHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(Data.pointerEnter);
            ExecuteEvents.ExecuteHierarchy(scrollHandler, Data, ExecuteEvents.scrollHandler);
        }
    }
}
