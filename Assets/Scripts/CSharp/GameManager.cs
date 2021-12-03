using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel;

public class GameManager : MonoBehaviour
{
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
        // Change Sorting Layer
        //GameObject[] objectList = FindGameObjectsInLayer(5);
        //foreach (GameObject obj in objectList)
        //{
        //    Canvas canvas = obj.GetComponent<Canvas>();
        //    if (canvas != null)
        //    {
        //        canvas.sortingLayerName = "Naninovel Layer";
        //    }
        //}

        // Engine is initialized here, it's safe to use the APIs.
        var naniScriptEngine = Engine.GetService<IScriptPlayer>();
        naniScriptEngine.PreloadAndPlayAsync("Test", label: "Chapter1").Forget();
    }

    GameObject[] FindGameObjectsInLayer(int layer)
    {
        var goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        var goList = new System.Collections.Generic.List<GameObject>();
        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].layer == layer)
            {
                goList.Add(goArray[i]);
                Debug.Log(goArray[i].name);
            }
        }
        if (goList.Count == 0)
        {
            return null;
        }
        return goList.ToArray();
    }
}
