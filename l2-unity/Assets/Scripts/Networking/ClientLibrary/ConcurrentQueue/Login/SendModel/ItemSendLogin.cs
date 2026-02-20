using FMOD.Studio;
using L2_login;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ItemSendLogin : IData
{
    private ClientPacket packet;
    private bool encrypt;
    public ItemSendLogin(ClientPacket packet , bool encrypt , bool blowfish)
    {
        this.packet = packet;
        this.encrypt = encrypt;
        Encrypt(packet, encrypt);
        Blowfish(packet.GetData(), blowfish);
    }
    public byte[] DecodeData()
    {
        throw new System.NotImplementedException();
    }

    private void Encrypt(ClientPacket packet , bool encrypt)
    {
        try
        {
            if (encrypt)
            {
                byte[] data = packet.GetData();

                if (packet.GetPacketType() == (byte)LoginClientPacketType.AuthGameGuard | packet.GetPacketType() == (byte)LoginClientPacketType.RequestAuthLogin | packet.GetPacketType() == (byte)LoginClientPacketType.RequestServerList)
                {

                }
                else
                {
                    NewCrypt.appendChecksum(data);
                }
 
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ItemSendLogin->Encrypt: " + ex.Message);
        }

    }

    private void Blowfish(byte[] data, bool blowfish)
    {
        if (blowfish)
        {
            LoginClient.Instance.EncryptBlowFish.processBigBlock(data, 0, data, 0, data.Length);
        }
        
    }

    public ClientPacket GetPacket()
    {
        return packet;
    }

    public byte[] DecodeExData()
    {
        throw new System.NotImplementedException();
    }
}
