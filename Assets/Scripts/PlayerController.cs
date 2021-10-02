using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public Animator Animator;

    private Rigidbody2D _rigidbody;
    private Transform _transform;

    private Vector2 movement;

    private List<ToolScript> _interactables;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _transform = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody.velocity = movement;

        var walking = movement.x != 0 || movement.y != 0;
        if(walking)
            _transform.rotation = Quaternion.Euler(
                transform.rotation.x,
                transform.rotation.y,
                Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg);

        Animator.SetBool("IsWalking", walking);
    }

    public void OnMove(InputValue input)
    {
        movement = input.Get<Vector2>() * Speed;
    }

    public void OnFire(InputValue input) 
    {
        GameObject ClosestObject = _interactables[0].gameObject;
        float ClosestDistance = (_interactables[0].gameObject.transform.position - gameObject.transform.position).magnitude;

        for (int i = 1; i < _interactables.Count; i++)
        {
            float D = (_interactables[i].gameObject.transform.position - gameObject.transform.position).magnitude;
            if (D < ClosestDistance)
            {
                ClosestObject = _interactables[i].gameObject;
            }
        }
        for (int i = 1; i < _interactables.Count; i++)
        {
            _interactables[i].GetComponent<Renderer>().material.color = new Color(0, 0, 1);
        }
        ClosestObject.GetComponent<Renderer>().material.color = new Color(0, 1, 0);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        ToolScript TS = collision.gameObject.GetComponent<ToolScript>();
        if (TS)
            _interactables.Add(TS);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        ToolScript TS = collision.gameObject.GetComponent<ToolScript>();
        if (TS)
            _interactables.Remove(TS);
    }
}
