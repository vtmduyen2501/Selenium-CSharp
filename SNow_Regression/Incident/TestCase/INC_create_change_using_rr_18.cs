using NUnit.Framework;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
namespace Incident
{
    [TestFixture]
    public class INC_create_change_using_rr_18
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
            System.Console.WriteLine("Finished - Change Id: " + changeId);
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
        //------------------------------------------------------------------
        Auto.Login login = null;
        Auto.Home home = null;
        Auto.Incident inc = null;
        Auto.Change chg = null;
        Auto.ChangeList chglist = null;
        Auto.KnowledgeSearch knls = null;
        Auto.IncidentList inclist = null;
        Auto.EmailList emailList = null;
        Auto.SystemSetting systemSetting = null;
        Auto.Search search = null;
        Auto.ChangeNoMainFrame chgNMF = null;

        //------------------------------------------------------------------
        string incidentId;
        string changeId;
        string chgState;

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
                chg = new Auto.Change(Base, "Change");
                chgNMF = new Auto.ChangeNoMainFrame(Base, "Change no main frame");
                chglist = new Auto.ChangeList(Base, "Change list");
                knls = new Auto.KnowledgeSearch(Base);
                inclist = new Auto.IncidentList(Base, "Incident list");
                emailList = new Auto.EmailList(Base, "Email list");
                systemSetting = new Auto.SystemSetting(Base);
                search = new Auto.Search(Base, "Search");
                //------------------------------------------------------------------
                incidentId = string.Empty;
                changeId = string.Empty;
                chgState = string.Empty;
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
        public void Step_003_ImpersonateUser_SDA1()
        {
            try
            {
                string temp = Base.GData("SDA1");
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
        public void Step_004_ChangeDomain()
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
        public void Step_007_PopulateCallerName()
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
        public void Step_008_PopulateBusinessService()
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
        public void Step_009_01_PopulateCategory()
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
                    else
                    {
                        error = "Cannot populate category.";
                    }
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
        public void Step_009_02_PopulateSubCategory()
        {
            try
            {
                string temp = Base.GData("Subcategory");
                combobox = inc.Combobox_SubCategory;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = inc.VerifyHaveItemInComboboxList("Subcategory", temp);
                    if (flag)
                    {
                        flag = combobox.SelectItem(temp);
                        if (flag)
                        {
                            inc.WaitLoading();
                        }
                        else
                        {
                            error = "Cannot populate subcategory value.";
                        }
                    }
                    else
                    {
                        error = "Cannot found item ( " + temp + " ) in sub category list.";
                    }

                }
                else
                {
                    error = "Cannot get combobox subcategory.";
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
        public void Step_010_PopulateShortDescription()
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
        public void Step_011_SaveIncident()
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
        /*
                [Test]
                public void Step_012_PopulateAssignmentGroup()
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
                */
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_013_01_ValidateImpact()
        {
            try
            {
                combobox = inc.Combobox_Impact;
                string temp = Base.GData("Impact");
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
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_013_02_ValidateUgency()
        {
            try
            {
                string temp = Base.GData("Urgency");
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
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_013_03_ValidatePriority()
        {
            try
            {
                string temp = Base.GData("Priority");
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
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_014_ImpersonateUser_RV()
        {
            try
            {
                string temp = Base.GData("Resolver");
                // string rootuser = Base.GData("UserFullName");
                flag = home.ImpersonateUser(temp);
                if (!flag) error = "Error when impersonate resolver user (" + temp + ")";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_015_ChangeDomain()
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
        public void Step_016_017_SearchAndOpenIncident()
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
        public void Step_018_PopulateAssignedTo_RV()
        {
            try
            {
                string temp = Base.GData("Resolver");
                lookup = inc.Lookup_AssignedTo;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag) error = "Cannot populate assigned to value.";
                }
                else { error = "Cannot get lookup assigned to."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_020_021_022_CreateChangeTicket()
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
                flag = tab.Header.Click();
                if (flag)
                {
                    flag = inc.Button_ChangeRequestLookup.Click();
                    if (flag)
                    {
                        flag = Base.SwitchToPage(1);
                        if (flag)
                        {
                            button = search.Button_New(true);
                            flag = button.Existed;
                            if (flag)
                            {
                                button.Click();
                                string temp = Base.GData("Change_Type");
                                flag = chgNMF.Select_ChangeType(temp);
                                if (!flag) error = "Error when select change type.";
                                else search.WaitLoading();
                            }
                            else
                            {
                                error = "Cannot not click NEW button";
                            }
                        }
                        else
                        {
                            error = "Error when switch to page 1";
                        }
                    }
                    else
                    {
                        error = "Cannot find search option go to next Change Request field";
                    }
                }
                else
                {
                    error = "Cannot open Related Records";
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
        public void Step_023_PopulateCompany()
        {
            try
            {
                string temp = Base.GData("Company");
                lookup = chgNMF.Lookup_Company_NoMainFrame;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp, true);
                    if (!flag) { error = "Cannot populate company value."; }
                }
                else { error = "Cannot get lookup company."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_024_PopulateCategory()
        {
            try
            {
                string temp = Base.GData("Category");
                combobox = chgNMF.Combobox_Category_NoMainFrame;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag) { error = "Cannot populate category value."; }
                }
                else { error = "Cannot get combobox category."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_025_PopulateAssignmentGroup()
        {
            try
            {
                string temp = Base.GData("ChangeAssignmentGroup");
                lookup = chgNMF.Lookup_AssignmentGroup_NoMainFrame;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp, true);
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
        public void Step_026_PopulateShortDescription()
        {
            try
            {
                string temp = Base.GData("ShortDescription");
                textbox = chgNMF.Textbox_ShortDescription_NoMainFrame;
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
        public void Step_027_PopulateJustification()
        {
            try
            {
                tab = chgNMF.GTabNoMainFrame("Planning");
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = chgNMF.GTab("Planning", true);
                    i++;
                }
                flag = tab.Header.Click(true);
                if (flag)
                {
                    string temp = Base.GData("ChangeJustification");
                    textarea = chgNMF.Textarea_Justification_NoMainFrame;
                    flag = textarea.Existed;
                    if (flag)
                    {
                        flag = textarea.SetText(temp);
                        if (!flag) { error = "Cannot populate justification value."; }
                    }
                    else { error = "Cannot get textarea justification."; }
                }
                else error = "Cannot select tab (Planning).";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_028_PopulatePlannedDate()
        {
            try
            {
                tab = chgNMF.GTabNoMainFrame("Schedule");
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = chgNMF.GTab("Schedule", true);
                    i++;
                }
                flag = tab.Header.Click(true);
                if (flag)
                {
                    string startDate = DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss");
                    string endDate = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
                    datetime = chgNMF.Datetime_PlannedStartDate_NoMainFrame;
                    flag = datetime.Existed;
                    if (flag)
                    {
                        flag = datetime.SetText(startDate, true);
                        if (flag)
                        {
                            datetime = chgNMF.Datetime_PlannedEndDate_NoMainFrame;
                            flag = datetime.Existed;
                            if (flag)
                            {
                                flag = datetime.SetText(endDate, true);
                                if (!flag) error = "Cannot populate planned end date.";
                            }
                            else error = "Cannot get datetime planned end date.";
                        }
                        else error = "Cannot populate planned start date.";
                    }
                    else error = "Cannot get datetime planned start date.";
                }
                else error = "Cannot select tab (Planning).";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_029_01_Verify_Number()
        {
            try
            {
                textbox = chgNMF.Textbox_Number_NoMainFrame;
                flag = textbox.Existed;
                if (flag)
                {
                    flag = textbox.Click();
                    if (flag)
                    {
                        string temp = textbox.Text;
                        flag = Regex.IsMatch(temp, "CHG*");
                        if (!flag) error = "Invalid format of change number.";
                        else
                        {
                            changeId = temp;
                            flag = combobox.Existed;
                            Console.WriteLine("-*-[STORE]: Change Id:(" + changeId + ")");
                            combobox = chgNMF.Combobox_State_NoMainFrame;
                            flag = combobox.Existed;
                            if (flag)
                            {
                                chgState = chgNMF.Combobox_State_NoMainFrame.Text;
                            }
                            else { error = "Cannot get combobox state."; }
                        }
                    }
                    else error = "Error when click on textbox number.";
                }
                else { error = "Cannot get textbox number."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_029_02_SaveChange()
        {
            try
            {

                button = chgNMF.Button_SubmitNoMainFrame;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click(true);
                    if (!flag) error = "Error when click on submit button.";
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
        public void Step_030_VerifyChangeRequest()
        {
            try
            {
                flag = Base.SwitchToPage(0);
                if (flag)
                {
                    string temp = changeId;
                    lookup = inc.Lookup_ChangeRequest;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.VerifyCurrentText(temp, true);
                        if (!flag) { error = "Invalid Change Request value or the value is not auto populate."; flagExit = false; }
                    }
                    else { error = "Cannot get lookup Change Request."; }
                }
                else
                {
                    error = "Cannot not Switch To Page 0";
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
        public void Step_031_ViewChangeRequest()
        {
            try
            {
                //TODO
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_032_SaveIncident()
        {
            try
            {
                inc.WaitLoading();
                flag = inc.Save();
                if (!flag)
                {
                    error = "Cannot save the incident.";
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
        public void Step_033_034_SearchAndOpenChange()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (changeId == null || changeId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input change Id.");
                    addPara.ShowDialog();
                    changeId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                flag = home.LeftMenuItemSelect("Change", "Open");
                if (flag)
                {
                    chglist.WaitLoading();
                    temp = chglist.Label_Title.Text;
                    flag = temp.Equals("Change Requests");
                    if (flag)
                    {
                        flag = chglist.SearchAndOpen("Number", changeId, "Number=" + changeId, "Number");
                        if (!flag) error = "Error when search and open change (id:" + changeId + ")";
                        else chg.WaitLoading();
                    }
                    else
                    {
                        error = "Invalid title of page. Runtime:(" + temp + "). Expected:(Changes)";
                    }
                }
                else error = "Error when select open change.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_035_036_ValidateIncidentRelatedToChange()
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
                tab = chg.GTab("Incidents Pending Change");
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = chg.GTab("Incidents Pending Change", true);
                    i++;
                }
                flag = tab.Header.Click(true);
                if (flag)
                {
                    flag = chg.RelatedTableOpenRecord("Incidents Pending Change", "Number=" + incidentId, "Number");
                    if (!flag)
                    {
                        Console.WriteLine("Incident " + incidentId + " is related to " + changeId);
                    }
                    else
                    {
                        error = "There is no incident related to this Change.";
                    }
                }
                else
                {
                    error = "Cannot click to the incident tab.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        /* Update the State of the Incident from 'Active' to 'Resolved' */
        [Test]
        public void Step_037_ResolvingTicket()
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
        public void Step_038_Populate_CloseCodeAndCloseNotes()
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
        public void Step_039_SaveIncident()
        {
            try
            {
                flag = inc.Save();
                if (!flag)
                {
                    error = "Cannot Save incident";
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
        public void Step_040_ValidateIncidentResolved()
        {
            try
            {
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.CurrentValue.Equals("Resolved");
                    if (!flag)
                    { error = "Invalid state selected."; }
                    else
                    {
                        Console.WriteLine("Confirmed that the incident have been resolved successfully!");
                    }
                }
                else
                { error = "Cannot get state control."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_041_SearchAndOpenChange()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (changeId == null || changeId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input change Id.");
                    addPara.ShowDialog();
                    changeId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                flag = home.LeftMenuItemSelect("Change", "Open");
                if (flag)
                {
                    chglist.WaitLoading();
                    temp = chglist.Label_Title.Text;
                    flag = temp.Equals("Change Requests");
                    if (flag)
                    {
                        flag = chglist.SearchAndOpen("Number", changeId, "Number=" + changeId, "Number");
                        if (!flag) error = "Error when search and open change (id:" + changeId + ")";
                        else chg.WaitLoading();
                    }
                    else
                    {
                        error = "Invalid title of page. Runtime:(" + temp + "). Expected:(Changes)";
                    }
                }
                else error = "Error when select open change.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_042_ValidateChange()
        {
            try
            {
                combobox = chg.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.VerifyCurrentText("Resolved");         
                    if (flag)
                    {
                        flag = false;
                        error = "The Change was resolved";
                    }
                    else
                    {
                        flag = true;
                    }
                }
                else
                { error = "Cannot get state control."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_043_Logout()
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
