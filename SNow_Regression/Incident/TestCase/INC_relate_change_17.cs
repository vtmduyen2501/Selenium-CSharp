using NUnit.Framework;
using System;
using System.Reflection;
using System.Threading;

namespace Incident
{
    public class INC_relate_change_17
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
        Auto.otextarea textarea;
        Auto.AddParameter addPara;
        Auto.odatetime datetime;
        //------------------------------------------------------------------
        Auto.Login login = null;
        Auto.Home home = null;
        Auto.Incident inc = null;
        Auto.IncidentList inclist = null;
        Auto.Change chg = null;
        Auto.ChangeList chgList = null;
        //------------------------------------------------------------------
        string incidentId;
        string changeId;
        string changeState;
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
                chg = new Auto.Change(Base, "Change");
                chgList = new Auto.ChangeList(Base, "ChangeList");
                //------------------------------------------------------------------
                incidentId = string.Empty;
                changeId = string.Empty;
                changeState = string.Empty;
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------
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
        ////-----------------------------------------------------------------------------------------------------------------------------------
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
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_004_ImpersonateUser_SDA1()
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
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_005_ChangeDomain()
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

        #region Pre-condition creating a new Change
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_005_Pre_01_OpenNewChange()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Change", "Create New");
                if (flag)
                {
                    chg.WaitLoading();
                    string temp = Base.GData("ChangeType");
                    flag = chg.Select_ChangeType(temp);
                    if (!flag) error = "Error when select change type.";
                    else chg.WaitLoading();
                }
                else
                    error = "Error when create new change.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_005_Pre_02_PopulateCompany()
        {
            try
            {
                //Get change ID number
                textbox = chg.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    changeId = textbox.Text;
                }
                else { error = "Cannot get text number control."; }

                //--------------------------------------------------------------

                string temp = Base.GData("Company");
                lookup = chg.Lookup_Company;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
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
        public void Step_005_Pre_03_PopulateCategory()
        {
            try
            {
                string temp = Base.GData("Chg_Category");
                combobox = chg.Combobox_Category;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        chg.WaitLoading();
                    }
                    else { error = "Cannot populate category value."; }
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
        public void Step_005_Pre_04_PopulateShortDescription()
        {
            try
            {
                string temp = Base.GData("Chg_ShortDescription");
                textbox = chg.Textbox_ShortDescription;
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
        public void Step_005_Pre_05_PopulateJustification()
        {
            try
            {
                tab = chg.GTab("Planning");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = chg.GTab("Planning", true);
                    i++;
                }
                //---------------------------------------
                flag = tab.Header.Click(true);
                if (flag)
                {
                    string temp = Base.GData("Justification");
                    textarea = chg.Textarea_Justification;
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
        public void Step_005_Pre_06_PopulatePlannedDate()
        {
            try
            {
                tab = chg.GTab("Schedule");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = chg.GTab("Schedule", true);
                    i++;
                }
                //---------------------------------------
                flag = tab.Header.Click(true);
                if (flag)
                {
                    string startDate = DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss");
                    string endDate = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
                    datetime = chg.Datetime_PlannedStartDate;
                    flag = datetime.Existed;
                    if (flag)
                    {
                        flag = datetime.SetText(startDate, true);
                        if (flag)
                        {
                            datetime = chg.Datetime_PlannedEndDate;
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
                else error = "Cannot select tab (Schedule).";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_005_Pre_07_SaveChange()
        {
            try
            {
                flag = chg.Save();
                if (!flag) { error = "Error when save problem."; }
                else
                {
                    changeId = chg.Textbox_Number.Text;
                    changeState = chg.Combobox_State.Text;
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        #endregion

        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_006_OpenIncident()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Incident", "Create New");
                if (flag)
                    inc.WaitLoading();
                else
                    error = "Error when create new incident";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
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
                    //-- Store the incident id
                    incidentId = textbox.Text;
                    string temp = Base.GData("Caller");
                    lookup = inc.Lookup_Caller;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.Select(temp);
                        if (!flag)
                        {
                            error = "Cannot populate caller value";
                        }
                        else
                        {
                            error = "Cannot get lookup caller";
                        }
                    }
                    else
                    {
                        error = "Cannot get textbox number.";
                    }
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_008_PopulateBussinessService()
        {
            try
            {
                string temp = Base.GData("BusinessService");
                lookup = inc.Lookup_BusinessService;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag)
                    {
                        error = "Cannot populate business service value.";
                    }
                }
                else
                {
                    error = "Cannot get lookup business service.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
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
        ////-----------------------------------------------------------------------------------------------------------------------------------
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
        ////-----------------------------------------------------------------------------------------------------------------------------------
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
                    if (!flag)
                    {
                        error = "Cannot populate short description value.";
                    }
                }
                else
                {
                    error = "Cannot get textbox short description.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_011_SubmitIncident()
        {
            try
            {
                flag = inc.Save();
                if (!flag)
                {
                    error = "Error when save incident.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_012_PopulatedAssignmentGroup()
        {
            try
            {
                string temp = Base.GData("ServiceDeskGroup");
                lookup = inc.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (flag)
                    {
                        flag = inc.Save();
                        if (!flag)
                        {
                            error = "Error when save incident.";
                        }
                    }
                    else
                    {
                        error = "Cannot populate assignment group value.";
                    }
                }
                else
                {
                    error = "Cannot get lookup assignment group.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
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
                    if (!combobox.VerifyCurrentText(temp))
                    {
                        flag = combobox.SelectItem(temp);
                        if (!flag)
                        {
                            error += "Cannot populate Impact.";
                        }
                    }
                }
                else
                {
                    error = "Not found combobox Impact";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
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
                    if (!combobox.VerifyCurrentText(temp))
                    {
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
        ////-----------------------------------------------------------------------------------------------------------------------------------
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
                    if (!combobox.VerifyCurrentText(temp))
                    {
                        flag = combobox.SelectItem(temp);
                        if (!flag)
                        {
                            error += "Cannot populate Priority.";
                        }
                    }
                }
                else
                {
                    error = "Not found combobox Priority";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_014_ImpersonateUser_Resolver()
        {
            try
            {
                //home.WaitLoading();
                var temp = Base.GData("Resolver");
                flag = home.ImpersonateUser(temp);
                if (!flag)
                {
                    error = "Cannot impersonate resolver.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
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
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_016_017_SearchAndOpenForIncident()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    addPara = new Auto.AddParameter("Please input incident Id.");
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
                        if (!flag)
                        {
                            error = "Error when search and open incident (id:" + incidentId + ")";
                        }
                    }
                    else
                    {
                        error = "Invalid title of page. Runtime:(" + temp + "). Expected:(Incidents)";
                    }
                }
                else
                {
                    error = "Error when select open incident.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_018_AssignedIncident()
        {
            try
            {
                string temp = Base.GData("Resolver");
                lookup = inc.Lookup_AssignedTo;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag)
                    {
                        error = "Cannot put Assigned To";
                    }
                }
                else
                {
                    error = "Cannot get Assigned To field";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
        /* Open Related Records section */
        [Test]
        public void Step_019_Open_RelatedRecordsSection()
        {
            try
            {
                tab = inc.GTab("Related Records");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = inc.GTab("Related Records", true);
                    i++;
                }
                //---------------------------------------
                flag = tab.Header.Click(true);
                if (!flag)
                { error = "Cannot open Related Records tab"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }        
        ////-----------------------------------------------------------------------------------------------------------------------------------        
        [Test]
        public void Step_020_Populate_ChangeRequest()
        {
            try
            {
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (changeId == null || changeId == string.Empty))
                {
                    addPara = new Auto.AddParameter("Please input change request number.");
                    addPara.ShowDialog();
                    changeId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //----------------------------------------------------------------------------------------------

                #region Populate Change Request field
                lookup = inc.Lookup_ChangeRequest;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(changeId);
                    if (flag)
                    {
                        inc.WaitLoading();
                    }
                    else
                    {
                        error = "Cannot populate Change Request field";
                    }
                }
                else
                {
                    error = "Change Request field is null";
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
        [Test]
        public void Step_021_Save_Incident()
        {
            try
            {
                flag = inc.Save();
                if (!flag)
                {
                    error = "Cannot save Incident";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_022_023_SearchAndOpenChange()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (changeId == null || changeId == string.Empty))
                {
                    addPara = new Auto.AddParameter("Please input change Id.");
                    addPara.ShowDialog();
                    changeId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                flag = home.LeftMenuItemSelect("Change", "Open");
                if (flag)
                {
                    chgList.WaitLoading();
                    temp = chgList.Label_Title.Text;
                    flag = temp.Equals("Change Requests");
                    if (flag)
                    {
                        flag = chgList.SearchAndOpen("Number", changeId, "Number=" + changeId, "Number");
                        if (!flag) error = "Error when search and open change (id:" + changeId + ")";
                        else chg.WaitLoading();
                    }
                    else
                    {
                        error = "Invalid title of page. Runtime:(" + temp + "). Expected:(Change Requests)";
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
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_024_ValidateIncidentRelatedToChange()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (changeId == null || incidentId == string.Empty))
                {
                    addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                
                flag = chg.RelatedTableVerifyRow("Incidents Pending Change", "Number=" + incidentId, false);
                if (!flag)
                {
                    error = "There is no incident related to this problem.";
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
        public void Step_025_Open_RelatedIncident()
        {
            try
            {
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && ( incidentId == null || incidentId == string.Empty))
                {
                    addPara = new Auto.AddParameter("Please input inc Child 02 Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }

                //----------------------------------------------------------------------------------------------

                
                /* Click on Child Incident in the list */
                flag = inc.RelatedTableOpenRecord("Incidents Pending Change", "Number=" + incidentId, "Number");
                if (flag)
                {
                    inc.WaitLoading();

                    /* Verify if the Child Incident is opened */
                    flag = inc.VerifyHeader(incidentId);
                    if (!flag)
                    { error = "Cannot open Incident"; }
                }
                else
                {
                    error = "The Incident " + incidentId + " is not related to the Change table";
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
        public void Step_026_ResolveIncident()
        {
            try
            {
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "Resolved";
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                    {
                        error = "Invalid state selected.";
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
        public void Step_027_01_Open_ClosureTab()
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
                //---------------------------------------
                flag = tab.Header.Click(true);
                if (!flag)
                { error = "Cannot open Related Records tab"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_027_02_PopulateCloseCode()
        {
            try
            {
                string temp = Base.GData("CloseCode");
                combobox = inc.Combobox_CloseCode;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        inc.WaitLoading();
                    }
                    else { error = "Cannot populate close code value."; }
                }
                else
                {
                    error = "Cannot get combobox close code.";
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
        public void Step_027_03_PopulateCloseNotes()
        {
            try
            {
                string temp = Base.GData("CloseNote");
                textarea = inc.Textarea_Closenotes;
                flag = textarea.Existed;
                if (flag)
                {
                    flag = textarea.SetText(temp);
                    if (!flag) { error = "Cannot populate close notes value."; }
                }
                else { error = "Cannot get textbox close notes."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-------------------------------------------------------------------------------------------------       
        [Test]
        public void Step_028_Save_Incident()
        {
            try
            {
                flag = inc.Save();
                if (!flag)
                {
                    error = "Cannot save Incident";
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
        public void Step_029_ValidateIncident()
        {
            try
            {
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.VerifyCurrentText("Resolved");
                    if (!flag)
                    {
                        error = "Invalid state selected.";
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
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_030_01_SearchAndOpenChange()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (changeId == null || changeId == string.Empty))
                {
                    addPara = new Auto.AddParameter("Please input change Id.");
                    addPara.ShowDialog();
                    changeId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                flag = home.LeftMenuItemSelect("Change", "Open");
                if (flag)
                {
                    chgList.WaitLoading();
                    temp = chgList.Label_Title.Text;
                    flag = temp.Equals("Change Requests");
                    if (flag)
                    {
                        flag = chgList.SearchAndOpen("Number", changeId, "Number=" + changeId, "Number");
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
        public void Step_030_02_ValidateChange()
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
                        error = "The Change should not be resolved";
                    }
                    else { flag = true; }
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
        [Test]
        public void Step_031_End_Logout()
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
        
        //***********************************************************************************************************************************
        #endregion End - Scenario of test case (NEED TO UPDATE)
    }
}
