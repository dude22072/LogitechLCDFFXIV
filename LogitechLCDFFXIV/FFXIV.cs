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

        public FFXIV(bool dx11)
        {
            _initiated = InitFFXIV(dx11);
        }

        public bool InitFFXIV(bool dx11)
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
                string gameLanguage = "English";
                // whether to always hit API on start to get the latest sigs based on patchVersion
                bool ignoreJSONCache = true;
                // patchVersion of game, or latest
                string patchVersion = "latest";
                Process process = processes[0];
                ProcessModel processModel = new ProcessModel
                {
                    Process = process,
                    IsWin64 = dx11
                };
                MemoryHandler.Instance.SetProcess(processModel, gameLanguage, patchVersion, ignoreJSONCache);
                return _initiated = true;

            }
            else
            {
                return _initiated = false;
            }
        }

        public class Character
        {
            private string firstName;
            private string lastName;
            private string fullName;
            public Sharlayan.Core.ActorEntity stats;
            public Character(string firstName, string lastName)
            {
                this.firstName = firstName;
                this.lastName = lastName;
                fullName = firstName + " " + lastName;
                stats = CharStats(this.firstName, this.lastName);
            }
            public Sharlayan.Core.ActorEntity CharStats(string firstName, string lastName)
            {
                var fullName = firstName + " " + lastName;
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
