using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cube : MonoBehaviour
{
    PlayerControls controls;
    Vector2 move;
    Vector2 rotate;

    void Awake()
    {
        controls = new PlayerControls();

        // Xử lý tăng/giảm kích thước
        controls.Gameplay.Grow.performed += ctx => Grow();
        controls.Gameplay.Shrink.performed += ctx => Shrink();

        // Xử lý di chuyển
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;

        // Xử lý xoay
        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;

        // 🖱 Xử lý Click chuột
        controls.Newactionmap.LeftClick.performed += ctx => Debug.Log("LeftClick: <Mouse>/leftButton clicked!");
        controls.Newactionmap.RightClick.performed += ctx => Debug.Log("RightClick: <Mouse>/rightButton clicked!");

        // 🖱 Xử lý vị trí chuột
        controls.Newactionmap.MousePosition.performed += ctx =>
        {
            Vector2 position = ctx.ReadValue<Vector2>();
            Debug.Log($"MousePosition: {position}");
        };

        // 🖱 Xử lý cuộn chuột
        controls.Newactionmap.ScrollWheel.performed += ctx =>
        {
            Vector2 scrollDelta = ctx.ReadValue<Vector2>();
            Debug.Log($"ScrollWheel: {scrollDelta}");
        };
    }

    void Shrink()
    {
        transform.localScale /= 1.1f;
    }

    void Grow()
    {
        transform.localScale *= 1.1f;
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.Newactionmap.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
        controls.Newactionmap.Disable();
    }

    void Update()
    {
        // Xử lý di chuyển
        Vector2 m = new Vector2(-move.x, move.y) * Time.deltaTime;
        transform.Translate(m, Space.World);

        // Xử lý xoay
        Vector2 r = new Vector2(-rotate.y, -rotate.x) * 100f * Time.deltaTime;
        transform.Rotate(r, Space.World);
    }
}
