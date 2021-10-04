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

    private Tool _toolHeld;
    public Tool ToolHeld 
    {
        get {
            return _toolHeld;
        }

        set {
            if(value!=_toolHeld)
                GetComponent<AudioSource>().Play();

            if (_toolHeld)
            {
                _toolHeld.transform.parent = null;
                _toolHeld.transform.position = DropAnchor.transform.position;
                _toolHeld.IsHeld = false;
                _interactables.Add(_toolHeld);
            }
            _toolHeld = value;
            if (value)
            {
                value.transform.parent = this.gameObject.transform;
                value.transform.position = ToolAnchor.position;
                value.transform.rotation = ToolAnchor.rotation;
                value.IsHeld = true;
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

        Interactable _closestObject = FindClosestInteractable<Tool>();
        foreach (Interactable interactable in _interactables)
        {
            //interactable.GetComponent<Renderer>().material.color = new Color(0, 0, 1);
            interactable.isHighlighted = interactable == _closestObject;
        }
        //_closestObject.GetComponent<Renderer>().material.color = new Color(0, 1, 0);
    }

    public void OnMove(InputValue input)
    {
        _movement = input.Get<Vector2>() * Speed;
    }

    public void OnFire(InputValue input) 
    {
        var _closestObject = FindClosestInteractable<Tool>();
        if (_closestObject != null)
        {
            ToolHeld = _closestObject;
            _closestObject.isHighlighted = false;
        }
    }

    public void OnDrop(InputValue input) 
    {
        ToolHeld = null;
    }

    public void OnUseTool(InputValue input) {

        Questable closestObject = FindClosestInteractable<Questable>();
        if (closestObject != null)
        {
            closestObject.TryInteract(ToolHeld.Type);
        }
    }

    public void OnAnyKeyPressed(InputValue input)
    {
        GameManager.Instance.OnAnyKeyPressed(input);
    }

    public T FindClosestInteractable<T>() where T : Interactable
    {
        T closestObject = default(T);
        float closestDistanceSqr = Mathf.Infinity;

        var possible = _interactables
            .Where(i => i.CanInteract && i is T)
            .Select(i => i as T);
        foreach (T interactable in possible)
        {
            float d = (interactable.gameObject.transform.position - gameObject.transform.position).sqrMagnitude;
            if (d < closestDistanceSqr)
            {
                closestObject = interactable;
                closestDistanceSqr = d;
            }
        }
        return closestObject;
    }


    public void InteractablesEnter(Interactable _newInt)
    {
        _interactables.Add(_newInt);
    }

    public void InteractablesExit(Interactable _oldInt)
    {
        _interactables.Remove(_oldInt);
        _oldInt.isHighlighted = false;

    }

}
