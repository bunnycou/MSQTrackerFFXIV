using System.Threading;
using FFXIVClientStructs.FFXIV.Client.Game;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace MSQTracker
{
    public class MSQProgress
    {
        private Configuration configuration;
        public MSQProgress(Plugin plugin)
        {
            configuration = plugin.Configuration;
        }

        public void SetQuest(string questNameNew)
        {
            if (questName == questNameNew) { return; }
            Thread thread = new Thread(() =>
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument document = web.Load("https://ffxiv-progress.com/?search=" + questNameToUrl(questNameNew));
                HtmlNode activeXpac = document.DocumentNode.QuerySelector("div.active");

                if (activeXpac == null) { return; } // quest input was not valid

                var xpacAndQuests = activeXpac.QuerySelectorAll("span");
                var questNums = xpacAndQuests[1].InnerText.Split(" of ");
                var progressBar = activeXpac.QuerySelector("div.progress-bar");

                questName = questNameNew;
                xpac = xpacAndQuests[0].InnerText.Trim();
                currentQuestNum = int.Parse(questNums[0]);
                totalQuests = int.Parse(questNums[1]);
                percentProgress = progressBar.InnerText;
            });
            thread.Start();
        }

        public unsafe void StartLoop()
        {
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    if (configuration.Tracking)
                    {
                        foreach (ref var questWork in QuestManager.Instance()->NormalQuests)
                        {
                            if (questWork.QuestId == 0) { continue; }
                            var questId = questWork.QuestId + 65536;
                            var questName = MSQTUtil.GetQuestName(questId.ToString());
                            SetQuest(questName);
                        }
                    }
                    Thread.Sleep(10 * 1000);
                }
            });
            thread.Start();
        }

        public string? questName { get; set; }
        public string? percentProgress { get; set; }
        public string? xpac { get; set; }
        public int currentQuestNum { get; set; }
        public int totalQuests { get; set; }

        public string numProgress()
        {
            return currentQuestNum + "/" + totalQuests;
        }
        private string questNameToUrl(string qname)
        {
            return string.Join("%20", qname.ToLower().Split(" "));
        }

        public string TestOutput()
        {
            return $"{xpac}: {questName} | {percentProgress} ({numProgress()})";
        }
    }
}
