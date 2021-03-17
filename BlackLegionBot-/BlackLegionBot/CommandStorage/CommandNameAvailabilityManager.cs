using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlackLegionBot.CommandHandling;

namespace BlackLegionBot.CommandStorage
{
    public static class CommandNameAvailabilityManager
    {
        private static readonly HashSet<string> _blockList = new HashSet<string>()
        {
            "!new", "!delete", "!edit", "!setalias", "!setgame", "!auth", "!deletealias", "settitle"
        };
        private static readonly HashSet<string> _usedCommandsSet = new HashSet<string>();

        public static void RemoveFromBlockListBulk(IEnumerable<string> names)
        {
            foreach (var name in names)
            {
                _blockList.Remove(name);
            }
        }

        public static bool Available(string name) => 
            _blockList.Contains(name.ToLower()) && _usedCommandsSet.Contains(name.ToLower());

        // General
        public static void AddToBlockList(string name) =>
            _blockList.Add(name);

        public static void AddAllToBlockListBulk(IEnumerable<string> names)
        {
            foreach (var name in names)
            {
                AddToBlockList(name.ToLower());
            }
        }

        public static void RemoveFromBlockList(string name) => 
            _blockList.Remove(name);


        // Commands
        public static void AddCommandToBlockList(string commandName) =>
            _usedCommandsSet.Add(commandName.ToLower());

        public static void AddCommandsToBlockList(IEnumerable<string> names)
        {
            foreach (var name in names)
            {
                AddCommandToBlockList(name);
            }
        }

        public static void RemoveCommandFromBlockList(string name) =>
            _usedCommandsSet.Remove(name.ToLower());

        public static void RemoveCommandsFromBlockList(IEnumerable<string> names)
        {

        }
    }
}
