using Domain.Models;

namespace Application.Services.Implementations;

public sealed class FeedService : IFeedService
{
    public bool CheckUpdate(ModelFeed oldFeed, ModelFeed newFeed, out List<string> newChapters)
    {
        NullCheck(oldFeed);
        NullCheck(newFeed);
        newChapters = [];
        
        newChapters.AddRange(
            from chapterId in newFeed.Data
            where oldFeed.Data.All(x => x!.Id != chapterId!.Id) select chapterId!.Id!);

        return newChapters.Count > 0;
    }

    private static void NullCheck(ModelFeed feed)
    {
        if (feed.Result == null 
            || feed.Response == null 
            || feed.Data == null 
            || feed.Data.Any(idModel => idModel!.Id == null)) 
            throw new NullReferenceException();
    }
}