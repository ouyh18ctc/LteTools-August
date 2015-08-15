using System;
using Lte.Domain.Regular;

namespace Lte.Domain.TypeDefs
{
    public interface IIpAddress
    {
        byte IpByte1 { get; set; }

        byte IpByte2 { get; set; }

        byte IpByte3 { get; set; }

        byte IpByte4 { get; set; }
    }

    public static class IpAddressOperations
    {
        public static string GetString(this IIpAddress address)
        {
            return address.IpByte1 + "." + address.IpByte2 + "."
                + address.IpByte3 + "." + address.IpByte4;
        }

        public static bool SetAddress(this IIpAddress address, string addressString)
        {
            if (!addressString.IsLegalIp()) { return false; }
            string[] parts = addressString.GetSplittedFields('.');
            address.IpByte1 = Convert.ToByte(parts[0]);
            address.IpByte2 = Convert.ToByte(parts[1]);
            address.IpByte3 = Convert.ToByte(parts[2]);
            address.IpByte4 = Convert.ToByte(parts[3]);
            return true;
        }
    }

    public class IpAddress : IIpAddress
    {
        public byte IpByte1 { get; set; }
        public byte IpByte2 { get; set; }
        public byte IpByte3 { get; set; }
        public byte IpByte4 { get; set; }

        public IpAddress()
        { }

        public IpAddress(string ip)
        {
            bool result = this.SetAddress(ip);
            if (!result) { this.SetAddress("0.0.0.0"); }
        }

        public string AddressString
        {
            get { return this.GetString(); }
        }

        public int AddressValue
        {
            get
            {
                return IpByte1 << 24 | IpByte2 << 16 | IpByte3 << 8 | IpByte4;
            }
            set
            {
                IpByte1 = (value >> 24).GetLastByte();
                IpByte2 = (value >> 16).GetLastByte();
                IpByte3 = (value >> 8).GetLastByte();
                IpByte4 = value.GetLastByte();
            }
        }
    }
}
