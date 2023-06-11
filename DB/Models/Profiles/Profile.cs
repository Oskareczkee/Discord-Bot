﻿using DB.Models.Items;
using System;
using System.Collections.Generic;

namespace DB.Models.Profiles
{
    public enum SkillType
    {
        None = 0,
        Strength = 1,
        Agility = 2,
        Intelligence = 3,
        Endurance = 4,
        Luck = 5
    }

    public class Profile : Entity
    {
        public new int Strength { get; set; } = 10;
        public new int Agility { get; set; } = 10;
        public new int Intelligence { get; set; } = 10;
        public new int Endurance { get; set; } = 10;
        public new int Luck { get; set; } = 10;

        public ulong DiscordID { get; set; }
        public ulong GuildID { get; set; }
        public int XP { get; set; }

        public int NextLevel { get; set; }
        public double Gold { get; set; } = 100;
        public List<ProfileItem> Items { get; set; } = new List<ProfileItem>();
        public List<EquipmentItem> Equipment { get; set; } = new List<EquipmentItem>();
        public List<ShopItem> ShopItems { get; set; } = new List<ShopItem>();
        //time for quests and fights
        public DateTime lastQuestTime { get; set; }
        public DateTime nextQuestTime { get; set; }

        public DateTime lastFightTime { get; set; }
        public DateTime nextFightTime { get; set; }

        //time for free shop reroll
        //shop rerolls every day at 00:00:00
        public DateTime lastFreeRerollTime { get; set; }
    }
}
