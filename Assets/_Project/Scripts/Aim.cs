using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Aim : ISingleton<Aim> {

    [SerializeField] private Sprite shotSprite;
    [SerializeField] private Sprite laserSprite;
    [SerializeField] private Sprite explosionSprite;
    [SerializeField] private Sprite breathSprite;

    private SpriteRenderer spriteRenderer;

    private bool isClipped = true;

    protected override void Awake() {
        base.Awake();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        Player.Instance.GetPlayerController().OnPlayerSpellTypeChanged += PlayerController_OnPlayerSpellTypeChanged;

        ChooseSprite(SpellType.None);
    }

    private void Update() {
        Vector2 mousePosition = InputManager.Instance.GetShotDirectionVector();
        Vector2 mouseInWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        Vector2 playerPosition = Player.Instance.transform.position;

        Vector2 playerToMouse = mouseInWorldPosition - playerPosition;

        if (isClipped) { 
            transform.position = playerPosition + (playerToMouse.normalized * 0.7f);
        } else {
            transform.position = mouseInWorldPosition;
        }

        Quaternion toRatation = Quaternion.LookRotation(Vector3.forward, playerToMouse);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRatation, 360);
    }

    private void PlayerController_OnPlayerSpellTypeChanged(object sender, PlayerController.OnPlayerSpellTypeChangedArgs e) {
        ChooseCliped(e.spellType);
        ChooseSprite(e.spellType);
        ScaleSprite(e.spellType);
        RotateSprite(e.spellType);
    }

    private void ChooseCliped(SpellType spellType) {
        switch (spellType) {
            case SpellType.None:
                isClipped = true;
                break;
            case SpellType.Laser:
                isClipped = true;
                break;
            case SpellType.Breath:
                isClipped = true;
                break;
            case SpellType.Explosion:
                isClipped = false;
                break;
        }
    }

    private void ChooseSprite(SpellType spellType) {
        Sprite newSprite = null;
        switch (spellType) {
            case SpellType.Laser:
                newSprite = laserSprite;
                break;
            case SpellType.Breath:
                newSprite = breathSprite;
                break;
            case SpellType.Explosion:
                newSprite = explosionSprite;
                break;
            case SpellType.None:
                newSprite = shotSprite;
                break;
        }
        spriteRenderer.sprite = newSprite;
    }

    private void ScaleSprite(SpellType spellType) {
        Vector3 newScale = Vector3.one;
        switch (spellType) {
            case SpellType.Laser:
                newScale = Vector3.one * 0.3f;
                break;
            case SpellType.Breath:
                newScale = Vector3.one * 0.3f;
                break;
            case SpellType.Explosion:
                newScale = Vector3.one * 1.2f;
                break;
            case SpellType.None:
                newScale = Vector3.one * 2f;
                break;
        }
        transform.localScale = newScale;
    }

    private void RotateSprite(SpellType spellType) {
        Vector3 newRotation = Vector3.one;
        switch (spellType) {
            case SpellType.Laser:
                newRotation = Vector3.one * 0.3f;
                break;
            case SpellType.Breath:
                newRotation = Vector3.one * 0.3f;
                break;
            case SpellType.Explosion:
                newRotation = Vector3.one * 1.2f;
                break;
            case SpellType.None:
                newRotation = Vector3.one * 2f;
                break;
        }
        // transform.quaternion.Euler = newRotation;
    }
}
