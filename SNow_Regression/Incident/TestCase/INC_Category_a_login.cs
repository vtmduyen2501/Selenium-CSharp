using NUnit.Framework;
using System;
using System.Reflection;

namespace Incident
{
    [TestFixture]
    public class INC_Category_a_login
    {
        #region Define default variables for test case (No need to update)
        //***********************************************************************************************************************************
        public bool flagC;
        public bool flag, flagExit, flagW;
        string caseName, error;
        Auto.obase Base;

        //***********************************************************************************************************************************
        #endregion End - Define default variables for test case (No need to update)

        #region Setup test case, set up and tear down test steps (No need to update)
        //***********************************************************************************************************************************
        [TestFixtureSetUp]
        public void Setup()
        {
            caseName = MethodBase.GetCurrentMethod().DeclaringType.Name;
            Base = new Auto.obase();
            Base.BeforeRunTestCase(caseName, ref Base, ref flagExit, ref flagW, ref flag, ref flagC);
        }
        //-------------------------------------------------------------------------------------------------
        [SetUp]
        public void RunBeforeAnyTests()
        {
            Base.BeforeRunTestStep(ref flag, ref flagExit, ref error);
        }
        //-------------------------------------------------------------------------------------------------
        [TearDown]
        public void RunAfterAnyTests()
        {
            Base.AfterRunTestStep(flag, ref flagExit, ref flagW, ref flagC, error);
        }
        //***********************************************************************************************************************************
        #endregion End - Setup test case, set up and tear down test steps (No need to update)

        #region Tear down test case (NEED TO UPDATE: write result)
        //***********************************************************************************************************************************
        [TestFixtureTearDown()]
        public void TearDown()
        {
            Base.AfterRunTestCase(flagC, caseName);

            System.Console.WriteLine("Finished");

            //----------------------------------------------------------------

            string temp = Base.GData("Debug").ToLower();

            if (Base.Driver != null && temp != "yes")
            {
                Base.Driver.Close();
                Base.Driver.Quit();
            }
        }
        //***********************************************************************************************************************************
        #endregion End - Tear down test case (NEED TO UPDATE: write result)

        #region Define variables and objects (class) are used in test cases (NEED TO UPDATE: This case variables)
        //***********************************************************************************************************************************
        
        //------------------------------------------------------------------
        Auto.Login login = null;
        Auto.Home home = null;
        Auto.Incident inc = null;
        //------------------------------------------------------------------

        //***********************************************************************************************************************************
        #endregion End - Define variables and objects (class) are used in test cases (NEED TO UPDATE: This case variables)

        #region Scenario of test case (NEED TO UPDATE)
        //***********************************************************************************************************************************

        [Test]
        public void ClassInit()
        {
            try
            {
                //------------------------------------------------------------------
                login = new Auto.Login(Base);
                home = new Auto.Home(Base);
                inc = new Auto.Incident(Base, "Incident");
                //------------------------------------------------------------------
                
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_001_OpenSystem()
        {
            try
            {
                Base.Driver.Navigate().GoToUrl(Base.GData("Url"));
                login.WaitLoading();
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_002_Login()
        {
            try
            {
                string user = Base.GData("User");
                string pwd = Base.GData("Pwd");
                flag = login.LoginToSystem(user, pwd);
                if (flag)
                {
                    home.WaitLoading();
                    string temp = home.Label_UserFullName().Text;
                    if (Base.GData("UserFullName") != temp)
                    {
                        flag = false;
                        flagExit = false;
                        error = "Welcome login account is NOT correct.";
                    }
                }
                else
                {
                    error = "Cannot login to system.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_003_ImpersonateUser_SDA()
        {
            try
            {
                string temp = Base.GData("SDA");
                flag = home.ImpersonateUser(temp);
                if (!flag) { error = "Cannot impersonate user (" + temp + ")"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_004_SystemSetting()
        {
            try
            {
                flag = home.SystemSetting();
                if (!flag) { error = "Error when config system."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        #endregion End - Scenario of test case (NEED TO UPDATE)
    }
}
