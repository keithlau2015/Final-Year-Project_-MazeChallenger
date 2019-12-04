[System.Serializable]
public class PlayerData
{
    public int count;

    public int getCount()
    {
        return count;
    }

    public void setCount()
    {
        count++;
    }

    public PlayerData(OpeningVideo openingVideo)
    {
        count = openingVideo.count;
    }
}
