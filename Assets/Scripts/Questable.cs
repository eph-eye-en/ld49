using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Questable : Interactable
{
    public int MinFixedSeconds;
    public int MaxFixedSeconds;
    public ToolType FixedBy;
    public SpriteRenderer ToolAlert;

    public bool IsActive;
    public System.DateTime NextActivateTime;
    public override bool CanInteract => IsActive;

    private GameManager GameManager;

    void Start()
    {
        //_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        GameManager = GameManager.Instance;
        GameManager.OnGameStart += OnGameStart;
    }

	private void OnGameStart(object sender, System.EventArgs e)
	{
        NextActivateTime = GetNextActivateTime();
	}

	void Update()
    {
        if(!GameManager.IsGameRunning)
            return;

        if(!IsActive && System.DateTime.Now > NextActivateTime)
            Activate();
    }

    void Activate()
    {
        IsActive = true;
        UpdateAlert();
    }

	void Deactivate()
	{
		IsActive = false;
        UpdateAlert();
        NextActivateTime = GetNextActivateTime();
	}

    void UpdateAlert()
    {
        ToolAlert.color = isHighlighted ? Color.black : Color.white;
        ToolAlert.sprite = IsActive ? FixedBy.Sprite : null;
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
        GetComponent<AudioSource>().Play();
        return true;
	}

	public override void StartHighlight()
	{
		UpdateAlert();
    }

	public override void EndHighlight()
	{
		UpdateAlert();
	}
}
