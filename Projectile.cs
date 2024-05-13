using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;

    private float lifetime;

    private Animator animator;
    private BoxCollider2D boxCollider;
    private TilePainter tp;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        tp = FindObjectOfType<TilePainter>();

        if (tp == null)
        {
            Debug.LogError("TilePainter not found in the scene.");
        }
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 5)
        {
            Deactivate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hit)
        {
            hit = true;
            boxCollider.enabled = false;
            animator.SetTrigger("explode");

        }
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;
        }

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    // This method is called by an animation event
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
