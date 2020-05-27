using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    private int currentAnimation;
    public int CurrentAnimation {
        get 
        {
            return currentAnimation;
        }
        set 
        {
            if (value >= animations.Count || value < 0) 
            {
                return;
            }
            currentAnimation = value;
            index = 0;
            spriteRenderer.sprite = animations[currentAnimation].sprites[0];
            timerFrame = 1 / animations[currentAnimation].framePerSecond;
        }
    }
    public List<SpriteAnimation> animations;

    private int index;
    private float timerFrame;

    protected SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (currentAnimation >= animations.Count || currentAnimation < 0) 
        {
            currentAnimation = 0;
        }
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (animations[currentAnimation].sprites.Count > 1) 
        {
            timerFrame -= Time.deltaTime;
            if (timerFrame <= 0) 
            {
                timerFrame = 1 / animations[currentAnimation].framePerSecond;
                index++;
                if (index == animations[currentAnimation].sprites.Count) 
                {
                    index = 0;
                }
                spriteRenderer.sprite = animations[currentAnimation].sprites[index];
            }
        }
    }

    public void SetAnimation(string name)
    {
        if (animations[CurrentAnimation].name == name) 
        {
            return;
        }
        for (int i = 0; i < animations.Count; i++) 
        {
            if (animations[i].name == name) 
            {
                CurrentAnimation = i;
            }
        }
    }
}

[System.Serializable]
public struct SpriteAnimation
{
    public string name;
    public float framePerSecond;
    public List<Sprite> sprites;
}
