using Auto;
using NUnit.Framework;
using System;
using System.Reflection;

namespace Incident
{
    [TestFixture]
    public class INC_Category_c_logout
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

        }
        //***********************************************************************************************************************************
        #endregion End - Tear down test case (NEED TO UPDATE: write result)

        #region Define variables and objects (class) are used in test cases (NEED TO UPDATE: This case variables)
        //***********************************************************************************************************************************
        
        //------------------------------------------------------------------
        Auto.Login login = null;
        Auto.Home home = null;
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
                home = new Home(Base);
                login = new Login(Base);
                //------------------------------------------------------------------                
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_End_Logout()
        {
            try
            {
                flag = home.Logout();
                if (flag)
                {
                    login.WaitLoading();
                }
                else
                {
                    error = "Error when logout system.";
                }
            }
            catch (Exception wex)
            {
                flag = false;
                error = wex.Message;
            }
        }
        #endregion End - Scenario of test case (NEED TO UPDATE)
    }
}
