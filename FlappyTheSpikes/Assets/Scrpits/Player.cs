using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] LayerMask wall;
    Animator animator;

    PlayerController playerController;
    ObstaclesBehaviour obstacles;

    public float horizontalSpeed;
    public float jumpPower;

    public int amountMoney;

    private void Start()
    {
        obstacles = FindObjectOfType<ObstaclesBehaviour>();
        
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    public bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Contains(wall, collision.gameObject.layer))
        {
            CameraShake shake = Camera.main.GetComponent<CameraShake>();
            if (shake != null)
                StartCoroutine(shake.Shake(0.1f, 0.2f));

            animator.SetTrigger("Die");
            obstacles.obstaclesActivated = false;
            playerController.rb.isKinematic = true;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
