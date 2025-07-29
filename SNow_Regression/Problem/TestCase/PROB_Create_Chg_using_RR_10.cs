using Auto;
using NUnit.Framework;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

namespace Problem
{
    class PROB_Create_Chg_using_RR_10
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

            System.Console.WriteLine("Finished - Problem Id: " + problemId);
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
        Auto.odatetime datetime;
        Auto.obutton button;
        //------------------------------------------------------------------
        Auto.Login login = null;
        Auto.Home home = null;
        Auto.Change chg = null;
        Auto.Problem prb = null;
        Auto.ProblemList prblist = null;
        Auto.Member member = null;
        Auto.ChangeNoMainFrame chgNoMainFrame = null;

        Auto.ChangeList chgList = null;
        Auto.Search search = null;
        //------------------------------------------------------------------
        string problemId, changeId;

        //***********************************************************************************************************************************
        #endregion End - Define variables and objects (class) are used in test cases (NEED TO UPDATE: This case variables)

        #region Scenario of test case (NEED TO UPDATE)
        //***********************************************************************************************************************************

        #region ClassInit
        [Test]
        public void ClassInit()
        {
            try
            {
                //------------------------------------------------------------------
                login = new Auto.Login(Base);
                home = new Auto.Home(Base);
                member = new Auto.Member(Base);
                prb = new Auto.Problem(Base, "Problem");
                chg = new Auto.Change(Base, "Incident");
                prblist = new Auto.ProblemList(Base, "Problem List");
                chgList = new Auto.ChangeList(Base, "Change List");
                search = new Auto.Search(Base, "Search");
                chgNoMainFrame = new Auto.ChangeNoMainFrame(Base, "Change No Main Frame");
           
                //------------------------------------------------------------------               
                changeId = string.Empty;
                problemId = string.Empty;

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        #endregion ClassInit

        #region Login
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
                    flag = false;
                    error = "Cannot login to system.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        #endregion Login

        #region Impersonate Problem Manager
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_003_ImpersonateProblemManager()
        {
            try
            {
                string temp = Base.GData("ProblemManager");
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
                 flag = home.SystemSetting(Base.GData("FullPathDomain"));
                 if (!flag) { error = "Error when config system."; }
             }
             catch (Exception ex)
             {
                 flag = false;
                 error = ex.Message;
             }
         } 
        #endregion Imersonate Service Desk

        #region Create a new Problem
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        /*Open New problem form*/
        public void Step_005_01_OpenNewProblem()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Problem", "Create New");
                if (flag)
                    prb.WaitLoading();
                else
                    error = "Error when create new problem.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_005_02_VerifyTemplateField()
        {
            try
            {
                lookup = prb.Lookup_Template();
                if(lookup.Existed)
                {
                    flag = false;
                    error = "-*- ERROR: Template field is showed on form. This field should not be showed on new form.";
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
        public void Step_005_03_ApplyProblemTemplate()
        {
            try
            {
                lookup = prb.Lookup_AssignmentGroup;
                lookup.Click();

                //-------------------------------------------------------------------
                string temp = Base.GData("Template_Menu");
                flag = prb.SelectTemplate(temp, true);
                if (!flag)
                    error = "-*- ERROR: Cannot select template.";
                else
                {
                    //verify Template field
                    temp = Base.GData("Template");
                    lookup = prb.Lookup_Template();
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.ReadOnly;
                        if(flag)
                        {
                            flag = lookup.VerifyCurrentText(temp);
                            if (!flag)
                                error = "-*-ERROR: Incorrect template value.";
                        }
                        error = "-*-ERROR: Template is not read only.";
                    }
                    else
                        error = "Cannot get lookup template.";
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
        public void Step_006_VerifyCompany()
        {
            try
            {
                textbox = prb.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    textbox.Click();
                    //-- Store Problem id
                    problemId = textbox.Text;
                    string temp = Base.GData("Company");
                    lookup = prb.Lookup_Company;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.VerifyCurrentText(temp);
                        if (!flag) { error = "Incorrect Company value"; }
                    }
                    else { error = "Cannot get lookup company."; }
                }
                else { error = "Cannot get texbox number."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_007_VerifyProblemStatement()
        {
            try
            {
                string temp = Base.GData("ProblemStatement");
                textbox = prb.Textbox_ProblemStatement;
                flag = textbox.Existed;
                if (flag)
                {
                    flag = textbox.VerifyCurrentText(temp);
                    if (!flag) { error = "Incorrect Problem Statement"; }
                }
                else { error = "Cannot get Problem Statement field"; }

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]

        public void Step_008_01_VerifyAssignmentGroup()
        {
            try
            {
                string temp = Base.GData("ProblemAssignmentGroup");
                lookup = prb.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.VerifyCurrentText(temp);
                    if (!flag) error = "Incorrect Assignment Group";

                }
                else error = "Cannot get Assignment Group field.";              
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-------------------------------------------------------------------------------------------------

        //[Test]
        //public void Step_008_02_Populate_More_Fields_If_Need()
        //{
        //    try
        //    {
        //        string temp = Base.GData("Populate_More_Fields");
        //        if (temp.Trim().ToLower() != "no")
        //        {
        //            flag = prb.GFillData(temp, true);
        //            if (!flag)
        //            {
        //                flagExit = false;
        //                error = "Cannot populate more fields.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        flag = false;
        //        error = ex.Message;
        //    }
        //}
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_009_01_SaveProblem()
        {
            try
            {
                flag = prb.Save();
                if (!flag) { error = "Error when save problem."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        public void Step_009_02_VerifyMessage()
        {
            try
            {
                flag = prb.VerifyMessageInfo("Please ensure that you add a Due date for each Problem task created from this template."); 
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        #endregion Create a new Problem

        #region Search and open Problem
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        /*Search and open problem*/
        public void Step_010_11_SearchAndOpenProblem()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (problemId == null || problemId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem Id.");
                    addPara.ShowDialog();
                    problemId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                flag = home.LeftMenuItemSelect("Problem", "Open");
                if (flag)
                {
                    prblist.WaitLoading();
                    temp = prblist.Label_Title.Text;
                    flag = temp.Equals("Problems");
                    if (flag)
                    {
                        flag = prblist.SearchAndOpen("Number", problemId, "Number=" + problemId, "Number");
                        if (!flag) error = "Error when search and open problem (id:" + problemId + ")";
                    }
                    else
                    {
                        error = "Invalid title of page. Runtime:(" + temp + "). Expected:(Problems)";
                    }
                }
                else error = "Error when select open problem.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_012_00_Verify_DeleteButton_Disappeared()
        {
            try
            {
                button = prb.Button_Delete;
                flag = button.Existed;
                if (!flag)
                {
                    flag = true;
                    System.Console.WriteLine("***PASS: Expected disappeared.");
                }
                else
                {
                    flag = false;
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
        public void Step_012_01_VerifyGeneratedTasks()
        {
            try
            {
                error = "";
                string temp = Base.GData("TaskTemplates_Name");
                string[] tasks = null;
                if (temp.Contains(";"))
                    tasks = temp.Split(';');
                else
                    tasks = new string[] {temp};

                    foreach (string t in tasks)
                    {
                        tab = prb.GTab("Problem Tasks");
                        flag = tab.Header.Click(true);
                        if (flag)
                        {
                            //flag = tab.RelatedTableSearchAndOpenViewButton("Short description", t, "State=Open|Short description=" + t);
                            flag = tab.RelatedTableSearchAndOpenRecord("Short description", t, "State=Open|Short description=" + t, "Number");
                            if (!flag)
                            {
                                error = error + "Cannot find the task with short description: " + t;
                            }
                            else
                            {
                                prb.WaitLoading();
                                string duedate = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
                                datetime = prb.Datetime_DueDate;
                                flag = datetime.Existed;

                                if (flag)
                                {
                                    flag = datetime.SetText(duedate);
                                    if (!flag)
                                        error = error + "Cannot input due date";
                                    else
                                    {
                                        button = prb.Button_Update;
                                        flag = button.Existed;
                                        if (flag)
                                        {
                                            button.Click(true);
                                            prb.WaitLoading();
                                        }

                                    }
                                }
                                else
                                    error = error + "Cannot get datetime duedate.";
                            }
                        }
                        else
                            error = error + "Cannot click on Problem Task tab.";
                    }
                    if (error != "")
                        flag = false;
                
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        
        [Test]
        /*Populate Assigned to*/
        public void Step_012_02_Populate_ProblemAssignee()
        {
            try
            {                
                string temp = Base.GData("ProblemAssignee");
                lookup = prb.Lookup_AssignedTo;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag) error = "Can not populate Assignee";
                }
                else error = "Cannot get Assignee field.";  
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        #endregion Search and open Problem

        #region Create a Change request

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        /*Click on magnifying glass of Change Request field*/
        public void Step_013_SelectChangeRequestLookup()
        {
            try
            {               
                button = prb.Button_ChangeRequestLookup;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click(true);
                    if (!flag)
                    {
                        error = "Error when select Change Request lookup";
                    }
                }
                else error = " Can not get Change Request lookup field";  
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_014_01_SwitchToPage()
        {
            try
            {
                flag = Base.SwitchToPage(1);
                if (!flag)
                {
                    error = "Cannot switch to page 1.";
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
        /* Click New button*/
        public void Step_014_02_ClickNewButton()
        {
            try
            {
                button = search.Button_New(true);
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (!flag) error = " Error when click New button";
                }
                else error = "Can not get New button";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }

        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        /* Select Change Type*/
        public void Step_015_SelectChangeType()
        {
            try
            {
                string temp = Base.GData("ChangeType");
                flag = chgNoMainFrame.Select_ChangeType(temp);
                if (!flag) error = "Error when select change type.";                
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        /* Poplate Company */
        public void Step_016_01_PopulateCompany()
        {
            try
            {
                //Get change number
                textbox = chgNoMainFrame.Textbox_Number_NoMainFrame;
                flag = textbox.Existed;
                if (flag)
                {
                    flag = textbox.Click();
                    if (flag)
                    {
                        changeId = textbox.Text;                        
                    }
                    else error = "Error when click on textbox Number";
                }
                else error = "Cannot get textbox Number";

                string temp = Base.GData("Company");
                lookup = chgNoMainFrame.Lookup_Company_NoMainFrame;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp, true);
                    if (!flag) error = "Cannot populate Company";
                }
                else error = "Cannot get Company field";
            }
           catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }

        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        /*Populate category*/
        public void Step_016_02_PopualteCategory()
        {
            try
            {
                string temp = Base.GData("Category");
                combobox = chgNoMainFrame.Combobox_Category_NoMainFrame;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag) error = "Cannot populate category";
                }
                else error = "Cannot get combobox category";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        /*Populate ShortDescription*/
        public void Step_016_03_PopulateShortDescrition()
        {
            try
            {
                string temp = Base.GData("ShortDescription");
                textbox = chgNoMainFrame.Textbox_ShortDescription_NoMainFrame;
                flag = textbox.Existed;
                if (flag)
                {
                    flag = textbox.SetText(temp);
                    if (!flag) error = "Cannot populate Short Description";
                }
                else error = "Cannot get textbox Short Description";
            }
            catch(Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        /*Populate Justification */
        public void Step_016_04_PopulateJustification()
        {
            try
            {
                tab = chgNoMainFrame.GTabNoMainFrame("Planning");
                flag = tab.Header.Click(true);
                if (flag)
                {
                    string temp = Base.GData("Justification");
                    textarea = chgNoMainFrame.Textarea_Justification_NoMainFrame;
                    flag = textarea.Existed;
                    if (flag)
                    {
                        flag = textarea.SetText(temp);
                        if (!flag) error = "cannot populate justification";
                    }
                    else error = "Cannot get textarea Justification";
                }
                else error = "Cannot Select Planning tab";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        /*Populate Assignment group*/
        public void Step_016_05_PopulateAssignmentGroup()
        {
            try
            {
                string temp = Base.GData("ChgAssignmentGroup");
                lookup = chgNoMainFrame.Lookup_AssignmentGroup_NoMainFrame;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp,true);
                    if (!flag) error = "Cannot populate Assignment group";
                }
                else error = "cannot get lookup Assignment group";

            }
            catch(Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        /*Populate planned start date planned end date*/
        public void Step_016_06_PopulatePlannedStartDatePlannedEndDate()
        {
            try
            {
                tab = chgNoMainFrame.GTabNoMainFrame("Schedule");
                flag = tab.Header.Click(true);
                if (flag)
                {
                    string startDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string endDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
                    datetime = chgNoMainFrame.Datetime_PlannedStartDate_NoMainFrame;
                    flag = datetime.Existed;
                    if (flag)
                    {
                        flag = datetime.SetText(startDate, true);
                        if (flag)
                        {
                            datetime = chgNoMainFrame.Datetime_PlannedEndDate_NoMainFrame;
                            flag = datetime.Existed;
                            if (flag)
                            {
                                flag = datetime.SetText(endDate, true);
                                if (!flag) error = "Cannot populate Planned End Date";
                            }
                            else error = "Cannot get Planned End Date field ";
                        }
                        else error = "Can not populate Planned Start Date";
                    }
                    else error = "Cannot get planned Start Date";
                }
                else error = "Error when open Schedule tab";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        /*Submit Change*/
        public void Step_017_SubmitChange()
        {
            try
            {
                button = chgNoMainFrame.Button_SubmitNoMainFrame;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (!flag) error = "Cannot submit change request";
                }
                else error = "Can not get Submit button";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        #endregion Create a Change request

        #region Save Problem
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        /*Swich to Problem page*/
        public void Step_018_01_SwitchToPage()
        {
            try
            {
                flag = Base.SwitchToPage(0);
                if (!flag)
                {
                    error = "Cannot switch to page 0.";
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
        /*SaveProblem*/
        public void Step_018_02_SaveProblem()
        {
            try
            {

                flag = prb.Save();
                if (!flag) error = "Can not save problem";
            }
            catch(Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        #endregion Save Problem

        #region Verify Problem is related to change
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        /*Search and Open Change*/
        public void Step_019_020_021_SearchAndOpenChange()
        {
            try
            {
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (changeId == null || changeId== string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input change Id.");
                    addPara.ShowDialog();
                    changeId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                flag = home.LeftMenuItemSelect("Change", "All");
                if (flag)
                {
                    chgList.WaitLoading();
                    temp = chgList.Label_Title.Text;
                    flag = temp.Equals("Change Requests");
                    if (flag)
                    {
                        flag = chgList.SearchAndOpen("Number", changeId, "Number=" + changeId, "Number");
                        if (!flag) error = "Error when Search and Open Change";
                        else chg.WaitLoading();
                    }
                    else { error = "Invalid title of page. Runtime:(" + temp + "). Expected:(Change Requests)"; }
                }
                else error = "Can not display list Change ";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        /*Verify problem tab*/
        public void Step_022_ValidateProblemTab()
        {
            try
            {
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (problemId== null || problemId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem Id.");
                    addPara.ShowDialog();
                    problemId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                tab = chg.GTab("Problems");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = chg.GTab("Problems", true);
                    i++;
                }
                flag = tab.Header.Click(true);
                if (flag)
                {
                    flag = chg.RelatedTableVerifyRow("Problems", "Number=" + problemId, false);
                    if (!flag) error = "There is no related problem record";
                }
                else error = "Error when open Problem tab";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

      
        #endregion Verify Problem is related to change

        #region Resolve Problem

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        /*impersonate Resolver*/
        public void Step_023_ImpersonateProblemAssignee()
        {
            try
            {
                string temp = Base.GData("ProblemAssignee");
                flag = home.ImpersonateUser(temp);
                if (!flag) error = "Cannot impersonate Problem Assignee";
            }
            catch(Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_024_SystemSetting()
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
        /*Search and Resolve Problem*/
        public void Step_025_SearchAndOpenProblem()
        {
            try
            {
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (problemId == null || problemId== string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem Id.");
                    addPara.ShowDialog();
                    problemId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                flag = home.LeftMenuItemSelect("Problem", "Open");
                if (flag)
                {
                    prblist.WaitLoading();
                    temp = prblist.Label_Title.Text;
                    flag = temp.Equals("Problems");
                    if (flag)
                    {
                        flag = prblist.SearchAndOpen("Number", problemId, "Number=" + problemId, "Number");
                        if (!flag) error = "Cannot open Change ";
                        else prb.WaitLoading();
                    }
                    else { error = "Invalid title of page. Runtime:(" + temp + "). Expected:(Problems)"; }
                }
                else { error = "Error when select open problem."; }
            }
            catch(Exception ex)
            {
                flag = false;
                error = ex.Message;
            }

        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_026_01_CloseProblemTasks()
        {
            try
            {
                error = "";
                string temp = Base.GData("TaskTemplates_Name");
                string[] tasks = null;
                if (temp.Contains(";"))
                    tasks = temp.Split(';');
                else
                    tasks = new string[] {temp};

                foreach (string t in tasks)
                {
                    tab = prb.GTab("Problem Tasks");
                    flag = tab.Header.Click(true);
                    if (flag)
                    {
                        //flag = tab.RelatedTableSearchAndOpenViewButton("Short description", t, "State=Open|Short description=" + t);
                        flag = tab.RelatedTableSearchAndOpenRecord("Short description", t, "State=Open|Short description=" + t, "Number");
                        if (!flag)
                        {
                            error = error + "Cannot find the task with short description: " + t;
                        }
                        else
                        {
                            prb.WaitLoading();
                            combobox = prb.Combobox_State;
                            flag = combobox.Existed;
                            if (flag)
                            {
                                flag = combobox.SelectItem("Closed Complete");
                                if (!flag)
                                    error = error + "Cannot select Closed Complete state.";
                                else
                                {
                                    button = prb.Button_Update;
                                    if (button.Existed)
                                    {
                                        button.Click(true);
                                        prb.WaitLoading();
                                    }
                                }
                            }
                            else
                                error = error + "Cannot get combobox state";
                        }
                    }
                    else
                        error = error + "Cannot click on Problem Tasks tab.";
                }

                if (error != "")
                    flag = false;
                
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        /*Change state*/
        public void Step_026_02_PopulateState()
        {
            try
            {                
                combobox = prb.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "Closed/Resolved";
                    flag = combobox.SelectItem(temp);
                    if (!flag) error = "Cannot select state option";
                }
                else error = "Can not get combobox state";
            }
            catch(Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        /*Populate Close Code*/
        public void Step_027_01_PopulateCloseCode()
        {
            try
            {
                string temp = Base.GData("CloseCode");
                combobox = prb.Combobox_CloseCode;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag) error = "Cannot populate Close Code";
                }
                else error = "Cannot get combobox Close Code";
            }
            catch(Exception ex)
            {
                flag = false;
                error = ex.Message;
            }

        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        /*Populate Close Note*/
        public void Step_027_02_PopulateCloseNotes()
        {
            try
            {
                string temp = Base.GData("CloseNotes");
                textarea = prb.Textarea_Closenotes;
                flag = textarea.Existed;
                if (flag)
                {
                    flag = textarea.SetText(temp);
                    if (!flag) error = "Cannot populate Close Notes";
                }
                else error = "Cannot get textarea Close notes";

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        /*Save Problem*/
        public void Step_028_SaveProblem()
        {
            try
            {
                flag = prb.Save();
                if (!flag) error = "Cannot save problem";

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
       
         #endregion Resolve Problem

        //-----------------------------------------------------------------------------------------------------------------------------------
       
        #region Verify Change ticket's State is unchanged
        [Test]
        /*Search and open Change*/
        public void Step_029_01_SearchAndOpenChange()
        {
            try
            {
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (changeId == null || changeId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input Change Id.");
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
                        if (!flag) error = "Cannot open Change ";
                        else chg.WaitLoading();
                    }
                    else error = "Invalid title of page. Runtime:(" + temp + "). Expected:(Change Requests)";
                }
                else error = "Error when select Change/Open on left navigation";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }

        }


        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        /*Verify that problem is resolved/closed*/
        public void Step_029_02_VerifyProblemIsResolved()
        {
            try
            {
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (problemId == null || problemId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem Id.");
                    addPara.ShowDialog();
                    problemId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                tab = chg.GTab("Problems");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = chg.GTab("Problems", true);
                    i++;
                }
                flag = tab.Header.Click();
                if (flag)
                {
                    flag = chg.RelatedTableVerifyRow("Problems", "Number=" + problemId + "|State=Closed/Resolved", false);
                    if (!flag) error = "There is no related problem record";
                }
                else error = "Error when open Problem tab";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        /*Verify Verify Change ticket's State is unchanged*/
        public void Step_030_VerifyStateOfChange()
        {
            try
            {
                combobox = chg.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.VerifyCurrentText("Review");
                    if (!flag) error = "State is not Review";
                }
                else error = "Cannot get state field";

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        #endregion Verify Change ticket's State is unchanged

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_031_Logout()
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


