using System;
using TagTool.Cache;

namespace TagTool.Serialization
{
    public class ExternalAddressException : Exception
    {
        public CacheAddress Address { get; }
        public CacheAddressType Type => Address.Type;
        public int Offset => Address.Offset;

        public ExternalAddressException(CacheAddress address, string message = null) :
            base(message ?? address.ToString())
        {
            Address = address;
        }
    }
}