using System.Collections.Generic;

[System.Serializable]
public class OpenGameCounting
{
    public int count;
    public int tempReinforcement;

    public OpenGameCounting(OpeningVideo ov)
    {
        count = ov.count;
        tempReinforcement = PlayerStatus.Instance.getReinforcement();
    }
}
