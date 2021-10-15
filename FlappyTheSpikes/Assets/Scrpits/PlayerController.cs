using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IPointerClickHandler
{
    Player player;
    Rigidbody rb;

    void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            MakeJump();

        if (rb.velocity.y < 0)
            rb.mass = 3;
    }

    public void MakeJump()
    {
        rb.mass = 1;
        
        Vector2 upDirection = new Vector2(0, player.jumpPower);
        Debug.Log(rb.velocity.y );

        if(rb.velocity.y < upDirection.y)
            rb.AddForce(upDirection, ForceMode.Impulse);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MakeJump();
    }
}
