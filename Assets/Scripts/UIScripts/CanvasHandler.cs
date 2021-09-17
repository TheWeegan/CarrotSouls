using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum ActiveScene {
    Menu,
    CarrotArena,
    None
}

public class CanvasHandler : MonoBehaviour {
    [SerializeField]
    private Canvas _menuScreen;
    [SerializeField]
    private Canvas _creditScreen;
    [SerializeField]
    private Canvas _victoryScreen;
    [SerializeField]
    private Canvas _defeatScreen;

    [SerializeField]
    private ActiveScene _currentScene;

    public Canvas GetMenuScreen { get => _menuScreen; }
    public Canvas GetCreditScreen { get => _creditScreen; }


    public Canvas GetVictoryScreen { get => _victoryScreen; }
    public Canvas GetDefeatScreen { get => _defeatScreen; }


    private void Awake() {
        switch (_currentScene) {
            case ActiveScene.Menu: {
                    _menuScreen.gameObject.SetActive(true);
                    _creditScreen.gameObject.SetActive(false);
                }
                break;
            case ActiveScene.CarrotArena: {
                    _victoryScreen.gameObject.SetActive(false);
                    _defeatScreen.gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }

    }
    public void Defeat() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        _defeatScreen.gameObject.SetActive(true);
    }

    public void Victory() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        _victoryScreen.gameObject.SetActive(true);
    }

}
