using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //unneccessary

    #region Singleton
    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    private InputActions inputActions;
    Vector2 mouseScreenPosition;
    
    private void OnEnable()
    {
        inputActions = new InputActions();

        inputActions.Enable();
        inputActions.Player.Click.performed += OnClickPerformed;
        inputActions.Player.Click.canceled += OnClickCanceled;
        inputActions.Player.Position.performed += OnPositionUpdate;
    }

    private void OnDisable()
    {
        inputActions.Player.Click.performed -= OnClickPerformed;
        inputActions.Player.Click.canceled -= OnClickCanceled;
        inputActions.Disable();
    }

    private void OnPositionUpdate(InputAction.CallbackContext context)
    {
        mouseScreenPosition = context.ReadValue<Vector2>();
        Debug.Log("Mouse Position: " + mouseScreenPosition);
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Mouse down");
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == this.gameObject)
        {
            //isDragging = true;
            //offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.WorldToScreenPoint(transform.position).z));
        }
    }

    private void OnClickCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("Mouse up");
        //isDragging = false;
    }
}
 