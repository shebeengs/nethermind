//  Copyright (c) 2018 Demerzel Solutions Limited
//  This file is part of the Nethermind library.
// 
//  The Nethermind library is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  The Nethermind library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with the Nethermind. If not, see <http://www.gnu.org/licenses/>.
// 

using System;
using Nethermind.Core;
using Nethermind.Core.Crypto;
using Nethermind.Trie;

namespace Nethermind.State.Witnesses
{
    public static class IKeyValueStoreExtensions
    {
        public static IKeyValueStore WitnessedBy(this IKeyValueStore @this, IWitnessCollector witnessCollector) => 
            witnessCollector == NullWitnessCollector.Instance ? @this : new WitnessingStore(@this, witnessCollector);
    }
    
    public class WitnessingStore : IKeyValueStore
    {
        private readonly IKeyValueStore _wrapped;
        private readonly IWitnessCollector _witnessCollector;

        public WitnessingStore(IKeyValueStore? wrapped, IWitnessCollector? witnessCollector)
        {
            _wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            _witnessCollector = witnessCollector ?? throw new ArgumentNullException(nameof(witnessCollector));
        }

        public byte[] this[byte[] key]
        {
            get
            {
                if (key.Length != 32)
                {
                    throw new NotSupportedException($"{nameof(WitnessingStore)} requires 32 bytes long keys.");
                }
                
                Touch(key);
                return _wrapped[key];
            }
            set => _wrapped[key] = value;
        }

        public void Touch(byte[] key)
        {
            _witnessCollector.Add(new Keccak(key));
        }
    }
}
