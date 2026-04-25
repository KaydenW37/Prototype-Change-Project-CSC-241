using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    public float mouse_x;
    public float mouse_y;

    [SerializeField] private Camera _cam; 

    void Update()
    {
        if (_cam == null || Mouse.current == null) return;

        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = _cam.ScreenToWorldPoint(mousePos);

<<<<<<< Updated upstream
        mouse_y = mousePos.y;
<<<<<<< Updated upstream
        bool right = GameObject.Find("Player").GetComponent<PlayerController>()._isFacingRight;

        if (right == true) {
            mouse_x = mousePos.x;
        }
        else {
            mouse_x = (mousePos.x * -1);
        }
=======
        
            mouse_x = mousePos.x;
       
>>>>>>> Stashed changes
    }

    }

=======
        mouse_x = mouseWorld.x;
        mouse_y = mouseWorld.y;
    }
}
>>>>>>> Stashed changes
