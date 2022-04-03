using UnityEngine;

public class Controller : MonoBehaviour {
    public static Controller main;

    private Vector2 velocity;
    [SerializeField] private float speed;
    [SerializeField] private SpriteAnimator spriteAnimator;
    [SerializeField] private Transform modelLight;
    private ControllerPointer pointer;

    public Inventory inventory;
    private bool warm;

    private void Awake() {
        main = this;
    }

    private void Start() {
        inventory = Inventory.Main;
        pointer = GetComponentInChildren<ControllerPointer>();
        spriteAnimator.PlayClip(0);
    }

    private void Update() {
        Move();
        TryInteract();
        UpdateRenderer();
        CheckBonfireProximity();
        inventory.UpdateConvoyPositions(transform.position);
    }

    private void Move() {
        var move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (move != Vector2.zero) {
            velocity += move * speed;
        }

        velocity = Vector2.Lerp(velocity, Vector2.zero, Time.deltaTime / speed);

        transform.Translate(velocity * Time.deltaTime);
    }

    private void TryInteract() {
        var hit = Physics2D.Raycast(transform.position, pointer.transform.localPosition, pointer.Extent);
        if (hit) {
            var interactable = hit.collider.gameObject.GetComponent<IInteractable>();
            if (interactable != null) {
                pointer.OnHoverOver(interactable);
                if (Input.GetMouseButtonDown(0)) {
                    interactable.Interact(this);
                }
            } else {
                pointer.OnStopHover();
            }
        } else {
            pointer.OnStopHover();
        }
    }

    private void UpdateRenderer() {
        var facingRight = spriteAnimator.GetFlipX();
        var offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if (facingRight && offset.x < 0) {
            spriteAnimator.SetFlipX(false);
            pointer.SetFlipX(false);
        }
        if (!facingRight && offset.x > 0) {
            spriteAnimator.SetFlipX(true);
            pointer.SetFlipX(true);
        }
    }

    private void CheckBonfireProximity() {
        var newWarm = (Vector3.Distance(transform.position, Vector3.zero) < 2);
        if (warm != newWarm) {
            spriteAnimator.PlayClip(newWarm ? 1 : 0);
            warm = newWarm;
        }
    }

    public Vector3 GetPointerPosition() => pointer.transform.position;
}
