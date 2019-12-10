using System.Collections.Generic;

[System.Serializable]
public class OpenGameCounting
{
    public int count;

    public OpenGameCounting(OpeningVideo ov)
    {
        count = ov.count;
    }
}
