using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMSPortalModemLibrary.Structure;
using SMSPortalModemLibrary.Utils;

namespace SMSPortalModemLibrary.Network
{
    class PacketHandler
    {


        private static PacketHandler instance;
        public static PacketHandler getInstance()
        {
            if (instance != null)
                return instance;

            else
                return new PacketHandler();
        }

        private PacketHandler()
        {
            instance = this;
        }

        public void parseRecievedPacket(OnDataRecievedListener listener, byte[] data)
        {
            try
            {

                if (Encoding.Default.GetString(data).Contains("Ready to Send SMS"))
                {
                    listener.onConnectionStarted();
                    return;
                }

                if (Encoding.Default.GetString(data).Contains(AbstractPacket.GSM_Sending_Error_text))
                {
                    listener.onGSMSendingError();
                    return;
                }


                RecievedPacket recPacket = new RecievedPacket(data);
                if (recPacket.AbsPacket != null)
                {
                    switch (recPacket.type)
                    {
                        case AbstractPacket.StructType.Registeration:
                            if (recPacket.AbsPacket.Cmd == AbstractPacket.WRITE_CMD)
                                listener.onWriteNumberRegister((RegisterationPacket)recPacket.AbsPacket);
                            else
                                listener.onReadNumberRegister((RegisterationPacket)recPacket.AbsPacket);
                            break;

                        case AbstractPacket.StructType.Activation:
                            if (recPacket.AbsPacket.Cmd == AbstractPacket.WRITE_CMD)
                                listener.onWriteActivate();
                            else
                                listener.onReadActivate((ActivationPacket)recPacket.AbsPacket);
                            break;

                        case AbstractPacket.StructType.Send:
                            listener.onSend();
                            break;

                        case AbstractPacket.StructType.AutoId:
                            listener.onAutoId((RegisterationPacket)recPacket.AbsPacket);
                            break;

                        case AbstractPacket.StructType.Delivery:
                            listener.onDelivery((DeliveryPacket)recPacket.AbsPacket);
                            break;

                        case AbstractPacket.StructType.AutoDelivery:
                            listener.onAutoDelivery((DeliveryPacket)recPacket.AbsPacket);
                            break;

                        case AbstractPacket.StructType.USSDWrite:
                            listener.onUSSDWrite();
                            break;
                        case AbstractPacket.StructType.USSDExecute:
                            listener.onUSSDExecute();
                            break;
                        case AbstractPacket.StructType.USSDRead:
                            listener.onUSSDRead((USSDPacket)recPacket.AbsPacket);
                            break;
                        case AbstractPacket.StructType.SMSText:
                            if (recPacket.AbsPacket.Cmd == AbstractPacket.WRITE_CMD)
                                listener.onMessageTextWrite();
                            else
                                listener.onMessageTextRead((SMSTextPacket)recPacket.AbsPacket);
                            break;
                        case AbstractPacket.StructType.Inbox:

                            listener.onReadInbox((InboxPacket)recPacket.AbsPacket);
                            break;
                        case AbstractPacket.StructType.AutoInbox:
                            listener.onAutoInbox((InboxPacket)recPacket.AbsPacket);
                            break;

                    }
                }

                return;
            }
            catch (Exception e)
            {

                Console.WriteLine(string.Format("Parse Packet Error => {0}", e.Message));
            }


        }


        class RecievedPacket : AbstractPacket
        {


            public AbstractPacket AbsPacket;

            public RecievedPacket(byte[] data)
            {
                cIndex = SRC_ADD_COUNTER + DEST_ADD_COUNTER + CMD_COUNTER;

                add_var = data.Skip(cIndex).Take(ADD_VAR_COUNTER).ToArray();

                address = BitConverter.ToInt16(add_var, 0);
                SimNumber = getSimNumber(address);

                this.AbsPacket = null;

                if (address >= getRegistrationAVBound(SimNumber)[0] && address <= getRegistrationAVBound(SimNumber)[1])
                {
                    this.type = StructType.Registeration;
                    this.AbsPacket = new RegisterationPacket(SimNumber).toObject(data);
                }
                else if (address == getAutoIdAV(SimNumber))
                {
                    this.type = StructType.AutoId;
                    this.AbsPacket = new RegisterationPacket(SimNumber).toObject(data);
                }
                else if (address == getActivationAV(SimNumber))
                {
                    this.type = StructType.Activation;
                    this.AbsPacket = new ActivationPacket(SimNumber).toObject(data);
                }
                else if (address == getSendAV(SimNumber))
                {
                    this.type = StructType.Send;
                    this.AbsPacket = new SendPacket(SimNumber).toObject(data);
                }

                else if (address >= getDeliveryAVBound(SimNumber)[0] && address <= getDeliveryAVBound(SimNumber)[1])
                {
                    this.type = StructType.Delivery;
                    this.AbsPacket = new DeliveryPacket(SimNumber).toObject(data);
                }
                else if (address == getAutoDeliveryAV(SimNumber))
                {
                    this.type = StructType.AutoDelivery;
                    this.AbsPacket = new DeliveryPacket(SimNumber).toObject(data);
                }

                else if (address == getAutoInboxAV(SimNumber))
                {
                    this.type = StructType.Inbox;
                    this.AbsPacket = new InboxPacket(SimNumber).toObject(data);
                }
                else if (address >= getInboxAVBound(SimNumber)[0] && address <= getInboxAVBound(SimNumber)[1])
                {
                    this.type = StructType.AutoInbox;
                    this.AbsPacket = new InboxPacket(SimNumber).toObject(data);
                }

                else if (address == getUSSDWriteAV(SimNumber))
                {
                    this.type = StructType.USSDWrite;
                    this.AbsPacket = new USSDPacket(SimNumber).toObject(data);
                }
                else if (address == getUSSDReadAV(SimNumber))
                {
                    this.type = StructType.USSDRead;
                    this.AbsPacket = new USSDPacket(SimNumber).toObject(data);
                }
                else if (address == getUSSDExecuteAV(SimNumber))
                {
                    this.type = StructType.USSDExecute;
                    this.AbsPacket = new USSDPacket(SimNumber).toObject(data);
                }
                else if (address == getSMSTextAV(SimNumber))
                {
                    this.type = StructType.SMSText;
                    this.AbsPacket = new SMSTextPacket(SimNumber).toObject(data);
                }


                Console.ResetColor();

                if (this.AbsPacket != null)
                    Console.WriteLine(this.AbsPacket.ToString());
                else
                {
                    Console.WriteLine(string.Format("Unexpected Packet...Could not parse adress => {0}" , address) );
                    
                }

            }
        }



    }



}
