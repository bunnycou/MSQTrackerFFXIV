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
            questBook = new();
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
                            if (questBook.FindIndex(l => l.name == questName) == -1) // new quest does not exist in questbook, add it
                            {
                                questBook.Add(new Quest(questName));
                                Thread.Sleep(1 * 1000);
                            }
                        }

                        List<Quest> removeQuests = new();
                        foreach(Quest quest in questBook)
                        {
                            if (!currentQuests.Contains(quest.name)) // current questlist does not have quest, meaning we dont have it anymore and need to remove it from questbook
                            {
                                removeQuests.Add(quest);
                            }
                        }

                        foreach(Quest quest in removeQuests)
                        {
                            questBook.Remove(quest);
                        }

                        quest = LowestMSQ();
                    }
                    configuration.QuestChecking = "Wait...";
                    Thread.Sleep(10 * 1000);
                }
            });
            thread.Start();
        }
        public Quest quest { get; set; }

        public List<Quest> questBook { get; set; }

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

        private Quest LowestMSQ()
        {
            if (questBook.Count == 0 || !questBook.Exists(l => l.currentQuestNum != -1)) // return empty quest if there is no quests
            {
                return new Quest("No Quests");
            }

            return questBook.OrderBy(l => l.currentQuestNum).First(l => l.currentQuestNum != -1);
        }
    }
}
