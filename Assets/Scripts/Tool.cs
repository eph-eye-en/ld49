using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : Interactable
{
    public ToolType Type;
    public float FlashFrequency;
    public float MinBrightness;
    public bool IsHeld;
    public override bool CanInteract => !IsHeld;

    private SpriteRenderer _spriteRenderer;

    private float _flashTime;

    void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		_spriteRenderer.sprite = Type.Sprite;
    }

    void Update()
    {
        if(isHighlighted)
        {
            var b = (Mathf.Cos(_flashTime) / 2 + 0.5f) * (1 - MinBrightness) + MinBrightness;
            _spriteRenderer.color = Color.HSVToRGB(0, 0, b);
            _flashTime += FlashFrequency * Time.deltaTime * Mathf.PI;
        }
        else
            _spriteRenderer.color = Color.white;
    }

	public override void StartHighlight()
	{
		_flashTime = 0;
	}
}
