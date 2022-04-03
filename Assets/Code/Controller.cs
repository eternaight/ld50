using UnityEngine;

public class Controller : MonoBehaviour {
    public static Controller main;

    private Vector2 velocity;
    [SerializeField] private float speed;
    [SerializeField] private Transform pointerTransform;
    [SerializeField] private SpriteAnimator spriteAnimator;
    [SerializeField] private Transform modelLight;

    private float pointerRadius = 1;
    private float pointerLength = 0.5f;

    public Inventory inventory;

    private void Awake() {
        main = this;
    }

    private void Start() {
        inventory = Inventory.Main;
    }

    private void Update() {
        Move();
        PositionPointer();
        TryInteract();
        UpdateRenderer();
    }

    private void Move() {
        var move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (move != Vector2.zero) {
            velocity += move * speed;
        }

        velocity = Vector2.Lerp(velocity, Vector2.zero, Time.deltaTime / speed);

        transform.Translate(velocity * Time.deltaTime);
    }

    private void PositionPointer() {
        var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0;
        var offset = Vector3.ClampMagnitude(cursorPosition - transform.position, pointerRadius);

        pointerTransform.localPosition = offset;
        pointerTransform.eulerAngles = Vector3.forward * (Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg);
    }

    private void TryInteract() {
        if (Input.GetMouseButtonDown(0)) {
            var hit = Physics2D.Raycast(transform.position, pointerTransform.localPosition, pointerRadius + pointerLength / 2);
            if (hit) {
                var interactable = hit.collider.gameObject.GetComponent<IInteractable>();
                if (interactable != null) {
                    interactable.Interact(this);
                }
            }
        }
    }

    private void UpdateRenderer() {
        var facingRight = spriteAnimator.GetFlipX();
        var offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        if (facingRight && offset.x < 0) {
            spriteAnimator.SetFlipX(false);
            modelLight.localScale = Vector3.one;
        }
        if (!facingRight && offset.x > 0) {
            spriteAnimator.SetFlipX(true);
            modelLight.localScale = new Vector3(-1, 1, 1);
        }
    }

    public Vector3 GetPointerPosition() => pointerTransform.position;
}
