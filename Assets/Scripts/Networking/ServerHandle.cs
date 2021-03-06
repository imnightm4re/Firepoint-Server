﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    /// <summary>Processes the first message sent from a player.</summary>
    /// <param name="_fromClient">The client sender's id.</param>
    /// <param name="_packet">The packet with the info from the player.</param>
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        }
        Server.clients[_fromClient].SendIntoGame(_username);
    }

    /// <summary>Processes the inputs sent from the player.</summary>
    /// <param name="_fromClient">The client sender's id.</param>
    /// <param name="_packet">The packet with the info from the player.</param>
    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }
        Quaternion _rotation = _packet.ReadQuaternion();

        Server.clients[_fromClient].player.SetInput(_inputs, _rotation);
    }

    /// <summary>Processes the shoot input sent from a player.</summary>
    /// <param name="_fromClient">The client sender's id.</param>
    /// <param name="_packet">The packet with the info from the player.</param>
    public static void PlayerShoot(int _fromClient, Packet _packet)
    {
        Vector3 _shootDirection = _packet.ReadVector3();

        Server.clients[_fromClient].player.Shoot(_shootDirection);

    }

    /// <summary>Processes the drop weapon input sent from a player.</summary>
    /// <param name="_fromClient">The client sender's id.</param>
    /// <param name="_packet">The packet with the info from the player.</param>
    public static void PlayerDropWeapon(int _fromClient, Packet _packet)
    {
        Vector3 _dropDirection = _packet.ReadVector3();

        Server.clients[_fromClient].player.DropWeapon(_dropDirection);
    }

    /// <summary>Processes the change weapon input sent from a player.</summary>
    /// <param name="_fromClient">The client sender's id.</param>
    /// <param name="_packet">The packet with the info from the player.</param>
    public static void PlayerChangeWeapon(int _fromClient, Packet _packet)
    {
        int _index = _packet.ReadInt();

        Server.clients[_fromClient].player.weaponManager.ChangeWeapon(_index);
    }

    /// <summary>Processes the reload weapon input sent from a player.</summary>
    /// <param name="_fromClient">The client sender's id.</param>
    /// <param name="_packet">The packet with the info from the player.</param>
    public static void PlayerReload(int _fromClient, Packet _packet)
    {
        Server.clients[_fromClient].player.weaponManager.ReloadWeapon(_fromClient);
    }

}