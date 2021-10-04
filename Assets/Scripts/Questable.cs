using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Questable : Interactable
{
    public int MinFixedSeconds;
    public int MaxFixedSeconds;
    public ToolType FixedBy;

    public bool IsActive;
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

	void Deactivate()
	{
		IsActive = false;
        _spriteRenderer.color = Color.white;
        NextActivateTime = GetNextActivateTime();
	}

    System.DateTime GetNextActivateTime()
    {
        return System.DateTime.Now.AddSeconds(
            Random.Range(MinFixedSeconds, MaxFixedSeconds));
    }

	public bool TryInteract(ToolType tool)
	{
        if(!IsActive)
            return false;
		if(FixedBy != tool)
            return false;
        Deactivate();
        return true;
	}
}
