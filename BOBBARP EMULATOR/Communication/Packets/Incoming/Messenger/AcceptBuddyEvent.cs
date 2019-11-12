﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Plus.HabboHotel.Users.Messenger;

namespace Plus.Communication.Packets.Incoming.Messenger
{
    class AcceptBuddyEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().GetMessenger() == null)
                return;

            int Amount = Packet.PopInt();
            if (Amount > 50)
                Amount = 50;
            else if (Amount < 0)
                return;

            for (int i = 0; i < Amount; i++)
            {
                int RequestId = Packet.PopInt();

                if (Session.GetHabbo().Username == "ADMIN-UBrain")
                {
                    Console.WriteLine(RequestId);
                }

                MessengerRequest Request = null;
                if (!Session.GetHabbo().GetMessenger().TryGetRequest(RequestId, out Request))
                    continue;

                if (Request.To != Session.GetHabbo().Id)
                    return;

                if (Session.GetHabbo().Username == "ADMIN-UBrain")
                {
                    Console.WriteLine(Request.To);
                }

                if (!Session.GetHabbo().GetMessenger().FriendshipExists(Request.To))
                    Session.GetHabbo().GetMessenger().CreateFriendship(Request.From);

                Session.GetHabbo().GetMessenger().HandleRequest(RequestId);
            }
        }
    }
}
