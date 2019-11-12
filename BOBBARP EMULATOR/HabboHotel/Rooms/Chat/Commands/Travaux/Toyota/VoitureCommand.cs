﻿using System;
using System.Linq;
using System.Text;
using System.Data;

using Plus.Database.Interfaces;
using Plus.Communication.Packets.Outgoing.Notifications;
using Plus.HabboHotel.GameClients;
using Plus.Communication.Packets.Outgoing.Inventory.Purse;

namespace Plus.HabboHotel.Rooms.Chat.Commands.User
{
    class VoitureCommand : IChatCommand
    {
        public bool getPermission(GameClient Session)
        {
            if (Session.GetHabbo().TravailId == 11 && Session.GetHabbo().Travaille == true)
                return true;

            return false;
        }

        public string TypeCommand
        {
            get { return "travail"; }
        }

        public string Parameters
        {
            get { return "<pseudonyme> <nom de la voiture>"; }
        }

        public string Description
        {
            get { return "Vendre une voiture à un civil"; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length < 3)
            {
                Session.SendWhisper("Syntaxe invalide, tapez :voiture <pseudonyme> <nom de la voiture>");
                return;
            }

            string Username = Params[1];
            GameClient TargetClient = PlusEnvironment.GetGame().GetClientManager().GetClientByUsername(Username);
            if (TargetClient == null || TargetClient.GetHabbo().CurrentRoom != Session.GetHabbo().CurrentRoom)
            {
                Session.SendWhisper("Impossible de trouver " + Username + " dans cet appartement.");
                return;
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            string Message = CommandManager.MergeParams(Params, 2);
            if (!PlusEnvironment.checkIfItemExist(Message, "voiture"))
            {
                Session.SendWhisper("Cette voiture n'existe pas.");
                return;
            }

            if (Message.ToLower() == "audi a8" && TargetClient.GetHabbo().AudiA8 == 1)
            {
                Session.SendWhisper(TargetClient.GetHabbo().Username + " a déjà une Audi A8.");
                return;
            }
            else if (Message.ToLower() == "porsche 911" && TargetClient.GetHabbo().Porsche911 == 1)
            {
                Session.SendWhisper(TargetClient.GetHabbo().Username + " a déjà une Porsche 911.");
                return;
            }
            else if (Message.ToLower() == "fiat punto" && TargetClient.GetHabbo().FiatPunto == 1)
            {
                Session.SendWhisper(TargetClient.GetHabbo().Username + " a déjà une Fiat Punto.");
                return;
            }
            else if (Message.ToLower() == "volkswagen jetta" && TargetClient.GetHabbo().VolkswagenJetta == 1)
            {
                Session.SendWhisper(TargetClient.GetHabbo().Username + " a déjà une Volkswagen Jetta.");
                return;
            }
            else if (Message.ToLower() == "bmw i8" && TargetClient.GetHabbo().BmwI8 == 1)
            {
                Session.SendWhisper(TargetClient.GetHabbo().Username + " a déjà une BMW i8.");
                return;
            }

            RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
            if (TargetUser.Transaction != null || TargetUser.isTradingItems)
            {
                Session.SendWhisper(TargetClient.GetHabbo().Username + " a déjà une transaction en cours, veuillez patienter.");
                return;
            }

            User.OnChat(User.LastBubble, "* Vend un(e) "+ PlusEnvironment.getNameOfItem(Message) + " à " + TargetClient.GetHabbo().Username + " *", true);
            TargetUser.Transaction = "voiture:" + PlusEnvironment.getNameOfItem(Message) + ":" + PlusEnvironment.getPriceOfItem(Message) + ":" + PlusEnvironment.getTaxeOfItem(Message);
            PlusEnvironment.GetGame().GetWebEventManager().SendDataDirect(TargetClient, "transaction;<b>" + Session.GetHabbo().Username + "</b> souhaite vous vendre un(e) <b>" + PlusEnvironment.getNameOfItem(Message) + "</b> pour <b>" + PlusEnvironment.getPriceOfItem(Message) + " crédits</b> dont <b>" + PlusEnvironment.getTaxeOfItem(Message) + "</b> qui iront à l'État.;" + PlusEnvironment.getPriceOfItem(Message));
        }
    }
}