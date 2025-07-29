/*
 * Danh Created Date March 23 2017
 */

using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;


namespace Incident
{

    class INC_relate_parent_11
    {

        #region Define default variables for test case (No need to update)
        public bool flagC;
        public bool flag, flagExit, flagW;
        string caseName, error, temp;
        Auto.obase Base;
        
        #endregion

        #region Setup test case, set up and tear down test steps (No need to update)

        //-------------------------------------------------------------------------------------------------

        [TestFixtureSetUp()]
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

        #endregion

        #region Tear down test case (Need to update)

        //-------------------------------------------------------------------------------------------------

        [TestFixtureTearDown()]
        public void TearDown()
        {
            Base.AfterRunTestCase(flagC, caseName);

            System.Console.WriteLine("Finished - Parent Incident 1 Id: " + incParentId01);
            System.Console.WriteLine("Finished - Child Incident 2 Id: " + incChildId02);
            System.Console.WriteLine("Finished - Child Incident 3 Id: " + incChildId03);
            System.Console.WriteLine("Finished - Child Incident 4 Id: " + incChildId04);
            System.Console.WriteLine("Finished - Parent Incident 1 Group: " + incParent01_Group);
            System.Console.WriteLine("Finished - Child Incident 2 Group: " + incChild02_Group);
            System.Console.WriteLine("Finished - Child Incident 3 Group: " + incChild03_Group);
            System.Console.WriteLine("Finished - Child Incident 4 Group: " + incChild04_Group);
            //-----------------------
            temp = Base.GData("Debug").ToLower();

            if (Base.Driver != null && temp != "yes")
            {
                Base.Driver.Close();
                Base.Driver.Quit();
            }
            //-----------------------
            login = null;
            home = null;
            inc = null;
            incList = null;
            emailList = null;
            incPrint = null;
            systemSetting = null;
            globalSearch = null;
        }

        #endregion

        #region Define variables and objects (class) are used in test cases (Need to update)


        string incParentId01, incChildId02, incChildId03, incChildId04;
        string incParent01_Group, incChild02_Group, incChild03_Group, incChild04_Group;

        Auto.otextbox textbox;
        Auto.olookup lookup;
        Auto.ocombobox combobox;
        Auto.otab tab;
        Auto.obutton button;
        Auto.otextarea textarea;
        
        Auto.oelement ele;
        //------------------------------------------------------------------
        Auto.Login login = null;
        Auto.Home home = null;
        Auto.Incident inc = null;
        Auto.IncidentPrint incPrint = null;
        Auto.IncidentList incList = null;
        Auto.EmailList emailList = null;
        Auto.SystemSetting systemSetting = null;
        Auto.GlobalSearch globalSearch = null;

        #endregion

        #region Test Steps (Need to update)

            #region Login to system

