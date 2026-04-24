using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    public float mouse_x;
    public float mouse_y;

    // Update is called once per frame
    void Update()
    {
        Vector3 mouse_position = Input.mousePosition;
        Vector3 mousePos = Mouse.current.position.ReadValue();

        mouse_y = mousePos.y;
        bool right = GameObject.Find("Player").GetComponent<PlayerController>()._isFacingRight;

        if (right == true) {
            mouse_x = mousePos.x;
        }
        else {
            mouse_x = (mousePos.x * -1);
        }
    }
}
