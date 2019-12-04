using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using System;

public class OpeningVideo : MonoBehaviour
{
    private string path_Char = "/Subject_Adam.Char";
    private int count = 1;
    private VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += LoadScene;

        if (Application.isPlaying)
        {
            if (File.Exists(path_Char))
            {
                return;
            }
            else if(count > 0 /*check is player first time opening up the game*/)
            {
                try
                {
                    FileStream fs = File.Create(path_Char);
                    /*
                    using (FileStream fs = File.Create(path))
                    {
                        byte[] info = new UTF8Encoding(true).GetBytes("0");
                        fs.Write(info, 0, info.Length);
                    }
                    */
                }
                catch (Exception ex)
                {
                    DisplayDialog("CreatingFile Error", "There is some error on creating files", "", "");
                }
            }
            //The file not exist & not first time to opening up the game
            else
            {
                Application.Quit();
            }
        }
    
        void LoadScene(VideoPlayer vp)
        {
            SceneManager.LoadScene(1);
        }
    }

    private void DisplayDialog(string v1, string v2, string v3, string v4)
    {
        throw new NotImplementedException();
    }
}
