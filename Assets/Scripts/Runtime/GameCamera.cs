using System;
using System.Collections;
using System.Collections.Generic;
using OmegaLeo.Toolbox.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameCamera : MonoBehaviour
{
    [ColoredHeader("Configuration")] 
    private Vector3 _origin;
    private Camera _camera;
    private bool _isDragging;
    private Vector3 _difference;
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        GameManager.instance.OnMouseDrag += OnMouseDrag;
    }

    private void OnMouseDrag(InputAction.CallbackContext ctx)
    {
        if (ctx.started) _origin = GetMousePosition();

        _isDragging = ctx.started || ctx.performed;
    }

    private void LateUpdate()
    {
        if (!_isDragging) return;

        _difference = GetMousePosition() - transform.position;
        transform.position = _origin - _difference;
    }

    private void OnDestroy()
    {
        GameManager.instance.OnMouseDrag -= OnMouseDrag;
    }

    private Vector3 GetMousePosition() => _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
}
