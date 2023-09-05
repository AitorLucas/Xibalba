using System;
using UnityEngine;

public class InputManager : ISingleton<InputManager> {

    private GameInputActions gameInputActions;

    // public event EventHandler OnShotFireAction;

    protected override void Awake() {
        base.Awake();
        gameInputActions = new GameInputActions();
        gameInputActions.Player.Enable();

        // gameInputActions.Player.ShotFire.performed += ShotFire_Performed;
    }

    private void OnDestroy() {
        // gameInputActions.Player.ShotFire.performed -= ShotFire_Performed;

        gameInputActions.Dispose();
    }

    // private void ShotFire_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
    //     OnShotFireAction?.Invoke(this, EventArgs.Empty);
    // }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = gameInputActions.Player.Movement.ReadValue<Vector2>();
        return inputVector.normalized;
    }

    public Vector2 GetShotVectorNormalized() {
        Vector2 shotVector = gameInputActions.Player.ShotFire.ReadValue<Vector2>();
        return shotVector.normalized;
    }

    // public Vector2 GetMousePositionVector() {
        // Vector2 mousePositionVector = gameInputActions.Player.Aim.ReadValue<Vector2>();
        // return mousePositionVector;
    // }
}