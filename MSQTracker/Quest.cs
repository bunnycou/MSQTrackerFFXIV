using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSQTracker
{
    public class Quest
    {
        public Quest(string newQuest)
        {
            SetQuest(newQuest);
        }

        public Quest() { }

        public void SetQuest(string questNameNew)
        {
            if (name == questNameNew) { return; }
            try
            {
                if (questNameNew == "No Quests") { throw new Exception("No Quest To Lookup"); }
                HtmlWeb web = new HtmlWeb();
                HtmlDocument document = web.Load("https://ffxiv-progress.com/?search=" + QuestNameToUrl(questNameNew));
                HtmlNode activeXpac = document.DocumentNode.QuerySelector("div.active");

                if (activeXpac == null) { throw new Exception(); } // quest input was not valid

                var xpacAndQuests = activeXpac.QuerySelectorAll("span");
                var questNums = xpacAndQuests[1].InnerText.Split(" of ");
                var progressBar = activeXpac.QuerySelector("div.progress-bar");

                name = questNameNew;
                xpac = xpacAndQuests[0].InnerText.Trim();
                currentQuestNum = int.Parse(questNums[0]);
                totalQuests = int.Parse(questNums[1]);
                percentProgress = progressBar.InnerText;
            }
            catch
            { // load dumy data
                name = questNameNew;
                percentProgress = "0%";
                xpac = "Not Found";
                currentQuestNum = -1; // tell tale sign no quest was found
                totalQuests = 99;
            }
        }

        public string? name { get; set; }
        public string? percentProgress { get; set; }
        public string? xpac { get; set; }
        public int currentQuestNum { get; set; }
        public int totalQuests { get; set; }

        private string QuestNameToUrl(string qname)
        {
            return string.Join("%20", qname.ToLower().Split(" "));
        }
    }
}
