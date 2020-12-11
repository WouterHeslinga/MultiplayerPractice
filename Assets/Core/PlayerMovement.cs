using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed;
    
    private void Update()
    {
        if(!isLocalPlayer)
            return;

        var movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.position += movement.normalized * (Time.deltaTime * speed);

        Shooting();
        LookAtMouse();
    }
    
    private void LookAtMouse()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var lookDir = mousePos - transform.position;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Shooting()
    {
        var hit = Physics2D.Raycast(transform.position, transform.up, 1.5f);
        Debug.DrawRay(transform.position, transform.up, Color.red);
        
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if(!hit || hit.transform.GetComponent<Rigidbody2D>() == null)
                return;
            
            CmdAddForce(transform.up);
        }
    }

    [Command]
    public void CmdAddForce(Vector3 direction)
    {
        FindObjectOfType<FootBall>().AddForce(direction);
    }
}