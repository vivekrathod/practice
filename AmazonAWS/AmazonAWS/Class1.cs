using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.RDS;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using NUnit.Framework;

namespace AmazonAWS
{
    [TestFixture]
    public class TestAWSAccess
    {
        [Test]
        public void TestSharedCredsFilePlaintextDefaultLocation()
        {
            var profileName = "shared_profile";
            var options = new CredentialProfileOptions
            {
                AccessKey = "testKeyId1",
                SecretKey = "testSecret1"
            };
            var profile = new CredentialProfile(profileName, options);
            profile.Region = RegionEndpoint.USWest1;
            var sharedFile = new SharedCredentialsFile();
            sharedFile.RegisterProfile(profile);

            //Get Credentials from the shared Shared Credentials File in the Default Location.
            var chain = new CredentialProfileStoreChain();
            AWSCredentials awsCredentials;
            Assert.True(chain.TryGetAWSCredentials(profileName, out awsCredentials));
            Assert.That(awsCredentials, Is.Not.Null);
        }

        [Test]
        public void TestSharedCredsFilePlaintextCustomLocation()
        {
            var profileName = "custom_shared_profile";
            var options = new CredentialProfileOptions
            {
                AccessKey = "testKeyId1",
                SecretKey = "testSecret1"
            };
            var profile = new CredentialProfile(profileName, options);
            profile.Region = RegionEndpoint.USWest1;
            var customSharedFilePath = @"c:\temp\custom_location\credentials";
            var sharedFile = new SharedCredentialsFile(customSharedFilePath);
            sharedFile.RegisterProfile(profile);

            //Get Credentials from the shared Shared Credentials File in the Default Location.
            var chain = new CredentialProfileStoreChain(customSharedFilePath);
            AWSCredentials awsCredentials;
            Assert.True(chain.TryGetAWSCredentials(profileName, out awsCredentials));
            Assert.That(awsCredentials, Is.Not.Null);
        }

        [Test]
        public void TestNetCredsFileEncrypted()
        {
            var profileName = "encrypted_dot_net_profile";
            var options = new CredentialProfileOptions
            {
                AccessKey = "testKeyId1",
                SecretKey = "testSecret1"
            };
            var profile = new CredentialProfile(profileName, options);
            profile.Region = RegionEndpoint.USWest1;
            var netSDKFile = new NetSDKCredentialsFile();
            netSDKFile.RegisterProfile(profile);

            //Get Credentials from the encrypted SDK Credentials File in the Default Location.
            var chain = new CredentialProfileStoreChain();
            AWSCredentials awsCredentials;
            Assert.True(chain.TryGetAWSCredentials(profileName, out awsCredentials));
            Assert.That(awsCredentials, Is.Not.Null);
        }

        [Test]
        public void TestRDSClient()
        {
            String filePath = Path.GetTempFileName();
            try
            {
                // the 'Shared Credentials File' can be manually created as well (as opposed to using the RegisterProfile method on SharedCredentialsFile class)
                const String profileName = "test_user_profile";
                const string accessKeyId = "testId1";
                const string accessKeySecret = "testKey1";
                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine($"[{profileName}]");
                    writer.WriteLine($"\taws_access_key_id = {accessKeyId}");
                    writer.WriteLine($"\taws_secret_access_key = {accessKeySecret} ");
                }

                // read credentials from the shared credentials file from specified location
                var chain = new CredentialProfileStoreChain(filePath);
                AWSCredentials awsCredentials;
                string expectedAccessKeyId = null;
                if (chain.TryGetAWSCredentials(profileName, out awsCredentials))
                {
                    // Use awsCredentials
                    expectedAccessKeyId = awsCredentials.GetCredentials().AccessKey;
                }
                Assert.That(accessKeyId, Is.EqualTo(expectedAccessKeyId));

                Assert.DoesNotThrow(() =>
                {
                    AmazonRDSClient client = new AmazonRDSClient(awsCredentials, RegionEndpoint.USEast1);
                    Assert.That(client.DescribeDBInstances(), Is.Not.Empty);
                });
            }
            finally
            {
                File.Delete(filePath);
            }
        }
    }
}