            /* Initialize all variables */
            [Test]
            public void ClassInit()
            {
                try
                {
                    //--------------------------------
                    login = new Auto.Login(Base);
                    home = new Auto.Home(Base);
                    inc = new Auto.Incident(Base, "Incident");
                    incList = new Auto.IncidentList(Base, "Incident list");
                    emailList = new Auto.EmailList(Base, "Email list");
                    incPrint = new Auto.IncidentPrint(Base, "Incident print page");
                    systemSetting = new Auto.SystemSetting(Base);
                    globalSearch = new Auto.GlobalSearch(Base);
                    //-------------------------------
                    incParentId01 = string.Empty;
                    incChildId02 = string.Empty;
                    incChildId03 = string.Empty;
                    incChildId04 = string.Empty;
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Open the browser, input the test URL */
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

            /* Try to log into the system, get the user and password from excel sheet */
            [Test]
            public void Step_002_003_Login()
            {
                try
                {
                    /* Log into the system */
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

            #endregion

            #region Impersonate as Service Desk

            //-------------------------------------------------------------------------------------------------

            /* Impersonate the Service Desk */
            [Test]
            public void Step_004_ImpersonateUser_ServiceDesk()
            {
                try
                {
                    /* Impersonate the Service Desk */

                    string temp = Base.GData("ServiceDesk");
                    flag = home.ImpersonateUser(temp);
                    if (!flag) { error = "Cannot impersonate user (" + temp + ")"; }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            ///* Change domain */
            //[Test]
            //public void Step_005_SystemSetting()
            //{
            //    try
            //    {
            //        /* Change system setting */
            //        flag = home.SystemSetting();
            //        if (!flag) { error = "Error when config system."; }
            //    }
            //    catch (Exception ex)
            //    {
            //        flag = false;
            //        error = ex.Message;
            //    }
            //}

            #endregion

            #region Create a new Incident as Parent Incident 01

            //-------------------------------------------------------------------------------------------------

            /* Click on the left menu: Incident > Create New to open Incident (New) page */
            [Test]
            public void Step_006_Open_NewIncident_1()
            {
                try
                {
                    flag = home.LeftMenuItemSelect("Incident", "Create New");
                    if (flag)
                        inc.WaitLoading();
                    else
                        error = "Error when create new incident.";
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Submit Incident (validating mandatory fields popup) */
            [Test]
            public void Step_007_00_Submit_BlankIncident()
            {
                try
                {
                    /* Click on the Submit button */ 
                    button = inc.Button_Submit;
                    flag = button.Existed;
                    if (flag)
                    {
                        button.Click(true);  
                    }
                    else
                    {
                        error = "Cannot get Submit button";
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
        public void Step_007_01_Verify_ErrorMessage()
        {
            try
            {
                string temp = Base.GData("BlankInc_Alert");
                flag = inc.VerifyErrorMessage(temp);
                if (!flag)
                {
                    error = "Cannot verify error message or Invalid message. Expected: [" + temp + "]";
                    flagExit = false;
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        /* Populate Caller Name and verify the autopopulated fields Company, Email and Location */
        [Test]
            public void Step_008_Populate_CallerName()
            {
                try
                {
                    #region Get the Incident Number field
                    textbox = inc.Textbox_Number;

                    /* Check if the Incident Number field exist */
                    flag = textbox.Existed;
                    if (flag)
                    {

                        /* Click on the text field to activate it */
                        textbox.Click(true);

                        /* Save Incident Id for reference */
                        incParentId01 = textbox.Text;

                        /* Check if Incident Id is valid */
                        flag = (incParentId01 != string.Empty);
                        if(!flag)
                        {
                            error = "Invalid Incident Id";
                        }
                    }
                    else
                    {
                        error = "Incident Number field does not exist";
                    }
                    #endregion

                    #region Input Caller field
                    if (flag)
                    {
                        /* Get the Caller field */
                        temp = Base.GData("IncCaller");
                        lookup = inc.Lookup_Caller;
                        flag = lookup.Existed;
                        if (flag)
                        {
                            flag = lookup.Select(temp);
                            /* Check if caller can be input */
                            if (flag)
                            {

                                /* Wait for Company to load */
                                lookup = inc.Lookup_Company;
                                string company = Base.GData("Company");
                                flag = lookup.Existed;
                                if (flag)
                                {
                                    int count = 0;
                                    while (lookup.Text == string.Empty && count < 5)
                                    {
                                        lookup = inc.Lookup_Company;
                                        Thread.Sleep(1000);
                                    }
                                    flag = lookup.VerifyCurrentText(company, true);
                                    if (!flag) { error = "Invalid company value or the value is not auto populate."; flagExit = false; }
                                }

                            }
                            else
                            { error = "Cannot input caller."; }
                        }
                        else
                        {error = "Caller field does not exist";}
                    }
                    #endregion

                    #region Verify Auto-Populated fields
                    if (flag)
                    {
                        error = "";

                        ///* Check if Company is auto-populated correctly */
                        //temp = inc.Lookup_Company.Text;
                        //flag = temp.Equals(Base.GData("Company"));
                        //if (!flag)
                        //{
                        //    error += "Invalid company value or company is not auto-populated.";
                        //}

                        /* Check if Email is auto-populated correctly */
                        temp = inc.Textbox_Email.Text;
                        flag = temp.Equals(Base.GData("IncCallerEmail"));
                        if (!flag)
                        {
                            error += "Invalid email value or email is not auto-populated.";
                        }

                        /* Check if Location is auto-populated correctly */
                        temp = inc.Lookup_Location.Text;
                        flag = temp.Equals(Base.GData("IncLocation"));
                        if (!flag)
                        {
                            error += "Invalid Location value or Location is not auto-populated.";
                        }
                        if (error != "") { flag = false; flagExit = false; }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
    }

            //-------------------------------------------------------------------------------------------------

            /* Populate Business Service*/
            [Test]
            public void Step_009_Populate_BusinessService()
            {
                try
                {
                    temp = Base.GData("BusinessService");
                    lookup = inc.Lookup_BusinessService;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.Select(temp);
                        if (!flag)
                        {error = "Cannot input business service or invalid business service";}
                    }
                    else
                    {
                        error = "Cannot get business service field.";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Populate Category and Subcategory */
            [Test]
            public void Step_010_Populate_CategoryAndSubcategory()
            {
                try
                {
                    temp = Base.GData("IncCat");
                    combobox = inc.Combobox_Category;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        flag = combobox.SelectItem(temp);
                        if (flag)
                        {
                            string sub_cat = Base.GData("IncSubCat");
                            
                            flag = inc.VerifyHaveItemInComboboxList("subcategory", sub_cat);

                            int count = 0;

                            while (count < 5 && !flag)
                            {
                                combobox.SelectItem("-- None --");
                                Thread.Sleep(2000);
                                combobox.SelectItem(temp);
                                Thread.Sleep(2000);
                                flag = inc.VerifyHaveItemInComboboxList("subcategory", sub_cat);
                                count++;
                            }

                            if (flag)
                            {
                                combobox = inc.Combobox_SubCategory;
                                flag = combobox.Existed;
                                if (flag)
                                {
                                    flag = combobox.SelectItem(sub_cat);
                                    if (!flag) error = "Cannot update Incident Subcategory.";
                                }
                            }
                            else error = "Not found item [" + sub_cat + "] in subcategory list.";
                        }
                        else
                        {
                            error = "Cannot update Incident Category.";
                        }

                    }
                    else
                    { error = "Cannot found Category combobox."; }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Populate Contact Type - different for each Incident */
            [Test]
            public void Step_011_Populate_ContactType()
            {
                try
                {
                    temp = Base.GData("Inc1_ContactType");
                    combobox = inc.Combobox_ContactType; ;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        flag = combobox.SelectItem(temp);
                        if (!flag)
                        {error = "Invalid contact type selected.";}
                    }
                    else{error = "Cannot get contact type field.";}
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Populate Short description - different for each Incident */
            [Test]
            public void Step_012_Populate_ShortDescription()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incParentId01 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Parent 01 Id.");
                        addPara.ShowDialog();
                        incParentId01 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------

                    temp = Base.GData("ShortDescription") + " - (Parent) - " + incParentId01;
                    textbox = inc.Textbox_ShortDescription;
                    flag = textbox.Existed;
                    if (flag)
                    {
                        flag = textbox.SetText(temp);
                        if (!flag)
                        {error = "Cannot input short description.";}
                    }
                    else
                    {error = "Cannot get short description field.";}
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Save Incident and remains on the Incident form */
            [Test]
            public void Step_013_Save_Incident_1()
            {
                try
                {
                    flag = inc.Save();
                    Thread.Sleep(2000);
                    if (!flag)
                    {
                        error = "Cannot save incident.";
                    }
                    else inc.WaitLoading();
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Verify Assignment Group - if not auto-populated then manually assign it */
            [Test]
            public void Step_014_015_Validate_AssignmentGroup_Impact_Urgency_Priority()
            {
                try
                {
                    error = "";

                    #region Validate Assignment Group
                    lookup = inc.Lookup_AssignmentGroup;
                    incParent01_Group = lookup.Text;
                    if (incParent01_Group != string.Empty)
                    {
                        System.Console.WriteLine("The Assignment Group is auto-populated correctly");
                    }
                    else
                    {
                        System.Console.WriteLine("The Assignment Group is not auto-populated. Users have to populate manually");
                        incParent01_Group = Base.GData("Inc1_Group");
                        flag = lookup.Select(incParent01_Group);
                        if (!flag)
                        {
                            error += "Cannot populate assignment group.";
                        }
                        inc.WaitLoading();
                    }
                    #endregion
                
                    #region Validate Impact
                    combobox = inc.Combobox_Impact;
                    temp = Base.GData("Impact");
                    flag = combobox.Existed;
                    if (flag)
                    {
                        if (temp.ToLower().Trim().Equals(combobox.CurrentValue.ToLower().Trim()))
                        {
                            System.Console.WriteLine("The Impact is auto-populated correctly");
                        }
                        else
                        {
                            System.Console.WriteLine("The Impact is different. Users have to populate manually so all Incidents have the same Impact");
                            flag = combobox.SelectItem(temp);
                            if (!flag)
                            {
                                error += "Cannot populate Impact.";
                            }
                        }
                    }
                    else { error = "Not found combobox Impact"; }
                    #endregion
                    
                    #region Validate Urgency
                    temp = Base.GData("Urgency");
                    combobox = inc.Combobox_Urgency;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        if (temp.ToLower().Trim().Equals(combobox.CurrentValue.ToLower().Trim()))
                        {
                            System.Console.WriteLine("The Urgency is auto-populated correctly");
                        }
                        else
                        {
                            System.Console.WriteLine("The Urgency is different. Users have to populate manually so all Incidents have the same Urgency");
                            flag = combobox.SelectItem(temp);
                            if (!flag)
                            {
                                error += "Cannot populate Urgency.";
                            }
                        }
                    }
                    else { error = "Not found combobox Urgency"; }
                    #endregion

                    #region Validate Priority
                    temp = Base.GData("Priority");
                    combobox = inc.Combobox_Priority;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        if (temp.ToLower().Trim().Equals(combobox.CurrentValue.ToLower().Trim()))
                        {
                            System.Console.WriteLine("The Priority is auto-populated correctly");
                        }
                        else
                        {
                            System.Console.WriteLine("The Priority is different. Users have to populate manually so all Incidents have the same Priority");
                            flag = false;
                            //flag = combobox.SelectItem(temp);
                            //if (!flag)
                            //{
                            //    error += "Cannot populate Priority.";
                            //}
                        }
                    }
                    else { error = "Not found combobox Priority"; }
                    #endregion

                    #region Save Incident
                    flag = (error == "");
                    if (flag)
                    {
                        flag = inc.Save();
                        if (!flag)
                        {
                            error = "Cannot save incident.";
                        }
                        else inc.WaitLoading();
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            #endregion

            #region Create a new Incident as Child Incident 02

            //-------------------------------------------------------------------------------------------------

            /* Click on the left menu: Incident > Create New to open Incident (New) page */
            [Test]
            public void Step_016_01_Open_NewIncident_2()
            {
                try
                {
                    flag = home.LeftMenuItemSelect("Incident", "Create New");
                    if (flag)
                        inc.WaitLoading();
                    else
                        error = "Error when create new incident.";
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Submit Incident (validating mandatory fields popup) */
            [Test]
            public void Step_016_02_Submit_BlankIncident()
            {
                try
                {
                    /* Click on the Submit button */
                    button = inc.Button_Submit;
                    flag = button.Existed;
                    if (flag)
                    {
                        button.Click(true);
                        Thread.Sleep(1000);
                        temp = Base.GData("BlankInc_Alert");
                        flag = inc.VerifyErrorMessage(temp);
                        if (!flag)
                            error = "Invalid error message.";
                    }
                    else
                    {
                        error = "Cannot get Submit button";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Populate Caller Name and verify the autopopulated fields Company, Email and Location */
            [Test]
            public void Step_016_03_Populate_CallerName()
            {
                try
                {
                    #region Get the Incident Number field
                    textbox = inc.Textbox_Number;

                    /* Check if the Incident Number field exist */
                    flag = textbox.Existed;
                    if (flag)
                    {

                        /* Click on the text field to activate it */
                        textbox.Click(true);

                        /* Save Incident Id for reference */
                        incChildId02 = textbox.Text;

                        /* Check if Incident Id is valid */
                        flag = (incChildId02 != string.Empty);
                        if (!flag)
                        {
                            error = "Invalid Incident Id";
                        }
                    }
                    else
                    {
                        error = "Incident Number field does not exist";
                    }
                    #endregion

                    #region Input Caller field
                    if (flag)
                    {
                        /* Get the Caller field */
                        temp = Base.GData("IncCaller");
                        lookup = inc.Lookup_Caller;
                        flag = lookup.Existed;
                        if (flag)
                        {
                            flag = lookup.Select(temp);
                            /* Check if caller can be input */
                            if (flag)
                            {
                                /* Wait for Company to load */
                                lookup = inc.Lookup_Company;
                                string company = Base.GData("Company");
                                flag = lookup.Existed;
                                if (flag)
                                {
                                    int count = 0;
                                    while (lookup.Text == string.Empty && count < 5)
                                    {
                                        lookup = inc.Lookup_Company;
                                        Thread.Sleep(1000);
                                    }
                                    flag = lookup.VerifyCurrentText(company, true);
                                    if (!flag) { error = "Invalid company value or the value is not auto populate."; flagExit = false; }
                                }
                            }
                            else
                            { error = "Cannot input caller."; }
                        }
                        else
                        { error = "Caller field does not exist"; }
                    }
                    #endregion

                    #region Verify Auto-Populated fields
                    if (flag)
                    {
                        error = "";

                        ///* Check if Company is auto-populated correctly */
                        //temp = inc.Lookup_Company.Text;
                        //flag = temp.Equals(Base.GData("Company"));
                        //if (!flag)
                        //{
                        //    error += "Invalid company value or company is not auto-populated.";
                        //}

                        /* Check if Email is auto-populated correctly */
                        temp = inc.Textbox_Email.Text;
                        flag = temp.Equals(Base.GData("IncCallerEmail"));
                        if (!flag)
                        {
                            error += "Invalid email value or email is not auto-populated.";
                        }

                        /* Check if Location is auto-populated correctly */
                        temp = inc.Lookup_Location.Text;
                        flag = temp.Equals(Base.GData("IncLocation"));
                        if (!flag)
                        {
                            error += "Invalid Location value or Location is not auto-populated.";
                        }
                        if (error != "") { flag = false; flagExit = false; }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Populate Business Service*/
            [Test]
            public void Step_016_04_Populate_BusinessService()
            {
                try
                {
                    temp = Base.GData("BusinessService");
                    lookup = inc.Lookup_BusinessService;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.Select(temp);
                        if (!flag)
                        { error = "Cannot input business service or invalid business service"; }
                    }
                    else
                    {
                        error = "Cannot get business service field.";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Populate Category and Subcategory */
            [Test]
            public void Step_016_05_Populate_CategoryAndSubcategory()
            {
                try
                {
                    temp = Base.GData("IncCat");
                    combobox = inc.Combobox_Category;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        flag = combobox.SelectItem(temp);
                        if (flag)
                        {
                            inc.WaitLoading();
                            string sub_cat = Base.GData("IncSubCat");
                            combobox = inc.Combobox_SubCategory;
                            flag = combobox.Existed;
                            if (flag)
                            {
                                flag = inc.VerifyHaveItemInComboboxList("subcategory", sub_cat);

                                int count = 0;

                                while (count < 5 && !flag)
                                {
                                    combobox.SelectItem("-- None --");
                                    Thread.Sleep(2000);
                                    combobox.SelectItem(temp);
                                    Thread.Sleep(2000);
                                    flag = inc.VerifyHaveItemInComboboxList("subcategory", sub_cat);
                                    count++;
                                }

                                if (flag)
                                {
                                    combobox = inc.Combobox_SubCategory;
                                    flag = combobox.Existed;
                                    if (flag)
                                    {
                                        flag = combobox.SelectItem(sub_cat);
                                        if (!flag) error = "Cannot update Incident Subcategory.";
                                    }
                                }
                                else error = "Not found item [" + sub_cat + "] in subcategory list.";
                            }
                            else
                            { error = "Cannot found Subcategory combobox."; }
                        }
                        else
                        {
                            error = "Cannot update Incident Category.";
                        }

                    }
                    else
                    { error = "Cannot found Category combobox."; }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Populate Contact Type - different for each Incident */
            [Test]
            public void Step_016_06_Populate_ContactType()
            {
                try
                {
                    temp = Base.GData("Inc2_ContactType");
                    combobox = inc.Combobox_ContactType; ;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        flag = combobox.SelectItem(temp);
                        if (!flag)
                        { error = "Invalid contact type selected."; }
                    }
                    else { error = "Cannot get contact type field."; }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Populate Short description - different for each Incident */
            [Test]
            public void Step_016_07_Populate_ShortDescription()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incChildId02 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Child 02 Id.");
                        addPara.ShowDialog();
                        incChildId02 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }
                    //----------------------------------------------------------------------------------------------

                    temp = Base.GData("ShortDescription") + " - (Child) - " + incChildId02;
                    textbox = inc.Textbox_ShortDescription;
                    flag = textbox.Existed;
                    if (flag)
                    {
                        flag = textbox.SetText(temp);
                        if (!flag)
                        { error = "Cannot input short description."; }
                    }
                    else
                    { error = "Cannot get short description field."; }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Save Incident and remains on the Incident form */
            [Test]
            public void Step_016_08_Save_Incident_2()
            {
                try
                {
                    flag = inc.Save();
                    Thread.Sleep(2000);
                    if (!flag)
                    {
                        error = "Cannot save incident.";
                    }
                    else inc.WaitLoading();
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Verify Assignment Group - if not auto-populated then manually assign it */
            [Test]
            public void Step_016_09_Validate_AssignmentGroup_Impact_Urgency_Priority()
            {
                try
                {
                    error = "";
                    #region Validate Assignment Group
                    lookup = inc.Lookup_AssignmentGroup;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        incChild02_Group = lookup.Text;
                        if (!incChild02_Group.Equals(string.Empty) && !incChild02_Group.Equals(incParent01_Group))
                        {
                            System.Console.WriteLine("The Assignment Group is auto-populated correctly");
                        }
                        else
                        {
                            if (incChild02_Group.Equals(incParent01_Group))
                            {
                                System.Console.WriteLine("The Assignment Group of Child Incident 02 is the same as of Parent Incident 01. Users have to change it manually");
                            }
                            else
                            {
                                System.Console.WriteLine("The Assignment Group is not auto-populated. Users have to populate manually");
                                incChild02_Group = Base.GData("Inc2_Group");
                                flag = lookup.Select(incChild02_Group);
                                if (!flag)
                                {
                                    error += "Cannot populate assignment group.";
                                }
                                inc.WaitLoading();
                            }
                           
                        }
                    }
                    else { error = "Not found lookup field Assignment group"; }
                    #endregion

                    #region Validate Impact
                    combobox = inc.Combobox_Impact;
                    temp = Base.GData("Impact");
                    flag = combobox.Existed;
                    if (flag)
                    {
                        if (temp.ToLower().Trim().Equals(combobox.CurrentValue.ToLower().Trim()))
                        {
                            System.Console.WriteLine("The Impact is auto-populated correctly");
                        }
                        else
                        {
                            System.Console.WriteLine("The Impact is different. Users have to populate manually so all Incidents have the same Impact");
                            flag = combobox.SelectItem(temp);
                            if (!flag)
                            {
                                error += "Cannot populate Impact.";
                            }
                        }
                    }
                    else { error = "Not found combobox Impact"; }
                    #endregion

                    #region Validate Urgency
                    temp = Base.GData("Urgency");
                    combobox = inc.Combobox_Urgency;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        if (temp.ToLower().Trim().Equals(combobox.CurrentValue.ToLower().Trim()))
                        {
                            System.Console.WriteLine("The Urgency is auto-populated correctly");
                        }
                        else
                        {
                            System.Console.WriteLine("The Urgency is different. Users have to populate manually so all Incidents have the same Urgency");
                            flag = combobox.SelectItem(temp);
                            if (!flag)
                            {
                                error += "Cannot populate Urgency.";
                            }
                        }
                    }
                    else { error = "Not found combobox Urgency"; }
                    #endregion

                    #region Validate Priority
                    temp = Base.GData("Priority");
                    combobox = inc.Combobox_Priority;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        if (temp.ToLower().Trim().Equals(combobox.CurrentValue.ToLower().Trim()))
                        {
                            System.Console.WriteLine("The Priority is auto-populated correctly");
                        }
                        else
                        {
                            System.Console.WriteLine("The Priority is different. Users have to populate manually so all Incidents have the same Priority");
                            flag = combobox.SelectItem(temp);
                            if (!flag)
                            {
                                error += "Cannot populate Priority.";
                            }
                        }
                    }
                    else { error = "Not found combobox Priority"; }
                    #endregion

                    #region Save Incident
                    flag = (error == "");
                    if (flag)
                    {
                        flag = inc.Save();
                        if (!flag)
                        {
                            error = "Cannot save incident.";
                        }
                        else inc.WaitLoading();
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            #endregion

            #region Impersonate as Resolver

            //-------------------------------------------------------------------------------------------------

            /* Impersonate the Resolver */
            [Test]
            public void Step_017_ImpersonateUser_Resolver()
            {
                try
                {
                    /* Impersonate the Resolver */
                    flag = home.ImpersonateUser(Base.GData("Resolver"),true,Base.GData("UserFullName"),false);
                    if (!flag)
                    {
                        error = "Cannot impersonate";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            ///* Change domain */
            //[Test]
            //public void Step_018_SystemSetting()
            //{
            //    try
            //    {
            //        /* Change system setting */
            //        flag = home.SystemSetting();
            //        if (!flag) { error = "Error when config system."; }
            //    }
            //    catch (Exception ex)
            //    {
            //        flag = false;
            //        error = ex.Message;
            //    }
            //}

            #endregion

            #region Verify Child Incident and Print it

            //-------------------------------------------------------------------------------------------------

            /* Under Incident, click on Open and search Incident Child */
            [Test]
            public void Step_019_020_SearchAndOpen_Incident_2()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incChildId02 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Child 02 Id.");
                        addPara.ShowDialog();
                        incChildId02 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }
                    //----------------------------------------------------------------------------------------------

                   flag = home.LeftMenuItemSelect("Incident", "Open");
                   if (flag)
                   {
                       incList.WaitLoading();
                       temp = incList.Label_Title.Text;
                       flag = temp.Equals("Incidents");
                       if (flag)
                       {
                           flag = incList.SearchAndOpen("Number", incChildId02, "Number=" + incChildId02, "Number");
                           if (!flag) error = "Error when search and open incident (id:" + incChildId02 + ")";
                           else inc.WaitLoading();
                       }
                       else
                       {
                           error = "Invalid title of page. Runtime:(" + temp + "). Expected:(Incidents)";
                       }
                   }
                   else error = "Error when select open incident.";
               
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Verify if State = New */
            [Test]
            public void Step_021_Validate_State()
            {
                try
                {
                    combobox = inc.Combobox_State;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        flag = combobox.CurrentValue.Equals("New");
                        if (!flag)
                        {error = "Invalid state selected.";}
                    }
                    else
                    {error = "Cannot get state control.";}
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Click "Printer friendly version" link */
            [Test]
            public void Step_022_Open_PrintWindow()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incChildId02 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Child 02 Id.");
                        addPara.ShowDialog();
                        incChildId02 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }
                    //----------------------------------------------------------------------------------------------

                    #region Open Print Window
                    flag = systemSetting.OpenPrintWindow();
                    if (flag)
                    {
                        if (!incPrint.Validate_Control("Number", incChildId02)) { flag = false; }
                        if (!incPrint.Validate_Control("Company", Base.GData("Company"))) { flag = false; }
                        if (!incPrint.Validate_Control("Caller", Base.GData("IncCaller"))) { flag = false; }
                        if (!incPrint.Validate_Control("Email", Base.GData("IncCallerEmail"))) { flag = false; }
                        if (!incPrint.Validate_Control("Location", Base.GData("IncLocation"))) { flag = false; }
                        if (!incPrint.Validate_Control("u_business_services", Base.GData("BusinessService"))) { flag = false; }
                        if (!incPrint.Validate_Control("Category", Base.GData("IncCat"))) { flag = false; }
                        if (!incPrint.Validate_Control("Subcategory", Base.GData("IncSubCat"))) { flag = false; }
                        if (!incPrint.Validate_Control("Short_description", Base.GData("ShortDescription") + " - (Child) - " + incChildId02)) { flag = false; }
                        if (!incPrint.Validate_Control("Contact_type", Base.GData("Inc2_ContactType"))) { flag = false; }
                        if (!incPrint.Validate_Control("State", "New")) { flag = false; }
                        if (!incPrint.Validate_Control("Assignment_group", Base.GData("Inc2_Group"))) { flag = false; }
                        if (!incPrint.Validate_Control("Impact", Base.GData("Impact"))) { flag = false; }
                        if (!incPrint.Validate_Control("Urgency", Base.GData("Urgency"))) { flag = false; }
                        if (!incPrint.Validate_Control("Priority", Base.GData("Priority"))) { flag = false; }
                        if (!flag) { flagExit = false; }
                    }
                    else
                    {
                        error = "Cannot open the Print window";
                    }
                    #endregion

                    #region Verify Incident information

                #endregion
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Click on one of the "Click to Print" Buttons */
            [Test]
            public void Step_023_Click_PrintButton()
            {
                try
                {
                    // Cannot click
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Click Cancel  */
            [Test]
            public void Step_024_Click_Cancel()
            {
                try
                {
                    // Cannot click
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Close Print window */
            [Test]
            public void Step_025_Close_PrintWindow()
            {
                try
                {
                    Base.Driver.Close();
                    flag = Base.SwitchToPage(0);
                    if (flag)
                    {
                        button = systemSetting.Button_Close();
                        flag = button.Existed;
                        if (flag)
                        {
                            button.Click(true);
                            home.WaitLoading();
                        }
                        else
                        {
                            error = "Cannot get the Close button";
                        }
                    }
                    else
                    {
                        error = "Cannot switch back to main page";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            #endregion

            #region Relate Parent Incident 01 onto Child Incident 02, Verify Assigment Group of the Child is changed

            //-------------------------------------------------------------------------------------------------
             
            /* Open Related Records section */
            [Test]
            public void Step_026_Open_RelatedRecordsSection()
            {
                try
                {
                    tab = inc.GTab("Related Records");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Related Records", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (!flag)
                    {error = "Cannot open Related Records tab";}
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Set Parent to Incident 01 */
            [Test]
            public void Step_027_Populate_Parent_Inc_1()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incParentId01 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Parent 01 Id.");
                        addPara.ShowDialog();
                        incParentId01 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }
                    //----------------------------------------------------------------------------------------------
                    lookup = inc.Lookup_ParentInc;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.Select(incParentId01);
                        if (flag)
                        {
                            /* Verify the displayed Confirmation Dialog */
                            inc.WaitLoading();
                            temp = Base.GData("ParentInc_Alert");
                            flag = inc.VerifyConfirmationDialog_Incident(temp, "yes");
                            if (flag)
                            {inc.WaitLoading();}
                            else
                            {error = "The Confirmation message is incorrect: <" + ele.Text + ">. It should be <" + temp + ">";}
                        }
                        else
                        {
                            error = "Cannot populate the Parent field";
                        }
                    }
                    else
                    {
                        error = "Parent field is null";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Verify Assignment Group of Incident 2 is the same as Incident 1 */
            [Test]
            public void Step_028_Verify_AssignmentGroup()
            {
                try
                {
                    temp = Base.GData("Inc1_Group");
                    lookup = inc.Lookup_AssignmentGroup;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.Text.ToLower().Trim().Equals(temp.ToLower().Trim());
                        if (!flag)
                        {
                            flagExit = false;
                            error = "The Assignment Group of Child Incident does not change to Parent's";
                        }
                    }
                    else{error = "The Assignment Group is null";}
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Save Incident and back to the Incident List form */
            [Test]
            public void Step_029_Save_Incident()
            {
                try
                {
                    flag = inc.Save();
                    if (!flag)
                    {
                        error = "Cannot save Incident";
                    }
                    else inc.WaitLoading();
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Under Incident, click on Open and search Parent Incident */
            [Test]
            public void Step_030_031_SearchAndOpen_Incident()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incParentId01 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Parent 01 Id.");
                        addPara.ShowDialog();
                        incParentId01 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------

                    /* Under Incident, click on Open */
                    flag = home.LeftMenuItemSelect("Incident", "Open");
                    if (flag)
                    {
                        incList.WaitLoading();
                        flag = (incList.Label_Title.Text == "Incidents");
                        if (flag)
                        {

                            /* Select the incident on the list */
                            flag = incList.SearchAndOpen("Number", incParentId01, "Number=" + incParentId01, "Number");
                            if (flag)
                            {
                                inc.WaitLoading();
                                flag = (inc.VerifyHeader(incParentId01));
                                if (!flag)
                                {
                                    error = "Cannot open Parent Incident";
                                }
                            }
                            /* Verify if open Parent Incident correctly */
                            else
                            {
                                error = "Cannot open Incident in the list";
                            }
                        }
                        else
                        {
                            error = "Cannot open incident list.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Validate Parent Children Relationship */
            [Test]
            public void Step_032_Validate_Relationship()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incChildId02 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Child 02 Id.");
                        addPara.ShowDialog();
                        incChildId02 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------

                    /* Open Child Incidents tab */
                    tab = inc.GTab("Child Incidents");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Child Incidents", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Verify the Inc Child in the list */
                        flag = inc.RelatedTableVerifyRow("Child Incidents", "Number=" + incChildId02);
                        if (!flag)
                        {
                            error = "The Incident Child " + incChildId02 + " is not in the Child Incidents table";
                        }
                    }
                    else
                    {
                        error = "There is no Child Incidents tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            #endregion

            #region Add a work note and verify it on Child Incident

            //-------------------------------------------------------------------------------------------------

            /* Add some Work Notes and save Incident */
            [Test]
            public void Step_033_Add_WorkNote()
            {
                try
                {
                    /* Select Notes tab */
                    tab = inc.GTab("Notes");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Notes", true);
                        i++;
                    }
                    flag = tab.Header.Click();
                    if (flag)
                    {
                        /* Click Show Journal Fields button to see Work Notes and Additional Comments */
                        button = inc.Button_ShowFields;
                        if (button.Existed && button.MyEle.GetAttribute("ng-click") == "toggleMultipleInputs(true)")
                        {
                            button.Click();
                            inc.WaitLoading();
                        }

                        /* Get the Work Notes Area */
                        textarea = inc.Textarea_Worknotes_Update;
                        flag = textarea.Existed;
                        if (flag)
                        {
                            temp = Base.GData("WorkNote1");

                            /* Input Work Notes */
                            flag = textarea.SetText(temp);
                            if (flag)
                            {
                                /* Save Incident */
                                flag = inc.Save();
                                if (!flag)
                                { error = "Cannot save incident."; }
                                else inc.WaitLoading();
                            }
                            else
                            {error = "Cannot input work notes.";}
                        }
                        else
                        {error = "Work Notes Area does not exist";}
                    }
                    else
                    {error = "Cannot open Notes tab";}
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Scroll down to Child Incident section, then click on the related Child incident */
            [Test]
            public void Step_034_Open_ChildIncident_2()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incChildId02 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Child 02 Id.");
                        addPara.ShowDialog();
                        incChildId02 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------

                    /* Open Child Incidents tab */
                    tab = inc.GTab("Child Incidents");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Child Incidents", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Click on Child Incident in the list */
                        flag = inc.RelatedTableOpenRecord("Child Incidents","Number="+ incChildId02, "Number");
                        if (flag)
                        {
                            inc.WaitLoading();

                            /* Verify if the Child Incident is opened */
                            flag = inc.VerifyHeader(incChildId02);
                            if (!flag)
                            {error = "Cannot open Child Incident";}
                        }
                        else
                        {
                            error = "The Incident Child " + incChildId02 + " is not in the Child Incidents table";
                        }
                    }
                    else
                    {
                        error = "There is no Child Incidents tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Verify the Work Note added in Parent Incident is brought to Child Incident */
            [Test]
            public void Step_035_Verify_WorkNote()
            {
                try
                {
                    /* Select Notes tab */
                    tab = inc.GTab("Notes");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Notes", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Verify Parent Work Note in Activity log */
                        temp = Base.GData("Resolver") + "|Work note copied from Parent Incident: " + Base.GData("WorkNote1");
                        flag = inc.VerifyActivity(temp);
                        if (!flag)
                        {
                            error = "Invalid activity.";
                            flagExit = false;
                        }
                    }
                    else
                    {error = "Cannot select Notes tab";}
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            #endregion

            #region Add an Additional comment and verify it on Child Incident

            /* Openthe Related Records tab and open the Parent Incident */
            [Test]
            public void Step_036_Open_ParentIncident()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incParentId01 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Parent 01 Id.");
                        addPara.ShowDialog();
                        incParentId01 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------

                    /* Open Related Record tab */
                    tab = inc.GTab("Related Records");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Related Records", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Click on View Parent Incident link */
                        ele = inc.View_Parent;
                        flag = ele.Existed;
                        if (flag)
                        {
                            ele.Click(true);
                            inc.WaitLoading();

                            int count = 5;
                            while (!inc.VerifyHeader(incParentId01) && count > 0)
                            {
                                Thread.Sleep(1000);
                                count--;
                            }

                            /* Verify if open Parent Incident correctly */
                            flag = (inc.VerifyHeader(incParentId01));
                            if (!flag)
                            {
                                error = "Cannot open Parent Incident";
                            }
                        }
                        else
                        {
                            error = "The View Parent Incident link is null";
                        }
                    }
                    else
                    {
                        error = "Cannot open Related Records tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Add some Addtional Comments and save Incident */
            [Test]
            public void Step_037_Add_AddtionalComment()
            {
                try
                {
                    /* Select Notes tab */
                    tab = inc.GTab("Notes");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Notes", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Click Show Journal Fields button to see Work Notes and Additional Comments */
                        button = inc.Button_ShowFields;
                        if (button.Existed && button.MyEle.GetAttribute("ng-click") == "toggleMultipleInputs(true)")
                        {
                            button.Click(true);
                            inc.WaitLoading();
                        }

                        /* Get the Addtional Comments Area */
                        textarea = inc.Textarea_AdditionComments_Update;
                        flag = textarea.Existed;
                        if (flag)
                        {
                            temp = Base.GData("AddComment1");
                            /* Input Addtional Comments */
                            flag = textarea.SetText(temp);
                            if (flag)
                            {
                                /* Save Incident */
                                flag = inc.Save();
                                if (!flag)
                                { error = "Cannot save incident."; }
                                else inc.WaitLoading();
                            }
                            else
                            {error = "Cannot input Addtional Comments.";}
                        }
                        else
                        {error = "Addtional Comments Area does not exist";}
                    }
                    else
                    {
                        error = "Cannot open Notes tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Scroll down to Child Incident section, then click on the related Child incident */
            [Test]
            public void Step_038_Open_ChildIncident()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incChildId02 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Child 02 Id.");
                        addPara.ShowDialog();
                        incChildId02 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }
                    //----------------------------------------------------------------------------------------------

                    /* Open Child Incidents tab */
                    tab = inc.GTab("Child Incidents");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Child Incidents", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Click on Child Incident in the list */
                        flag = inc.RelatedTableOpenRecord("Child Incidents","Number=" + incChildId02, "Number");
                        if (flag)
                        {
                            inc.WaitLoading();
                            /* Verify if the Child Incident is opened */
                            int count = 5;
                                while (!inc.VerifyHeader(incChildId02) && count > 0)
                                {
                                    Thread.Sleep(1000);
                                    count--;
                                }
                                flag = inc.VerifyHeader(incChildId02);
                                if (!flag)
                                {error = "Cannot open Child Incident";}
                        }
                        else
                        {
                            error = "The Incident Child " + incChildId02 + " is not in the Child Incidents table";
                        }
                    }
                    else
                    {
                        error = "There is no Child Incidents tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Verify the Additional Comments added in Parent Incident is brought to Child Incident */
            [Test]
            public void Step_039_Verify_AdditionalComments()
            {
                try
                {
                    /* Select Notes tab */
                    tab = inc.GTab("Notes");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Notes", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Verify Parent Additional Comments in Activity log */
                        temp = Base.GData("Resolver") + "|Comment copied from Parent Incident: " + Base.GData("AddComment1");
                        flag = inc.VerifyActivity(temp);
                        if (!flag)
                        {
                            error = "Invalid activity.";
                            flagExit = false;
                        }
                    }
                    else
                    {
                        error = "Cannot select Notes tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            #endregion

            #region Add a Work note and an Additional comment and verify it on Child Incident

            /* Openthe Related Records tab and open the Parent Incident */
            [Test]
            public void Step_040_Open_ParentIncident()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incParentId01 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Parent 01 Id.");
                        addPara.ShowDialog();
                        incParentId01 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------

                    /* Open Related Record tab */
                    tab = inc.GTab("Related Records");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Related Records", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Click on View Parent Incident link */
                        ele = inc.View_Parent;
                        flag = ele.Existed;
                        if (flag)
                        {
                            ele.Click(true);
                            inc.WaitLoading();

                            int count = 5;
                            while (!inc.VerifyHeader(incParentId01) && count > 0)
                            {
                                Thread.Sleep(1000);
                                count--;
                            }

                            /* Verify if open Parent Incident correctly */
                            flag = (inc.VerifyHeader(incParentId01));
                            if (!flag)
                            {
                                error = "Cannot open Parent Incident";
                            }

                        }
                        else
                        {
                            error = "The View Parent Incident link is null";
                        }
                    }
                    else
                    {
                        error = "Cannot open Related Records tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Add some Work Notes and Addtional Comments and save Incident */
            [Test]
            public void Step_041_Add_WorkNote_And_AddtionalComment()
            {
                try
                {
                    /* Select Notes tab */
                    tab = inc.GTab("Notes");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Notes", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Click Show Journal Fields button to see Work Notes and Additional Comments */
                        button = inc.Button_ShowFields;
                        if (button.Existed && button.MyEle.GetAttribute("ng-click") == "toggleMultipleInputs(true)")
                        {
                            button.Click(true);
                            inc.WaitLoading();
                        }

                        error = "";

                        /* Get the Addtional Comments Area */
                        textarea = inc.Textarea_AdditionComments_Update;
                        flag = textarea.Existed;
                        if (flag)
                        {
                            temp = Base.GData("AddComment2");
                            /* Input Addtional Comments */
                            flag = textarea.SetText(temp);
                            if (!flag)
                            {
                                error += "Cannot input Addtional Comments.";
                            }
                        }
                        else
                        {
                            error += "Addtional Comments Area does not exist";
                        }

                        /* Get the Work Notes Area */
                        textarea = inc.Textarea_Worknotes_Update;
                        flag = textarea.Existed;
                        if (flag)
                        {
                            temp = Base.GData("WorkNote2");

                            /* Input Work Notes */
                            flag = textarea.SetText(temp);
                            inc.WaitLoading();
                            if (!flag)
                            {error += "Cannot input Work Notes";}
                        }
                        else
                        {
                            error += "Work Notes Area does not exist";
                        }

                        if (error == "")
                        {
                            /* Save Incident */
                            flag = inc.Save();
                            if (!flag)
                            { error = "Cannot save incident."; }
                            else inc.WaitLoading();
                        }
                        else
                        {flag = false;}
                    }
                    else
                    {
                        error = "Cannot open Notes tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Scroll down to Child Incident section, then click on the related Child incident */
            [Test]
            public void Step_042_Open_ChildIncident()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incChildId02 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Child 02 Id.");
                        addPara.ShowDialog();
                        incChildId02 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------

                    /* Open Child Incidents tab */
                    tab = inc.GTab("Child Incidents");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Child Incidents", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Click on Child Incident in the list */
                        flag = inc.RelatedTableOpenRecord("Child Incidents", "Number=" + incChildId02, "Number");
                        if (flag)
                        {
                            inc.WaitLoading();

                            /* Verify if the Child Incident is opened */
                            int count = 5;
                            while (!inc.VerifyHeader(incChildId02) && count > 0)
                            {
                               Thread.Sleep(1000);
                               count--;
                            }
                            flag = inc.VerifyHeader(incChildId02);
                            if (!flag)
                            {
                              error = "Cannot open Child Incident";
                            }
                        }
                        else
                        {
                            error = "The Incident Child " + incChildId02 + " is not in the Child Incidents table";
                        }
                    }
                    else
                    {
                        error = "There is no Child Incidents tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Verify the Work Notes and Additional Comments added in Parent Incident is brought to Child Incident */
            [Test]
            public void Step_043_Verify_WorkNotes_And_AdditionalComments()
            {
                try
                {
                    /* Select Notes tab */
                    tab = inc.GTab("Notes");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Notes", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        error = "";
                        /* Verify Parent Additional Comments in Activity log */
                        temp = Base.GData("Resolver") + "|Comment copied from Parent Incident: " + Base.GData("AddComment2");
                        flag = inc.VerifyActivity(temp);
                        if (!flag)
                        {   error += "Invalid Additional comment activity.";
                            flagExit = false;
                        }

                        /* Verify Parent Work Notes in Activity log */
                        temp = Base.GData("Resolver") + "|Work note copied from Parent Incident: " + Base.GData("WorkNote2");
                        flag = inc.VerifyActivity(temp);
                        if (!flag)
                        {
                            error += "Invalid Work note activity.";
                            flagExit = false;
                        }
                        if (error != "") { flag = false; }
                    }
                    else
                    {
                        error = "Cannot select Notes tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            #endregion

            #region Create a new Incident as Child Incident 03 with blank Assigment Group, Not Save Incident

            //-------------------------------------------------------------------------------------------------

            /* Click on the left menu: Incident > Create New to open Incident (New) page */
            [Test]
            public void Step_044_01_Open_NewIncident()
            {
                try
                {
                    flag = home.LeftMenuItemSelect("Incident", "Create New");
                    if (flag)
                    {inc.WaitLoading();}
                    else
                    {error = "Cannot select create new incident";}
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Submit Incident (validating mandatory fields popup) */
            [Test]
            public void Step_044_02_Submit_BlankIncident()
            {
                try
                {
                    /* Click on the Submit button */
                    button = inc.Button_Submit;
                    flag = button.Existed;
                    if (flag)
                    {
                        button.Click(true);
                        Thread.Sleep(1000);
                        temp = Base.GData("BlankInc_Alert");
                        flag = inc.VerifyErrorMessage(temp);
                        if (!flag)
                            error = "Invalid error message.";
                    }
                    else
                    {
                        error = "Cannot get Submit button";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Populate Caller Name and verify the autopopulated fields Company, Email and Location */
            [Test]
            public void Step_044_03_Populate_CallerName()
            {
                try
                {
                    #region Get the Incident Number field
                    textbox = inc.Textbox_Number;

                    /* Check if the Incident Number field exist */
                    flag = textbox.Existed;
                    if (flag)
                    {

                        /* Click on the text field to activate it */
                        textbox.Click(true);

                        /* Save Incident Id for reference */
                        incChildId03 = textbox.Text;

                        /* Check if Incident Id is valid */
                        flag = (incChildId03 != string.Empty);
                        if (!flag)
                        {
                            error = "Invalid Incident Id";
                        }
                    }
                    else
                    {
                        error = "Incident Number field does not exist";
                    }
                    #endregion

                    #region Input Caller field
                    if (flag)
                    {
                        /* Get the Caller field */
                        temp = Base.GData("IncCaller");
                        lookup = inc.Lookup_Caller;
                        flag = lookup.Existed;
                        if (flag)
                        {
                            flag = lookup.Select(temp);
                            /* Check if caller can be input */
                            if (flag)
                            {

                                /* Wait for Company to load */
                                lookup = inc.Lookup_Company;
                                string company = Base.GData("Company");
                                flag = lookup.Existed;
                                if (flag)
                                {
                                    int count = 0;
                                    while (lookup.Text == string.Empty && count < 5)
                                    {
                                        lookup = inc.Lookup_Company;
                                        Thread.Sleep(1000);
                                    }
                                    flag = lookup.VerifyCurrentText(company, true);
                                    if (!flag) { error = "Invalid company value or the value is not auto populate."; flagExit = false; }
                                }

                            }
                            else
                            { error = "Cannot input caller."; }
                        }
                        else
                        { error = "Caller field does not exist"; }
                    }
                    #endregion

                    #region Verify Auto-Populated fields
                    if (flag)
                    {
                        error = "";
                        /* Check if Email is auto-populated correctly */
                        temp = inc.Textbox_Email.Text;
                        flag = temp.Equals(Base.GData("IncCallerEmail"));
                        if (!flag)
                        {
                            error += "Invalid email value or email is not auto-populated.";
                        }

                        /* Check if Location is auto-populated correctly */
                        temp = inc.Lookup_Location.Text;
                        flag = temp.Equals(Base.GData("IncLocation"));
                        if (!flag)
                        {
                            error += "Invalid Location value or Location is not auto-populated.";
                        }
                        if (error != "") { flag = false; flagExit = false; }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Populate Business Service*/
            [Test]
            public void Step_044_04_Populate_BusinessService()
            {
                try
                {
                    temp = Base.GData("BusinessService");
                    lookup = inc.Lookup_BusinessService;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.Select(temp);
                        if (!flag)
                        { error = "Cannot input business service or invalid business service"; }
                    }
                    else
                    {
                        error = "Cannot get business service field.";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Populate Category and Subcategory */
            [Test]
            public void Step_044_05_Populate_CategoryAndSubcategory()
            {
                try
                {
                    temp = Base.GData("IncCat");
                    combobox = inc.Combobox_Category;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        flag = combobox.SelectItem(temp);
                        if (flag)
                        {
                            inc.WaitLoading();
                            string sub_cat = Base.GData("IncSubCat");
                            combobox = inc.Combobox_SubCategory;
                            flag = combobox.Existed;
                            if (flag)
                            {
                                combobox.MyEle.SendKeys(Keys.Tab);
                                inc.WaitLoading();
                                flag = inc.VerifyHaveItemInComboboxList("subcategory", sub_cat);
                                if (flag)
                                {
                                    flag = combobox.SelectItem(sub_cat);
                                    if (!flag) error = "Cannot update Incident Subcategory.";
                                }
                            }
                            else
                            { error = "Cannot found Subcategory combobox."; }
                        }
                        else
                        {
                            error = "Cannot update Incident Category.";
                        }

                    }
                    else
                    { error = "Cannot found Category combobox."; }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Populate Contact Type - different for each Incident */
            [Test]
            public void Step_044_06_Populate_ContactType()
            {
                try
                {
                    temp = Base.GData("Inc3_ContactType");
                    combobox = inc.Combobox_ContactType; ;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        flag = combobox.SelectItem(temp);
                        if (!flag)
                        { error = "Invalid contact type selected."; }
                    }
                    else { error = "Cannot get contact type field."; }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Populate Short description - different for each Incident */
            [Test]
            public void Step_044_07_Populate_ShortDescription()
            {
                try
                {

                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incChildId03 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Child 03 Id.");
                        addPara.ShowDialog();
                        incChildId03 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------

                    temp = Base.GData("ShortDescription") + " - (Child) - " + incChildId03;
                    textbox = inc.Textbox_ShortDescription;
                    flag = textbox.Existed;
                    if (flag)
                    {
                        flag = textbox.SetText(temp);
                        if (!flag)
                        { error = "Cannot input short description."; }
                    }
                    else
                    { error = "Cannot get short description field."; }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Verify Impact, Urgency, Priority - if not auto-populated then manually assign it */
            [Test]
            public void Step_044_08_Validate_AssignmentGroup_Impact_Urgency_Priority()
            {
                try
                {
                    error = "";
                    #region Validate Assignment Group
                    lookup = inc.Lookup_AssignmentGroup;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        incChild03_Group = lookup.Text;
                        if (!incChild03_Group.Equals(string.Empty))
                        {
                            System.Console.WriteLine("The Assignment Group is auto-populated correctly");
                        }
                       
                    }
                    else { error = "Not found lookup field Assignment group"; }
                    #endregion

                    #region Validate Impact
                    combobox = inc.Combobox_Impact;
                    temp = Base.GData("Impact");
                    flag = combobox.Existed;
                    if (flag)
                    {
                        if (temp.ToLower().Trim().Equals(combobox.CurrentValue.ToLower().Trim()))
                        {
                            System.Console.WriteLine("The Impact is auto-populated correctly");
                        }
                        else
                        {
                            System.Console.WriteLine("The Impact is different. Users have to populate manually so all Incidents have the same Impact");
                            flag = combobox.SelectItem(temp);
                            if (!flag)
                            {
                                error += "Cannot populate Impact.";
                            }
                        }
                    }
                    else { error = "Not found combobox Impact"; }
                    #endregion

                    #region Validate Urgency
                    temp = Base.GData("Urgency");
                    combobox = inc.Combobox_Urgency;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        if (temp.ToLower().Trim().Equals(combobox.CurrentValue.ToLower().Trim()))
                        {
                            System.Console.WriteLine("The Urgency is auto-populated correctly");
                        }
                        else
                        {
                            System.Console.WriteLine("The Urgency is different. Users have to populate manually so all Incidents have the same Urgency");
                            flag = combobox.SelectItem(temp);
                            if (!flag)
                            {
                                error += "Cannot populate Urgency.";
                            }
                        }
                    }
                    else { error = "Not found combobox Urgency"; }
                    #endregion

                    #region Validate Priority
                    temp = Base.GData("Priority");
                    combobox = inc.Combobox_Priority;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        if (temp.ToLower().Trim().Equals(combobox.CurrentValue.ToLower().Trim()))
                        {
                            System.Console.WriteLine("The Priority is auto-populated correctly");
                        }
                        else
                        {
                            System.Console.WriteLine("The Priority is different. Users have to populate manually so all Incidents have the same Priority");
                            flag = combobox.SelectItem(temp);
                            if (!flag)
                            {
                                error += "Cannot populate Priority.";
                            }
                        }
                    }
                    else { error = "Not found combobox Priority"; }
                    #endregion

                    if (error != "") { flag = false; flagExit = false; }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Populate Assigned To so users can create Blank Assignment Incident */
            [Test]
            public void Step_044_09_Populate_AssignedTo()
            {
                try
                {
                    temp = Base.GData("Resolver");
                    lookup = inc.Lookup_AssignedTo;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.Select(temp);
                        if (!flag)
                        {error = "Cannot input Assigned To";}
                    }
                    else
                    {error = "Cannot get Assigned To field";}
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            #endregion

            #region Relate Child Incident 02 onto Child Incident 03, Verify Assigment Group of the Child 03 is not changed

            //-------------------------------------------------------------------------------------------------

            /* Open Related Records section */
            [Test]
            public void Step_045_Open_RelatedRecordsSection()
            {
                try
                {
                    tab = inc.GTab("Related Records");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Related Records", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (!flag)
                    {error = "Cannot open Related Records tab";}
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Set Parent to Incident 01 */
            [Test]
            public void Step_046_Populate_Parent()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incChildId02 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Child 02 Id.");
                        addPara.ShowDialog();
                        incChildId02 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------
                    #region Populate Parent field
                    lookup = inc.Lookup_ParentInc;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.Select(incChildId02);
                        if (flag)
                        {
                            /* Verify the displayed Confirmation Dialog */
                            inc.WaitLoading();
                            temp = Base.GData("ParentInc_Alert");
                            flag = inc.VerifyConfirmationDialog_Incident(temp, "yes", true);
                            if (!flag)
                            { error = "The Confirmation message is incorrect: <" + ele.Text + ">. It should be <" + temp + ">"; }

                        }
                        else
                        {
                            error = "Cannot populate the Parent field";
                        }
                    }
                    else
                    {
                        error = "Parent field is null";
                    }

                    #endregion

                    #region Verify Assigment Group
                    if (flag)
                    {
                        incChild02_Group = Base.GData("Inc2_Group");
                        lookup = inc.Lookup_AssignmentGroup;
                        flag = lookup.Existed;
                        if (flag)
                        {
                            flag = (!lookup.Text.ToLower().Trim().Equals(temp.ToLower().Trim()));
                            if (!flag)
                            {
                                error = "The Assignment Group of Child Incident change to Parent's";
                                flagExit = false;
                            }
                        }
                        else
                        {
                            error = "The Assignment Group is null";
                        }
                    }
                    #endregion

                    #region Save Incident
                    if (flag)
                    {
                        flag = inc.Save();
                        if (!flag)
                        {
                            error = "Cannot save Incident";
                        }
                        else inc.WaitLoading();
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            #endregion

            #region Create a new Incident as Child Incident 04, with Assignment Group, Save Incident

            //-------------------------------------------------------------------------------------------------

            /* Click on the left menu: Incident > Create New to open Incident (New) page */
            [Test]
            public void Step_047_01_Open_NewIncident()
            {
                try
                {
                    flag = home.LeftMenuItemSelect("Incident", "Create New");
                    if (flag)
                    { inc.WaitLoading();}
                    else
                    {
                        error = "Cannot select create new incident";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Submit Incident (validating mandatory fields popup) */
            [Test]
            public void Step_047_02_Submit_BlankIncident()
            {
                try
                {
                    /* Click on the Submit button */
                    button = inc.Button_Submit;
                    flag = button.Existed;
                    if (flag)
                    {
                        button.Click(true);
                        Thread.Sleep(1000);
                        temp = Base.GData("BlankInc_Alert");
                        flag = inc.VerifyErrorMessage(temp);
                        if (!flag)
                            error = "Invalid error message.";
                    }
                    else
                    {
                        error = "Cannot get Submit button";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Populate Caller Name and verify the autopopulated fields Company, Email and Location */
            [Test]
            public void Step_047_03_Populate_CallerName()
            {
                try
                {
                    #region Get the Incident Number field
                    textbox = inc.Textbox_Number;

                    /* Check if the Incident Number field exist */
                    flag = textbox.Existed;
                    if (flag)
                    {

                        /* Click on the text field to activate it */
                        textbox.Click(true);

                        /* Save Incident Id for reference */
                        incChildId04 = textbox.Text;

                        /* Check if Incident Id is valid */
                        flag = (incChildId04 != string.Empty);
                        if (!flag)
                        {
                            error = "Invalid Incident Id";
                        }
                    }
                    else
                    {
                        error = "Incident Number field does not exist";
                    }
                    #endregion

                    #region Input Caller field
                    if (flag)
                    {
                        /* Get the Caller field */
                        temp = Base.GData("IncCaller");
                        lookup = inc.Lookup_Caller;
                        flag = lookup.Existed;
                        if (flag)
                        {
                            flag = lookup.Select(temp);
                            /* Check if caller can be input */
                            if (flag)
                            {

                                /* Wait for Company to load */
                                lookup = inc.Lookup_Company;
                                string company = Base.GData("Company");
                                flag = lookup.Existed;
                                if (flag)
                                {
                                    int count = 0;
                                    while (lookup.Text == string.Empty && count < 5)
                                    {
                                        lookup = inc.Lookup_Company;
                                        Thread.Sleep(1000);
                                    }
                                    flag = lookup.VerifyCurrentText(company, true);
                                    if (!flag) { error = "Invalid company value or the value is not auto populate."; flagExit = false; }
                                }

                            }
                            else
                            { error = "Cannot input caller."; }
                        }
                        else
                        { error = "Caller field does not exist"; }
                    }
                    #endregion

                    #region Verify Auto-Populated fields
                    if (flag)
                    {
                        error = "";

                        ///* Check if Company is auto-populated correctly */
                        //temp = inc.Lookup_Company.Text;
                        //flag = temp.Equals(Base.GData("Company"));
                        //if (!flag)
                        //{
                        //    error += "Invalid company value or company is not auto-populated.";
                        //}

                        /* Check if Email is auto-populated correctly */
                        temp = inc.Textbox_Email.Text;
                        flag = temp.Equals(Base.GData("IncCallerEmail"));
                        if (!flag)
                        {
                            error += "Invalid email value or email is not auto-populated.";
                        }

                        /* Check if Location is auto-populated correctly */
                        temp = inc.Lookup_Location.Text;
                        flag = temp.Equals(Base.GData("IncLocation"));
                        if (!flag)
                        {
                            error += "Invalid Location value or Location is not auto-populated.";
                        }
                        if (error != "") { flag = false; flagExit = false; }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Populate Business Service*/
            [Test]
            public void Step_047_04_Populate_BusinessService()
            {
                try
                {
                    temp = Base.GData("BusinessService");
                    lookup = inc.Lookup_BusinessService;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.Select(temp);
                        if (!flag)
                        { error = "Cannot input business service or invalid business service"; }
                    }
                    else
                    {
                        error = "Cannot get business service field.";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Populate Category and Subcategory */
            [Test]
            public void Step_047_05_Populate_CategoryAndSubcategory()
            {
                try
                {

                     temp = Base.GData("IncCat");
                    combobox = inc.Combobox_Category;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        flag = combobox.SelectItem(temp);
                        if (flag)
                        {
                            inc.WaitLoading();
                            string sub_cat = Base.GData("IncSubCat");
                            combobox = inc.Combobox_SubCategory;
                            flag = combobox.Existed;
                            if (flag)
                            {
                                flag = inc.VerifyHaveItemInComboboxList("subcategory", sub_cat);
                                if (flag)
                                {
                                    flag = combobox.SelectItem(sub_cat);
                                    if (!flag) error = "Cannot update Incident Subcategory.";
                                }
                            }
                            else
                            { error = "Cannot found Subcategory combobox."; }
                        }
                        else
                        {
                            error = "Cannot update Incident Category.";
                        }

                    }
                    else
                    { error = "Cannot found Category combobox."; }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }
            //-------------------------------------------------------------------------------------------------

            /* Populate Contact Type - different for each Incident */
            [Test]
            public void Step_047_06_Populate_ContactType()
            {
                try
                {
                    temp = Base.GData("Inc3_ContactType");
                    combobox = inc.Combobox_ContactType; ;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        flag = combobox.SelectItem(temp);
                        if (!flag)
                        { error = "Invalid contact type selected."; }
                    }
                    else { error = "Cannot get contact type field."; }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Populate Short description - different for each Incident */
            [Test]
            public void Step_047_07_Populate_ShortDescription()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incChildId04 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Child 04 Id.");
                        addPara.ShowDialog();
                        incChildId04 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------

                    temp = Base.GData("ShortDescription") + " - (Child) - " + incChildId04;
                    textbox = inc.Textbox_ShortDescription;
                    flag = textbox.Existed;
                    if (flag)
                    {
                        flag = textbox.SetText(temp);
                        if (!flag)
                        { error = "Cannot input short description."; }
                    }
                    else
                    { error = "Cannot get short description field."; }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Verify Assignment Group - if not auto-populated then manually assign it */
            [Test]
            public void Step_047_08_Validate_AssignmentGroup_Impact_Urgency_Priority()
            {
                try
                {
                    error = "";
                    #region Validate Assignment Group
                    lookup = inc.Lookup_AssignmentGroup;
                    incChild04_Group = lookup.Text;
                    if (!incChild04_Group.Equals(string.Empty) && !incChild04_Group.Equals(incChild03_Group))
                    {
                        System.Console.WriteLine("The Assignment Group is auto-populated correctly");
                    }
                    else
                    {
                        if (incChild04_Group.Equals(incChild03_Group))
                        {
                            System.Console.WriteLine("The Assignment Group of Child Incident 04 is the same as of Child Incident 03. Users have to change it manually");
                        }
                        else
                        {
                            System.Console.WriteLine("The Assignment Group is not auto-populated. Users have to populate manually");
                        }
                        incChild04_Group = Base.GData("Inc4_Group");
                        flag = lookup.Select(incChild04_Group);
                        if (!flag)
                        {
                            error += "Cannot populate assignment group.";
                        }
                        inc.WaitLoading();
                    }
                    #endregion

                    #region Validate Impact
                    combobox = inc.Combobox_Impact;
                    temp = Base.GData("Impact");
                    flag = combobox.Existed;
                    if (flag)
                    {
                        if (temp.ToLower().Trim().Equals(combobox.CurrentValue.ToLower().Trim()))
                        {
                            System.Console.WriteLine("The Impact is auto-populated correctly");
                        }
                        else
                        {
                            System.Console.WriteLine("The Impact is different. Users have to populate manually so all Incidents have the same Impact");
                            flag = combobox.SelectItem(temp);
                            if (!flag)
                            {
                                error += "Cannot populate Impact.";
                            }
                        }
                    }
                    else { error = "Not found combobox Impact"; }
                    #endregion

                    #region Validate Urgency
                    temp = Base.GData("Urgency");
                    combobox = inc.Combobox_Urgency;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        if (temp.ToLower().Trim().Equals(combobox.CurrentValue.ToLower().Trim()))
                        {
                            System.Console.WriteLine("The Urgency is auto-populated correctly");
                        }
                        else
                        {
                            System.Console.WriteLine("The Urgency is different. Users have to populate manually so all Incidents have the same Urgency");
                            flag = combobox.SelectItem(temp);
                            if (!flag)
                            {
                                error += "Cannot populate Urgency.";
                            }
                        }
                    }
                    else { error = "Not found combobox Urgency"; }
                    #endregion

                    #region Validate Priority
                    temp = Base.GData("Priority");
                    combobox = inc.Combobox_Priority;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        if (temp.ToLower().Trim().Equals(combobox.CurrentValue.ToLower().Trim()))
                        {
                            System.Console.WriteLine("The Priority is auto-populated correctly");
                        }
                        else
                        {
                            System.Console.WriteLine("The Priority is different. Users have to populate manually so all Incidents have the same Priority");
                            flag = combobox.SelectItem(temp);
                            if (!flag)
                            {
                                error += "Cannot populate Priority.";
                            }
                        }
                    }
                    else { error = "Not found combobox Priority"; }
                    #endregion

                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Save Incident and remains on the Incident form */
            [Test]
            public void Step_048_Save_Incident()
            {
                try
                {
                    flag = inc.Save();
                    if (!flag)
                    {
                        error = "Cannot save incident.";
                    }
                    else inc.WaitLoading();
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            #endregion

            #region Relate Blank-Assignment Child Incident 03 onto Child Incident 04, Verify Assigment Group of the Child 04 is not changed

            //-------------------------------------------------------------------------------------------------

            /* Open Related Records section */
            [Test]
            public void Step_049_Open_RelatedRecordsSection()
            {
                try
                {
                    tab = inc.GTab("Related Records");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Related Records", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (!flag)
                    {error = "Cannot open Related Records tab";}
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Set Parent to Incident 02 */
            [Test]
            public void Step_050_Populate_Parent()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incChildId03 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input incident 03 Id.");
                        addPara.ShowDialog();
                        incChildId03 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }
                    //----------------------------------------------------------------------------------------------
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incParentId01 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input incident parent Id 1.");
                        addPara.ShowDialog();
                        incParentId01 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }
                    //----------------------------------------------------------------------------------------------
                    #region Populate Parent field
                    lookup = inc.Lookup_ParentInc;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.Select(incChildId03);
                        if (flag)
                        {
                            inc.WaitLoading();
                        }
                        else
                        {
                            error = "Cannot populate the Parent field";
                        }
                    }
                    else
                    {
                        error = "Parent field is null";
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Verify Assignment Group is not changed and Save Incident */
            [Test]
            public void Step_051_Verify_AssignmentGroup()
            {
                try
                {
                    #region Verify Assigment Group
                    if (flag)
                    {
                        temp = incChild03_Group;
                        lookup = inc.Lookup_AssignmentGroup;
                        flag = lookup.Existed;
                        if (flag)
                        {
                            flag = (!lookup.Text.ToLower().Trim().Equals(temp.ToLower().Trim()));
                            if (!flag)
                            {
                                error += "The Assignment Group of Child Incident change to Parent's";
                                flagExit = false;
                            }
                        }
                        else
                        {error = "The Assignment Group is null";}
                    }
                    #endregion

                    #region Save Incident

                    flag = inc.Save();
                    if (!flag)
                    {
                        error = "Cannot save Incident";
                    }
                    else inc.WaitLoading();

                    #endregion
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            #endregion

            #region Change Assignment Group of Parent then verify it on Child Incidents

            //-------------------------------------------------------------------------------------------------

            /* Search Parent Incident by Global Search */
            [Test]
            public void Step_052_SearchGlobal_ParentIncident()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incParentId01 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Parent 01 Id.");
                        addPara.ShowDialog();
                        incParentId01 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------


                    flag = globalSearch.GlobalSearchItem(incParentId01, true);

                    if (flag)
                    {
                        inc.WaitLoading();
                    }
                    else
                    { error = "Cannot search Incident via Global Search field "; }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Change Assignment Group, Verify Confirmation text, click Yes and Save incident */
            [Test]
            public void Step_053_ChangeAssigmentGroup()
            {
                try
                {
                    error = "";

                    #region Validate Assignment Group
                    temp = Base.GData("Inc1_GroupChange");
                    lookup = inc.Lookup_AssignmentGroup;
                    flag = lookup.Select(temp);
                    inc.WaitLoading();
                    if (!flag)
                    {
                        error += "Cannot populate assignment group.";
                    }
                    #endregion

                    #region Validate Popup Window
                    temp = Base.GData("ChangeGroup_Alert");
                    flag = inc.VerifyConfirmationDialog_Incident(temp, "yes");
                    if (flag)
                        { inc.WaitLoading(); }
                    else{ error = "The Confirmation message is incorrect: <" + ele.Text + ">. It should be <" + temp + ">";}
                      
                    #endregion

                    #region Save Incident
                    if (flag)
                    {
                        flag = inc.Save();
                        if (!flag)
                        {
                            error = "Cannot save incident.";
                        }
                        else inc.WaitLoading();
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Scroll down to Child Incident section, then click on the related Child incident */
            [Test]
            public void Step_054_Open_ChildIncident()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incChildId02 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Child 02 Id.");
                        addPara.ShowDialog();
                        incChildId02 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------

                    /* Open Child Incidents tab */
                    tab = inc.GTab("Child Incidents");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Child Incidents", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {

                         /* Click on Child Incident in the list */
                        flag = inc.RelatedTableOpenRecord("Child Incidents", "Number=" + incChildId02, "Number");
                        if (flag)
                        {
                            inc.WaitLoading();

                            /* Verify if the Child Incident is opened */
                            int count = 5;
                            while (!inc.VerifyHeader(incChildId02) && count > 0)
                            {
                               Thread.Sleep(1000);
                               count--;
                            }
                            flag = inc.VerifyHeader(incChildId02);
                            if (!flag)
                            {
                              error = "Cannot open Child Incident";
                            }
                        }
                        else
                        {
                            error = "The Incident Child " + incChildId02 + " is not in the Child Incidents table";
                        }
                    }     
                    else
                    {
                        error = "There is no Child Incidents tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Verify Assignment Group of Incident 2 is the same as Incident 1 */
            [Test]
            public void Step_055_Verify_AssignmentGroup()
            {
                try
                {
                    temp = Base.GData("Inc1_GroupChange");
                    lookup = inc.Lookup_AssignmentGroup;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.Text.ToLower().Trim().Equals(temp.ToLower().Trim());
                        if (!flag)
                        {
                            flagExit = false;
                            error = "The Assignment Group of Child Incident does not change to Parent's";
                        }
                    }
                    else
                    {
                        error = "The Assignment Group is null";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Scroll down to Child Incident section, then click on the related Child incident */
            [Test]
            public void Step_056_Open_ChildIncident()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incChildId03 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Child 03 Id.");
                        addPara.ShowDialog();
                        incChildId03 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------

                    /* Open Child Incidents tab */
                    tab = inc.GTab("Child Incidents");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Child Incidents", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Click on Child Incident in the list */
                        flag = inc.RelatedTableOpenRecord("Child Incidents", "Number=" + incChildId03, "Number");
                        if (flag)
                        {
                            inc.WaitLoading();

                            /* Verify if the Child Incident is opened */
                            int count = 5;
                            while (!inc.VerifyHeader(incChildId03) && count > 0)
                            {
                                Thread.Sleep(1000);
                                count--;
                            }
                            flag = inc.VerifyHeader(incChildId03);
                            if (!flag)
                            {
                                error = "Cannot open Child Incident";
                            }
                        }
                        else
                        {
                            error = "The Incident Child " + incChildId03 + " is not in the Child Incidents table";
                        }
                    }
                    else
                    {
                        error = "There is no Child Incidents tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Verify Assignment Group of Incident 3 is the same as Incident 1 */
            [Test]
            public void Step_057_Verify_AssignmentGroup()
            {
                try
                {
                    temp = Base.GData("Inc1_GroupChange");
                    lookup = inc.Lookup_AssignmentGroup;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.Text.ToLower().Trim().Equals(temp.ToLower().Trim());
                        if (!flag)
                        {
                            flagExit = false;
                            error = "The Assignment Group of Child Incident does not change to Parent's";
                        }
                    }
                    else
                    {
                        error = "The Assignment Group is null";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            #endregion

            #region Verify mail notification NOT send to Caller regarding Work note changed (as additional comment)

            //-------------------------------------------------------------------------------------------------

            /* Impersonate the CSC Support Staff */
            [Test]
            public void Step_058_ImpersonateUser_CSCSupportStaff()
            {
                try
                {
                    /* Impersonate the Support User */
                    flag = home.ImpersonateUser(Base.GData("UserFullName"), true, Base.GData("UserFullName"), false);
                    if (flag)
                    {
                        home.WaitLoading();
                    }
                    else
                    {error = "Cannot impersonate";}
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Select CSC Run and Maintenance > Email Log */
            [Test]
            public void Step_059_Open_EmailLog()
            {
                try
                {
                    flag = home.LeftMenuItemSelect("Email Log", "Email Log");
                    if (flag)
                    {
                        emailList.WaitLoading();
                        flag = (emailList.Label_Title.Text.Contains("Emails"));
                        if (!flag)
                        {
                            error = "Cannot open email list.";
                        }
                    }
                    else
                    {
                        error = "Cannot select Email Log";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Verify the notification sent out to Caller */
            [Test]
            public void Step_060_061_Validate_EmailSentToCaller()
            {
                try
                {

                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incChildId02 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Child 02 Id.");
                        addPara.ShowDialog();
                        incChildId02 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------
                    temp = "Subject;contains;" + incChildId02 + "|and|Subject;contains;notes|and|" + "Recipients;contains;" + Base.GData("IncCallerEmail");
                    flag = emailList.EmailFilter(temp);
                    if (flag)
                    {
                        string conditions = "Subject=@@" + incChildId02;
                        flag = emailList.Verify(conditions);
                        if (flag)
                        {
                            flag = false;
                            flagExit = false;
                            error = "There is email sent to caller about work notes change";
                        }
                        else { flag = true; }
                    }
                    else
                    {
                        error = "Error when filter email.";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            #endregion

            #region Resolve the Parent Incident 1 and verify the duplicated Incident 2 also be resolved

            //-------------------------------------------------------------------------------------------------

            /* Impersonate the Resolver */
            [Test]
            public void Step_062_01_ImpersonateUser_Resolver()
            {
                try
                {
                    /* Impersonate the Resolver */
                    flag = home.ImpersonateUser(Base.GData("Resolver"));
                    if (!flag)
                    {
                        error = "Cannot impersonate";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            ////-------------------------------------------------------------------------------------------------

            ///* Change domain */
            //[Test]
            //public void Step_062_02_ChangeDomainIfNeed()
            //{
            //    try
            //    {
            //        /* Change system setting */
            //        flag = home.SystemSetting();
            //        if (!flag) { error = "Error when config system."; }
            //    }
            //    catch (Exception ex)
            //    {
            //        flag = false;
            //        error = ex.Message;
            //    }
            //}

            //-------------------------------------------------------------------------------------------------

            /* Search Incident using Global Search */
            [Test]
            public void Step_062_03_GlobalSearch_Incident()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incParentId01 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Parent 01 Id.");
                        addPara.ShowDialog();
                        incParentId01 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------

                    flag = globalSearch.GlobalSearchItem(incParentId01, true);

                    if (flag)
                    {
                        inc.WaitLoading();
                    }
                    else
                    { error = "Cannot search Incident via Global Search field "; }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Update the State of the Incident from 'Active' to 'Resolved' */
            [Test]
            public void Step_062_04_ResolvingTicket()
            {
                try
                {
                    combobox = inc.Combobox_State;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        flag = combobox.SelectItem("Resolved");
                        inc.WaitLoading();
                        if (!flag)
                        {
                            error = "Cannot select State = Resolved";
                        }
                    }
                    else
                    {
                        error = "Cannot get state control.";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Select a Close Code from the Drop Down list, Type a Note in the Close Notes. */
            [Test]
            public void Step_063_Populate_CloseCodeAndCloseNotes()
            {
                try
                {
                    tab = inc.GTab("Closure Information");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Closure Information", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        #region Populate Close Code
                        combobox = inc.Combobox_CloseCode;
                        flag = combobox.Existed;
                        if (flag)
                        {
                            flag = combobox.SelectItem(Base.GData("CloseCode"));
                            if (!flag)
                            {
                                error = "Invalid close code selected.";
                            }
                        }
                        else
                        {
                            error = "Cannot get close code control.";
                        }
                        #endregion

                        #region Populate Close Notes
                        textarea = inc.Textarea_Closenotes;
                        flag = textarea.Existed;
                        if (flag)
                        {
                            flag = textarea.SetText(Base.GData("CloseNote"));
                            if (!flag)
                            {
                                error = "Cannot add close notes.";
                            }
                        }
                        else
                        {
                            error = "Cannot get close note control.";
                        }
                        #endregion
                    }
                    else
                    {
                        error = "Cannot select closure information tab.";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Save Incident */
            [Test]
            public void Step_064_SaveIncident()
            {
                try
                {
                    flag = inc.Save();
                    if (!flag)
                    {
                        error = "Cannot Save incident";
                    }
                    else inc.WaitLoading();
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Select Incident > Resolved, Search Parent Incident and Child Incident 1 and click on it */
            [Test]
            public void Step_065_Open_ResolvedIncident()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incParentId01 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Parent 01 Id.");
                        addPara.ShowDialog();
                        incParentId01 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------

                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incChildId02 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Child 02 Id.");
                        addPara.ShowDialog();
                        incChildId02 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------

                    /* Open Resolved Incident form */
                    flag = home.LeftMenuItemSelect("Resolved", "Resolved");
                    if (flag)
                    {
                        incList.WaitLoading();
                        flag = (incList.Label_Title.Text == "Incidents");
                        if (flag)
                        {
                            /* Verify Parent Incident is in the list */
                            temp = "Number=" + incParentId01;
                            flag = incList.SearchAndVerify("Number",incParentId01,temp);
                            if (!flag)
                            {
                                error += "The Parent Incident is not in Resolved Incident form";
                            }

                            /* Verify Child Incident 1 is in the list and Click on Child Incident 1 */
                            temp = "Number=" + incChildId02;
                            flag = incList.SearchAndOpen("Number", incChildId02, temp, "Number");
                            if (!flag)
                            {
                                error += "The Child Incident 1 is not in Resolved Incident form";
                            }
                            else
                            { inc.WaitLoading(); }
                        }
                        else
                        {
                            error = "Cannot open incident list.";
                        }
                    }
                    else
                    {
                        error = "Cannot select Resolved Incident list form";
                    }
                    if (error != "")
                    {
                        flag = false; flagExit = false;
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Verify if State = Resolved in Child Incident 1 */
            [Test]
            public void Step_066_Validate_State()
            {
                try
                {
                    combobox = inc.Combobox_State;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        flag = combobox.CurrentValue.Equals("Resolved");
                        if (!flag)
                        {
                            error = "Invalid state selected:" + combobox.CurrentValue;
                        }
                    }
                    else
                    {
                        error = "Cannot get state control.";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Validate the Close Code and Closed Notes from Parent Incident was copied to Child */
            [Test]
            public void Step_067_Validate_CloseCodeAndCloseNotes()
            {
                try
                {
                    tab = inc.GTab("Closure Information");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Closure Information", true);
                        i++;
                    }
                    flag = tab.Header.Click();
                    if (flag)
                    {
                        error = "";

                        #region Validate Close Code
                        combobox = inc.Combobox_CloseCode;
                        flag = combobox.Existed;
                        if (flag)
                        {
                            flag = combobox.CurrentValue.Equals(Base.GData("CloseCode"));
                            if (!flag)
                            {
                                error += "Invalid close code: " + combobox.CurrentValue;
                            }
                        }
                        else
                        {
                            error += "Cannot get close code control.";
                        }
                        #endregion

                        #region Validate Close Notes
                        textarea = inc.Textarea_Closenotes;
                        flag = textarea.Existed;
                        if (flag)
                        {
                            temp = "Close notes copied from Parent Incident: " + Base.GData("CloseNote");
                            flag = textarea.Text.Equals(temp);
                            if (!flag)
                            {
                                error += "Invalid close note: " + textarea.Text;
                            }
                        }
                        else
                        {
                            error += "Cannot get close note control.";
                        }
                        #endregion

                        if (error != "") { flag = false; flagExit = false; }
                    }
                    else
                    {
                        error = "Cannot select closure information tab.";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            #endregion

            #region Add a work note and verify it is NOT on Child Incident

            /* Openthe Related Records tab and open the Parent Incident */
            [Test]
            public void Step_068_Open_ParentIncident()
            {
                try
                {
                    //----------------------------------------------------------------------------------------------

                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incParentId01 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Parent 01 Id.");
                        addPara.ShowDialog();
                        incParentId01 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------

                    /* Open Related Record tab */
                    tab = inc.GTab("Related Records");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Related Records", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Click on View Parent Incident link */
                        ele = inc.View_Parent;
                        flag = ele.Existed;
                        if (flag)
                        {
                            ele.Click(true);
                            inc.WaitLoading();
                            int count = 5;
                            while (!inc.VerifyHeader(incParentId01) && count > 0)
                            {
                                Thread.Sleep(1000);
                                count--;
                            }

                            /* Verify if open Parent Incident correctly */
                            flag = (inc.VerifyHeader(incParentId01));
                            if (!flag)
                            {
                                error = "Cannot open Parent Incident";
                            }
                        }
                        else
                        {
                            error = "The View Parent Incident link is null";
                        }
                    }
                    else
                    {
                        error = "Cannot open Related Records tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Add some Work Notes and save Incident */
            [Test]
            public void Step_069_Add_WorkNote()
            {
                try
                {
                    /* Select Notes tab */
                    tab = inc.GTab("Notes");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Notes", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Click Show Journal Fields button to see Work Notes and Additional Comments */
                        button = inc.Button_ShowFields;
                        if (button.Existed && button.MyEle.GetAttribute("ng-click") == "toggleMultipleInputs(true)")
                        {
                            button.Click(true);
                            inc.WaitLoading();
                        }

                        /* Get the Work Notes Area */
                        textarea = inc.Textarea_Worknotes_Update;
                        flag = textarea.Existed;
                        if (flag)
                        {
                            temp = Base.GData("WorkNote3");

                            /* Input Work Notes */
                            flag = textarea.SetText(temp);
                            inc.WaitLoading();
                            if (flag)
                            {
                                /* Save Incident */
                                flag = inc.Save();
                                if (!flag)
                                {
                                    error = "Cannot save incident.";
                                }
                                else inc.WaitLoading();
                            }
                            else
                            {
                                error = "Cannot input work notes.";
                            }
                        }
                        else
                        {
                            error = "Work Notes Area does not exist";
                        }
                    }
                    else
                    {
                        error = "Cannot open Notes tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Scroll down to Child Incident section, then click on the related Child incident */
            [Test]
            public void Step_070_Open_ChildIncident()
            {
                try
                {

                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incChildId02 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Child 02 Id.");
                        addPara.ShowDialog();
                        incChildId02 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------

                    /* Open Child Incidents tab */
                    tab = inc.GTab("Child Incidents");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Child Incidents", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Click on Child Incident in the list */
                        flag = inc.RelatedTableOpenRecord("Child Incidents","Number=" + incChildId02, "Number");
                        if (flag)
                        {
                            inc.WaitLoading();

                            /* Verify if the Child Incident is opened */
                            flag = inc.VerifyHeader(incChildId02);
                            if (!flag)
                            {
                               error = "Cannot open Child Incident";   
                            }

                        }
                        else
                        {
                            error = "The Incident Child " + incChildId02 + " is not in the Child Incidents table";
                        }
                    }
                    else
                    {
                        error = "There is no Child Incidents tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Verify the Work Note added in Parent Incident is NOT brought to Child Incident */
            [Test]
            public void Step_071_Verify_WorkNote()
            {
                try
                {
                    /* Select Notes tab */
                    tab = inc.GTab("Notes");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Notes", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Verify Parent Work Note in Activity log */
                        temp = Base.GData("Resolver") + "|Work note copied from Parent Incident: " + Base.GData("WorkNote3");
                        flag = inc.VerifyActivity(temp);
                        if (flag)
                        {
                            flag = false;
                            error = "There is Work note from Parent Incident.";
                            flagExit = false;
                        }
                        else
                        { flag = true; }
                    }
                    else
                    {
                        error = "Cannot select Notes tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            #endregion

            #region Add an Additional comment and verify it is NOT on Child Incident

            /* Openthe Related Records tab and open the Parent Incident */
            [Test]
            public void Step_072_Open_ParentIncident()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incParentId01 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Parent 01 Id.");
                        addPara.ShowDialog();
                        incParentId01 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------

                    /* Open Related Record tab */
                    tab = inc.GTab("Related Records");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Related Records", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Click on View Parent Incident link */
                        ele = inc.View_Parent;
                        flag = ele.Existed;
                        if (flag)
                        {
                            ele.Click(true);
                            inc.WaitLoading();
                            int count = 5;
                            while (!inc.VerifyHeader(incParentId01) && count > 0)
                            {
                                Thread.Sleep(1000);
                                count--;
                            }

                            /* Verify if open Parent Incident correctly */
                            flag = (inc.VerifyHeader(incParentId01));
                            if (!flag)
                            {
                                error = "Cannot open Parent Incident";
                            }
                        }
                        else
                        {
                            error = "The View Parent Incident link is null";
                        }
                    }
                    else
                    {
                        error = "Cannot open Related Records tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Add some Addtional Comments and save Incident */
            [Test]
            public void Step_073_Add_AddtionalComment()
            {
                try
                {
                    /* Select Notes tab */
                    tab = inc.GTab("Notes");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Notes", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Click Show Journal Fields button to see Work Notes and Additional Comments */
                        button = inc.Button_ShowFields;
                        if (button.Existed && button.MyEle.GetAttribute("ng-click") == "toggleMultipleInputs(true)")
                        {
                            button.Click(true);
                            inc.WaitLoading();
                        }

                        /* Get the Addtional Comments Area */
                        textarea = inc.Textarea_AdditionComments_Update;
                        flag = textarea.Existed;
                        if (flag)
                        {
                            temp = Base.GData("AddComment3");

                            /* Input Addtional Comments */
                            flag = textarea.SetText(temp);
                            inc.WaitLoading();
                            if (flag)
                            {
                                /* Save Incident */
                                flag = inc.Save();
                                if (!flag)
                                {
                                    error = "Cannot save incident.";
                                }
                                else inc.WaitLoading();
                            }
                            else
                            {
                                error = "Cannot input Addtional Comments.";
                            }
                        }
                        else
                        {
                            error = "Addtional Comments Area does not exist";
                        }
                    }
                    else
                    {
                        error = "Cannot open Notes tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Scroll down to Child Incident section, then click on the related Child incident */
            [Test]
            public void Step_074_Open_ChildIncident()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incChildId02 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Child 02 Id.");
                        addPara.ShowDialog();
                        incChildId02 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------

                    /* Open Child Incidents tab */
                    tab = inc.GTab("Child Incidents");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Child Incidents", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Click on Child Incident in the list */
                        flag = inc.RelatedTableOpenRecord("Child Incidents", "Number=" + incChildId02, "Number");
                        if (flag)
                        {
                            inc.WaitLoading();

                            /* Verify if the Child Incident is opened */
                            flag = inc.VerifyHeader(incChildId02);
                            if (!flag)
                            {
                                error = "Cannot open Child Incident";
                            }

                        }
                        else
                        {
                            error = "The Incident Child " + incChildId02 + " is not in the Child Incidents table";
                        }
                    }
                    else
                    {
                        error = "There is no Child Incidents tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Verify the Additional Comments added in Parent Incident is NOT brought to Child Incident */
            [Test]
            public void Step_075_Verify_AdditionalComments()
            {
                try
                {
                    /* Select Notes tab */
                    tab = inc.GTab("Notes");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Notes", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Verify Parent Additional Comments in Activity log */
                        temp = Base.GData("Resolver") + "|Comment copied from Parent Incident: " + Base.GData("AddComment3");
                        flag = (inc.VerifyActivity(temp));
                        if (flag)
                        {
                            flag = false;
                            error = "There is addtional comments from Parent Incident";
                            flagExit = false;
                        }
                        else { flag = true; }
                    }
                    else
                    {
                        error = "Cannot select Notes tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            #endregion

            #region Add a Work note and an Additional comment and verify they are NOT on Child Incident

            /* Openthe Related Records tab and open the Parent Incident */
            [Test]
            public void Step_076_Open_ParentIncident()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incParentId01 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Parent 01 Id.");
                        addPara.ShowDialog();
                        incParentId01 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------

                    /* Open Related Record tab */
                    tab = inc.GTab("Related Records");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Related Records", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Click on View Parent Incident link */
                        ele = inc.View_Parent;
                        flag = ele.Existed;
                        if (flag)
                        {
                            ele.Click(true);
                            inc.WaitLoading();
                            int count = 5;
                            while (!inc.VerifyHeader(incParentId01) && count > 0)
                            {
                                Thread.Sleep(1000);
                                count--;
                            }

                            /* Verify if open Parent Incident correctly */
                            flag = (inc.VerifyHeader(incParentId01));
                            if (!flag)
                            {
                                error = "Cannot open Parent Incident";
                            }
                        }
                        else
                        {
                            error = "The View Parent Incident link is null";
                        }
                    }
                    else
                    {
                        error = "Cannot open Related Records tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Add some Work Notes and Addtional Comments and save Incident */
            [Test]
            public void Step_077_Add_WorkNote_And_AddtionalComment()
            {
                try
                {
                    /* Select Notes tab */
                    tab = inc.GTab("Notes");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Notes", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Click Show Journal Fields button to see Work Notes and Additional Comments */
                        button = inc.Button_ShowFields;
                        if (button.Existed && button.MyEle.GetAttribute("ng-click") == "toggleMultipleInputs(true)")
                        {
                            button.Click(true);
                            inc.WaitLoading();
                        }

                        error = "";

                        /* Get the Addtional Comments Area */
                        textarea = inc.Textarea_AdditionComments_Update;
                        flag = textarea.Existed;
                        if (flag)
                        {
                            temp = Base.GData("AddComment4");

                            /* Input Addtional Comments */
                            flag = textarea.SetText(temp);
                            inc.WaitLoading();
                            if (!flag)
                            {
                                error += "Cannot input Addtional Comments.";
                            }
                        }
                        else
                        {
                            error += "Addtional Comments Area does not exist";
                        }

                        /* Get the Work Notes Area */
                        textarea = inc.Textarea_Worknotes_Update;
                        flag = textarea.Existed;
                        if (flag)
                        {
                            temp = Base.GData("WorkNote4");

                            /* Input Work Notes */
                            flag = textarea.SetText(temp);
                            inc.WaitLoading();
                            if (!flag)
                            {
                                error += "Cannot input Work Notes";
                            }
                        }
                        else
                        {
                            error += "Work Notes Area does not exist";
                        }

                        if (error == "")
                        {
                            /* Save Incident */
                            flag = inc.Save();
                            if (!flag)
                            {
                                error = "Cannot save incident.";
                            }
                            else inc.WaitLoading();
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                    else
                    {
                        error = "Cannot open Notes tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Scroll down to Child Incident section, then click on the related Child incident */
            [Test]
            public void Step_078_Open_ChildIncident()
            {
                try
                {

                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incChildId02 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Child 02 Id.");
                        addPara.ShowDialog();
                        incChildId02 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------

                    /* Open Child Incidents tab */
                    tab = inc.GTab("Child Incidents");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Child Incidents", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        /* Click on Child Incident in the list */
                        flag = inc.RelatedTableOpenRecord("Child Incidents", "Number=" + incChildId02, "Number");
                        if (flag)
                        {
                            inc.WaitLoading();

                            /* Verify if the Child Incident is opened */
                            flag = inc.VerifyHeader(incChildId02);
                            if (!flag)
                            {
                                error = "Cannot open Child Incident";
                            }

                        }
                        else
                        {
                            error = "The Incident Child " + incChildId02 + " is not in the Child Incidents table";
                        }
                    }
                    else
                    {
                        error = "There is no Child Incidents tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Verify the Work Notes and Additional Comments added in Parent Incident are NOT brought to Child Incident */
            [Test]
            public void Step_079_Verify_WorkNotes_And_AdditionalComments()
            {
                try
                {
                    /* Select Notes tab */
                    tab = inc.GTab("Notes");
                    int i = 0;
                    while (tab == null && i < 5)
                    {
                        Thread.Sleep(2000);
                        tab = inc.GTab("Notes", true);
                        i++;
                    }
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        error = "";
                        /* Verify Parent Additional Comments in Activity log */
                        temp = Base.GData("Resolver") + "|Comment copied from Parent Incident: " + Base.GData("AddComment4");
                        flag = (inc.VerifyActivity(temp));
                        if (flag)
                        {
                            flag = false;
                            error += "There is work notes from Parent Incident";
                            flagExit = false;
                        }
                        else { flag = true; }

                        /* Verify Parent Work Notes in Activity log */
                        temp = Base.GData("Resolver") + "|Work note copied from Parent Incident: " + Base.GData("WorkNote4");
                        flag = (inc.VerifyActivity(temp));
                        if (flag)
                        {
                            flag = false;
                            error += "There is addtional comments from Parent Incident";
                            flagExit = false;
                        }
                        else { flag = true; }
                        if (error != "") { flag = false; }
                    }
                    else
                    {
                        error = "Cannot select Notes tab";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            #endregion

            #region Verify mail notification NOT send to Caller regarding Work note changed (as additional comment)

            //-------------------------------------------------------------------------------------------------

            /* Impersonate the CSC Support Staff */
            [Test]
            public void Step_080_ImpersonateUser_CSCSupportStaff()
            {
                try
                {
                    /* Impersonate the Logistic Coordinator */
                    flag = home.ImpersonateUser(Base.GData("UserFullName"), true, Base.GData("UserFullName"));
                    if (flag)
                    {
                        home.WaitLoading();
                    }
                    else
                    {
                        error = "Cannot impersonate";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Select CSC Run and Maintenance > Email Log */
            [Test]
            public void Step_081_Open_EmailLog()
            {
                try
                {
                    flag = home.LeftMenuItemSelect("CSC Run", "Email Log");
                    if (flag)
                    {
                        emailList.WaitLoading();

                        if (!emailList.Label_Title.Text.Contains("Emails"))
                        {
                            flag = false;
                            error = "Cannot open email list.";
                        }
                    }
                    else error = "Error when open email log.";
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            //-------------------------------------------------------------------------------------------------

            /* Verify the notification sent out to Caller */
            [Test]
            public void Step_082_083_Validate_EmailNOTSentToCaller()
            {
                try
                {
                    temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incChildId02 == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input inc Child 02 Id.");
                        addPara.ShowDialog();
                        incChildId02 = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    //----------------------------------------------------------------------------------------------
                    temp = "Subject;contains;" + incChildId02 + "|and|Subject;contains;notes|and|" + "Recipients;contains;" + Base.GData("IncCallerEmail");
                    flag = emailList.EmailFilter(temp);
                    if (flag)
                    {
                        string conditions = "Subject=@@" + incChildId02;
                        flag = emailList.Verify(conditions);
                        if (flag)
                        {
                            flag = false;
                            flagExit = false;
                            error = "There is email sent to caller about work notes change";
                        }
                        else { flag = true; }
                    }
                    else
                    {
                        error = "Error when filter email.";
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

            #endregion

            //-------------------------------------------------------------------------------------------------

            [Test]
            public void Step_End_Logout()
            {
                try
                {
                    flag = home.Logout();
                    if (flag == false)
                    {
                        error = "Cannot logout system.";
                    }
                    Thread.Sleep(5000);
                }
                catch (Exception ex)
                {
                    flag = false;
                    error = ex.Message;
                }
            }

        #endregion
    }
}
