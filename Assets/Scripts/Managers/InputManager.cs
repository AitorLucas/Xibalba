using System;
using UnityEngine;

public class InputManager : ISingleton<InputManager> {

    private GameInputActions gameInputActions;

    public event EventHandler OnSpellCastAction;
    public event EventHandler OnLightSpellSelectedAction;
    public event EventHandler OnHeavySpellSelectedAction;
    public event EventHandler OnDashAction;

    protected override void Awake() {
        base.Awake();
        gameInputActions = new GameInputActions();
        gameInputActions.Player.Enable();

        gameInputActions.Player.ShotFire.performed += CastSpell_Performed;
        gameInputActions.Player.LightSpell.performed += LightSpell_Performed;
        gameInputActions.Player.HeavySpell.performed += HeavySpell_Performed;
        gameInputActions.Player.Dash.performed += Dash_Performed;
    }

    private void OnDestroy() {
        gameInputActions.Player.ShotFire.performed -= CastSpell_Performed;
        gameInputActions.Player.LightSpell.performed -= LightSpell_Performed;
        gameInputActions.Player.HeavySpell.performed -= HeavySpell_Performed;
        gameInputActions.Player.Dash.performed -= Dash_Performed;

        gameInputActions.Dispose();
    }

    private void CastSpell_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnSpellCastAction?.Invoke(this, EventArgs.Empty);
    }

    private void LightSpell_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnLightSpellSelectedAction?.Invoke(this, EventArgs.Empty);
    }

    private void HeavySpell_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnHeavySpellSelectedAction?.Invoke(this, EventArgs.Empty);
    }

    private void Dash_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnDashAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = gameInputActions.Player.Movement.ReadValue<Vector2>();
        return inputVector.normalized;
    }

    public Vector2 GetShotVectorNormalized() {
        Vector2 shotVector = gameInputActions.Player.ShotFire.ReadValue<Vector2>();
        return shotVector.normalized;
    }

    public Vector2 GetShotDirectionVector() {
        Vector2 shotVector = gameInputActions.Player.ShotDirection.ReadValue<Vector2>();
        return shotVector;
    }

}