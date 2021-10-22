using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _rotationSpeed = 3f;

    private Vector2 _rotation;
    private Vector3 _moveVector;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        _rotation += new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        transform.eulerAngles = _rotation * _rotationSpeed;

        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        _moveVector = new Vector3(x, 0, z) * _moveSpeed;
    }

    private void FixedUpdate()
    {
        _rb.velocity = transform.TransformDirection(_moveVector);   
    }
}
