using Auto;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
namespace Incident
{
    [TestFixture]
    public class INC_view_access_inc_alert_22
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

            System.Console.WriteLine("Finished - Incident Id 1: " + incidentId_1);
            System.Console.WriteLine("Finished - Incident Id 2: " + incidentId_2);
            System.Console.WriteLine("Finished - Incident Alert 1: " + incAlert_1);
            System.Console.WriteLine("Finished - Incident Alert 2: " + incAlert_2);
            System.Console.WriteLine("Finished - Incident Alert 3: " + incAlert_3);
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

        Auto.otextbox textbox;
        Auto.olookup lookup;
        Auto.ocombobox combobox;
        Auto.otab tab;
        Auto.obutton button;
        Auto.otextarea textarea;
        
        //------------------------------------------------------------------
        Auto.Login login = null;
        Auto.Home home = null;
        Auto.Incident inc = null;
        Auto.IncidentList inclist = null;
       
        Auto.GlobalSearch global = null;
        //------------------------------------------------------------------
        string incidentId_1, incidentId_2, incAlert_1, incAlert_2, incAlert_3;

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
                inclist = new Auto.IncidentList(Base, "Incident list");
                
                global = new Auto.GlobalSearch(Base);
                //------------------------------------------------------------------
                incidentId_1 = string.Empty;
                incidentId_2 = string.Empty;
                incAlert_1 = string.Empty;
                incAlert_2 = string.Empty;
                incAlert_3 = string.Empty;
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Prestep_001_OpenSystem()
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
        public void Prestep_002_Login()
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
        public void Prestep_003_ImpersonateUser_SDA()
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
        public void Prestep_004_Create_Incident01()
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
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Prestep_005_00_Get_TicketNumber()
        {
            try
            {
                textbox = inc.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    textbox.Click();
                    incidentId_1 = textbox.Text;
                }
                else { error = "Cannot get Number textbox."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
     

        [Test]
        public void Prestep_005_01_PopulateCallerName()
        {
            try
            {
                textbox = inc.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    string temp = Base.GData("Caller");
                    lookup = inc.Lookup_Caller;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.Select(temp);
                        if (!flag) { error = "Cannot populate caller value."; }
                    }
                    else { error = "Cannot get lookup caller."; }
                }
                else
                {
                    error = "Cannot get texbox number.";
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
        public void Prestep_006_01_Verify_Company()
        {
            try
            {
                string temp = Base.GData("Company");
                if (temp.ToLower() != "no")
                {
                    lookup = inc.Lookup_Company;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.VerifyCurrentText(temp, true);
                        if (!flag) { error = "Invalid company value or the value is not auto populate."; flagExit = false; }
                    }
                    else { error = "Cannot get lookup company."; }
                }
                else Console.WriteLine("Not verify.");
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Prestep_006_02_Verify_CallerEmail()
        {
            try
            {
                string temp = Base.GData("CallerEmail");
                if (temp.ToLower() != "no")
                {
                    textbox = inc.Textbox_Email;
                    flag = textbox.Existed;
                    if (flag)
                    {
                        flag = textbox.VerifyCurrentText(temp, true);
                        if (!flag) { error = "Invalid caller email or the value is not auto populate."; flagExit = false; }
                    }
                    else
                        error = "Cannot get caller email.";
                }
                else Console.WriteLine("Not verify.");

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Prestep_007_Populate_Contact_Type()
        {
            try
            {
                combobox = inc.Combobox_ContactType;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = Base.GData("ContactType");
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                    {
                        error = "Contact Type is not correct.";
                    }
                }
                else
                {
                    error = "Can not get Contact Type combobox.";
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
        public void Prestep_008_Populate_Category()
        {
            try
            {
                combobox = inc.Combobox_Category;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = Base.GData("Category");
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                    {
                        error = "Category is not correct.";
                    }
                }
                else
                {
                    error = "Can not get Category combobox.";
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
        public void Prestep_009_Populate_SubCategory()
        {
            try
            {
                combobox = inc.Combobox_SubCategory;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = Base.GData("SubCategory");
                    Thread.Sleep(3000);
                    flag = inc.VerifyHaveItemInComboboxList("subcategory", temp);
                    if (flag)
                    {
                        flag = combobox.SelectItem(temp);
                        if (!flag)
                        {
                            error = "Can not select Subcategory in the list.";
                        }
                    }
                    else
                    {
                        error = "SubCategory list is not correct.";
                    }
                }
                else
                {
                    error = "Can not get SubCategory combobox.";
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
        public void Prestep_010_Populate_ShortDescription()
        {
            try
            {
                textbox = inc.Textbox_ShortDescription;
                flag = textbox.Existed;
                if (flag)
                {
                    string temp = Base.GData("ShortDescription01");
                    flag = textbox.SetText(temp);
                    if (!flag)
                    {
                        error = "Short Description is not correct.";
                    }
                }
                else
                {
                    error = "Can not get Short Description combobox.";
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
        public void Prestep_011_Populate_AdditionalComment()
        {
            try
            {
                textarea = inc.Textarea_AdditionComments_Create;
                flag = textarea.Existed;
                if (flag)
                {
                    string temp = Base.GData("AdditionalComment");
                    flag = textarea.SetText(temp);
                    if (!flag)
                    {
                        error = "Additional Comment is not correct.";
                    }
                }
                else
                {
                    error = "Can not get Additional Comment combobox.";
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
        public void Prestep_012_Save_Incident01()
        {
            try
            {
                flag = inc.Save();
                if (!flag)
                {
                    error = "Cannot Save Incident 1.";
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
        public void Prestep_013_Create_Incident02()
        {
            try
            {
                //Go back to Incident Management list--------------------
                flag = home.LeftMenuItemSelect("Incident", "Open");
                if (flag)
                {
                    inc.WaitLoading();
                }
                else
                {
                    error = "Error when create new incident.";
                }
                //------------------------------------------------
                button = inclist.Button_New;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (!flag)
                    {
                        error = "Can not click on New button Incident list.";
                    }
                }
                else
                    error = "Can not get New button in Incident list.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Prestep_014_PopulateCallerName()
        {
            try
            {
                textbox = inc.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    textbox.Click();
                    //-- Store incident id
                    incidentId_2 = textbox.Text;
                    Console.WriteLine("-*-[Store]: Incident Id:(" + incidentId_2 + ")");
                    string temp = Base.GData("Caller");
                    lookup = inc.Lookup_Caller;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.Select(temp);
                        if (!flag) { error = "Cannot populate caller value."; }
                    }
                    else { error = "Cannot get lookup caller."; }
                }
                else
                {
                    error = "Cannot get texbox number.";
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
        public void Prestep_015_01_Verify_Company()
        {
            try
            {
                string temp = Base.GData("Company");
                if (temp.ToLower() != "no")
                {
                    lookup = inc.Lookup_Company;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.VerifyCurrentText(temp, true);
                        if (!flag) { error = "Invalid company value or the value is not auto populate."; flagExit = false; }
                    }
                    else { error = "Cannot get lookup company."; }
                }
                else Console.WriteLine("Not verify.");
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Prestep_015_02_Verify_CallerEmail()
        {
            try
            {
                string temp = Base.GData("CallerEmail");
                if (temp.ToLower() != "no")
                {
                    textbox = inc.Textbox_Email;
                    flag = textbox.Existed;
                    if (flag)
                    {
                        flag = textbox.VerifyCurrentText(temp, true);
                        if (!flag) { error = "Invalid caller email or the value is not auto populate."; flagExit = false; }
                    }
                    else
                        error = "Cannot get caller email.";
                }
                else Console.WriteLine("Not verify.");

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Prestep_016_Populate_Contact_Type()
        {
            try
            {
                combobox = inc.Combobox_ContactType;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = Base.GData("ContactType");
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                    {
                        error = "Contact Type is not correct.";
                    }
                }
                else
                {
                    error = "Can not get Contact Type combobox.";
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
        public void Prestep_017_Populate_Category()
        {
            try
            {
                combobox = inc.Combobox_Category;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = Base.GData("Category");
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                    {
                        error = "Category is not correct.";
                    }
                    else
                    {
                        Thread.Sleep(3000);
                    }
                }
                else
                {
                    error = "Can not get Category combobox.";
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
        public void Prestep_018_Populate_SubCategory()
        {
            try
            {
                combobox = inc.Combobox_SubCategory;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = Base.GData("SubCategory");
                    Thread.Sleep(3000);
                    flag = inc.VerifyHaveItemInComboboxList("subcategory", temp);
                    if (flag)
                    {
                        flag = combobox.SelectItem(temp);
                        if (!flag)
                        {
                            error = "Can not select Subcategory in the list.";
                        }
                    }

                    else
                    {
                        error = "SubCategory is not correct.";
                    }
                }
                else
                {
                    error = "Can not get SubCategory combobox.";
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
        public void Prestep_019_Populate_ShortDescription()
        {
            try
            {
                textbox = inc.Textbox_ShortDescription;
                flag = textbox.Existed;
                if (flag)
                {
                    string temp = Base.GData("ShortDescription02");
                    flag = textbox.SetText(temp);
                    if (!flag)
                    {
                        error = "Short Description is not correct.";
                    }
                }
                else
                {
                    error = "Can not get Short Description combobox.";
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
        public void Prestep_020_Populate_AdditionalComment()
        {
            try
            {
                textarea = inc.Textarea_AdditionComments_Create;
                flag = textarea.Existed;
                if (flag)
                {
                    string temp = Base.GData("AdditionalComment");
                    flag = textarea.SetText(temp);
                    if (!flag)
                    {
                        error = "Additional Comment is not correct.";
                    }
                }
                else
                {
                    error = "Can not get Additional Comment combobox.";
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
        public void Prestep_021_Submit_Incident02()
        {
            try
            {
                button = inc.Button_Submit;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (!flag)
                    {
                        error = "Can not submit Incident 02.";
                    }
                }
                else
                {
                    error = "Can not get Submit button.";
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
        public void Prestep_022_01_Create_Incident_Alert01()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Incident Alert Management", "Create New");
                if (flag)
                {
                    inc.WaitLoading();
                    
                }
                else
                {
                    error = "Can not open Incident Alert Create New module.";
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
        public void Prestep_022_02_Relate_To_Incident01()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId_1 == null || incidentId_1 == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id 1.");
                    addPara.ShowDialog();
                    incidentId_1 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                lookup = inc.Lookup_Alert_SourceIncident;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.SetText(incidentId_1);
                    if (!flag)
                    {
                        error = "Cannot populate Incident ID 01";
                    }
                }
                else
                {
                    error = "Cannot get Source Incident look up.";
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
        public void Prestep_023_Populate_Description()
        {
            try
            {
                textbox = inc.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    //Store Incident Alert 01-----------------
                    incAlert_1 = textbox.Text;
                    Console.WriteLine("-*-[Store]: Incident Alert:(" + incAlert_1 + ")");
                    string temp = Base.GData("IncAlert_Description_1");
                    textarea = inc.Textarea_Description;
                    flag = textarea.Existed;
                    if (flag)
                    {
                        flag = textarea.SetText(temp);
                        if (!flag)
                        {
                            error = "Can not set text for Description.";
                        }
                        else
                        {
                            error = "Can not get Description textarea.";
                        }
                    }
                }
                else
                {
                    error = "Can not get Alert Number textbox.";
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
        public void Prestep_024_Populate_Priority()
        {
            try
            {
                string temp = "3 - Medium";
                combobox = inc.Combobox_Priority;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                    {
                        error = "Can not select value in Priority combobox.";
                    }
                }
                else
                {
                    error = "Can not get combobox Priorty.";
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
        public void Prestep_025_Populate_EventType()
        {
            try
            {
                string temp = Base.GData("IncAlert_EventType_1");
                combobox = inc.Combobox_Alert_EventType;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                    {
                        error = "Can not select value in EventType combobox.";
                    }
                }
                else
                {
                    error = "Can not get combobox EventType.";
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
        public void Prestep_026_Populate_Business_Service_Impact()
        {
            try
            {
                string temp = "Yes";
                combobox = inc.Combobox_Alert_BusinessServiceImpact;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        temp = Base.GData("IncAlert_BS_Impact_1");
                        textbox = inc.Textbox_Alert_BusinessServiceImpacted;
                        flag = textbox.Existed;
                        if (flag)
                        {
                            flag = textbox.SetText(temp);
                            if (!flag)
                            {
                                error = "Can not set text in BusinessServiceImpacted textbox.";
                            }
                        }
                        else
                        {
                            error = "Can not get BusinessServiceImpacted textbox.";
                        }
                    }
                    else
                    {
                        error = "Can not select value in BusinessServiceImpact combobox.";

                    }
                }
                else
                {
                    error = "Can not get BusinessServiceImpact EventType.";
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
        public void Prestep_027_Populate_Assignment_Group()
        {
            try
            {
                string temp = Base.GData("IncAlert_AssignmentGroup");
                lookup = inc.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag)
                    {
                        error = "Can not select Assignment Group.";
                    }
                }
                else
                {
                    error = "Can not get Assignment Group lookup.";
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
        public void Prestep_028_Populate_Assignee()
        {
            try
            {
                string temp = Base.GData("IncAlert_Assignee_1");
                lookup = inc.Lookup_AssignedTo;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag)
                    {
                        error = "Can not select Assignee.";
                    }
                }
                else
                {
                    error = "Can not get Assignee lookup.";
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
        public void Prestep_029_Click_Submit()
        {
            try
            {
                
                button = inc.Button_Submit;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (!flag)
                    {
                        error = "Can not click Submit.";
                    }
                }
                else
                {
                    error = "Can not get Submit button.";
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
        public void Step_001_ImpersonateUser_ITIL_User()
        {
            try
            {
                string temp = Base.GData("ITIL_User");
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
        public void Step_002_Change_Domain()
        {
            try
            {
                string temp = Base.GData("FullPathDomain");
                flag = home.SystemSetting(temp);
                if (!flag) { error = "Error when config system."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_003_Open_Incident_Alert_Management()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Incident Alert Management", "Open");
                if (flag)
                {
                    inclist.WaitLoading();
                }
                else
                {
                    error = "Can not open Incident Alert module.";
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
        public void Step_004_Verify_User_Can_Not_Create()
        {
            try
            {
                button = inclist.Button_New;
                flag = button.Existed;
                if (flag)
                {
                    error = "New button should not be existed.";
                    flag = false;
                }
                else 
                {
                    flag = true;
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
        public void Step_005_Open_Incident_Alert01()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incAlert_1 == null || incAlert_1 == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input Incident Alert 01 Id.");
                    addPara.ShowDialog();
                    incAlert_1 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                string condition = "Number;is;" + incAlert_1;
                flag = inclist.Filter(condition);
                if (flag)
                {
                    condition = "Number=" + incAlert_1;
                    flag = inclist.Open(condition,"Number");
                    if (!flag)
                    {
                        error = "Can not open Incident Alert 1";
                    }
                }
                else
                {
                    error = "Can not filter with Number of Incident Alert";
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
        public void Step_006_01_Verify_Read_Only_Field_On_Form()
        {
            try
            {
                string fields = "textbox;lookup;combobox;datetime;textarea";
                // Initiate the list of controls on form
                Dictionary<string, oelement> listOfControl = inc.GControlByType(fields);

                string expectedControlList = Base.GData("ReadOnly_Form");
                flag = inc.VerifyControls(expectedControlList, fields, listOfControl, ref error, "readonly");

                if (!flag) { flagExit = false; }
            }
            catch (Exception e)
            {
                flag = false;
                error = e.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_006_02_Verify_Read_Only_Field_On_Details_Tab()
        {
            try
            {
                tab = inc.GTab("Details");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = inc.GTab("Details", true);
                    i++;
                }
                flag = tab.Header.Click(true);
                if (flag)
                {
                    string fields = "textarea";
                    // Initiate the list of controls on form
                    Dictionary<string, oelement> listOfControl = inc.GControlByType(fields);

                    string expectedControlList = "Description;Background";
                    flag = inc.VerifyControls(expectedControlList, fields, listOfControl, ref error, "readonly");

                    if (!flag) { flagExit = false; }
                }
                else
                {
                    error = "Cannot click on (Details) tab.";
                }
            }
            catch (Exception e)
            {
                flag = false;
                error = e.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_007_Verify_User_Can_Not_Update()
        {
            try
            {
                button = inc.Button_Update;
                flag = button.Existed;
                if (flag)
                {
                    error = "Update button should not be existed.";
                    flag = false;
                }
                else
                {
                    flag = true;
                }
            }
            catch (Exception e)
            {
                flag = false;
                error = e.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_008_Open_Incident02()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId_2 == null || incidentId_2 == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input Incident 02 Id.");
                    addPara.ShowDialog();
                    incidentId_2 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                //Open incident module
                flag = home.LeftMenuItemSelect("Incident", "All");
                if (!flag)
                {
                    error = "Can not open Incident module.";
                }

                //Search Incident 02
                string condition = "Number=" + incidentId_2;
                flag = inclist.SearchAndOpen("Number", incidentId_2, condition, "Number");
                if (!flag)
                {
                    error = "Can not open Incident 02.";
                }
                else
                {
                    inc.WaitLoading();
                }
            }
            catch (Exception e)
            {
                flag = false;
                error = e.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_009_Verify_User_Can_Not_Create_Incident_Alert_Tab()
        {
            try
            {
                tab = inc.GTab("Incident Alerts");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = inc.GTab("Incident Alerts", true);
                    i++;
                }
                flag = tab.Header.Click(true);
                if (flag)
                {
                    flag = tab.ClickNew();
                    if (flag)
                    {
                        error = "User doesn't allow to click New";
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                    }
                }
                else { error = "Cannot click on tab (Incident Alerts)"; }

            }
            catch (Exception e)
            {
                flag = false;
                error = e.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_010_ImpersonateUser_MIM_User()
        {
            try
            {
                string temp = Base.GData("MIM_User");
                string temp2 = Base.GData("UserFullName");
                flag = home.ImpersonateUser(temp, true, temp2);
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
        public void Step_011_Create_Incident_Alert02()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Incident Alert Management", "Create New");
                if (flag)
                {
                    inclist.WaitLoading();
                }
                else
                {
                    error = "Can not open Incident Alert module.";
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
        public void Step_012_01_Populate_Description()
        {
            try
            {
                textbox = inc.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    //Store Incident Alert 02-----------------
                    incAlert_2 = textbox.Text;
                    Console.WriteLine("-*-[Store]: Incident Alert:(" + incAlert_2 + ")");
                    string temp = Base.GData("IncAlert_Description_2");
                    textarea = inc.Textarea_Description;
                    flag = textarea.Existed;
                    if (flag)
                    {
                        flag = textarea.SetText(temp);
                        if (!flag)
                        {
                            error = "Can not set text for Description.";
                        }
                        else
                        {
                            error = "Can not get Description textarea.";
                        }
                    }
                }
                else
                {
                    error = "Can not get Alert Number textbox.";
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
        public void Step_012_02_Populate_Priority()
        {
            try
            {
                string temp = "3 - Medium";
                combobox = inc.Combobox_Priority;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                    {
                        error = "Can not select value in Priority combobox.";
                    }
                }
                else
                {
                    error = "Can not get combobox Priorty.";
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
        public void Step_012_03_Populate_EventType()
        {
            try
            {
                string temp = Base.GData("IncAlert_EventType_2");
                combobox = inc.Combobox_Alert_EventType;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                    {
                        error = "Can not select value in EventType combobox.";
                    }
                }
                else
                {
                    error = "Can not get combobox EventType.";
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
        public void Step_012_04_Populate_Business_Service_Impact()
        {
            try
            {
                string temp = "Yes";
                combobox = inc.Combobox_Alert_BusinessServiceImpact;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        temp = Base.GData("IncAlert_BS_Impact_2");
                        textbox = inc.Textbox_Alert_BusinessServiceImpacted;
                        flag = textbox.Existed;
                        if (flag)
                        {
                            flag = textbox.SetText(temp);
                            if (!flag)
                            {
                                error = "Can not set text in BusinessServiceImpacted textbox.";
                            }
                        }
                        else
                        {
                            error = "Can not get BusinessServiceImpacted textbox.";
                        }
                    }
                    else
                    {
                        error = "Can not select value in BusinessServiceImpact combobox.";

                    }
                }
                else
                {
                    error = "Can not get BusinessServiceImpact EventType.";
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
        public void Step_012_05_Populate_Assignment_Group()
        {
            try
            {
                string temp = Base.GData("IncAlert_AssignmentGroup");
                lookup = inc.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag)
                    {
                        error = "Can not select Assignment Group.";
                    }
                }
                else
                {
                    error = "Can not get Assignment Group lookup.";
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
        public void Step_012_06_Populate_Assignee()
        {
            try
            {
                string temp = Base.GData("IncAlert_Assignee_2");
                lookup = inc.Lookup_AssignedTo;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag)
                    {
                        error = "Can not select Assignee.";
                    }
                }
                else
                {
                    error = "Can not get Assignee lookup.";
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
        public void Step_012_07_Populate_Short_Description()
        {
            try
            {
                string temp = Base.GData("IncAlert_ShortDescription_2");
                textbox = inc.Textbox_ShortDescription;
                flag = textbox.Existed;
                if (flag)
                {
                    flag = textbox.SetText(temp);
                    if (!flag)
                    {
                        error = "Can not set text for Short Description.";
                    }
                }
                else
                {
                    error = "Can not get  Short Description textbox.";
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
        public void Step_013_Click_Submit()
        {
            try
            {

                button = inc.Button_Submit;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (!flag)
                    {
                        error = "Can not click Submit.";
                    }
                }
                else
                {
                    error = "Can not get Submit button.";
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
        public void Step_014_01_Open_Incident_Alert02()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incAlert_2 == null || incAlert_2 == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input Incident Alert 02 Id.");
                    addPara.ShowDialog();
                    incAlert_2 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------                
                string condition = "Number=" + incAlert_2;    
                flag = inclist.SearchAndOpen("Number", incAlert_2 ,condition, "Number");
                if (!flag)
                {
                    error = "Can not open Incident Alert 02";
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
        public void Step_014_02_Update_Incident_Alert02()
        {
            try
            {
                textarea = inc.Textarea_Description;
                flag = textarea.Existed;
                if (flag)
                {
                    string temp = Base.GData("IncAlert_Description_Update_2");
                    flag = textarea.SetText(temp);
                    if (!flag)
                    {
                        error = "Can not set text for Description.";
                    }
                }                
                else
                {
                    error = "Cannot get Description textarea.";
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
        public void Step_014_03_Update_Incident_Alert02_Cancelled_State()
        {
            try
            {
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem("Cancelled");
                    if (!flag)
                    {
                        error = "Can not select Cancelled in State combobox";
                    }
                }
                else
                {
                    error = "Can not get State combobox";
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
        public void Step_014_04_Select_Cancellation_Code()
        {
            try
            {
                combobox = inc.Combobox_Alert_CancellationCode;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = Base.GData("IncAlert_2_CancellationCode");
                    flag = combobox.VerifyItemList(temp);
                    if (flag)
                    {
                        flag = combobox.SelectItem(temp);
                        if (!flag)
                        {
                            error = "Can not select Item in CancellationCode";
                        }
                    }
                    else
                    {
                        error = "Selected Item is not existed in combobox";
                    }
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
        public void Step_015_Click_Update_Button()
        {
            try
            {
                button = inc.Button_Update;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (flag)
                    {
                        inc.WaitLoading();
                    }
                    else
                    {
                        error = "Can not click Update button";
                    }
                }
                else
                {
                    error = "Can not get Update button";
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
        public void Step_016_ImpersonateUser_SDA()
        {
            try
            {
                string temp = Base.GData("SDA");
                string temp2 = Base.GData("UserFullName");
                flag = home.ImpersonateUser(temp, true, temp2);
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
        public void Step_017_Create_Incident_Alert03()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Incident Alert Management", "Create New");
                if (flag)
                {
                    inclist.WaitLoading();
                }
                else
                {
                    error = "Can not open Incident Alert module.";
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
        public void Step_018_01_Populate_Description()
        {
            try
            {
                textbox = inc.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    //Store Incident Alert 02-----------------
                    incAlert_3 = textbox.Text;
                    Console.WriteLine("-*-[Store]: Incident Alert:(" + incAlert_3 + ")");
                    string temp = Base.GData("IncAlert_Description_3");
                    textarea = inc.Textarea_Description;
                    flag = textarea.Existed;
                    if (flag)
                    {
                        flag = textarea.SetText(temp);
                        if (!flag)
                        {
                            error = "Can not set text for Description.";
                        }                        
                    }
                    else
                    {
                        error = "Can not get Description textarea.";
                    }
                }
                else
                {
                    error = "Can not get Alert Number textbox.";
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
        public void Step_018_02_Populate_Priority()
        {
            try
            {
                string temp = "3 - Medium";
                combobox = inc.Combobox_Priority;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                    {
                        error = "Can not select value in Priority combobox.";
                    }
                }
                else
                {
                    error = "Can not get combobox Priorty.";
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
        public void Step_018_03_Populate_EventType()
        {
            try
            {
                string temp = Base.GData("IncAlert_EventType_3");
                combobox = inc.Combobox_Alert_EventType;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                    {
                        error = "Can not select value in EventType combobox.";
                    }
                }
                else
                {
                    error = "Can not get combobox EventType.";
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
        public void Step_018_04_Populate_Business_Service_Impact()
        {
            try
            {
                string temp = "Yes";
                combobox = inc.Combobox_Alert_BusinessServiceImpact;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        temp = Base.GData("IncAlert_BS_Impact_3");
                        textbox = inc.Textbox_Alert_BusinessServiceImpacted;
                        flag = textbox.Existed;
                        if (flag)
                        {
                            flag = textbox.SetText(temp);
                            if (!flag)
                            {
                                error = "Can not set text in BusinessServiceImpacted textbox.";
                            }
                        }
                        else
                        {
                            error = "Can not get BusinessServiceImpacted textbox.";
                        }
                    }
                    else
                    {
                        error = "Can not select value in BusinessServiceImpact combobox.";

                    }
                }
                else
                {
                    error = "Can not get BusinessServiceImpact EventType.";
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
        public void Step_018_05_Populate_Assignment_Group()
        {
            try
            {
                string temp = Base.GData("IncAlert_AssignmentGroup");
                lookup = inc.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag)
                    {
                        error = "Can not select Assignment Group.";
                    }
                }
                else
                {
                    error = "Can not get Assignment Group lookup.";
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
        public void Step_018_06_Populate_Assignee()
        {
            try
            {
                string temp = Base.GData("IncAlert_Assignee_3");
                lookup = inc.Lookup_AssignedTo;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag)
                    {
                        error = "Can not select Assignee.";
                    }
                }
                else
                {
                    error = "Can not get Assignee lookup.";
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
        public void Step_018_07_Populate_Short_Description()
        {
            try
            {
                string temp = Base.GData("IncAlert_ShortDescription_3");
                textbox = inc.Textbox_ShortDescription;
                flag = textbox.Existed;
                if (flag)
                {
                    flag = textbox.SetText(temp);
                    if (!flag)
                    {
                        error = "Cannot set text for Short Description.";
                    }
                }
                else
                {
                    error = "Cannot get Short Description textbox.";
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
        public void Step_019_Click_Submit()
        {
            try
            {

                button = inc.Button_Submit;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (!flag)
                    {
                        error = "Can not click Submit.";
                    }
                }
                else
                {
                    error = "Can not get Submit button.";
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
        public void Step_020_01_Open_Incident_Alert03()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incAlert_3 == null || incAlert_3== string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input Incident Alert 03 Id.");
                    addPara.ShowDialog();
                    incAlert_3 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                string condition = "Number=" + incAlert_3;
                flag = inclist.SearchAndOpen("Number", incAlert_3, condition, "Number");
                if (!flag)
                {
                    error = "Can not open Incident Alert 03";
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
        public void Step_020_02_Update_Incident_Alert03()
        {
            try
            {
                textarea = inc.Textarea_Description;
                flag = textarea.Existed;
                if (flag)
                {
                    string temp = Base.GData("IncAlert_Description_Update_3");
                    flag = textarea.SetText(temp);
                    if (!flag)
                    {
                        error = "Can not set text for Description.";
                    }
                }
                else
                {
                    error = "Can not get Description textarea.";
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
        public void Step_020_03_Verify_Incident_Alert03_State()
        {
            try
            {
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.VerifyItemList("Cancelled");
                    if (flag)
                    {
                        error = "Cancelled State should not be existed";
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                    }
                }
                else
                {
                    error = "Can not get State combobox";
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
        public void Step_021_Click_Update_Button()
        {
            try
            {
                button = inc.Button_Update;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (flag)
                    {
                        inc.WaitLoading();
                    }
                    else
                    {
                        error = "Can not click Update button";
                    }
                }
                else
                {
                    error = "Can not get Update button";
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
        public void Step_022_ImpersonateUser_Customer_Support()
        {
            try
            {
                string temp = Base.GData("Customer_SupportStaff_User");
                string temp2 = Base.GData("UserFullName");
                flag = home.ImpersonateUser(temp, true, temp2);
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
        public void Step_023_Verify_Incident_Alert01_Not_Found()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incAlert_1 == null || incAlert_1 == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input Incident Alert 01 Id.");
                    addPara.ShowDialog();
                    incAlert_1 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //Open Incident Alert Management----------------------------------------
                flag = home.LeftMenuItemSelect("Incident Alert Management", "Open");
                if (flag)
                {
                    inclist.WaitLoading();
                }
                else
                {
                    error = "Can not open Incident Alert module.";
                }
                //-----------------------------------------------------------------------
                string condition = "Number=" + incAlert_1;
                flag = inclist.SearchAndVerify("Number", incAlert_1, condition);
                if (flag)
                {
                    error = "Incident Alert 01 should not be displayed.";
                    flag = false;
                }
                else
                {
                    flag = true;
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
        public void Step_024_ImpersonateUser_Customer_Manager()
        {
            try
            {
                string temp = Base.GData("Customer_Manager_User");
                string temp2 = Base.GData("UserFullName");
                flag = home.ImpersonateUser(temp, true, temp2);
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
        public void Step_025_Verify_Incident_Alert01_Not_Found()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incAlert_1 == null || incAlert_1 == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input Incident Alert 01 Id.");
                    addPara.ShowDialog();
                    incAlert_1 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //Open Incident Alert Management----------------------------------------
                flag = home.LeftMenuItemSelect("Incident Alert Management", "Open");
                if (flag)
                {
                    inc.WaitLoading();

                }
                else
                {
                    error = "Can not open Incident Alert module.";
                }                
                //-----------------------------------------------------------------------
                string condition = "Number=" + incAlert_1;
                flag = inclist.SearchAndVerify("Number", incAlert_1, condition);
                if (flag)
                {
                    error = "Incident Alert 01 should not be displayed.";
                    flag = false;
                }
                else
                {
                    flag = true;
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
        public void Step_026_Logout()
        {
            try
            {
                flag = home.Logout();
                if (!flag)
                {
                    error = "Error when logout system.";
                }
                else
                    login.WaitLoading();
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }


        //***********************************************************************************************************************************
        #endregion End - Scenario of test case (NEED TO UPDATE)
    }
}
