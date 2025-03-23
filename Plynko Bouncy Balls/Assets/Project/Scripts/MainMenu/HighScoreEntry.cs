[System.Serializable]
public struct HighScoreEntry
{
    public int score;
    public int reward;

    public HighScoreEntry(int s, int r)
    {
        score = s;
        reward = r;
    }
}