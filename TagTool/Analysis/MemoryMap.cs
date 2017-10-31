using System;
using System.Collections.Generic;

namespace TagTool.Analysis
{
    public class MemoryMap
    {
        private readonly uint _baseAddress;
        private readonly uint _endAddress;
        private readonly List<uint> _addresses = new List<uint>();

        public MemoryMap(uint baseAddress, uint size)
        {
            if (size == 0)
                throw new ArgumentException("size is 0", "size");
            _baseAddress = baseAddress;
            _endAddress = _baseAddress + size;
            _addresses.Add(_baseAddress);
            _addresses.Add(_endAddress);
        }

        public bool IsAddressValid(uint address)
        {
            return (address >= _baseAddress && address <= _endAddress);
        }

        public void AddBoundary(uint address)
        {
            if (address < _baseAddress || address >= _endAddress)
                throw new ArgumentException("Invalid address", "address");
            var insertPos = _addresses.BinarySearch(address);
            if (insertPos >= 0)
                return; // Address is already defined
            insertPos = ~insertPos;
            _addresses.Insert(insertPos, address);
        }

        public void AddBoundaries(IEnumerable<uint> addresses)
        {
            foreach (var address in addresses)
                AddBoundary(address);
        }

        public bool IsBoundary(uint address)
        {
            return (_addresses.BinarySearch(address) >= 0);
        }

        public uint GetNextBoundary(uint address)
        {
            if (!IsAddressValid(address))
                throw new ArgumentException("Invalid address", "address");
            var index = _addresses.BinarySearch(address);
            if (index >= 0)
                index++;
            else
                index = ~index;
            return (index < _addresses.Count) ? _addresses[index] : _endAddress;
        }

        public uint GetRegionStart(uint address)
        {
            if (!IsAddressValid(address))
                throw new ArgumentException("Invalid address", "address");
            var index = _addresses.BinarySearch(address);
            if (index <= 0)
                index = ~index - 1;
            return (index >= 0) ? _addresses[index] : _baseAddress;
        }
    }
}
