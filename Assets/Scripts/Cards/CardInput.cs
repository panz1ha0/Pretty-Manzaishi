using UnityEngine;
using UnityEngine.Windows;

public class CardInput : MonoBehaviour
{
    PlayerInputActions inputActions;

    public bool dragCard => inputActions.GamePlay.CardController.WasPressedThisFrame();
    public bool releaseCard => inputActions.GamePlay.CardController.WasReleasedThisFrame();
    Vector3 MousePosition;
    public Vector3 GetMousePosition() => MousePosition;
    private void Awake()
    {
        this.inputActions = new PlayerInputActions();
    }
    private void Update()
    {
        MousePosition = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);  
    }
    private void OnEnable()
    {
        EnableInputActions();
    }
    public void EnableInputActions()
    {
        inputActions.GamePlay.Enable();
        //Cursor.lockState = CursorLockMode.Locked;
    }
}