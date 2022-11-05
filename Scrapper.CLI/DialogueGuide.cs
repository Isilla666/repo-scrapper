using System.Text.RegularExpressions;

namespace Scrapper.CLI;

public static class DialogueGuide
{
    public static void Start()
    {
        Console.WriteLine("Введи название канала?"); 
        var organizationName = Console.ReadLine();
        Console.WriteLine("Введи название репозитория?"); 
        var repository = Console.ReadLine();
        Console.WriteLine("Введи номер дискуссии?"); 
        var discussionID = Console.ReadLine();
        if (organizationName == null || repository == null || discussionID == null) return;
        var links = Task.Run(()=>GetLinksFromCommentsInDiscussion(organizationName, repository, discussionID)).Result;
        Console.WriteLine($"Всего {links.Count} ссылок"); 
        Console.WriteLine($"Сколько хотите получить ссылок?"); 
        var numName = Console.ReadLine();
        if (!int.TryParse(numName, out var numLinks)) return;
        var output = links.Shuffle().Stretch(numLinks);
        foreach (var linkList in output)
        {
            Console.WriteLine();
            foreach (var link in linkList)
            {
                Console.WriteLine(link);
            }
        }
        Console.WriteLine($"\nСпасибо за использование"); 
        Console.ReadLine();
    }

    private static async Task<List<List<string>>> GetLinksFromCommentsInDiscussion(string organizationName, string repositoryName, string discussionID)
    {
        var url = @$"https://github.com/{organizationName}/{repositoryName}/discussions/{discussionID}";
        var page = await WebService.GetPage(url);
        const string targetParse = "js-timeline-item js-timeline-progressive-focus-container";
        var dirtySplittedPageByComments = page.Split(targetParse);
        var urls = new List<List<string>>();
        for (var i = 1; i < dirtySplittedPageByComments.Length; i++)
        {
            var tagRegex = new Regex("<(?:\"[^\"]*\"['\"]*|'[^']*'['\"]*|[^'\">])+>");
            var newText = tagRegex.Replace(dirtySplittedPageByComments[i], "");
            var next = Regex.Replace(newText, @"\s+", " ");
            var ms2 = Regex.Matches(next, @"(http|ftp|https):\/\/([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:\/~+#-]*[\w@?^=%&\/~+#-])");
            urls.Add(ms2.Select(x=>x.Value).ToList());
        }
        return urls;
    }
    
}