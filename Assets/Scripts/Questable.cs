using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Questable : Interactable
{
    public bool IsActive;
    public int MinFixedSeconds;
    public int MaxFixedSeconds;
    public System.DateTime NextActivateTime;
    public override bool CanInteract => IsActive;

    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        NextActivateTime = GetNextActivateTime();
    }

    void Update()
    {
        if(!IsActive && System.DateTime.Now > NextActivateTime)
            Activate();
    }

    void Activate()
    {
        IsActive = true;
        _spriteRenderer.color = new Color(255, 0, 0);
    }

    System.DateTime GetNextActivateTime()
    {
        return System.DateTime.Now.AddSeconds(
            Random.Range(MinFixedSeconds, MaxFixedSeconds));
    }
}
