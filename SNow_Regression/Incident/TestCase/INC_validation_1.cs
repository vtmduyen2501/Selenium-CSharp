using Auto;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Incident
{
    [TestFixture]
    class INC_validation_1
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
            System.Console.WriteLine("*|||*[Run step:" + TestContext.CurrentContext.Test.Name + "]");
            System.Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
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

            System.Console.WriteLine("Finished - Incident Id: " + incidentId);

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

        otextbox textbox;
        olookup lookup;
        ocombobox combobox;
        
        //------------------------------------------------------------------
        Login login = null;
        Home home = null;
        Auto.Incident inc = null;
        //------------------------------------------------------------------
        string incidentId;
        Dictionary <string, oelement> listOfControl = null;

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
                incidentId = string.Empty;
                listOfControl = new Dictionary<string, oelement>();
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------
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

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_002_003_Login()
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
                    flag = (Base.GData("UserFullName") == temp);
                    if (!flag)
                    {
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

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_004_Impersonate_ServiceDesk()
        {
            try
            {
                flag = home.ImpersonateUser(Base.GData("ServiceDesk"));
                if (!flag) 
                { 
                    error = "Cannot impersonate Service Desk"; 
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_005_SystemSetting()
        {
            try
            {
                //flag = home.SystemSetting();
                //if (!flag) 
                //{ 
                //    error = "Error when config system."; 
                //}
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_006_01_Open_NewIncident()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Incident", "Create New");
                if (flag)
                {
                    inc.WaitLoading();
                }
                else
                {
                    error = "Error when create new incident.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_006_02_VerifyDefaultValue()
        {
            try
            {
                error = "";

                // Verify Category
                combobox = inc.Combobox_Category;
                if(combobox.Existed)
                {
                    flag = combobox.VerifyCurrentText("-- None --");
                    if(!flag)
                    {
                        error += "The default value of Category is not [-- None --]";
                    }
                }
                else
                {
                    error += "Cannot get Category combobox";
                }

                // Verify SubCategory
                combobox = inc.Combobox_SubCategory;
                if (combobox.Existed)
                {
                    flag = combobox.VerifyCurrentText("-- None --");
                    if (!flag)
                    {
                        error += "The default value of SubCategory is not [-- None --]";
                    }
                }
                else
                {
                    error += "Cannot get SubCategory combobox";
                }

                // Verify Contact Type
                combobox = inc.Combobox_ContactType;
                if (combobox.Existed)
                {
                    flag = combobox.VerifyCurrentText("Phone");
                    if (!flag)
                    {
                        error += "The default value of Contact Type is not [Phone]";
                    }
                }
                else
                {
                    error += "Cannot get Contact Type combobox";
                }

                // Verify State
                combobox = inc.Combobox_State;
                if (combobox.Existed)
                {
                    flag = combobox.VerifyCurrentText("New");
                    if (!flag)
                    {
                        error += "The default value of State is not [New]";
                    }
                }
                else
                {
                    error += "Cannot get State combobox";
                }

                // Verify Impact
                combobox = inc.Combobox_Impact;
                if (combobox.Existed)
                {
                    flag = combobox.VerifyCurrentText("3 - Medium");
                    if (!flag)
                    {
                        error += "The default value of Impact is not [3 - Medium]";
                    }
                }
                else
                {
                    error += "Cannot get Impact combobox";
                }

                // Verify Urgency
                combobox = inc.Combobox_Urgency;
                if (combobox.Existed)
                {
                    flag = combobox.VerifyCurrentText("3 - Medium");
                    if (!flag)
                    {
                        error += "The default value of Urgency is not [3 - Medium]";
                    }
                }
                else
                {
                    error += "Cannot get Urgency combobox";
                }

                // Verify Priority
                combobox = inc.Combobox_Priority;
                if (combobox.Existed)
                {
                    flag = combobox.VerifyCurrentText("3 - Medium");
                    if (!flag)
                    {
                        error += "The default value of Priority is not [3 - Medium]";
                    }
                }
                else
                {
                    error += "Cannot get Priority combobox";
                }

                if (error != "") { flag = false; flagExit = false; }
            }
            catch (Exception wex)
            {
                flag = false;
                error = wex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_007_Verify_MandatoryFields()
        {
            try
            {
                string expectedControlList = "";
                error = string.Empty;                

                // Initiate the list of controls on form
                listOfControl = inc.GControlByType("mandatory");

                expectedControlList = Base.GData("MandatoryFields_Visible");
                inc.VerifyControls(expectedControlList, "mandatory", listOfControl, ref error);
                
                expectedControlList = Base.GData("MandatoryFields_Enable");
                inc.VerifyControls(expectedControlList, "mandatory", listOfControl, ref error, "enable");
                
                expectedControlList = Base.GData("MandatoryFields_Readonly");
                inc.VerifyControls(expectedControlList, "mandatory", listOfControl, ref error, "readonly");
                
                if (error != string.Empty) { flag = false; flagExit = false; }
            }
            catch (Exception wex)
            {
                flag = false;
                error = wex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_008_Verify_TextboxFields()
        {
            try
            {
                string expectedControlList = "";
                error = string.Empty;

                // Initiate the list of controls on form
                listOfControl = inc.GControlByType("textbox");

                expectedControlList = Base.GData("TextboxFields_Visible");
                inc.VerifyControls(expectedControlList, "textbox", listOfControl, ref error);

                expectedControlList = Base.GData("TextboxFields_Enable");
                inc.VerifyControls(expectedControlList, "textbox", listOfControl, ref error, "enable", true);

                expectedControlList = Base.GData("TextboxFields_Readonly");
                inc.VerifyControls(expectedControlList, "textbox", listOfControl, ref error, "readonly", true);

                if (error != string.Empty) { flag = false; flagExit = false; }
            }
            catch (Exception wex)
            {
                flag = false;
                error = wex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_009_Verify_TextareaFields()
        {
            try
            {
                string expectedControlList = "";
                error = string.Empty;

                // Initiate the list of controls on form
                listOfControl = inc.GControlByType("textarea");

                expectedControlList = Base.GData("TextareaFields_Visible");
                inc.VerifyControls(expectedControlList, "textarea", listOfControl, ref error);
               
                expectedControlList = Base.GData("TextareaFields_Enable");
                inc.VerifyControls(expectedControlList, "textarea", listOfControl, ref error, "enable", true);
               
                expectedControlList = Base.GData("TextareaFields_Readonly");
                inc.VerifyControls(expectedControlList, "textarea", listOfControl, ref error, "readonly", true);

                if (error != string.Empty) { flag = false; flagExit = false; }
            }
            catch (Exception wex)
            {
                flag = false;
                error = wex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_010_Verify_LookupFields()
        {
            try
            {
                string expectedControlList = "";
                error = string.Empty;

                // Initiate the list of controls on form
                listOfControl = inc.GControlByType("lookup");

                expectedControlList = Base.GData("LookupFields_Visible");
                inc.VerifyControls(expectedControlList, "lookup", listOfControl, ref error);

                expectedControlList = Base.GData("LookupFields_Enable");
                inc.VerifyControls(expectedControlList, "lookup", listOfControl, ref error, "enable", true);

                expectedControlList = Base.GData("LookupFields_Readonly");
                inc.VerifyControls(expectedControlList, "lookup", listOfControl, ref error, "readonly", true);

                if (error != string.Empty) { flag = false; flagExit = false; }
            }
            catch (Exception wex)
            {
                flag = false;
                error = wex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_011_Verify_DatetimeFields()
        {
            try
            {
                string expectedControlList = "";
                error = string.Empty;

                // Initiate the list of controls on form
                listOfControl = inc.GControlByType("datetime");

                expectedControlList = Base.GData("DatetimeFields_Visible");
                inc.VerifyControls(expectedControlList, "datetime", listOfControl, ref error);

                expectedControlList = Base.GData("DatetimeFields_Enable");
                inc.VerifyControls(expectedControlList, "datetime", listOfControl, ref error, "enable", true);

                expectedControlList = Base.GData("DatetimeFields_Readonly");
                inc.VerifyControls(expectedControlList, "datetime", listOfControl, ref error, "readonly", true);

                if (error != string.Empty) { flag = false; flagExit = false; }
            }
            catch (Exception wex)
            {
                flag = false;
                error = wex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_012_Verify_CheckboxFields()
        {
            try
            {
                string expectedControlList = "";
                error = string.Empty;

                // Initiate the list of controls on form
                listOfControl = inc.GControlByType("checkbox");

                expectedControlList = Base.GData("CheckboxFields_Visible");
                inc.VerifyControls(expectedControlList, "checkbox", listOfControl, ref error);

                expectedControlList = Base.GData("CheckboxFields_Enable");
                inc.VerifyControls(expectedControlList, "checkbox", listOfControl, ref error, "enable", true);

                expectedControlList = Base.GData("CheckboxFields_Readonly");
                inc.VerifyControls(expectedControlList, "checkbox", listOfControl, ref error, "readonly", true);

                if (error != string.Empty) { flag = false; flagExit = false; }
            }
            catch (Exception wex)
            {
                flag = false;
                error = wex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_013_01_Verify_ComboboxFields()
        {
            try
            {
                string expectedControlList = "";
                error = string.Empty;

                // Initiate the list of controls on form
                listOfControl = inc.GControlByType("combobox");

                expectedControlList = Base.GData("ComboboxFields_Visible");
                inc.VerifyControls(expectedControlList, "combobox", listOfControl, ref error);

                expectedControlList = Base.GData("ComboboxFields_Enable");
                inc.VerifyControls(expectedControlList, "combobox", listOfControl, ref error, "enable", true);
 

                expectedControlList = Base.GData("ComboboxFields_Readonly");
                inc.VerifyControls(expectedControlList, "combobox", listOfControl, ref error, "readonly", true);

                if (error != string.Empty) { flag = false; flagExit = false; }
            }
            catch (Exception wex)
            {
                flag = false;
                error = wex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_013_02_Verify_Subcategory()
        {
            try
            {
                string expectedControlList = Base.GData("SubcategoryField");

                flag = inc.VerifySubcategory(expectedControlList);
                if(!flag)
                {
                    error = "There is something wrong when checking Category and Subcategory";
                    flagExit = false;
                }
            }
            catch (Exception wex)
            {
                flag = false;
                error = wex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_014_01_Select_State_Resolved()
        {
            try
            {
                /* Set the State = 'Resolved' to display the 2 Close code and Close notes */
                flag = inc.Combobox_State.SelectItem("Resolved");
                if(!flag)
                {
                    error = "Cannot select [Resolve] in State combobox";
                }
            }
            catch (Exception wex)
            {
                flag = false;
                error = wex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_014_02_Verify_Additional_MandatoryFields()
        {
            try
            {
                string expectedControlList = "";
                error = string.Empty;

                // Initiate the list of controls on form
                listOfControl = inc.GControlByType("mandatory");

                expectedControlList = Base.GData("AdditionalMandatoryFields_Visible");
                inc.VerifyControls(expectedControlList, "mandatory", listOfControl, ref error);

                expectedControlList = Base.GData("AdditionalMandatoryFields_Enable");
                inc.VerifyControls(expectedControlList, "mandatory", listOfControl, ref error, "enable", true);
                
                expectedControlList = Base.GData("AdditionalMandatoryFields_Readonly");
                inc.VerifyControls(expectedControlList, "mandatory", listOfControl, ref error, "readonly", true);

                if (error != string.Empty) { flag = false; flagExit = false; }
            }
            catch (Exception wex)
            {
                flag = false;
                error = wex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_014_03_Verify_Additional_ComboboxFields()
        {
            try
            {
                string expectedControlList = "";
                error = string.Empty;

                // Initiate the list of controls on form
                listOfControl = inc.GControlByType("combobox");

                expectedControlList = Base.GData("AdditionalComboboxFields_Visible");
                inc.VerifyControls(expectedControlList, "combobox", listOfControl, ref error);

                expectedControlList = Base.GData("AdditionalComboboxFields_Enable");
                inc.VerifyControls(expectedControlList, "combobox", listOfControl, ref error, "enable", true);

                expectedControlList = Base.GData("AdditionalComboboxFields_Readonly");
                inc.VerifyControls(expectedControlList, "combobox", listOfControl, ref error, "readonly", true);

                if (error != string.Empty) { flag = false; flagExit = false; }
            }
            catch (Exception wex)
            {
                flag = false;
                error = wex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_014_04_Verify_Additional_TextareaFields()
        {
            try
            {
                string expectedControlList = "textarea";
                error = string.Empty;

                // Initiate the list of controls on form
                listOfControl = inc.GControlByType("textarea");

                expectedControlList = Base.GData("AdditionalTextareaFields_Visible");
                inc.VerifyControls(expectedControlList, "textarea", listOfControl, ref error);

                expectedControlList = Base.GData("AdditionalTextareaFields_Enable");
                inc.VerifyControls(expectedControlList, "textarea", listOfControl, ref error, "enable", true);

                expectedControlList = Base.GData("AdditionalTextareaFields_Readonly");
                inc.VerifyControls(expectedControlList, "textarea", listOfControl, ref error, "readonly", true);

                if (error != string.Empty) { flag = false; flagExit = false; }
            }
            catch (Exception wex)
            {
                flag = false;
                error = wex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_15_01_CreateIncident()
        {
            try
            {
                error = "";
                string temp = "";

                #region Get the Incident id
                
                textbox = inc.Textbox_Number;
                if (textbox.Existed)
                {
                    textbox.Click();

                    //-- Store incident id
                    incidentId = textbox.Text;
                    Console.WriteLine("-*-[Store]: Incident Id:(" + incidentId + ")");
                }
                else
                {
                    error += "Cannot get the Number textbox";
                }

                #endregion

                #region Populate Caller
                
                lookup = inc.Lookup_Caller;
                if (lookup.Existed)
                {
                    flag = lookup.Select(Base.GData("Caller"));
                    if (!flag) 
                    { 
                        error += "Cannot populate caller value."; 
                    }
                }
                else 
                { 
                    error += "Cannot get lookup caller."; 
                }

                #endregion

                #region Populate Category

                combobox = inc.Combobox_Category;
                if (combobox.Existed)
                {
                    flag = combobox.SelectItem(Base.GData("Category"));
                    if (!flag)
                    { 
                        error += "Cannot populate category value."; 
                    }
                }
                else
                {
                    error += "Cannot get combobox category.";
                }

                #endregion

                #region Populate Subcategory

                temp = Base.GData("Subcategory");
                combobox = inc.Combobox_SubCategory;
                if (combobox.Existed)
                {
                    flag = inc.VerifyHaveItemInComboboxList("subcategory", temp);
                    if (flag)
                    {
                        flag = combobox.SelectItem(temp);
                        if (flag)
                        {
                            inc.WaitLoading();
                        }
                        else
                        {
                            error += "Cannot populate sub category value.";
                        }
                    }
                    else
                    {
                        error += "Not found item (" + temp + ") in sub category list.";
                    }
                }
                else
                {
                    error += "Cannot get combobox sub category.";
                }

                #endregion

                #region Populate Short Description

                textbox = inc.Textbox_ShortDescription;
                if (textbox.Existed)
                {
                    flag = textbox.SetText(Base.GData("ShortDescription"));
                    if (!flag) 
                    { 
                        error += "Cannot populate short description value."; 
                    }
                }
                else 
                { 
                    error += "Cannot get textbox short description."; 
                }

                #endregion

                #region Set State = 'Active'

                combobox = inc.Combobox_State;
                if (combobox.Existed)
                {
                    flag = combobox.SelectItem("Active");
                    if (!flag)
                    {
                        error += "Cannot select Active in combobox State";
                    }
                }
                else
                {
                    error += "Cannot get combobox State";
                }

                #endregion

                #region Save incident

                flag = inc.Save();
                if (!flag)
                { 
                    error += "Error when save incident."; 
                }

                #endregion

                if (error != "") { flag = false; }

            }
            catch (Exception wex)
            {
                flag = false;
                error = wex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_15_02_IncidentForm_Verify_RelatedList()
        {
            try
            {
                string temp = Base.GData("Related_List");
                flag = inc.VerifyRelatedTable(temp);
                if(!flag)
                {
                    error = "There is something wrong when checking related list";
                    flagExit = false;
                }
            }
            catch (Exception wex)
            {
                flag = false;
                error = wex.Message;
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
