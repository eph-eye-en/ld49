using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractableType { 
        Tool,
        Objective,
        Door,
    };

    public InteractableType type;
    private bool _isHighlighted;
    public bool isHighlighted {
        get {
            return _isHighlighted;
        }
        set {
            _isHighlighted = value;
            if (value)
                StartHighlight();
            else
                EndHighlight();
        }
    }
    private Color _defaultColor;

    void StartHighlight() { 
    
    }

    void EndHighlight() { 
    
    }

}
