﻿//  Copyright (c) 2018 Demerzel Solutions Limited
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

using Nethermind.Core.Extensions;
using Nethermind.Core.Test.Builders;
using Nethermind.Network.P2P.Subprotocols.Eth.V62;
using Nethermind.Serialization.Rlp;
using NUnit.Framework;

namespace Nethermind.Network.Test.P2P.Subprotocols.Eth.V62
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class BlockHeadersMessageSerializerTests
    {
        [Test]
        public void Roundtrip()
        {
            BlockHeadersMessage message = new BlockHeadersMessage();
            message.BlockHeaders = new[] {Build.A.BlockHeader.TestObject};

            BlockHeadersMessageSerializer serializer = new BlockHeadersMessageSerializer();
            byte[] bytes = serializer.Serialize(message);
            byte[] expectedBytes = Bytes.FromHexString("f901fcf901f9a0ff483e972a04a9a62bb4b7d04ae403c615604e4090521ecc5bb7af67f71be09ca01dcc4de8dec75d7aab85b567b6ccd41ad312451b948a7413f0a142fd40d49347940000000000000000000000000000000000000000a056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421a056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421a056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421b9010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000830f424080833d090080830f424083010203a02ba5557a4c62a513c7e56d1bf13373e0da6bec016755483e91589fe1c6d212e28800000000000003e8");

            Assert.AreEqual(bytes.ToHexString(), expectedBytes.ToHexString(), "bytes");

            BlockHeadersMessage deserialized = serializer.Deserialize(bytes);
            Assert.AreEqual(message.BlockHeaders.Length, deserialized.BlockHeaders.Length, "length");
            Assert.AreEqual(message.BlockHeaders[0].Hash, deserialized.BlockHeaders[0].Hash, "hash");

            SerializerTester.TestZero(serializer, message);
        }
        
        [Test]
        public void Roundtrip_nulls()
        {
            BlockHeadersMessage message = new BlockHeadersMessage();
            message.BlockHeaders = new[] {Build.A.BlockHeader.TestObject, null};

            BlockHeadersMessageSerializer serializer = new BlockHeadersMessageSerializer();
            byte[] bytes = serializer.Serialize(message);

            BlockHeadersMessage deserialized = serializer.Deserialize(bytes);
            Assert.AreEqual(message.BlockHeaders.Length, deserialized.BlockHeaders.Length, "length");
            Assert.AreEqual(message.BlockHeaders[0].Hash, deserialized.BlockHeaders[0].Hash, "hash");
            Assert.Null(message.BlockHeaders[1]);

            SerializerTester.TestZero(serializer, message);
        }

        [Test]
        public void Can_decode_249_bloom()
        {
            Rlp rlp = new Rlp(Bytes.FromHexString("f910d6f90215a08b8c20b1111b5878303659d6d031410dae6b47585fe234e20b938dbaf6a9923aa0866fb183df639e433c5384d72dd1c35b35211369ae422bf579ff7afe2fb34c4e9428921e4e2c9d84f4c0f0c0ceb991f45751a0fe93a04ba55dee2c3db4c86330c11d41d344361c31969c40837d4a522b43ebd0c19b82a056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421a056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421f90102817f00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000850491f9d09282011f821388808455ba455d99476574682f76312e302e302f6c696e75782f676f312e342e32a00b7569d0f36a9f4fde37f77eca6a28a63e1492a92424b72e22de6f3410053c3f886647bd7a7a538ae8f90215a0295e34c3e8e8c2a6c47c57fbbca73b4e5e850d520c1edb3beb14c0fa4c947f26a01dcc4de8dec75d7aab85b567b6ccd41ad312451b948a7413f0a142fd40d493479428921e4e2c9d84f4c0f0c0ceb991f45751a0fe93a0834a40551ab921955a21ae89a496fb66a421cff1e2b37540c0b77866a99593eea056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421a056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421f90102817f000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000008504928c0fcc820120821388808455ba455e99476574682f76312e302e302f6c696e75782f676f312e342e32a0db320302cb2151d8e4f5cdcfe151a9bd5d24aa42bf7dcb13bb6d478fd1a21a0888c4492cd95dac48a7f9021ca062e0387d05aff1fe68908a4b1fe16ec77404c0ea47eae69fff7de169bea22fc3a01dcc4de8dec75d7aab85b567b6ccd41ad312451b948a7413f0a142fd40d49347945088d623ba0fcf0131e0897a91734a4d83596aa0a00599a594fee6857c194abfc7527f5d2fa640a402db82df333651160fb0312c6da056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421a056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421f90102817f000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000008504931e614d820121821388808455ba455fa0476574682f76312e302e302d66633739643332642f6c696e75782f676f312e34a0cf66152a3b185b7279266af8f58a456d4c306d3beb9de4e6322e54fa86b575988861e28007c09d5da5f90215a0db2ca8e376e9cf43b22247f8667d2f442a372cbf69eb0c9e2092c26cb181d9cea0bcfd748f0c3f3df45082eb8e9df9a374b9ee5c7452cde33cf3e9e66e777e5c739428921e4e2c9d84f4c0f0c0ceb991f45751a0fe93a0baef66ae9f81dd8c760311b4c8fc40f778c79adfb222b7db2296755c62fe160ba056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421a056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421f90102817f00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000850493b0c519820122821388808455ba456299476574682f76312e302e302f6c696e75782f676f312e342e32a0e2d67432faa69b6e8becc228a2263f0d6170304bc7cabb424b0c9a025bfcd8e788dd0fdf364b69c838f9021aa07abfd11e862ccde76d6ea8ee20978aac26f4bcb55de1188cc0335be13e817017a01dcc4de8dec75d7aab85b567b6ccd41ad312451b948a7413f0a142fd40d4934794bb7b8287f3f0a933474a79eae42cbca977791171a03fe6bd17aa85376c7d566df97d9f2e536f37f7a87abb3a6f9e2891cf9442f2e4a056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421a056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421f90102817f00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000850494433b31820123821388808455ba45649e476574682f4c5649562f76312e302e302f6c696e75782f676f312e342e32a0943056aa305aa6d22a3c06110942980342d1f4d4b11c17711961436a0f963ea08829d6547c196e00e0f9021aa0c5dab4e189004a1312e9db43a40abb2de91ad7dd25e75880bf36016d8e9df524a01dcc4de8dec75d7aab85b567b6ccd41ad312451b948a7413f0a142fd40d4934794bb7b8287f3f0a933474a79eae42cbca977791171a0a81ba06268f6c38a31f013533919865c9ef9bf99000be82448b3636854796d78a056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421a056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421f90102817f00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000850494d5c398820124821388808455ba45689e476574682f4c5649562f76312e302e302f6c696e75782f676f312e342e32a0e3316e17ecdaf6bd28a80d5d8d7d0d949db4a8a270e47b1f81d2483f714145af88d3c2b61d983eadaaf9021aa0feeb6c4b368a1b1e2352a1294d8639c30ae0a80649774b27affafb630c374d4ea08f3b8f2291c71ab534edfa0f12152030a37d39395c645d0a38a7e125f6715a5f94bb7b8287f3f0a933474a79eae42cbca977791171a0698bf7bd1e9944e79e2e336cd7ad10b6ebc6d3e59b1e60bed04ad0620409854ba056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421a056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421f90102817f00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000850495685e50820125821388808455ba45699e476574682f4c5649562f76312e302e302f6c696e75782f676f312e342e32a0248937d2c7e78b06f22f094df2b31924a585206b1e5272a5282704359bc173af88a9be835214328debf90215a075a4bcc34789e630cf090c35e963509ec722536691abffb92e0f39681ec6a485a01dcc4de8dec75d7aab85b567b6ccd41ad312451b948a7413f0a142fd40d493479428921e4e2c9d84f4c0f0c0ceb991f45751a0fe93a0e38d24a17a68dc6ce7d9de9e5dfc717940520440fd5715a6956f8e551655eb42a056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421a056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421f90102817f00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000850495fb0b5b820126821388808455ba456a99476574682f76312e302e302f6c696e75782f676f312e342e32a09fa5dbef8aacbd9d07a070621f42ccc84e66b188e091583013a7eeba7a67b7fd882729cad648f253bd"));
            // f9 01 02 81 7f 0 0 0 ... 0
            // 249 -> 258 -> 129 127 0 0 0 ... 0 (strange?)
            BlockHeadersMessageSerializer serializer = new BlockHeadersMessageSerializer();
            BlockHeadersMessage message = serializer.Deserialize(rlp.Bytes);
            Assert.AreEqual(8, message.BlockHeaders.Length);
        }

        [Test]
        public void Throws_on_invalid_goerli_headers()
        {
            Rlp rlp1 = new Rlp(Bytes.FromHexString("f901d8f901d5a06c1b254ca0552790694760d5435bc4812ab1688c75ad078c9e813d1eb4139c80a01dcc4de8dec75d7aab85b567b6ccd41ad312451b948a7413f0a142fd40d49347940000000000000000000000000000000000000000a0518af1940525e0b68d089416a0c79ff0f244c4c8f7d65c32f2581c6833bae7aca056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421a056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421b90100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000282b2de837a120080845bf2a08883010203000102000102"));
            Rlp rlp2 = new Rlp(Bytes.FromHexString("f901e1f901daf901d5a06c1b254ca0552790694760d5435bc4812ab1688c75ad078c9e813d1eb4139c80a01dcc4de8dec75d7aab85b567b6ccd41ad312451b948a7413f0a142fd40d49347940000000000000000000000000000000000000000a0518af1940525e0b68d089416a0c79ff0f244c4c8f7d65c32f2581c6833bae7aca056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421a056e81f171bcc55a6ff8345e692c0f86e5b48e01b996cadc001622fb5e363b421b90100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000282b2de837a120080845bf2a08883010203000102000102c0c083010866"));
            // f9 01 02 81 7f 0 0 0 ... 0
            // 249 -> 258 -> 129 127 0 0 0 ... 0 (strange?)
            BlockHeadersMessageSerializer serializer = new BlockHeadersMessageSerializer();
            Assert.Throws<RlpException>(() => serializer.Deserialize(rlp1.Bytes));
            Assert.Throws<RlpException>(() => serializer.Deserialize(rlp2.Bytes));
        }
        
        [Test]
        public void To_string()
        {
            BlockHeadersMessage newBlockMessage = new BlockHeadersMessage();
            _ = newBlockMessage.ToString();
        }
    }
}