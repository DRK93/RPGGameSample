using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgAdventure.Scripts.Helpers
{

    public enum MessageType
    {
        DAMAGED,
        DEAD,
        BLOCKED,
        HIGHDAMAGED
    }
    interface IMessageReceiver
    {
        void OnReceiveMessage(
            MessageType type, 
            object sender, 
            object message);
    }
}
