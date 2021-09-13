using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ButtonTypes {
    Start,
    Credits,
    Quit,
    MainMenu,
    BackToMenu,
    None
}

public class ButtonType : MonoBehaviour {
    [SerializeField]
    private ButtonTypes _buttonType = ButtonTypes.None;
    private CanvasHandler _canvasHandler;

    private void Awake() {
        _canvasHandler = gameObject.GetComponentInParent<CanvasHandler>();
    }

    public void OnClick() {
        switch (_buttonType) {
            case ButtonTypes.Start: {
                SceneManager.LoadScene("CarrotArena", LoadSceneMode.Single);                    
                break;
            }
            case ButtonTypes.Credits: {
                HandleScreens(false);
                break;
            }
            case ButtonTypes.Quit: {
                Application.Quit();
                break;
            }
            case ButtonTypes.MainMenu: {
                HandleScreens(true);
                break;
            }
            case ButtonTypes.BackToMenu: {
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
                break;
            }
            default: {                    
                break;
            }
        }


    }

    void HandleScreens(bool isMenu) {
        _canvasHandler.GetMenuScreen.gameObject.SetActive(isMenu);
        _canvasHandler.GetCreditScreen.gameObject.SetActive(!isMenu);
    }
}
