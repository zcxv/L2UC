using System;
using System.Collections.Generic;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Core.Security {
    
    public class RsaService {

        public static RsaService Instance { get; private set; } = new RsaService();

        public RsaKeyParameters UnscramblePublicKey(byte[] rsaKey) {
            byte[] key = new byte[rsaKey.Length];
            rsaKey.CopyTo(key, 0);

            // step 4: xor last 0x40 bytes with first 0x40 bytes
            for (int i = 0; i < 0x40; i++) {
                key[0x40 + i] = (byte)(key[0x40 + i] ^ key[i]);
            }

            // step 3: xor bytes 0x0d-0x10 with bytes 0x34-0x38
            for (int i = 0; i < 4; i++) {
                key[0x0d + i] = (byte)(key[0x0d + i] ^ key[0x34 + i]);
            }

            // step 2: xor first 0x40 bytes with last 0x40 bytes 
            for (int i = 0; i < 0x40; i++) {
                key[i] = (byte)(key[i] ^ key[0x40 + i]);
            }

            // step 1: 0x4d-0x50 <-> 0x00-0x04
            for (int i = 0; i < 4; i++) {
                (key[0x00 + i], key[0x4d + i]) = (key[0x4d + i], key[0x00 + i]);
            }

            var modulus = new BigInteger(1, key);
            var exponent = new BigInteger(1, new byte[] { 0x01, 0x00, 0x01 }); // 65537
            return new RsaKeyParameters(false, modulus, exponent);
        }

        public byte[] Encrypt1024NoPadding(byte[] raw, RsaKeyParameters key) {
            var engine = new RsaEngine();
            engine.Init(true, key);
            return engine.ProcessBlock(raw, 0, raw.Length);
        }

    }

}