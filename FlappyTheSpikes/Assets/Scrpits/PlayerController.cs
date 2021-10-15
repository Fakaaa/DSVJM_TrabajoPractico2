using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IPointerClickHandler
{
    Player player;
    Rigidbody rb;

    float t;
    float timeToTap;

    void Start()
    {
        t = 0;
        timeToTap = 0.6f;

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
}
