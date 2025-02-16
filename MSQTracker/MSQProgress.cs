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
            quests = new();
        }

        public unsafe void StartLoop()
        {
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    if (configuration.Tracking)
                    {
                        List<string> currentQuests = new();
                        foreach (ref var questWork in QuestManager.Instance()->NormalQuests)
                        {
                            if (questWork.QuestId == 0) { continue; }
                            var questId = questWork.QuestId + 65536;
                            var questName = MSQTUtil.GetQuestName(questId.ToString());
                            configuration.QuestChecking = questName;
                            currentQuests.Add(questName);
                            if (quests.Find(l => l.name == questName) == null) // does not exist in list
                            {
                                quests.Add(new Quest(questName));
                            }
                            Thread.Sleep(1 * 1000);
                        }
                        foreach(Quest qst in quests)
                        {
                            if (!currentQuests.Contains(qst.name)) // current quests does not have quest, meaning we dont have it and need to remvoe it
                            {
                                quests.Remove(qst);
                            }
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

        public List<Quest> quests { get; set; }

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
                return quests[0];
            }
        }
    }
}
