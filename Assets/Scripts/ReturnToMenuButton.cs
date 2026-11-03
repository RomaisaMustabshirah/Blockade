using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ReturnToMenuButton : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(StartMenu.GoToMenu);
    }
}