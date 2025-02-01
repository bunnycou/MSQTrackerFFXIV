using System.Collections.Generic;
using System.Linq;
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

        public unsafe void StartLoop()
        {
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    if (configuration.Tracking)
                    {
                        List<Quest> quests = new();
                        foreach (ref var questWork in QuestManager.Instance()->NormalQuests)
                        {
                            if (questWork.QuestId == 0) { continue; }
                            var questId = questWork.QuestId + 65536;
                            var questName = MSQTUtil.GetQuestName(questId.ToString());
                            configuration.QuestChecking = questName;
                            quests.Add(new Quest(questName));
                            Thread.Sleep(1 * 1000);
                        }
                        quest = LowestMSQ(quests);
                    }
                    configuration.QuestChecking = "Wait...";
                    Thread.Sleep(10 * 1000);
                }
            });
            thread.Start();
        }
        public Quest quest { get; set; }
        //public string? questName { get; set; }
        //public string? percentProgress { get; set; }
        //public string? xpac { get; set; }
        //public int currentQuestNum { get; set; }
        //public int totalQuests { get; set; }

        public string numProgress()
        {
            return quest.currentQuestNum + "/" + quest.totalQuests;
        }
        private string questNameToUrl(string qname)
        {
            return string.Join("%20", qname.ToLower().Split(" "));
        }

        public string TestOutput()
        {
            return $"{quest.xpac}: {quest.name} | {quest.percentProgress} ({numProgress()})";
        }

        private Quest LowestMSQ(List<Quest> quests)
        {
            List<Quest> MSQuest = new();
            foreach(Quest quest in quests)
            {
                if (quest.currentQuestNum != -1)
                {
                    MSQuest.Add(quest);
                }
            }
            if (MSQuest.Count > 1)
            {
                return MSQuest.OrderBy(l => l.currentQuestNum).ToList()[0];
            } else if (MSQuest.Count == 1)
            {
                return MSQuest[0];
            } else // no msq
            {
                return new Quest("no quest");
            }
        }
    }
}
