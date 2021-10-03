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

    private List<Interactable> _interactables = new List<Interactable>();

    private Interactable _toolHeld;
    public Interactable toolHeld 
    {
        get {
            return _toolHeld;
        }

        set {
            if (_toolHeld)
            {
                _toolHeld.transform.parent = null;
                _interactables.Add(_toolHeld);
            }
            _toolHeld = value;
            if (value)
            {
                value.transform.parent = this.gameObject.transform;
                _interactables.Remove(value);
            }
        }

    }

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

        if (_interactables.Count >0)
        {
            Interactable _closestObject = _interactables[0];
            float _closestDistance = (_interactables[0].gameObject.transform.position - gameObject.transform.position).magnitude;

            for (int i = 1; i < _interactables.Count; i++)
            {
                float D = (_interactables[i].gameObject.transform.position - gameObject.transform.position).magnitude;
                if (D < _closestDistance)
                {
                    _closestObject = _interactables[i];
                }

                _interactables[i].GetComponent<Renderer>().material.color = new Color(0, 0, 1);
            }
            _closestObject.GetComponent<Renderer>().material.color = new Color(0, 1, 0);
        }
    }

    public void OnMove(InputValue input)
    {
        movement = input.Get<Vector2>() * Speed;
    }

    public void OnFire(InputValue input) 
    {
        Interactable _closestObject = _interactables[0];
        float _closestDistance = (_interactables[0].gameObject.transform.position - gameObject.transform.position).magnitude;

        for (int i = 1; i < _interactables.Count; i++)
        {
            float D = (_interactables[i].gameObject.transform.position - gameObject.transform.position).magnitude;
            if (D < _closestDistance)
            {
                _closestObject = _interactables[i];
            }
        }

        if (_closestObject.type == Interactable.InteractableType.Tool) {
            toolHeld = _closestObject;
        }
    }



    public void InteractablesEnter(Interactable _newInt)
    {
        _interactables.Add(_newInt);

    }

    public void InteractablesExit(Interactable _oldInt)
    {
        _interactables.Remove(_oldInt);

    }

}
