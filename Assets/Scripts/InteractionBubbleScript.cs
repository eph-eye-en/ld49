using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBubbleScript : MonoBehaviour
{
    private PlayerController _playerController;
    
    void Start()
    {
        _playerController = transform.parent.GetComponent<PlayerController>();
        if (!_playerController)
            Debug.LogError("No player controller on parent");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable _newInteractable = collision.gameObject.GetComponent<Interactable>();
        if (_newInteractable != null)
        {
            _playerController.InteractablesEnter(_newInteractable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Interactable _oldInteractable = collision.gameObject.GetComponent<Interactable>();
        if (_oldInteractable != null)
        {
            _playerController.InteractablesExit(_oldInteractable);
        }
    }

}
