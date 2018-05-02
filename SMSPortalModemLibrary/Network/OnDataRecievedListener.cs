using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMSPortalModemLibrary.Structure;

namespace SMSPortalModemLibrary.Network
{
    public interface OnDataRecievedListener
    {
        void onConnectionAcceted();

        void onConnectionStarted();
        void onWriteNumberRegister(RegisterationPacket packet);

        void onReadNumberRegister(RegisterationPacket packet);

        void onWriteActivate();

        void onReadActivate(ActivationPacket packet);

        void onSend();

        void onAutoId(RegisterationPacket packet);

        void onDelivery(DeliveryPacket packet);

        void onAutoDelivery(DeliveryPacket packet);

        void onUSSDWrite();

        void onUSSDExecute();

        void onUSSDRead(USSDPacket packet);

        void onMessageTextWrite();

        void onMessageTextRead(SMSTextPacket packet);

        void onAutoInbox(InboxPacket packet);

        void onReadInbox(InboxPacket packet);

        void onGSMSendingError();


    }
}
