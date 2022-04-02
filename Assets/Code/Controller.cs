using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Controller main;

    private Vector2 velocity;
    [SerializeField] private float speed;
    [SerializeField] private Transform pointerTransform;

    private float pointerRadius = 1;
    private float pointerLength = 0.5f;

    public Inventory inventory;

    private void Awake() {
        main = this;
    }

    private void Start() {
        inventory = new Inventory();
    }

    private void Update() {
        Move();
        PositionPointer();
        TryInteract();
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
        var offset = cursorPosition - transform.position;

        if (offset.sqrMagnitude < 1) {
            pointerTransform.gameObject.SetActive(false);
        } else {
            pointerTransform.gameObject.SetActive(true);
            pointerTransform.localPosition = offset.normalized * pointerRadius;
            pointerTransform.eulerAngles = Vector3.forward * (Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg);
        }
    }

    private void TryInteract() {
        if (Input.GetMouseButton(0)) {
            var hit = Physics2D.Raycast(transform.position, pointerTransform.localPosition, pointerRadius + pointerLength / 2);
            if (hit) {
                var interactable = hit.collider.gameObject.GetComponent<IInteractable>();
                if (interactable != null) {
                    interactable.Interact(this);
                }
            }
        }
    }
}
