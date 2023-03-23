using Dalamud.Game.ClientState;
using System;

namespace EmiThingsPlugin.Models
{
    public class XIVSkill
    {
        public string EN { get; set; }
        public string FR { get; set; }

        public XIVSkill(string EN, string FR)
        {
            this.EN = EN;
            this.FR = FR;
        }

        public string GetSkillName(ClientState clientState)
        {
            switch (clientState.ClientLanguage)
            {
                case Dalamud.ClientLanguage.English:
                    return EN;

                case Dalamud.ClientLanguage.French:
                    return FR;

                default:
                    throw new IndexOutOfRangeException($"Unsupported client language: {clientState.ClientLanguage.ToString()}");
            }
        }
    }
}
