using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public Animator Animator;
    public Transform ToolAnchor;
    public Transform DropAnchor;


    private Rigidbody2D _rigidbody;
    private Transform _transform;

    private Vector2 _movement;

    private HashSet<Interactable> _interactables = new HashSet<Interactable>();

    private Interactable _toolHeld;
    public Interactable ToolHeld 
    {
        get {
            return _toolHeld;
        }

        set {
            if (_toolHeld)
            {
                _toolHeld.transform.parent = null;
                _toolHeld.transform.position = DropAnchor.transform.position;
                _interactables.Add(_toolHeld);
            }
            _toolHeld = value;
            if (value)
            {
                value.transform.parent = this.gameObject.transform;
                value.transform.position = ToolAnchor.position;
                value.transform.rotation = ToolAnchor.rotation;
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
        _rigidbody.velocity = _movement;

        var walking = _movement.x != 0 || _movement.y != 0;
        if(walking)
            _transform.rotation = Quaternion.Euler(
                transform.rotation.x,
                transform.rotation.y,
                Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg);

        Animator.SetBool("IsWalking", walking);

        if (_interactables.Any())
        {
            Interactable _closestObject = FindClosestInteractable();
            foreach (Interactable interactable in _interactables)
            {
                interactable.GetComponent<Renderer>().material.color = new Color(0, 0, 1);
            }
            _closestObject.GetComponent<Renderer>().material.color = new Color(0, 1, 0);
        }
    }

    public void OnMove(InputValue input)
    {
        _movement = input.Get<Vector2>() * Speed;
    }

    public void OnFire(InputValue input) 
    {
        Interactable _closestObject = FindClosestInteractable();
        if (_closestObject && _closestObject.type == Interactable.InteractableType.Tool)
        {
            ToolHeld = _closestObject;
        }
    }

    public void OnDrop(InputValue input) 
    {
        ToolHeld = null;    
    }

    public void OnUseTool(InputValue input) {

        Interactable _closestObject = FindClosestInteractable();
        if (_closestObject && _closestObject.type == Interactable.InteractableType.Objective)
        {
            //Interact!
            Debug.Log("Interacting");
        }
    }

    public Interactable FindClosestInteractable()
    {
        if (_interactables.Any())
        {
            Interactable _closestObject = _interactables.First();
            float _closestDistance = (_interactables.First().gameObject.transform.position - gameObject.transform.position).magnitude;

            foreach (Interactable interactable in _interactables)
            {
                float D = (interactable.gameObject.transform.position - gameObject.transform.position).magnitude;
                if (D < _closestDistance)
                {
                    _closestObject = interactable;
                }
            }

            return _closestObject;
        }
        else
        {
            return null;
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
