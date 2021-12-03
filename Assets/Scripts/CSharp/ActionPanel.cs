using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel;

public class ActionPanel : MonoBehaviour
{
#region Button_Clicked_Reaction
    public void Education()
    {
        Debug.Log("<color=red>Education</color> button clicked.");
    }
    public void Work()
    {
        Debug.Log("<color=red>Work</color> button clicked.");
    }
    public void Rest()
    {
        Debug.Log("<color=red>Rest</color> button clicked.");
    }
    public void Status()
    {
        Debug.Log("<color=red>Status</color> button clicked.");
    }
    public void Talk()
    {
        Debug.Log("<color=red>Talk</color> button clicked.");

        var naniScriptEngine = Engine.GetService<IScriptPlayer>();
        naniScriptEngine.PreloadAndPlayAsync("Test", label: "Chapter1").Forget();

        // 4. Enable Naninovel input.
        var inputManager = Engine.GetService<IInputManager>();
        inputManager.ProcessInput = true;

        ReferenceManager.Instance.gameMng.DialogStarted();
    }
    public void System()
    {
        Debug.Log("<color=red>System</color> button clicked.");
    }
#endregion
}
