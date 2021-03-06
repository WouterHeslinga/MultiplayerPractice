﻿using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        if(!hasAuthority)
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
        var body = transform;
        var hit = Physics2D.Raycast(body.position, body.up, 1.5f);
        
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if(!hit || hit.transform.TryGetComponent<FootBall>(out var ball) == false)
                return;
            
            CmdAddForce(ball.gameObject, gameObject);
        }
    }

    [Command]
    public void CmdAddForce(GameObject ball, GameObject shooter)
    {
        ball.GetComponent<FootBall>().AddForce(shooter);
    }
}