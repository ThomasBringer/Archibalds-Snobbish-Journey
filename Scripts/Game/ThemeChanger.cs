using UnityEngine;

public class ThemeChanger : MonoBehaviour
{
    public Color[] themes;
    int index;
    int Index
    {
        get { return index; }
        set { index = value % themes.Length; }
    }

    void Awake()
    {
        Index = PlayerPrefs.GetInt("theme", Index);
        ChangeTheme(Index);
    }

    public void NextColor()
    {
        Index++;
        ChangeTheme(index);

        PlayerPrefs.SetInt("theme", Index);
    }

    void ChangeTheme(int i)
    {
        GameManager.theme = themes[i];
    }
}