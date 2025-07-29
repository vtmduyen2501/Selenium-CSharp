using NUnit.Framework;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
namespace Incident
{
    [TestFixture]
    public class INC_create_change_using_hdr_19
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
        string incidentId, changeId, PriorityState, UrgencyState, ImpactState;

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
        public void Step_005_OpenNewIncident()
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
        public void Step_006_PopulateCallerName()
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
        public void Step_007_PopulateBusinessService()
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
        public void Step_008_PopulateCategory()
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
        public void Step_009_PopulateSubCategory()
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
        [Test]
        public void Step_012_1_PopulateAssignmentGroup()
        {
            try
            {
                string temp = Base.GData("AssignmentGroup");
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
        public void Step_012_2_SaveIncident()
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
        public void Step_013_01_ValidateImpact()
        {
            try
            {
                combobox = inc.Combobox_Impact;
                flag = combobox.Existed;
                if (flag)
                {
                    ImpactState = combobox.CurrentValue;
                    if (ImpactState == null)
                    {
                        flag = false;
                        error = "Impact state is not populated.";
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
                combobox = inc.Combobox_Urgency;
                flag = combobox.Existed;
                if (flag)
                {
                    UrgencyState = combobox.CurrentValue;
                    if (UrgencyState == null)
                    {
                        flag = false;
                        error = "Impact state is not populated.";
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
                combobox = inc.Combobox_Priority;
                flag = combobox.Existed;
                if (flag)
                {
                    PriorityState = combobox.CurrentValue;
                    if (PriorityState == null)
                    {
                        flag = false;
                        error = "Impact state is not populated.";
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
        public void Step_019_CreateChangeTicket()
        {
            try
            {
                flag = inc.CreateNormalChange();
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_020_ValidateChangeInformation()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && PriorityState == null)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input Priority State of the incident.");
                    addPara.ShowDialog();
                    PriorityState = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------

                System.Console.WriteLine("Change ShortDescription#:" + chg.Textbox_ShortDescription.MyEle.GetAttribute("value"));
                System.Console.WriteLine("Change Priority#:" + chg.Combobox_Priority.CurrentValue);
                System.Console.WriteLine("Incident Priority#:" + PriorityState);
                System.Console.WriteLine("Assignment Group#:" + chg.Lookup_AssignmentGroup.MyEle.GetAttribute("value"));
                System.Console.WriteLine("The State #:" + chg.Combobox_State.CurrentValue);
                System.Console.WriteLine("The Type #:" + chg.Combobox_Type.CurrentValue);

                System.Console.WriteLine("Change Priority(Processed)#:" + chg.Combobox_Priority.CurrentValue.Split('-')[0].Trim());

                if (Base.GData("ShortDescription") != chg.Textbox_ShortDescription.MyEle.GetAttribute("value"))
                {
                    flag = false;
                    error = "Change Short description does not equal Incident Short description:" + chg.Textbox_ShortDescription.MyEle.GetAttribute("value");
                }
                else
                {

                    System.Console.WriteLine("Change Short description" + chg.Textbox_ShortDescription.MyEle.GetAttribute("value"));

                }
                if (PriorityState == null)
                {
                    flag = false;
                    error = "Incident Priority is null";
                }
                else if (PriorityState.Split('-')[0].Trim() != chg.Combobox_Priority.CurrentValue.Split('-')[0].Trim())
                {
                    flag = false;
                    error = "Change Priority does not equal Incident Priority";
                }

                else
                {

                    System.Console.WriteLine("Change Priority" + chg.Combobox_Priority.CurrentValue);

                }

                if (chg.Lookup_AssignmentGroup.MyEle.GetAttribute("value") != "")
                {
                    flag = false;
                    error = "Assignment Group is not Blank";
                }

                else
                {

                    System.Console.WriteLine("Assignment Group" + chg.Lookup_AssignmentGroup.MyEle.GetAttribute("value"));

                }

                temp = Base.GData("RFC");
                string expChangeState = string.Empty;

                if (temp.ToLower() == "no")
                {
                    expChangeState = "Review";
                }
                else if (temp.ToLower() == "yes")
                {
                    expChangeState = "Draft";
                }
                temp = chg.Combobox_State.CurrentValue;

                if (chg.Combobox_State.CurrentValue != expChangeState)
                {
                    flag = false;
                    error = "The State is [" + temp + "]. Expected is [" + expChangeState + "].";
                }
                else
                {

                    System.Console.WriteLine("The State is " + chg.Combobox_State.CurrentValue);

                }


                if (chg.Combobox_Type.CurrentValue != "Normal")
                {
                    flag = false;
                    error = "The Type is not Normal";
                }

                else
                {

                    System.Console.WriteLine("The Type is " + chg.Combobox_Type.CurrentValue);

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
        public void Step_021_VadidateIncidentRelatedToChange()
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

                //-------------------------------------------------------------------------------------------------

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
                    flag = chg.RelatedTableVerifyRow("Incidents Pending Change", "Number=" + incidentId);
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
        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_022_Populate_Assigment_Group_If_Need()
        {

            try
            {
                if (chg.Lookup_AssignmentGroup.MyEle.GetAttribute("value") == "")
                {

                    string temp = Base.GData("AssignmentGroup");
                    lookup = chg.Lookup_AssignmentGroup;

                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.Select(temp);

                        if (flag == false)
                        {
                            error = "Cannot populate assignment group.";
                        }

                        chg.WaitLoading();
                    }
                    else
                    {
                        error = "Cannot get assignment group control.";
                    }
                }
                else
                {
                    System.Console.WriteLine("Have Assigment Group already #:" + chg.Lookup_AssignmentGroup.MyEle.GetAttribute("value"));

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

        public void Step_023_PopulateCategory()
        {
            try
            {
                combobox = chg.Combobox_Category;
                string temp = Base.GData("Category_chg");

                flag = combobox.Existed;
                if (flag)
                {
                    combobox.SelectItem(temp);
                }
                else
                {
                    error = "Cannot get Category combobox";
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

        public void Step_024_PopulateJustification()
        {
            try
            {
                tab = chg.GTab("Planning");
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = chg.GTab("Planning", true);
                    i++;
                }
                flag = tab.Header.Click();

                if (flag == true)
                {
                    textarea = chg.Textarea_Justification;
                    string temp = Base.GData("Justification_chg");

                    flag = textarea.Existed;
                    if (flag)
                    {
                        flag = textarea.SetText(temp);

                        if (flag == false)
                        {
                            error = "Cannot input justification.";
                        }
                    }
                    
                }
                else
                {
                    error = "Cannot select tab.";
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
        public void Step_025_PopulatePlannedStartDateANDPlannedEndDate()
        {
            try
            {
                tab = chg.GTab("Schedule");
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = chg.GTab("Schedule", true);
                    i++;
                }
                flag = tab.Header.Click();
                if (flag == true)
                {
                    string startDate = DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss");
                    string endDate = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");

                    datetime = chg.Datetime_PlannedStartDate;
                    flag = datetime.SetText(startDate);

                    if (flag == true)
                    {
                        datetime = chg.Datetime_PlannedEndDate;
                        flag = datetime.SetText(endDate);
                        if (flag == false)
                        {
                            error = "Cannot input planned end date.";
                        }
                    }
                    else
                    {
                        error = "Cannot input planned start date.";
                    }
                }
                else
                {
                    error = "Cannot select tab shedule";
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
        public void Step_026_UpdateChange()
        {
            try
            {
                changeId = chg.Textbox_Number.MyEle.GetAttribute("value");

                button = chg.Button_Update;

                flag = button.Existed;
                if (flag)
                {
                    button.Click();
                    inc.WaitLoading();
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

        public void Step_027_Save_Incident()
        {
            try
            {
                flag = inc.Save();
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_028_SearchAndOpenChange()
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
        public void Step_029_ValidateIncidentRelatedToChange()
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
                flag = chg.RelatedTableOpenRecord("Incidents Pending Change", "Number=" + incidentId, "Number");
                if (!flag)
                {
                    error = "Cannot open record incidentn (" + incidentId + ")";
                }
                else { inc.WaitLoading(); }
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
        public void Step_030_ResolvingTicket()
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
        public void Step_031_Populate_CloseCodeAndCloseNotes()
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
        public void Step_032_UpdateIncident()
        {
            try
            {
                flag = inc.Update();
                if (!flag)
                    error = "Cannot update incident";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_033_ValidateIncidentResolved()
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

                flag = chg.RelatedTableVerifyRow("Incidents Pending Change", "Number=" + incidentId + "|State=Resolved");

                if (!flag)
                {
                    error = "Incident " + incidentId + " was not resolved";
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
        public void Step_034_VadidateChangeState()
        {
            try
            {
                string temp = Base.GData("RFC");
                string expChangeState = string.Empty;

                if (temp.ToLower() == "no")
                {
                    expChangeState = "Review";
                }
                else if (temp.ToLower() == "yes")
                {
                    expChangeState = "Draft";
                }
                temp = chg.Combobox_State.CurrentValue;

                if (chg.Combobox_State.CurrentValue != expChangeState)
                {
                    flag = false;
                    error = "The State is [" + temp + "]. Expected is [" + expChangeState + "].";
                }
                else
                {

                    System.Console.WriteLine("The State" + chg.Combobox_State.CurrentValue);

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
        public void Step_035_Logout()
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
