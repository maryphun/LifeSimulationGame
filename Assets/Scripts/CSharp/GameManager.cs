using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Naninovel;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] Canvas FrontUI;

    private void Awake()
    {
        ReferenceManager.Instance.gameMng = this;
    }

    private async void Start()
    {
        // Initiate naninovel engine manually.
        await RuntimeInitializer.InitializeAsync();

        // Execute after naninovel got initialized successfully.
        if (Engine.Initialized) StartGame();
        else Engine.OnInitializationFinished += StartGame;
    }

    private async void StartGame()
    {
        CreateUI();

        // Engine is initialized here, it's safe to use the APIs.
        var naniNovelInput = GameObject.Find("ContinueInputUI").GetComponent<CanvasGroup>();
        naniNovelInput.blocksRaycasts = false;

        // Audio
        AudioManager.Instance.SetMusicVolume(0.15f);
        AudioManager.Instance.PlayMusic("Yet_Another_Sunset");
    }

    private void CreateUI()
    {
        // Instantiate UI Objects
        var obj = Instantiate(FrontUI);
        obj.GetComponent<CanvasGroup>().alpha = 0.0f;
        obj.GetComponent<CanvasGroup>().DOFade(1.0f, 1.0f);
        ReferenceManager.Instance.frontUI = obj.gameObject;
    }

    public void DialogEnded()
    {
        var obj = ReferenceManager.Instance.frontUI;
        obj.GetComponent<CanvasGroup>().blocksRaycasts = true;
        obj.GetComponent<CanvasGroup>().DOFade(1.0f, 1.0f);

        var naniNovelInput = GameObject.Find("ContinueInputUI").GetComponent<CanvasGroup>();
        naniNovelInput.blocksRaycasts = false;
    }
    public void DialogStarted()
    {
        var obj = ReferenceManager.Instance.frontUI;
        obj.GetComponent<CanvasGroup>().blocksRaycasts = false;
        obj.GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f);

        var naniNovelInput = GameObject.Find("ContinueInputUI").GetComponent<CanvasGroup>();
        naniNovelInput.blocksRaycasts = true;
    }
}
