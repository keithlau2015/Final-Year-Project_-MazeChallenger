using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class OpeningVideo : MonoBehaviour
{
    private string path_Char;
    private VideoPlayer videoPlayer;
    public int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        path_Char = "/Subject_Adam.Char";
        if(count != 0)count = SaveSystem.LoadData().count;
        PlayerStatus.Instance.setReinforcement(SaveSystem.LoadData().tempReinforcement);
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += LoadScene;

        if (Application.isPlaying)
        {
            if(count == 0 && !File.Exists(path_Char))
            {                
                try
                {
                    FileStream fs = File.Create(path_Char);
                    count++;
                }
                catch (Exception ex)
                {
                    DisplayDialog("CreatingFile Error", "There is some error on creating files", "", "");
                }
            }
            //The file not exist & not first time to opening up the game
            else if(!File.Exists(path_Char) && count != 0)
            {
                Application.Quit();
            }
            else
            {
                count++;
                SavePlayerData();
            }
        }
    }

    private void SavePlayerData()
    {
        SaveSystem.SavingData(this);
    }

    private void LoadScene(VideoPlayer vp)
    {
        SceneManager.LoadScene(1);
    }

    private void DisplayDialog(string v1, string v2, string v3, string v4)
    {
        throw new NotImplementedException();
    }
}
