using Domain.Models;

namespace Application.Services.Implementations;

public sealed class FeedService : IFeedService
{
    public bool CheckUpdate(ModelFeed oldFeed, ModelFeed newFeed, out List<string> newChapters)
    {
        NullCheck(oldFeed);
        NullCheck(newFeed);
        newChapters = [];
        foreach (ModelId? chapterId in newFeed.Data)
        {
            // if all chapters in old feed are different from the chapter id, it means it's a new chapter
            if (oldFeed.Data.All(x => x!.Id != chapterId!.Id))
            {
                newChapters.Add(chapterId!.Id!);
            }
        }

        return newChapters.Count > 0;
    }

    private static void NullCheck(ModelFeed feed)
    {
        if (feed.Result == null || feed.Response == null || feed.Data == null) throw new NullReferenceException();
        foreach (var idModel in feed.Data)
            if (idModel!.Id == null)
                throw new NullReferenceException();
    }
}