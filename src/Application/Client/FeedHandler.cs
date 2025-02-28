using Domain.Models.JSON;

namespace Application.Client;

public static class FeedHandler
{
    public static bool AreEqual(ModelFeed oldFeed, ModelFeed newFeed, out List<string> newChapters)
    {
        NullCheck(oldFeed);
        NullCheck(newFeed);
        newChapters = [];
        for (var i = 0; i < oldFeed.Data.Count; i++)
            if (oldFeed.Data[i]!.Id != newFeed.Data[i]!.Id)
                newChapters.Add(newFeed.Data[i]!.Id!);

        return newChapters.Count == 0;
    }

    private static void NullCheck(ModelFeed feed)
    {
        if (feed.Result == null || feed.Response == null || feed.Data == null) throw new NullReferenceException();
        foreach (var idModel in feed.Data)
            if (idModel!.Id == null)
                throw new NullReferenceException();
    }
}