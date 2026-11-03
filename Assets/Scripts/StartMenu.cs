using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [Header("Menu UI")]
    public GameObject menuPanel;
    public TMP_Text titleText;
    public Button playButton;

    [Header("Music toggle (single button)")]
    public Button musicButton;
    public Image musicIcon;        // the Image that shows the icon
    public Sprite musicOnSprite;   // speaker with waves
    public Sprite musicOffSprite;  // speaker with X
    public AudioSource musicSource;

    [Header("Title")]
    public string gameName = "BLOCKADE";
    public string[] letterColors =
    {
        "FF4D4D", // red
        "FF9F1C", // orange
        "FFE14D", // yellow
        "3DDC84", // green
        "33C7FF", // cyan
        "B06CFF", // purple
        "FF5CA8"  // pink
    };
    public float floatHeight = 10f;
    public float floatSpeed = 2f;

    // static = survives scene reloads, so Try Again skips the menu and keeps the music setting
    static bool hasStarted;
    static bool musicOn = true;

    RectTransform titleRect;
    Vector2 titleStartPos;

    // Resets the statics when entering Play mode in the Editor
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics()
    {
        hasStarted = false;
        musicOn = true;
    }

    void Awake()
    {
        // Music button works at all times (menu and gameplay)
        if (musicButton != null)
            musicButton.onClick.AddListener(ToggleMusic);

        ApplyMusic();
        if (musicOn && musicSource != null && !musicSource.isPlaying)
    musicSource.Play();

        if (hasStarted)
        {
            menuPanel.SetActive(false);
            return;
        }

        Time.timeScale = 0f;
        menuPanel.SetActive(true);

        ColorizeTitle();

        if (titleText != null)
        {
            titleRect = titleText.rectTransform;
            titleStartPos = titleRect.anchoredPosition;
        }

        playButton.onClick.AddListener(OnPlay);
    }

    void Update()
    {
        // Gentle floating title (unscaled time, because the game is paused)
        if (titleRect != null && menuPanel.activeSelf)
        {
            float offset = Mathf.Sin(Time.unscaledTime * floatSpeed) * floatHeight;
            titleRect.anchoredPosition = titleStartPos + new Vector2(0f, offset);
        }
    }

    void ColorizeTitle()
    {
        if (titleText == null || letterColors.Length == 0) return;

        string result = "";
        for (int i = 0; i < gameName.Length; i++)
        {
            string hex = letterColors[i % letterColors.Length];
            result += "<color=#" + hex + ">" + gameName[i] + "</color>";
        }
        titleText.text = result;
    }

    void OnPlay()
{
    hasStarted = true;
    menuPanel.SetActive(false);
    Time.timeScale = 1f;

    if (musicOn && musicSource != null && !musicSource.isPlaying)
    {
        musicSource.Play();
    }
}

    void ToggleMusic()
{
    musicOn = !musicOn;
    ApplyMusic();

    if (musicOn && musicSource != null && !musicSource.isPlaying)
    {
        musicSource.Play();
    }
}
    void ApplyMusic()
    {
        if (musicSource != null)
            musicSource.mute = !musicOn;
        else
            AudioListener.volume = musicOn ? 1f : 0f; // fallback: mutes everything

        if (musicIcon != null)
            musicIcon.sprite = musicOn ? musicOnSprite : musicOffSprite;
    }
    
}