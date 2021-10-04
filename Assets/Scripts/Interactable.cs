using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public virtual bool CanInteract { get; set; }
    private bool _isHighlighted;
    public bool isHighlighted {
        get {
            return _isHighlighted;
        }
        set {
            var oldVal = _isHighlighted;
            _isHighlighted = value;
            if (_isHighlighted && !oldVal)
                StartHighlight();
            if (!_isHighlighted && oldVal)
                EndHighlight();
        }
    }

    public virtual void StartHighlight() { }
    public virtual void EndHighlight() { }
}
