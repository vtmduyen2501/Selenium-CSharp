using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Reflection;
using System.Threading;
namespace Incident
{
    [TestFixture]
    public class INC_e2e_7
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
            System.Console.WriteLine("Defect: " + Base.GData("Defect"));
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
        Auto.odatetime datetime;
        Auto.oelement ele;
        Auto.olabel label;
        //------------------------------------------------------------------
        Auto.Login login = null;
        Auto.Home home = null;
        Auto.Incident inc = null;
        Auto.KnowledgeSearch knls = null;
        Auto.IncidentList inclist = null;
        Auto.EmailList emailList = null;
        Auto.Member member = null;
        Auto.PHome phome = null;
        Auto.PSearchResult psearchResult = null;
        //------------------------------------------------------------------
        string incidentId;

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
                knls = new Auto.KnowledgeSearch(Base);
                inclist = new Auto.IncidentList(Base, "Incident list");
                emailList = new Auto.EmailList(Base, "Email list");
                member = new Auto.Member(Base);
                phome = new Auto.PHome(Base);
                psearchResult = new Auto.PSearchResult(Base, "Incident search result");
                //------------------------------------------------------------------
                incidentId = string.Empty;
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
                string temp = Base.GData("UseGlobalPass");
                if (temp.ToLower() == "yes")
                {
                    Thread.Sleep(5000);
                }
                else login.WaitLoading();
                
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
        public void Step_003_004_ImpersonateUser_SDA1()
        {
            try
            {
                string temp = Base.GData("SDA1");
                flag = home.ImpersonateUser(temp);
                if (!flag) { error = "Cannot impersonate user (" + temp + ")"; }
                else home.WaitLoading();
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_005_SystemSetting()
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
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_006_OpenNewIncident()
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
        public void Step_007_01_PopulateCallerName()
        {
            try
            {
                textbox = inc.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    textbox.Click();
                    //-- Store incident id
                    incidentId = textbox.Text;
                    Console.WriteLine("-*-[Store]: Incident Id:(" + incidentId + ")");
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
        public void Step_007_02_Verify_Company()
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
        public void Step_007_03_Verify_CallerEmail()
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
        public void Step_007_04_Verify_CallerPhone()
        {
            try
            {
                string temp = Base.GData("CallerPhone");
                if (temp.ToLower() != "no")
                {
                    textbox = inc.Textbox_BusinessPhone;
                    flag = textbox.Existed;
                    if (flag)
                    {
                        flag = textbox.VerifyCurrentText(temp, true);
                        if (!flag) { error = "Invalid caller phone or the value is not auto populate."; flagExit = false; }
                    }
                    else
                        error = "Cannot get textbox business phone.";
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
        public void Step_008_Verify_Location()
        {
            try
            {
                string temp = Base.GData("CallerLocation");
                if (temp.ToLower() != "no")
                {
                    lookup = inc.Lookup_Location;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.VerifyCurrentText(temp, true);
                        if (!flag) { error = "Invalid location or the value is not auto populate."; flagExit = false; }
                    }
                    else
                        error = "Cannot get lookup location.";
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
        public void Step_009_Change_Location()
        {
            try
            {
                string temp = Base.GData("Location_Update");
                lookup = inc.Lookup_Location;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag) { error = "Error when select location."; }
                }
                else
                    error = "Cannot get lookup location.";

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_010_PopulateBusinessService()
        {
            try
            {
                string temp = Base.GData("BusinessService");
                lookup = inc.Lookup_BusinessService;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag) { error = "Cannot populate business service value."; }
                }
                else
                    error = "Cannot get lookup business service.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_011_01_PopulateCategory()
        {
            try
            {
                string temp = Base.GData("Category");
                combobox = inc.Combobox_Category;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        inc.WaitLoading();
                    }
                    else { error = "Cannot populate category value."; }
                }
                else
                {
                    error = "Cannot get combobox category.";
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
        public void Step_011_02_PopulateSubCategory()
        {
            try
            {
                string temp = Base.GData("Subcategory");
                combobox = inc.Combobox_SubCategory;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = inc.VerifyHaveItemInComboboxList("subcategory", temp);
                    if (flag)
                    {
                        flag = combobox.SelectItem(temp);
                        if (flag)
                        {
                            inc.WaitLoading();
                        }
                        else { error = "Cannot populate sub category value."; }
                    }
                    else error = "Not found item (" + temp + ") in sub category list.";
                }
                else
                {
                    error = "Cannot get combobox sub category.";
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
        public void Step_012_PopulateContactType()
        {
            try
            {
                string temp = Base.GData("ContactType");
                combobox = inc.Combobox_ContactType;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag) { error = "Cannot populate contact type value."; }
                }
                else
                    error = "Cannot get combobox contact type.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_013_PopulateShortDescription()
        {
            try
            {
                string temp = Base.GData("ShortDescription");
                textbox = inc.Textbox_ShortDescription;
                flag = textbox.Existed;
                if (flag)
                {
                    flag = textbox.SetText(temp);
                    if (!flag) { error = "Cannot populate short description value."; }
                }
                else { error = "Cannot get textbox short description."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_014_AddACI()
        {
            try
            {
                string temp = Base.GData("CI");
                lookup = inc.Lookup_ConfigurationItem;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag) { error = "Cannot add CI."; }
                }
                else
                    error = "Cannot get lookup CI.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_015_PopulateAssignmentGroup_SDG()
        {
            try
            {
                string temp = Base.GData("ServiceDeskGroup");
                lookup = inc.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag) { error = "Cannot populate assignment group value."; }
                }
                else { error = "Cannot get lookup assignment group."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_016_SaveIncident()
        {
            try
            {
                flag = inc.Save();
                if (!flag) { error = "Error when save incident."; }
                else { inc.WaitLoading(); }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_017_SearchAndOpenIncident()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                flag = home.LeftMenuItemSelect("Incident", "Open");
                if (flag)
                {
                    inclist.WaitLoading();
                    temp = inclist.Label_Title.Text;
                    flag = temp.Equals("Incidents");
                    if (flag)
                    {
                        flag = inclist.SearchAndOpen("Number", incidentId, "Number=" + incidentId, "Number");
                        if (!flag) error = "Error when search and open incident (id:" + incidentId + ")";
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
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_018_Verify_ProcessFlow_New()
        {
            try
            {
                string temp = "New";
                flag = inc.CheckCurrentState(temp);
                if (!flag)
                {
                    flagExit = false;
                    error = "Invalid current state or Cannot check current state. Expected: [" + temp + "].";
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
        public void Step_019_01_Verify_Impact()
        {
            try
            {
                combobox = inc.Combobox_Impact;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.VerifyCurrentText("3 - Medium", true);
                    if (!flag) { error = "Invalid impact value."; flagExit = false; }
                }
                else { error = "Cannot get combobox impact."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_019_02_Verify_Urgency()
        {
            try
            {
                combobox = inc.Combobox_Urgency;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.VerifyCurrentText("3 - Medium", true);
                    if (!flag) { error = "Invalid urgency value."; flagExit = false; }
                }
                else { error = "Cannot get combobox urgency."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_019_03_Verify_Priority()
        {
            try
            {
                combobox = inc.Combobox_Priority;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.VerifyCurrentText("3 - Medium", true);
                    if (!flag) { error = "Invalid priority value."; flagExit = false; }
                }
                else { error = "Cannot get combobox priority."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_020_01_Verify_State_New()
        {
            try
            {
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.VerifyCurrentText("New", true);
                    if (!flag) { error = "Invalid state value."; flagExit = false; }
                }
                else { error = "Cannot get combobox state."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        ////-----------------------------------------------------------------------------------------------------------------------------------
        //[Test]
        //public void Step_020_02_Verify_NotFound_FollowUpdate()
        //{
        //    try
        //    {
        //        datetime = inc.Datetime_FollowUpDate;
        //        flag = datetime.Existed;
        //        if (flag)
        //        {
        //            error = "Follow up date field is visible.";
        //            flag = false;
        //            flagExit = false;
        //        }
        //        else 
        //        {
        //            flag = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        flag = false;
        //        error = ex.Message;
        //    }
        //}

        //-----------------------------------------------------------------------------------------------------------------------------------
        /*Ha Nguyen-remove validation step - Reviewed by Huong C
        [Test]
        public void Step_020_02_Verify_NotFound_FollowUpdate()
        {
            try
            {
                datetime = inc.Datetime_FollowUpDate;
                flag = datetime.Existed;
                if (flag)
                {
                    error = "Follow up date field is visible.";
                    flag = false;
                    flagExit = false;
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
        } */

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_021_CheckActivityNote_21()
        {
            try
            {
                string temp = Base.GData("Activity_21");
                flag = inc.VerifyActivity(temp);
                if (!flag)
                {
                    flagExit = false;
                    error = "Invalid activity";
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
        public void Step_022_1_Validate_BusinessService()
        {
            try
            {
                string conditions = "Configuration Item=" + Base.GData("BusinessService");
                flag = inc.RelatedTableVerifyRow("Affected CIs", conditions);
                if (!flag)
                {
                    flagExit = false;
                    error = "Not found business service: " + conditions;
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
        public void Step_022_2_Validate_CI()
        {
            try
            {
                string conditions = "Configuration Item=" + Base.GData("CI");
                flag = inc.RelatedTableVerifyRow("Affected CIs", conditions);
                if (!flag)
                {
                    flagExit = false;
                    error = "Not found CI: " + conditions;
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
        public void Step_023_PopulateAssignedTo_Assignee()
        {
            try
            {
                string temp = Base.GData("SDA2_Assignee");
                lookup = inc.Lookup_AssignedTo;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag) { error = "Cannot populate assigned to value."; }
                }
                else error = "Cannot get lookup assigned to.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_024_UpdateIncident()
        {
            try
            {
                flag = inc.Update();
                if (!flag) { error = "Error when save incident."; }
                else { inc.WaitLoading(); }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_025_SearchAndOpenIncident()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                flag = home.LeftMenuItemSelect("Incident", "Open");
                if (flag)
                {
                    inclist.WaitLoading();
                    temp = inclist.Label_Title.Text;
                    flag = temp.Equals("Incidents");
                    if (flag)
                    {
                        flag = inclist.SearchAndOpen("Number", incidentId, "Number=" + incidentId, "Number");
                        if (!flag) error = "Error when search and open incident (id:" + incidentId + ")";
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
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_026_01_Verify_ProcessFlow_Active()
        {
            try
            {
                string temp = "Active";
                flag = inc.CheckCurrentState(temp);
                if (!flag)
                {
                    flagExit = false;
                    error = "Invalid current state or Cannot check current state. Expected: [" + temp + "].";
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
        public void Step_026_02_Verify_State_Active()
        {
            try
            {
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.VerifyCurrentText("Active", true);
                    if (!flag)
                    {
                        error = "Invalid state value. Expected: [Active]. Runtime: [" + combobox.Text + "].";
                        flagExit = false;
                    }
                }
                else { error = "Cannot get combobox state."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_027_CheckActivityNote_27()
        {
            try
            {
                string temp = Base.GData("Activity_27");
                flag = inc.VerifyActivity(temp);
                if (!flag)
                {
                    flagExit = false;
                    error = "Invalid activity";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        ////-----------------------------------------------------------------------------------------------------------------------------------
        //[Test]
        //public void Step_028_Verify_NotFound_FollowUpdate()
        //{
        //    try
        //    {
        //        datetime = inc.Datetime_FollowUpDate;
        //        flag = datetime.Existed;
        //        if (flag)
        //        {
        //            error = "Follow up date field is visible.";
        //            flag = false;
        //            flagExit = false;
        //        }
        //        else
        //        {
        //            flag = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        flag = false;
        //        error = ex.Message;
        //    }
        //}

        //-----------------------------------------------------------------------------------------------------------------------------------
        /*Ha Nguyen-remove validation step - Reviewed by Huong C
        [Test]
        public void Step_028_Verify_NotFound_FollowUpdate()
        {
            try
            {
                datetime = inc.Datetime_FollowUpDate;
                flag = datetime.Existed;
                if (flag)
                {
                    error = "Follow up date field is visible.";
                    flag = false;
                    flagExit = false;
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
        } */

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_029_AddWorkNote()
        {
            try
            {
                tab = inc.GTab("Notes");
                //---------------------------------------
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
                    string temp = Base.GData("WorkNote");
                    textarea = inc.Textarea_Worknotes_Update;
                    flag = textarea.Existed;
                    if (flag)
                    {
                        flag = textarea.SetText(temp);
                        if (!flag) { error = "Cannot populate work note value."; }
                    }

                    else
                    {
                        error = "Cannot get textarea work note.";
                    }
                }
                else { error = "Cannot click on tab (Notes)"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_030_SaveIncident()
        {
            try
            {
                flag = inc.Save();
                if (!flag) { error = "Error when save incident."; }
                else { inc.WaitLoading(); }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_031_CheckActivityNote_31()
        {
            try
            {
                string temp = Base.GData("Activity_31");
                flag = inc.VerifyActivity(temp);
                if (!flag)
                {
                    flagExit = false;
                    error = "Invalid activity";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        /* Huong C - remove steps for adding/removing the attachment
        [Test]
        public void Step_032_1_AttachAFile()
        {
            try
            {
                string attachmentFile = "incidentAttachment.txt";
                flag = inc.AttachmentFile(attachmentFile);
                if (flag == false)
                {
                    error = "Error when attachment file.";
                    flagExit = false;
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
        public void Step_032_2_Verify_AttachmentFile()
        {
            try
            {
                string attachmentFile = "incidentAttachment.txt";
                flag = inc.VerifyAttachmentFile(attachmentFile);
                if (!flag)
                {
                    error = "Not found attachment file (" + attachmentFile + ") in attachment container.";
                    flagExit = false;
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
        public void Step_033_1_Rename_AttachAFile()
        {
            try
            {
                string attachmentFile = "incidentAttachment.txt";
                flag = inc.RenameAttachmentFile(attachmentFile);
                if (flag == false)
                {
                    error = "Error when rename attachment file.";
                    flagExit = false;
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
        public void Step_033_2_Verify_AttachmentFile_Rename()
        {
            try
            {
                string attachmentFile = "incidentAttachment_rename.txt";
                flag = inc.VerifyAttachmentFile(attachmentFile);
                if (!flag)
                {
                    error = "Not found attachment file (" + attachmentFile + ") in attachment container.";
                    flagExit = false;
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
        public void Step_034_1_Delete_AttachAFile()
        {
            try
            {
                string attachmentFile = "incidentAttachment_rename.txt";
                flag = inc.Delete_AttachmentFile(attachmentFile);
                if (flag == false)
                {
                    error = "Error when delete attachment file.";
                    flagExit = false;
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
        public void Step_035_Verify_AttachmentFile_Deleted()
        {
            try
            {
                string attachmentFile = "incidentAttachment_rename.txt";
                flag = inc.VerifyAttachmentFile(attachmentFile);
                if (flag)
                {
                    error = "Found attachment file (" + attachmentFile + ") in attachment container.";
                    flag = false;
                    flagExit = false;
                }
                else flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        } */
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_036_01_Add_2User_To_WatchList()
        {
            try
            {
                string temp = Base.GData("User_WatchList_Email");
                button = inc.Button_UnlockWatchList;
                if (button.Existed)
                {
                    button.Click();
                    inc.WaitLoading();
                    Thread.Sleep(2000);
                    textbox = inc.Textbox_AddWatchListEmail;
                    if (textbox.Existed)
                    {
                        string[] arr = null;
                        if (temp.Contains(","))
                            arr = temp.Split(',');
                        else
                            arr = new string[] { temp };

                        foreach (string user in arr) 
                        {
                            bool flagTemp = true;
                            flagTemp = textbox.SetText(user, true);
                            if (!flagTemp && flag)
                                flag = false;
                            Thread.Sleep(1000);
                        }

                        if (!flag)
                        {
                            error = "Error when add user to watch list";
                        }
                        else 
                        {
                            Thread.Sleep(2000);
                            button = inc.Button_LockWatchList;
                            if (button.Existed)
                            {
                                flag = button.Click();
                                if (flag)
                                    inc.WaitLoading();
                            }
                        }
                    }
                    else
                    {
                        flag = false;
                        error = "Not found Watchlist lookup";
                    }
                }
                else
                {
                    flag = false;
                    error = "Not found Unlock Watchlist button";
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
        public void Step_036_02_VerifyWatchList()
        {
            try
            {
                string temp = Base.GData("User_WatchList_Email");
                if (temp.Contains(","))
                    temp = temp.Replace(",", ", ");
                ele = inc.WatchList;
                Console.WriteLine("***Expected: [" + temp + "]");
                Console.WriteLine("***Runtime: [" + ele.Text + "]");
                if (ele.Existed)
                {
                    if (ele.Text.Trim().ToLower() != temp.Trim().ToLower()) 
                    {
                        flag = false;
                        error = "Not found added user in watch list.";
                    }
                }
                else { flag = false; error = "Cannot get control watch list."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_037_AddAdditionalComment()
        {
            try
            {
                tab = inc.GTab("Notes");
                //---------------------------------------
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
                    button = inc.Button_ShowFields;
                    flag = button.Existed;
                    if (flag)
                    {
                        if (button.MyEle.GetAttribute("title").ToLower() == "show all journal fields")
                        {
                            flag = button.Click(true);
                        }

                        if (flag)
                        {
                            string temp = Base.GData("AdditionalComment");
                            textarea = inc.Textarea_AdditionComments_Update;
                            flag = textarea.Existed;
                            if (flag)
                            {
                                flag = textarea.SetText(temp);
                                if (!flag) { error = "Cannot populate additional comment value."; }
                            }
                            else
                                error = "Cannot get textarea additional comment.";
                        }
                        else { error = "Cannot click on button show fields."; }
                    }
                    else { error = "Cannot get button show fields."; }
                }
                else { error = "Cannot click on tab (Notes)"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_038_SaveIncident()
        {
            try
            {
                flag = inc.Save();
                if (!flag) { error = "Error when save incident."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_039_CheckActivityNote_39()
        {
            try
            {
                string temp = Base.GData("Activity_39");
                flag = inc.VerifyActivity(temp);
                if (!flag)
                {
                    flagExit = false;
                    error = "Invalid activity";
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
        public void Step_040_ClickOnSearchKnowledge()
        {
            try
            {
                button = inc.Button_SearchKnowledge;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (flag)
                    {
                        //-- Switch to page 1
                        flag = Base.SwitchToPage(1);
                        if (flag)
                        {
                            flag = knls.Existed();
                            if (flag)
                            {
                                string temp = Base.GData("ShortDescription");
                                textbox = knls.Textbox_Search();
                                flag = textbox.Existed;
                                if (flag)
                                {
                                    flag = textbox.VerifyCurrentText(temp);
                                    if (!flag)
                                    {
                                        error = "Default value of texbox search knowledge is NOT THE SAME with short description.";
                                    }
                                }
                                else
                                    error = "Cannot get textbox search knowledge.";
                            }
                            else { error = "Knowledge search page is not opened."; }
                        }
                        else { error = "Error when switch to page 1"; }
                    }
                    else { error = "Cannot click on button search knowledge."; }
                }
                else { error = "Cannot get button search knownledge."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_041_SearchAndSelectArticle()
        {
            try
            {
                string temp = Base.GData("KnowledgeArticle01").Trim();
                textbox = knls.Textbox_Search();
                flag = textbox.Existed;
                if (flag)
                {
                    flag = textbox.SetText(temp, true);
                    if (flag)
                    {
                        button = knls.Button_Search;
                        flag = button.Existed;
                        if (flag)
                        {
                            flag = button.Click(true);
                            if (flag)
                            {
                                knls.WaitLoading();

                                //************Verify Knowledge List************//
                                //************Update 11.7 By Loc Truong********//
                                string kb1 = Base.GData("KnowledgeArticle01");
                                string kb1_info = Base.GData("KB1");
                                flag = knls.VerifyKnowledgeInfo_KnowledgeSearch(kb1, kb1_info);
                                if (flag) 
                                {
                                    string kb2 = Base.GData("KnowledgeArticle02");
                                    string kb2_info = Base.GData("KB2");
                                    flag = knls.VerifyKnowledgeInfo_KnowledgeSearch(kb2, kb2_info);
                                    if (!flag)
                                    {
                                        flagExit = false;
                                        error = "Cannot Verify Knowledge Info of KB 2.";
                                    }
                                }
                                else
                                {
                                    flagExit = false;
                                    error = "Cannot Verify Knowledge Info of KB 1.";
                                }
                                //************Verify Knowledge List************//

                                flag = knls.FindKnowledge(kb1);
                                if (!flag)
                                {
                                    error = "Cannot find and select: [" + kb1 +"] in Knowledge Base window.";
                                }
                            }
                            else error = "Cannot click on button search.";
                        }
                        else error = "Cannot get button search.";
                    }
                    else { error = "Cannot set value for textbox search knowledge."; }
                }
                else error = "Cannot get textbox search knowledge.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_042_ClickOnAttachToIncident()
        {
            try
            {
                button = knls.Button_AttachToIncident;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (flag)
                    {
                        Thread.Sleep(2000);
                        flag = Base.SwitchToPage(0);
                        if (!flag) { error = "Error when switch to page 0"; }
                    }
                    else { error = "Cannot click on button attach to incident."; }
                }
                else error = "Cannot get button attach to incident.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_043_SaveIncident()
        {
            try
            {
                flag = inc.Save();
                if (!flag)
                    error = "Error when save incident.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_044_Verify_Knowledge_Was_Attached()
        {
            try
            {
                string temp = Base.GData("KnowledgeArticle01").Trim();
                if (temp == string.Empty || temp == null)
                {
                    flag = false;
                    error = "There is no value. Please input data.";
                }
                else
                {
                    ele = inc.AttachedKnowledge(null);
                    flag = ele.Existed;
                    if (flag)
                    {
                        Console.WriteLine("***Expected:[" + temp + "]");
                        Console.WriteLine("***Runtime:[" + ele.Text + "]");
                        if (!ele.Text.Trim().ToLower().Contains(temp.ToLower()))
                        {
                            flag = false;
                            flagExit = false;
                            error = "Not found knowledge attached.";
                        }
                    }
                    else error = "Cannot get attached knowledge.";
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
        public void Step_046_ImpersonateUser_Assignee()
        {
            try
            {
                string temp = Base.GData("SDA2_Assignee");
                string rootuser = Base.GData("UserFullName");
                flag = home.ImpersonateUser(temp, true, rootuser);
                if (!flag) error = "Error when impersonate assignee user (" + temp + ")";
                else home.WaitLoading();
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_047_SystemSetting()
        {
            try
            {
                flag = home.SystemSetting();
                if (!flag) error = "Error when config system.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_048_049_SearchAndOpenIncident()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                flag = home.LeftMenuItemSelect("Incident", "Open");
                if (flag)
                {
                    inclist.WaitLoading();
                    temp = inclist.Label_Title.Text;
                    flag = temp.Equals("Incidents");
                    if (flag)
                    {
                        flag = inclist.SearchAndOpen("Number", incidentId, "Number=" + incidentId, "Number");
                        if (!flag) error = "Error when search and open incident (id:" + incidentId + ")";
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
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_050_01_Update_Impact()
        {
            try
            {
                combobox = inc.Combobox_Impact;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = Base.GData("Impact_Update");
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                        error = "Error when select impact.";
                }
                else error = "Cannot get combobox Impact.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_050_02_Update_Urgency()
        {
            try
            {
                combobox = inc.Combobox_Urgency;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = Base.GData("Urgency_Update");
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                        error = "Error when select urgency.";
                }
                else error = "Cannot get combobox urgency.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_051_SaveIncident()
        {
            try
            {
                flag = inc.Save();
                if (!flag)
                    error = "Error when save incident.";
                else inc.WaitLoading();
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_052_053_ClickOn_Edit_Of_AffectedCIs_Tab()
        {
            try
            {
                tab = inc.GTab("Affected CIs");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 10)
                {
                    Thread.Sleep(2000);
                    tab = inc.GTab("Affected CIs", true);
                    i++;
                }
                flag = tab.Header.Click(true);
                if (flag)
                {
                    flag = tab.ClickEdit();
                    if (flag)
                    {
                        member.WaitLoading();
                    }
                    else { error = "Error when click on Edit button."; }
                }
                else { error = "Cannot click on tab (Affected CIs)"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_054_01_Add_CI2()
        {
            try
            {
                string temp = Base.GData("CI2");
                flag = member.AddMembers("affected cis", temp);
                if (!flag)
                    error = "Error when add ci.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_054_02_Verify_CI2_Added()
        {
            try
            {
                string temp = Base.GData("CI2");
                string conditions = "Configuration Item=" + temp;
                flag = inc.RelatedTableVerifyRow("Affected CIs", conditions);
                if (!flag)
                    error = "Not found CI2.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_055_SaveIncident()
        {
            try
            {
                flag = inc.Save();
                if (!flag)
                    error = "Error when save incident.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_056_057_ClickOn_Edit_Of_AffectedCIs_Tab()
        {
            try
            {
                tab = inc.GTab("Affected CIs");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = inc.GTab("Affected CIs", true);
                    i++;
                }
                flag = tab.Header.Click(true);
                if (flag)
                {
                    flag = tab.ClickEdit();
                    if (flag)
                    {
                        member.WaitLoading();
                    }
                    else { error = "Error when click on Edit button."; }
                }
                else { error = "Cannot click on tab (Affected CIs)"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_058_01_Delete_CI2()
        {
            try
            {
                string temp = Base.GData("CI2");
                flag = member.RemoveMembers("affected cis", temp);
                if (!flag)
                    error = "Error when delete ci.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_058_02_Verify_CI2_Deleted()
        {
            try
            {
                string temp = Base.GData("CI2");
                string conditions = "Configuration Item=" + temp;
                flag = inc.RelatedTableVerifyRow("Affected CIs", conditions, true);
                if (!flag)
                    error = "Not found CI2.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_059_SaveIncident()
        {
            try
            {
                flag = inc.Save();
                if (!flag)
                    error = "Error when save incident.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_060_01_ChangeState_AwaitingProblem()
        {
            try
            {
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem("Awaiting Problem");
                    if (!flag)
                        error = "Error when select state.";
                }
                else error = "Cannot get combobox state.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
        //[Test]
        //public void Step_060_02_Verify_Found_FollowUpdate()
        //{
        //    try
        //    {
        //        datetime = inc.Datetime_FollowUpDate;
        //        flag = datetime.Existed;
        //        if (!flag)
        //            error = "Follow up date field is NOT visible.";
        //    }
        //    catch (Exception ex)
        //    {
        //        flag = false;
        //        error = ex.Message;
        //    }
        //}
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_060_03_Save_Incident()
        {
            try
            {
                flag = inc.Save();
                if (!flag)
                {
                    error = "Cannot save Incident.";
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
        public void Step_060_04_Verify_ProcessFlow_Awaiting()
        {
            try
            {
                string temp = "Awaiting";
                flag = inc.CheckCurrentState(temp);
                if (!flag)
                {
                    flagExit = false;
                    error = "Invalid current state or Cannot check current state. Expected: [" + temp + "].";
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
        public void Step_061_01_ChangeState_AwaitingCustomer()
        {
            try
            {
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem("Awaiting Customer");
                    if (!flag)
                        error = "Error when select state.";
                }
                else error = "Cannot get combobox state.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
        //[Test]
        //public void Step_061_02_Verify_Found_FollowUpdate()
        //{
        //    try
        //    {
        //        datetime = inc.Datetime_FollowUpDate;
        //        flag = datetime.Existed;
        //        if (!flag)
        //            error = "Follow up date field is NOT visible.";
        //    }
        //    catch (Exception ex)
        //    {
        //        flag = false;
        //        error = ex.Message;
        //    }
        //}
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_061_03_Save_Incident()
        {
            try
            {
                flag = inc.Save();
                if (!flag)
                {
                    error = "Cannot save Incident.";
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
        public void Step_061_04_Verify_ProcessFlow_Awaiting()
        {
            try
            {
                string temp = "Awaiting";
                flag = inc.CheckCurrentState(temp);
                if (!flag)
                {
                    flagExit = false;
                    error = "Invalid current state or Cannot check current state. Expected: [" + temp + "].";
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
        public void Step_062_01_ChangeState_AwaitingEvidence()
        {
            try
            {
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem("Awaiting Evidence");
                    if (!flag)
                        error = "Error when select state.";
                }
                else error = "Cannot get combobox state.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
        //[Test]
        //public void Step_062_02_Verify_Found_FollowUpdate()
        //{
        //    try
        //    {
        //        datetime = inc.Datetime_FollowUpDate;
        //        flag = datetime.Existed;
        //        if (!flag)
        //            error = "Follow up date field is NOT visible.";
        //    }
        //    catch (Exception ex)
        //    {
        //        flag = false;
        //        error = ex.Message;
        //    }
        //}
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_062_03_Save_Incident()
        {
            try
            {
                flag = inc.Save();
                if (!flag)
                {
                    error = "Cannot save Incident.";
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
        public void Step_062_04_Verify_ProcessFlow_Awaiting()
        {
            try
            {
                string temp = "Awaiting";
                flag = inc.CheckCurrentState(temp);
                if (!flag)
                {
                    flagExit = false;
                    error = "Invalid current state or Cannot check current state. Expected: [" + temp + "].";
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
        public void Step_063_01_ChangeState_AwaitingVendor()
        {
            try
            {
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem("Awaiting Vendor");
                    if (!flag)
                        error = "Error when select state.";
                    else { Thread.Sleep(2000); }
                }
                else error = "Cannot get combobox state.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_063_02_Populate_Vendor()
        {
            try
            {
                lookup = inc.Lookup_Vendor();
                flag = lookup.Existed;
                if (flag)
                {
                    string temp = Base.GData("Vendor");
                    flag = lookup.Select(temp);
                    if (!flag)
                    {
                        error = "Cannot select Vendor value (" + temp + ").";
                    }
                }
                else { error = "Cannot get Vendor lookup."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------

        //[Test]
        //public void Step_063_03_Verify_Found_FollowUpdate()
        //{
        //    try
        //    {
        //        datetime = inc.Datetime_FollowUpDate;
        //        flag = datetime.Existed;
        //        if (!flag)
        //            error = "Follow up date field is NOT visible.";
        //    }
        //    catch (Exception ex)
        //    {
        //        flag = false;
        //        error = ex.Message;
        //    }
        //}
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_063_04_Save_Incident()
        {
            try
            {
                flag = inc.Save();
                if (!flag)
                {
                    error = "Cannot save Incident.";
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
        public void Step_063_05_Verify_ProcessFlow_Awaiting()
        {
            try
            {
                string temp = "Awaiting";
                flag = inc.CheckCurrentState(temp);
                if (!flag)
                {
                    flagExit = false;
                    error = "Invalid current state or Cannot check current state. Expected: [" + temp + "].";
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
        public void Step_064_01_Populate_FollowUpdate_Invalid_Value()
        {
            try
            {
                datetime = inc.Datetime_FollowUpDate;
                flag = datetime.Existed;
                if (flag)
                {
                    DateTime date = DateTime.Now;
                    string today = date.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    flag = datetime.SetText(today);
                    if (!flag)
                        error = "Error when populate follow up date value.";
                }
                else error = "Cannot get datetime follow up date.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_064_02_Save_Expected_CannotSave()
        {
            try
            {
                flag = inc.SaveNoVerify();
                if (flag) 
                {
                    flag = inc.VerifyErrorMessage("The Follow up date must be greater than the Current date");
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Not found error message or Invalid message.";
                    }
                       
                }
                else { error = "Cannot Save no verify."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_065_Populate_FollowUpdate_Valid_Value_And_Save()
        {
            try
            {
                datetime = inc.Datetime_FollowUpDate;
                flag = datetime.Existed;
                if (flag)
                {
                    DateTime date = DateTime.Now;
                    string today = date.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
                    flag = datetime.SetText(today);
                    if (flag)
                    {
                        flag = inc.Save();
                        if (!flag)
                            error = "Error when save incident.";
                    }
                    else
                        error = "Error when populate follow up date value.";
                }
                else error = "Cannot get datetime follow up date.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_067_ImpersonateUser_SDA3()
        {
            try
            {
                string temp = Base.GData("SDA3");
                string rootuser = Base.GData("UserFullName");
                flag = home.ImpersonateUser(temp, true, rootuser);
                if (!flag) error = "Error when impersonate assignee user (" + temp + ")";
                else home.WaitLoading();
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_068_SystemSetting()
        {
            try
            {
                flag = home.SystemSetting();
                if (!flag) error = "Error when config system.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        /* Huong C - Verify a field readonly/edit on the list should be moved to GUI Validation ts (Steps 069_070_071)
        [Test]
        public void Step_069_070_071_Search_DoubleClick_StateColumn_SelectClosed_Expected_HaveError()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                flag = home.LeftMenuItemSelect("Incident", "Open");
                if (flag)
                {
                    inclist.WaitLoading();
                    temp = inclist.Label_Title.Text;
                    flag = temp.Equals("Incidents");
                    if (flag)
                    {
                        flag = inclist.SearchAndEditCellValue("Number", incidentId, "Number=" + incidentId, "State", "Closed");
                        if (!flag) 
                            error = "Error when search and verify incident (id:" + incidentId + ")";
                        else 
                        {
                            //flag = inclist.VerifyErrorMessage("You do not have permission to Close an incident", false, true);
                            //if (!flag)
                            //    error = "Invalid error message.";
                            try
                            {
                                IAlert alert = Base.Driver.SwitchTo().Alert();
                                temp = "Incidents cannot be Resolved or Closed from the list view.";
                                string runtime = alert.Text.Replace("\r\n", "");
                                if (!runtime.Equals(temp))
                                {
                                    flag = false;
                                    flagExit = false;
                                    error = "Invalid alert message. Runtime:(" + runtime + "). Expexted:(" + temp + ")";
                                }
                                alert.Accept();
                                inc.WaitLoading();
                            }
                            catch
                            {
                                flag = false;
                                flagExit = false;
                                error = "Alert is not visible.";
                            }
                        }
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
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_072_OpenIncident()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                flag = inclist.Open("Number=" + incidentId, "Number");
                if (flag)
                    inc.WaitLoading();
                else error = "Error when open incident.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        */
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_072_SearchAndOpenIncident()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                flag = home.LeftMenuItemSelect("Incident", "Open");
                if (flag)
                {
                    inclist.WaitLoading();
                    temp = inclist.Label_Title.Text;
                    flag = temp.Equals("Incidents");
                    if (flag)
                    {
                        flag = inclist.SearchAndOpen("Number", incidentId, "Number=" + incidentId, "Number");
                        if (!flag) error = "Error when search and open incident (id:" + incidentId + ")";
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
        [Test]
        public void Step_073_074_AddMeToWatchList()
        {
            try
            {
                button = inc.Button_AddMeToWatchList;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (flag)
                    {
                        inc.WaitLoading();
                        string temp = Base.GData("SDA3");
                        ele = inc.WatchList;

                        if (!ele.Existed || !ele.Text.Contains(temp))
                        {
                            flag = false;
                            error = "Cannot add me to watch list.";
                        }
                    }
                    else error = "Canno click on button add me to watch list.";
                }
                else
                {
                    error = "Cannot get button add me to watch list.";
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
        public void Step_075_SaveIncident()
        {
            try
            {
                flag = inc.Save();
                if (!flag)
                    error = "Error when save incident.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_076_01_Open_SelfService_WatchedIncident()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Self-Service", "Watched Incidents");
                if (flag)
                    inclist.WaitLoading();
                else
                    error = "Error when open watched incident list.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_076_02_VerifyIncidentInList()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                flag = inclist.SearchAndVerify("Number", incidentId, "Number=" + incidentId);
                if (!flag)
                    error = "Not found incident [" + incidentId +"]";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_077_ImpersonateUser_Caller()
        {
            try
            {
                string temp = Base.GData("Caller");
                string rootuser = Base.GData("UserFullName");
                flag = home.ImpersonateUser(temp, true, rootuser, true);
                if (!flag) error = "Error when impersonate assignee user (" + temp + ")";
                else phome.MyWaitLoading();
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_078_Validate_Landing_Page()
        {
            try
            {
                string temp = Base.GData("Caller");
                label = phome.Label_UserFullName();
                flag = label.Existed;
                if (flag)
                {
                    Console.WriteLine("Run time:(" + label.Text.Trim() + ")");
                    if (label.Text.Trim().ToLower() != temp.Trim().ToLower())
                    {
                        flag = false; error = "Invalid customer name.";
                    }
                }
                else error = "Cannot get label user full name.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_079_FindMyIncident()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                textbox = phome.Textbox_Global_Search;
                flag = textbox.Existed;
                if (flag)
                {
                    flag = textbox.SetText(incidentId, true);
                    if (!flag)
                        error = "Error when set text on global search.";
                    else psearchResult.WaitLoading(false, true);
                }
                else error = "Cannot get textbox global search.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_080_OpenIncident()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                flag = psearchResult.SelectItem("Incident", incidentId, true);
                if (!flag)
                    error = "Error when opent incident [" + incidentId + "]";
                else inc.WaitLoading();
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_081_01_Add_AdditionComment()
        {
            try
            {
                textarea = inc.Textarea_AdditionComments_Update;
                flag = textarea.Existed;
                if (flag) 
                {
                    flag = textarea.SetText("Caller add addition comment.");
                } else error = "Cannot get addition comment text area";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_081_02_ClickOn_ResolveIncident()
        {
            try
            {
                button = inc.Button_ResolveIncident;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (!flag) error = "Error when click button resolve incident.";
                    else phome.MyWaitLoading();
                }
                else error = "Cannot get button resolve incident.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-------------------------------------------------------------------------------------------------
        [Test]
        public void Step_082_01_Return_LoginPage()
        {
            try
            {
                string temp = Base.GData("Url");
                Base.ClearCache();
                Base.Driver.Navigate().GoToUrl(temp);
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-------------------------------------------------------------------------------------------------
        [Test]
        public void Step_082_02_ReLogin()
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
        public void Step_082_03_ImpersonateUser_Assignee()
        {
            try
            {
                string temp = Base.GData("SDA2_Assignee");
                flag = home.ImpersonateUser(temp);
                if (!flag) error = "Error when impersonate assignee user (" + temp + ")";
                else home.WaitLoading();
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_083_SystemSetting()
        {
            try
            {
                flag = home.SystemSetting();
                if (!flag) error = "Error when config system.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_084_ClickOn_Incident_Resolved()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Incident", "Resolved");
                if (flag)
                    inclist.WaitLoading();
                else
                    error = "Error when open watched incident list.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_085_SearchAndOpenIncident()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                flag = inclist.SearchAndOpen("Number", incidentId, "Number=" + incidentId, "Number");
                if (!flag) error = "Error when search and open incident (id:" + incidentId + ")";
                else inc.WaitLoading();
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_086_Verify_CloseCodeAndCloseNotes()
        {
            try
            {
                tab = inc.GTab("Closure Information");
                //---------------------------------------
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
                    combobox = inc.Combobox_CloseCode;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        flag = combobox.VerifyCurrentText("Closed/Resolved by Caller", true);
                        if (flag)
                        {
                            textarea = inc.Textarea_Closenotes;
                            flag = textarea.Existed;
                            if (flag)
                            {
                                flag = textarea.VerifyCurrentText("Closed by Caller", true);
                                if (!flag) error = "Invalid close notes.";
                            }
                            else error = "Cannot get textarea close notes.";
                        }
                        else error = "Invalid close code value.";
                    }
                    else error = "Cannot get combobox close code.";
                }
                else error = "Cannot select tab Closure Information.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_087_01_ImpersonateUser_SU()
        {
            try
            {
                string temp = Base.GData("SupportUser");
                string rootuser = Base.GData("UserFullName");
                flag = home.ImpersonateUser(temp, true, rootuser);
                if (!flag) error = "Error when impersonate support user (" + temp + ")";
                else home.WaitLoading();
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_087_02_SystemSetting()
        {
            try
            {
                flag = home.SystemSetting();
                if (!flag) error = "Error when config system.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_088_01_OpenEmailLog()
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
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_088_02_Filter_Email_For_Incident_Opened_For_Me()
        {
            try
            {
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && incidentId == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //---------------------------------------------------------------------------------------------------
                string email = Base.GData("CallerEmail");
                temp = "Subject;contains;" + incidentId + "|and|Subject;contains;opened|and|" + "Recipients;contains;" + email;
                flag = emailList.EmailFilter(temp);
                if (flag)
                {
                    emailList.WaitLoading();
                }
                else { error = "Error when filter."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_088_03_Validate_Email_For_Incident_Opened_For_Me()
        {
            try
            {
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && incidentId == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //---------------------------------------------------------------------------------------------------
                string conditions = "Subject=@@" + incidentId;
                flag = emailList.Verify(conditions);
                if (!flag){ error = "Not found email sent to caller (opened)."; flagExit = false;}
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_089_01_OpenEmailLog()
        {
            try
            {
                string neddVerify = Base.GData("Submiter_IS_Assignee");
                if (neddVerify.ToLower() == "yes")
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
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_089_02_Filter_Email_For_Incident_Assigned_To_My_Group()
        {
            try
            {
                string group = Base.GData("ServiceDeskGroup");
                string groupEmail = Base.GData("SDG_Email");
                string groupEmailMember = Base.GData("SDG_Email_Member");
                if (groupEmail.ToLower() != "no" && groupEmailMember != "no")
                {
                    string recipient = groupEmailMember;

                    if (groupEmail.ToLower() != "no")
                        recipient = groupEmail;

                    string temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incidentId == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                        addPara.ShowDialog();
                        incidentId = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }
                    //---------------------------------------------------------------------------------------------------
                    
                    temp = "Subject;contains;" + incidentId + "|and|Subject;contains;has been assigned to group " + group + "|and|" + "Recipients;contains;" + recipient;
                    flag = emailList.EmailFilter(temp);
                    if (flag)
                    {
                        emailList.WaitLoading();
                    }
                    else { error = "Error when filter."; }
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
        public void Step_089_03_Validate_Email_For_Incident_Assigned_To_My_Group()
        {
            try
            {
                string groupEmail = Base.GData("SDG_Email");
                string groupEmailMember = Base.GData("SDG_Email_Member");
                if (groupEmail.ToLower() != "no" && groupEmailMember != "no")
                {
                    string temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incidentId == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                        addPara.ShowDialog();
                        incidentId = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }
                    //---------------------------------------------------------------------------------------------------
                    string conditions = "Subject=@@" + incidentId;
                    flag = emailList.Verify(conditions);
                    if (!flag) { error = "Not found email sent to assignment group."; flagExit = false; }
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
        public void Step_090_01_OpenEmailLog()
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
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_090_02_Filter_Email_For_Incident_Assigned_To_Me()
        {
            try
            {
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && incidentId == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //---------------------------------------------------------------------------------------------------
                string recipient = Base.GData("SDA2_Assignee_Email");
                temp = "Subject;contains;" + incidentId + "|and|Subject;contains;assigned to you|and|" + "Recipients;contains;" + recipient;
                flag = emailList.EmailFilter(temp);
                if (flag)
                {
                    emailList.WaitLoading();
                }
                else { error = "Error when filter."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_090_03_Validate_Email_For_Incident_Assigned_To_Me()
        {
            try
            {
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && incidentId == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //---------------------------------------------------------------------------------------------------
                string conditions = "Subject=@@" + incidentId;
                flag = emailList.Verify(conditions);
                if (!flag) { error = "Not found email sent to assignee."; flagExit = false; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_091_01_OpenEmailLog()
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
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_091_02_Filter_Email_For_Incident_Commented_SendTo_Caller()
        {
            try
            {
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && incidentId == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //---------------------------------------------------------------------------------------------------
                string recipient = Base.GData("CallerEmail");
                temp = "Subject;contains;" + incidentId + "|and|Subject;contains;comments added|and|Recipients;contains;" + recipient;
                flag = emailList.EmailFilter(temp);
                if (flag)
                {
                    emailList.WaitLoading();
                }
                else { error = "Error when filter."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_091_03_Validate_Email_For_Incident_Commented_SendTo_Caller()
        {
            try
            {
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && incidentId == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //---------------------------------------------------------------------------------------------------
                
                string conditions = "Subject=@@" + incidentId;
                flag = emailList.Verify(conditions);
                if (!flag) { error = "Not found email sent to caller."; flagExit = false; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_092_01_OpenEmailLog()
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
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_092_02_Filter_Email_For_Incident_Commented_SendTo_AssignedTo()
        {
            try
            {
                string recipient = Base.GData("SDA2_Assignee_Email");
                if (recipient.ToLower() != "no") 
                {
                    string temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incidentId == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                        addPara.ShowDialog();
                        incidentId = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }
                    //---------------------------------------------------------------------------------------------------

                    temp = "Subject;contains;" + incidentId + "|and|Subject;contains;comments added|and|Recipients;contains;" + recipient;
                    flag = emailList.EmailFilter(temp);
                    if (flag)
                    {
                        emailList.WaitLoading();
                    }
                    else { error = "Error when filter."; }
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
        public void Step_092_03_Validate_Email_For_Incident_Commented_SendTo_AssignedTo()
        {
            try
            {
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && incidentId == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //---------------------------------------------------------------------------------------------------

                string conditions = "Subject=@@" + incidentId;
                flag = emailList.Verify(conditions);
                if (!flag) { error = "Not found email sent to assigned to."; flagExit = false; } 
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_093_01_OpenEmailLog()
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
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_093_02_Filter_Email_For_Incident_Commented_SendTo_WatchedList()
        {
            try
            {
                string recipient = Base.GData("User_WatchList_Email");
                if (recipient.ToLower() != "no")
                {
                    string temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incidentId == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                        addPara.ShowDialog();
                        incidentId = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }
                    //---------------------------------------------------------------------------------------------------

                    temp = "Subject;contains;" + incidentId + "|and|Subject;contains;comments added|and|Recipients;contains;" + recipient;
                    flag = emailList.EmailFilter(temp);
                    if (flag)
                    {
                        emailList.WaitLoading();
                    }
                    else { error = "Error when filter."; }
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
        public void Step_093_03_Validate_Email_For_Incident_Commented_SendTo_WatchedList()
        {
            try
            {
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && incidentId == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //---------------------------------------------------------------------------------------------------

                string conditions = "Subject=@@" + incidentId;
                flag = emailList.Verify(conditions);
                if (!flag){ error = "Not found email sent to watched list."; flagExit = false;}
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_094_01_OpenEmailLog()
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
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_094_02_Filter_Email_For_Incident_Resolved()
        {
            try
            {
                string recipient = Base.GData("CallerEmail");
                if (recipient.ToLower() != "no")
                {
                    string temp = Base.GData("Debug").ToLower();
                    if (temp == "yes" && incidentId == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                        addPara.ShowDialog();
                        incidentId = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }
                    //---------------------------------------------------------------------------------------------------

                    temp = "Subject;contains;" + incidentId + "|and|Subject;contains;has been resolved|and|Recipients;contains;" + recipient;
                    flag = emailList.EmailFilter(temp);
                    if (flag)
                    {
                        emailList.WaitLoading();
                    }
                    else { error = "Error when filter."; }
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
        public void Step_094_03_Validate_Email_For_Incident_Resolved()
        {
            try
            {
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && incidentId == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //---------------------------------------------------------------------------------------------------

                string conditions = "Subject=@@" + incidentId;
                flag = emailList.Verify(conditions);
                if (!flag) { error = "Not found email sent to caller."; flagExit = false; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_095_Logout()
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
