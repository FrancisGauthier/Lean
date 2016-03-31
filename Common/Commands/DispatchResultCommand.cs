using System;
using QuantConnect.Interfaces;
using QuantConnect.Packets;
using System.Xml.Serialization;

namespace QuantConnect.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DispatchResultCommand : ICommand
    {
        public DispatchResultCommand()
        {
            StakeHolder = new StakeHolder("mgl843@francisgauthier.ca", "4388684188");
        }
        public DispatchResultCommand(StakeHolder s)
        {
            StakeHolder = s;
        }
        [XmlIgnore]
        public StakeHolder StakeHolder { get; private set; }
        public CommandResultPacket Run(IAlgorithm algorithm)
        {
            algorithm.Notify.Email(StakeHolder.Email, "Stocks: Everthing went better thank expected", "You are rich!");
            algorithm.Notify.Sms(StakeHolder.PhoneNumber, "Stocks: Everthing went better thank expected");
            return new CommandResultPacket(this,true);
        }
    }

    public class StakeHolder
    {
        public StakeHolder(string email, string phoneNumber)
        {
            this.Email = email;
            this.PhoneNumber = phoneNumber;
        }

        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }

    }
}
