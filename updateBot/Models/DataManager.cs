namespace updateBot.Models;

public static class DataManager
{
    public static bool AreEqual(ModelFeed oldFeed, ModelFeed newFeed)
    {
        NullCheck(oldFeed);
        NullCheck(newFeed);
        for (int i = 0; i < oldFeed.Data.Count; i++)
        {
            if (oldFeed.Data[i]!.Id != newFeed.Data[i]!.Id) return false;
        }
        return true;
    }

    private static void NullCheck(ModelFeed feed)
    {
        if (feed.Result == null || feed.Response == null) throw new NullReferenceException();
        foreach (var idModel in feed.Data)
        {
            if (idModel!.Id == null) throw new NullReferenceException();
        }
    }
}