using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public List<Sprite> sprites;
    public static SpriteManager Instance { get; private set; }

    Dictionary<char, Sprite> dictSprites;

    private void Awake()
    {
        dictSprites = new Dictionary<char, Sprite>();
        char[] chars = {'8', '9', '6', '3', '2', '1', '4', '7', '5', 'p', 'k' };
        for (int i=0; i<chars.Length; i++)
        {
            char c = chars[i];
            dictSprites[c] = sprites[i];
        }
        Instance = this;
    }

    public Sprite Get(char c)
    {
        c = char.ToLower(c);
        if (!dictSprites.ContainsKey(c))
        {
            Debug.LogError("查询的字符不存在 "+c);
            return sprites[0];
        }
        return dictSprites[c];
    }
}
