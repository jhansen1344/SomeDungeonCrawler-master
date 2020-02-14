using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPairGame
{
   
    public class Room 
    {
        public List<RoomObject> RoomContents { get; set; } = new List<RoomObject>();
        public List<RoomObject> HiddenContents { get; set; } = new List<RoomObject>();

        public string Description { get; set; }
    }

    public class RoomObject
    {
        public string Name { get; set; } 
        public string Type { get; set; }
        public string ID { get; set; }
        public string Verbage { get; set; }
        public string DescriptiveText { get; set; }
        public bool isLocked { get; set; } = false;
        public string LockID { get; set; }
        public int ConnectionA { get; set; }
        public int ConnectionB { get; set; }
    }

    public class Player
    {
        public string Name { get; set; }
        public int position { get; set; }
        public List<RoomObject> Inventory { get; set; } = new List<RoomObject>();
        public bool IsAlive { get; set; } = true;
        public bool HasMacguffin { get; set; } = false;
        public bool IsAQuitter { get; set; } = false;

        public void Move(Player player, RoomObject target )
        {
            if (player.position == target.ConnectionA) { player.position = target.ConnectionB; }

            else if (player.position == target.ConnectionB) { player.position = target.ConnectionA; }
        }

        public string Use(RoomObject target, Player player,Room room)
        {
            switch (target.Type)
            {
                case "hallway":
                    Move(player, target);
                    return "you walk down the hallway...";
                case "passageway":
                    if (target.isLocked)
                    {
                        if (CheckAccess())
                        {
                            Move(player, target);
                            return "you have lit your torch to illuminate the passageway.  you proceed...";
                        }
                        return "the passageway is dark.  You should look for something to light your way...";
                    }
                    else { Move(player, target); }
                    return "you proceed down passageway, aided by your lit torch...";
                case "door":
                    if (target.isLocked)
                        {
                            if (CheckAccess()) 
                            {
                            Move(player, target);
                            return "You have used the key to unlock this door. you now proceed...";
                            }
                        return "the door is locked, you should look for the key.";
                        }
                    else { Move(player, target); }
                    return "you open the door...";
                case "door with combination lock":
                    if (target.isLocked)
                    {
                        if (CheckAccess())
                        {
                            Move(player, target);
                            return "With a loud click, the door swings open. You proceed... ";
                        }
                        return "Look for the combination you can use to open the door.";
                    }
                    else { Move(player, target); }
                    return "you open the door...";

                case "chest":
                    if (target.isLocked)
                    {
                        if (CheckAccess())
                        {
                            return RevealHiddenContent();
                        }
                        return "Look for the key to use to open the chest.";
                    }
                    return RevealHiddenContent();
                case "torch":
                case "key":
                
                case "scroll":
                    player.Inventory.Add(target);
                    room.RoomContents.Remove(target);
                    return $"You pick up the {target.Type}...";

                case "goober":
                    player.HasMacguffin = true;
                    player.Inventory.Add(target);
                    room.HiddenContents.Remove(target);
                    room.RoomContents.Remove(target);
                    return "you pick up the goober!";
                case "tripwire":
                    IsAlive = false;
                    return "You tripped on a wire and fell on your face! HOW EMBARASSING!";

                case "sweetloot":
                    player.Inventory.Add(target);
                    room.HiddenContents.Remove(target);
                    return "You collect the Gold!";
                case "exit":
                    if (player.HasMacguffin == true) { player.IsAQuitter = true; return "You have succeeded where all have failed! HUZZAH";  }

                    else { player.IsAQuitter = true; return "you are a coward, shame upon your house!"; }

                default:
                    
                    return "";
            }

            bool CheckAccess()
            {
                foreach (RoomObject item in player.Inventory)
                {
                    if (item.ID == target.LockID)
                    {
                        target.isLocked = false;
                        return true;
                    }
                }
                return false;
            }

            string RevealHiddenContent()
            {
                room.RoomContents.Remove(target);
                if (room.HiddenContents.Count != 0)
                {
                    room.RoomContents.Add(room.HiddenContents[0]);
                    room.HiddenContents.Remove(room.HiddenContents[0]);
                    return "The chest opens in your hands, and out drops an item... ";
                }
                return "The chest opens, revealing it contains absolutely nothing...";
            }
        }
    }
}
