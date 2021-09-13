using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CanvasHandler : MonoBehaviour {
    [SerializeField]
    private Canvas _menuScreen;
    [SerializeField]
    private Canvas _creditScreen;

    public Canvas GetMenuScreen { get => _menuScreen; }
    public Canvas GetCreditScreen { get => _creditScreen; }


    private void Awake() {
        _menuScreen.gameObject.SetActive(true);
        _creditScreen.gameObject.SetActive(false);
    }


    
}
