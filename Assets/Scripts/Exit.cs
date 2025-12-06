using UnityEngine;

public class Exit : MonoBehaviour
{
    public void ExitApplication()
    {
        Application.Quit();
        Debug.Log("The game has ended");
    }
}   
