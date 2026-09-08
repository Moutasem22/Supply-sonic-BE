using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace NReco.PdfGenerator
{
    public sealed class LicenseInfo
    {
        private LicenseInfo.Info I;

        private const int magic_pub_idx = 20;

        private const int magic_size = 4;

        public bool IsLicensed
        {
            get
            {
                return this.I.IsLicensed;
            }
        }

        public string LicenseOwner
        {
            get
            {
                return this.I.Owner;
            }
        }

        internal LicenseInfo()
        {
            this.I = new LicenseInfo.Info()
            {
                IsLicensed = false
            };
        }

        private static byte[] BlockCopy(byte[] source, int startAt, int size)
        {
            if (source == null || (int)source.Length < startAt + size)
            {
                return null;
            }
            byte[] numArray = new Byte[size];
            Buffer.BlockCopy(source, startAt, numArray, 0, size);
            return numArray;
        }

        internal void Check()
        {
            //if (this.IsLicensed && !String.IsNullOrEmpty(this.LicenseOwner))
            //{
            //    return;
            //}
            //string str = null;
            //string str1 = null;
            //if (!String.IsNullOrEmpty(str) && !String.IsNullOrEmpty(str1))
            //{
            //    this.SetLicenseKey(str1, str);
            //    if (this.IsLicensed && !String.IsNullOrEmpty(this.LicenseOwner))
            //    {
            //        return;
            //    }
            //}
            //throw new Exception("This feature requires PdfGenerator commercial license key: http://www.nrecosite.com/pdf_generator_net.aspx");
        }

        private byte[] GetLicenseKeyBytes(string key)
        {
            byte[] numArray;
            try
            {
                numArray = Convert.FromBase64String(key);
            }
            catch
            {
                throw new Exception("Invalid license key");
            }
            return numArray;
        }

        private static RSAParameters GetPublicKeyRSAParameters(byte[] keyBytes)
        {
            RSAParameters rSAParameter = new RSAParameters();
            if (keyBytes == null || (int)keyBytes.Length < 1)
            {
                throw new ArgumentNullException("keyBytes");
            }
            int num = 20 + 4 + 4;
            int num1 = 4;
            int num2 = num + num1;
            int num3 = 128;
            int num4 = num2 + num3;
            int num5 = 64;
            int num6 = num4 + num5;
            int num7 = 64;
            int num8 = num6 + num7;
            int num9 = 64;
            int num10 = num8 + num9;
            int num11 = 64;
            int num12 = num10 + num11;
            int num13 = 64;
            int num14 = num12 + num13;
            int num15 = 128;
            rSAParameter.Exponent = LicenseInfo.BlockCopy(keyBytes, num, num1);
            Array.Reverse(rSAParameter.Exponent);
            rSAParameter.Modulus = LicenseInfo.BlockCopy(keyBytes, num2, num3);
            Array.Reverse(rSAParameter.Modulus);
            if (1 != 0)
            {
                return rSAParameter;
            }
            rSAParameter.P = LicenseInfo.BlockCopy(keyBytes, num4, num5);
            Array.Reverse(rSAParameter.P);
            rSAParameter.Q = LicenseInfo.BlockCopy(keyBytes, num6, num7);
            Array.Reverse(rSAParameter.Q);
            rSAParameter.DP = LicenseInfo.BlockCopy(keyBytes, num8, num9);
            Array.Reverse(rSAParameter.DP);
            rSAParameter.DQ = LicenseInfo.BlockCopy(keyBytes, num10, num11);
            Array.Reverse(rSAParameter.DQ);
            rSAParameter.InverseQ = LicenseInfo.BlockCopy(keyBytes, num12, num13);
            Array.Reverse(rSAParameter.InverseQ);
            rSAParameter.D = LicenseInfo.BlockCopy(keyBytes, num14, num15);
            Array.Reverse(rSAParameter.D);
            return rSAParameter;
        }

        public void SetLicenseKey(string owner, string key)
        {
            byte[] licenseKeyBytes = this.GetLicenseKeyBytes(key);
            byte[] publicKey = typeof(LicenseInfo).Assembly.GetName().GetPublicKey();
            if (publicKey == null)
            {
                throw new Exception("PdfGenerator is not strongly signed");
            }
            using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
            {
                RSAParameters publicKeyRSAParameters = LicenseInfo.GetPublicKeyRSAParameters(publicKey);
                rSACryptoServiceProvider.PersistKeyInCsp = false;
                rSACryptoServiceProvider.ImportParameters(publicKeyRSAParameters);
                SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
                try
                {
                    if (!rSACryptoServiceProvider.VerifyData(Encoding.UTF8.GetBytes(owner), sHA1CryptoServiceProvider, licenseKeyBytes))
                    {
                        throw new Exception();
                    }
                    this.I.Owner = owner;
                    this.I.IsLicensed = !String.IsNullOrEmpty(this.I.Owner);
                }
                catch (Exception exception)
                {
                    throw new Exception("Invalid license owner or key");
                }
            }
        }

        internal sealed class Info
        {
            internal bool IsLicensed;

            internal string Owner;

            internal Info()
            {
            }
        }
    }
}