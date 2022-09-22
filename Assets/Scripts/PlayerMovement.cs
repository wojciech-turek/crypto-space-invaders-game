using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 rawInput;

    Shooter shooter;

    [SerializeField]
    float moveSpeed = 5f;

    Vector2 minBounds;

    Vector2 maxBounds;

    [SerializeField]
    float paddingLeft = 0f;

    [SerializeField]
    float paddingRight = 0f;

    [SerializeField]
    float paddingTop = 0f;

    [SerializeField]
    float paddingBottom = 0f;

    private void Awake()
    {
        shooter = GetComponent<Shooter>();
    }

    private void Start()
    {
        InitBounds();
    }

    void Update()
    {
        Move();
    }

    void InitBounds()
    {
        Camera mainCamera = Camera.main;
        minBounds = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        maxBounds = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
    }

    void Move()
    {
        Vector2 delta = rawInput * moveSpeed * Time.deltaTime;
        Vector2 newPos = new Vector2();
        newPos.x =
            Mathf
                .Clamp(transform.position.x + delta.x,
                minBounds.x + paddingLeft,
                maxBounds.x - paddingRight);
        newPos.y =
            Mathf
                .Clamp(transform.position.y + delta.y,
                minBounds.y + paddingBottom,
                maxBounds.y - paddingTop);
        transform.position = newPos;
    }

    void OnMove(InputValue value)
    {
        rawInput = value.Get<Vector2>();
    }

    void OnFire(InputValue value)
    {
        if (shooter != null)
        {
            shooter.isFiring = value.isPressed;
        }
    }
}
