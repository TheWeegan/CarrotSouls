using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    [SerializeField]
    private Canvas _victoryScreen;

    private void Awake() {
        Time.timeScale = 1f;
        _victoryScreen.gameObject.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            ActivateVictoryScreen();
        }
    }

    private void ActivateVictoryScreen() {
        _victoryScreen.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
}
