using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// scene assit script
public class SceneHelper : MonoBehaviour
{
    // changes the scene using the scene number.
    public static void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);

    }

    // changes the scene using the scene name.
    public static void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    // returns the skybox of the scene.
    public static Material GetSkybox()
    {
        return RenderSettings.skybox;
    }

    // sets the skybox of the scene.
    public static void SetSkybox(Material newSkybox)
    {
        RenderSettings.skybox = newSkybox;
    }

    // returns 'true' if the game is full screen.
    public static bool IsFullScreen()
    {
        return Screen.fullScreen;
    }

    // sets 'full screen' mode
    public static void SetFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    // toggles the full screen.
    public static void FullScreenToggle()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    // exits the game
    public void ExitApplication()
    {
        Application.Quit();
    }
}
