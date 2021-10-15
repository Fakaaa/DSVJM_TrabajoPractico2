using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] LayerMask wall;

    Animator animator;

    Player player;
    Rigidbody rb;

    float t;
    float timeToTap;

    ObstaclesBehaviour obstacles;

    void Start()
    {
        obstacles = FindObjectOfType<ObstaclesBehaviour>();

        t = 0;
        timeToTap = 0.6f;

        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        t += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0) && t > timeToTap)
        {
            MakeJump();
            t = 0;
        }

        if (Input.GetKeyDown(KeyCode.F))
            FacuLoggerPlug.SendLog("UWU");
    }

    public void MakeJump()
    {
        Vector2 upDirection = new Vector2(0, player.jumpPower);

        Vector3.Normalize(upDirection);
        rb.velocity = new Vector3(0, upDirection.y, 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MakeJump();
    }

    public bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(Contains(wall, collision.gameObject.layer))
        {
            CameraShake shake = Camera.main.GetComponent<CameraShake>();
            if (shake != null)
                StartCoroutine(shake.Shake(0.1f, 0.2f));

            animator.SetTrigger("Die");
            obstacles.obstaclesActivated = false;
            rb.isKinematic = true;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
