//  Copyright (c) 2021 Demerzel Solutions Limited
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

using Nethermind.Core;
using Nethermind.Core.Specs;
using Nethermind.Int256;

namespace Nethermind.Consensus.Transactions
{
    public class MinGasPriceTxFilter : IMinGasPriceTxFilter
    {
        private readonly UInt256 _minGasPrice;
        private readonly ISpecProvider _specProvider;

        public MinGasPriceTxFilter(
            UInt256 minGasPrice,
            ISpecProvider specProvider)
        {
            _minGasPrice = minGasPrice;
            _specProvider = specProvider;
        }

        public (bool Allowed, string Reason) IsAllowed(Transaction tx, BlockHeader parentHeader)
        {
            long number = (parentHeader?.Number ?? 0) + 1;
            return IsAllowed(tx, number, _minGasPrice);
        }

        public (bool Allowed, string Reason) IsAllowed(Transaction tx, long blockNumber, UInt256 minGasPriceFloor)
        {
            UInt256 gasPrice = tx.GasPrice;
            if (_specProvider.GetSpec(blockNumber).IsEip1559Enabled)
                gasPrice = tx.FeeCap;

            bool allowed = gasPrice >= minGasPriceFloor;
            return (allowed, allowed ? string.Empty : $"gas price too low {gasPrice} < {minGasPriceFloor}");
        }
    }
}
