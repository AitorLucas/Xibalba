using System;
using UnityEngine;

public class InputManager : ISingleton<InputManager> {

    private GameInputActions gameInputActions;

    public event EventHandler OnPlayerSpellCastAction;
    public event EventHandler OnPlayerLightSpellSelectedAction;
    public event EventHandler OnPlayerHeavySpellSelectedAction;
    public event EventHandler OnPlayerDashAction;
    public event EventHandler OnDebugTogglePaneAction;

    protected override void Awake() {
        base.Awake();
        gameInputActions = new GameInputActions();
        gameInputActions.Player.Enable();
        gameInputActions.Debug.Enable();

        gameInputActions.Player.CastSpell.performed += PlayerCastSpell_Performed;
        gameInputActions.Player.LightSpell.performed += PlayerLightSpell_Performed;
        gameInputActions.Player.HeavySpell.performed += PlayerHeavySpell_Performed;
        gameInputActions.Player.Dash.performed += PlayerDash_Performed;
        gameInputActions.Debug.TogglePane.performed += DebugTogglePane_Performed;
    }

    private void OnDestroy() {
        gameInputActions.Player.CastSpell.performed -= PlayerCastSpell_Performed;
        gameInputActions.Player.LightSpell.performed -= PlayerLightSpell_Performed;
        gameInputActions.Player.HeavySpell.performed -= PlayerHeavySpell_Performed;
        gameInputActions.Player.Dash.performed -= PlayerDash_Performed;
        gameInputActions.Debug.TogglePane.performed -= DebugTogglePane_Performed;

        gameInputActions.Dispose();
    }

    private void PlayerCastSpell_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPlayerSpellCastAction?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerLightSpell_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPlayerLightSpellSelectedAction?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerHeavySpell_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPlayerHeavySpellSelectedAction?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerDash_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPlayerDashAction?.Invoke(this, EventArgs.Empty);
    }

    private void DebugTogglePane_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnDebugTogglePaneAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = gameInputActions.Player.Movement.ReadValue<Vector2>();
        return inputVector;
    }

    // public Vector2 GetShotVectorNormalized() {
    //     Vector2 shotVector = gameInputActions.Player.ShotFire.ReadValue<Vector2>();
    //     return shotVector;
    // }

    public Vector2 GetShotDirectionVector() {
        Vector2 shotVector = gameInputActions.Player.ShotDirection.ReadValue<Vector2>();
        return shotVector;
    }

}