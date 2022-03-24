using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ELMFS_40340725;

namespace ELMFSTest.Test
{
    [TestClass]
    public class ELMFSTest
    {
        // Testing the handling of abbreviations
        [TestMethod]
        public void AbbreviationTest()
        {
            string testCase1 = "This is to test if ASAP is handled correctly";
            string expectedResult = "This is to test if ASAP <As soon as possible> is handled correctly";
            AbreviationHandling abreviations = new AbreviationHandling();
            string actualResult = abreviations.ChangeAbreviations(testCase1);

            Assert.AreEqual(expectedResult, actualResult);

        }

        // Testing the handling of abbreviations
        [TestMethod]
        public void AbbreviationTest1()
        {
            string testCase2 = "This is to test if AAS is handled correctly";
            string expectedResult = "This is to test if AAS <Alive and smiling> is handled correctly";
            AbreviationHandling abreviations = new AbreviationHandling();
            string actualResult = abreviations.ChangeAbreviations(testCase2);

            Assert.AreEqual(expectedResult, actualResult);

        }

        // Testing the handling of abbreviations
        [TestMethod]
        public void AbbreviationTest2()
        {
            string testCase3 = "This is to test if ROTFL is handled correctly";
            string expectedResult = "This is to test if ROTFL <Rolling on the floor laughing> is handled correctly";
            AbreviationHandling abreviations = new AbreviationHandling();
            string actualResult = abreviations.ChangeAbreviations(testCase3);

            Assert.AreEqual(expectedResult, actualResult);

        }

        // Testing the handling of url
        [TestMethod]
        public void UrlProcessorTest()
        {
            string testCase4 = "This is to test if http://google.com is handled correctly";
            string expectedResult = "This is to test if  <URL Quarantined>  is handled correctly";
            UrlProcessor urlProcessor = new UrlProcessor();
            string actualResult = urlProcessor.FindUrl(testCase4);
            Assert.AreEqual(expectedResult, actualResult);
        }

        // Testing the handling of url
        [TestMethod]
        public void UrlProcessorTest1()
        {
            string testCase5 = "This is to test if http://facebook.com is handled correctly";
            string expectedResult = "This is to test if  <URL Quarantined>  is handled correctly";
            UrlProcessor urlProcessor = new UrlProcessor();
            string actualResult = urlProcessor.FindUrl(testCase5);
            Assert.AreEqual(expectedResult, actualResult);
        }

        // Testing the handling of url
        [TestMethod]
        public void UrlProcessorTest2()
        {
            string testCase6 = "This is to test if http://napier.com is handled correctly";
            string expectedResult = "This is to test if  <URL Quarantined>  is handled correctly";
            UrlProcessor urlProcessor = new UrlProcessor();
            string actualResult = urlProcessor.FindUrl(testCase6);
            Assert.AreEqual(expectedResult, actualResult);
        }

        // Testing correct format for SMS
        [TestMethod]
        public void processTxtBoxTest1()
        {
            string testCase7a = "S123456789";
            string testCase7b = "12345678912 This is the content of message body";
            string expectedResult = "  This is the content of message body";
            ProcessTxtBox processTxtBox = new ProcessTxtBox();
            string actualResult = processTxtBox.procesTxtBox(testCase7a, testCase7b);
            Assert.AreEqual(expectedResult, actualResult);
        }

        // Testing correct format for Tweets
        [TestMethod]
        public void processTxtBoxTest2()
        {
            string testCase8a = "T123456789";
            string testCase8b = "@testcase This is the content of message body";
            string expectedResult = "  This is the content of message body";
            ProcessTxtBox processTxtBox = new ProcessTxtBox();
            string actualResult = processTxtBox.procesTxtBox(testCase8a, testCase8b);
            Assert.AreEqual(expectedResult, actualResult);
        }

        // Testing correct format for emails
        [TestMethod]
        public void processTxtBoxTest3()
        {
            string testCase9a = "E123456789";
            string testCase9b = "noone@email.com This is the subject  This is the content of message body";
            string expectedResult = "  This is the content of message body";
            ProcessTxtBox processTxtBox = new ProcessTxtBox();
            string actualResult = processTxtBox.procesTxtBox(testCase9a, testCase9b);
            Assert.AreEqual(expectedResult, actualResult);
        }

        // Even having an incorrect message ID (not long enough), because it starts with an E
        // it still process the body correctly as the validation for message id length
        // is done somewhere else
        [TestMethod]
        public void processTxtBoxTest4()
        {
            string testCase10a = "E1234";
            string testCase10b = "noone@email.com This is the subject  This is the content of message body";
            string expectedResult = "  This is the content of message body";
            ProcessTxtBox processTxtBox = new ProcessTxtBox();
            string actualResult = processTxtBox.procesTxtBox(testCase10a, testCase10b);
            Assert.AreEqual(expectedResult, actualResult);
        }

        // In the following I test an incorrect message id
        [TestMethod]
        public void processTxtBoxTest5()
        {
            string testCase11a = "e1234";
            string testCase11b = "noone@email.com This is the subject  This is the content of message body";
            string expectedResult = "";
            ProcessTxtBox processTxtBox = new ProcessTxtBox();
            string actualResult = processTxtBox.procesTxtBox(testCase11a, testCase11b);
            Assert.AreEqual(expectedResult, actualResult);
        }

        // In the following I test an incorrect message Id
        [TestMethod]
        public void processTxtBoxTest6()
        {
            string testCase12a = "1234";
            string testCase12b = "noone@email.com This is the subject  This is the content of message body";
            string expectedResult = "";
            ProcessTxtBox processTxtBox = new ProcessTxtBox();
            string actualResult = processTxtBox.procesTxtBox(testCase12a, testCase12b);
            Assert.AreEqual(expectedResult, actualResult);
        }

        // please note that to be able to run this test you have to change the Sms class
        // and the FindPhoneNumber method to public until the testing is finished
        // Then uncoment the text below and yo will be able to run it
        /*[TestMethod]
        public void FindPhoneNumber1() 
        {
            string testCase13 = "12345678901 This is the content of the message body";
            string testCase13b = "S123456789";
            string expectedResult = "12345678901";
            Sms sms = new Sms(testCase13b, testCase13);
            string actualResult = sms.FindPhoneNumber(testCase13);
            Assert.AreEqual(expectedResult, actualResult);
        }*/
    }
}
