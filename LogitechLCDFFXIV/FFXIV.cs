using Sharlayan;
using Sharlayan.Core;
using Sharlayan.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;

namespace LogitechLCDFFXIV
{
    class FFXIV
    {
        public bool _initiated;

        public FFXIV(bool dx11, int locale)
        {
            _initiated = InitFFXIV(dx11, locale);
        }

        public bool InitFFXIV(bool dx11, int locale)
        {
            // Set initiated to false
            _initiated = false;

            Process[] processes;
            // Load FFXIV
            if (dx11)
            {
                processes = Process.GetProcessesByName("ffxiv_dx11");
            } else
            {
                processes = Process.GetProcessesByName("ffxiv");
            }
            
            if (processes.Length > 0)
            {
                // supported: English, Chinese, Japanese, French, German, Korean
                string gameLanguage = locale == 0 ? "English" : locale == 1 ? "Japanese" : "";
                // whether to always hit API on start to get the latest sigs based on patchVersion
                bool useLocalCache = false;
                // patchVersion of game, or latest
                string patchVersion = "latest";
                Process process = processes[0];
                ProcessModel processModel = new ProcessModel
                {
                    Process = process,
                    IsWin64 = dx11
                };
                MemoryHandler.Instance.SetProcess(processModel, gameLanguage, patchVersion, useLocalCache);
                return _initiated = true;

            }
            else
            {
                return _initiated = false;
            }
        }

        public class Character
        {
            private string fullName;
            public Sharlayan.Core.ActorEntity stats;
            public Character(string fullName)
            {
                this.fullName = fullName;
                stats = CharStats(this.fullName);
            }

            public Sharlayan.Core.ActorEntity CharStats(string fullName)
            {
                ICollection<Sharlayan.Core.ActorEntity> PlayerInfo = Reader.GetActors()?.PCEntities?.Values;
                foreach (var k in PlayerInfo)
                {
                    if (k.Name == fullName)
                    {
                        return k;
                    }
                }
                return null;
            }
        }
    }
}
