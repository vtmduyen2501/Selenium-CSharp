using NUnit.Framework;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

namespace Problem
{
    class PROB_e2e_proactive_task_close_3
    {
        #region Define default variables for test case (No need to update)
        //***********************************************************************************************************************************
        public bool flagC;
        public bool flag, flagExit, flagW;
        string caseName, temp, error;
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

            System.Console.WriteLine("Finished - Problem Id: " + ProblemId);
            System.Console.WriteLine("Problem Task 01 Id: " + ProTask01);
            System.Console.WriteLine("Problem Task 02 Id: " + ProTask02);
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
       
        Auto.ocheckbox checkbox;
        Auto.otextarea textarea;
        Auto.oelement ele;
        Auto.odatetime datetime;
        Auto.obutton button;
        //------------------------------------------------------------------
        Auto.Login login = null;
        Auto.Home home = null;
        Auto.Problem prb = null;
        Auto.ProblemList prblist = null;
        Auto.Member member = null;
        Auto.TaskList tsklist = null;
        Auto.EmailList emailList = null;
        Auto.GlobalSearch globalSearch = null;
        //------------------------------------------------------------------
        string ProblemId, ProTask01, ProTask02;

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
                member = new Auto.Member(Base);
                prb = new Auto.Problem(Base, "Problem");
                prblist = new Auto.ProblemList(Base, "Problem List");
                tsklist = new Auto.TaskList(Base, "Task list");
                emailList = new Auto.EmailList(Base, "Email List");
                globalSearch = new Auto.GlobalSearch(Base);
                //------------------------------------------------------------------
                ProblemId = string.Empty;
                ProTask01 = string.Empty;
                ProTask02 = string.Empty;
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
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_003_ImpersonateUser_ProblemManager()
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
                flag = home.SystemSetting();
                if (!flag) { error = "Error when config system."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_006_OpenNewProblem()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Problem", "Create New");
                if (flag)
                {
                    prb.WaitLoading();
                }
                else
                {
                    error = "Cannot select create new problem";
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
        public void Step_007_PopulateCompany()
        {
            try
            {
                textbox = prb.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    flag = textbox.Click();
                    if (flag)
                    {
                        string temp = textbox.Text;
                        flag = Regex.IsMatch(temp, "PRB*");
                        if (!flag) error = "Invalid format of Problem number.";
                        else { ProblemId = temp; Console.WriteLine("-*-[STORE]: Problem Id:(" + ProblemId + ")"); } 
                    }
                    else error = "Error when click on textbox number.";
                }
                else { error = "Cannot get textbox number."; }
                
                //-- Input company
                lookup = prb.Lookup_Company;
                flag = lookup.Existed;
                if (flag)
                {
                    string company = Base.GData("Company");
                    flag = lookup.Select(company);
                    if (!flag) { error = "Cannot populate company value."; }
                }
                else
                { error = "Cannot get company field."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_008_PopulateProblemStatement()
        {
            try
            {
                textbox = prb.Textbox_ProblemStatement;
                temp = Base.GData("ProStatement") + " - " + ProblemId;
                flag = textbox.Existed;
                if (flag)
                {
                    flag = textbox.SetText(temp);
                    if (!flag) { error = "Cannot populate Problem statement."; }
                }
                else { error = "Cannot get textbox Problem statement."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------
            
        [Test]
        public void Step_009_AddACI()
        {
            try
            {
                temp = Base.GData("ProCI01");
                lookup = prb.Lookup_ConfigurationItem;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag) { error = "Cannot populate CI."; }

                }
                else { error = "Cannot get lookup Configuration Item."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_010_01_PopulateImpact()
        {
            try
            {
                temp = Base.GData("ProImpact");
                combobox = prb.Combobox_Impact;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag) error = "Cannot update impact.";

                }
                else
                {
                    error = "Cannot found impact combobox.";
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
        public void Step_010_02_UpdatePriority()
        {
            try
            {
                temp = Base.GData("ProPriority");
                combobox = prb.Combobox_Priority;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag) error = "Cannot update priority."; 
                   
                }
                else
                {
                    error = "Cannot found Priority combobox.";
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
        public void Step_011_PopulateCategoryAndSubcategory()
        {
            try
            {
                temp = Base.GData("ProCat");
                combobox = prb.Combobox_Category;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        prb.WaitLoading();
                        string sub_cat = Base.GData("ProSubCat");
                        combobox = prb.Combobox_SubCategory;
                        flag = combobox.Existed;
                        if (flag)
                        {
                            flag = prb.VerifyHaveItemInComboboxList("subcategory", sub_cat);
                            if (flag)
                            {
                                flag = combobox.SelectItem(sub_cat);
                                if (!flag) error = "Cannot update Problem Subcategory.";
                            }
                        }
                        else
                        {error = "Cannot found Subcategory combobox.";}
                    }
                    else
                    {
                        error = "Cannot update Problem Category.";
                    }
                   
                }
                else
                {error = "Cannot found Category combobox.";}
                
                    
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_012_PopulateAssignmentGroup()
        {
            try
            {
                temp = Base.GData("ProAssignmentGroup");
                lookup = prb.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag) { error = "Cannot populate Assignment Group."; }

                }
                else { error = "Cannot get lookup Assignment Group."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_013_01_PopulateDescription()
        {
            try
            {                
                textarea = prb.Textarea_Description;
                flag = textarea.Existed;
                if(flag)
                {
                    temp = Base.GData("ProDescription");
                    flag = textarea.SetText(temp);
                    if (!flag)
                    {error = "Cannot populate problem description."; }
                }
                else
                {error = "Cannot found problem description."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_013_02_Populate_More_Fields_If_Need()
        {
            try
            {
                temp = Base.GData("Populate_More_Fields");
                if (temp.Trim().ToLower() != "no") 
                {
                    flag = prb.GFillData(temp, true);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Cannot populate more fields.";
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

        [Test]
        public void Step_014_SaveProblem()
        {
            try
            {
                flag = prb.Save();
                if (flag)
                {
                    prb.WaitLoading();
                }
                else
                {error = "Cannot save problem.";}
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_015_VerifySLATasks()
        {
            try
            {
                temp = Base.GData("SLAs");
                if (temp != string.Empty && temp.ToLower() != "no")
                {
                    string[] tempSLA = null;
                    if (temp.Contains(";"))
                    {
                        tempSLA = temp.Split(';');
                    }
                    else tempSLA = new string[]{temp};
                    //int iRow = -1;
                    string tempStage = string.Empty;
                    string tempTime = string.Empty;
                    for (int i = 0; i < tempSLA.Length; i++)
                    {
                        tempStage = Base.GData("StageSLA" + (i + 1).ToString() + "_1");
                        tempTime = Base.GData("DurationSLA" + (i + 1).ToString());
                        string condition = "SLA=" + tempSLA[i] + "|Stage=" + tempStage + "|Business time left=" + tempTime;
                        flag = prb.RelatedTableVerifyRow("Task SLAs", condition);
                        if (!flag)
                        {
                                flagExit = false;
                                error = string.Format("Cannot find SLA = {0} - Stage = {1} - Business Time Left = {2}", tempSLA[i], tempStage, tempTime);
                            
                        }
                    }       
                }
                else
                    System.Console.WriteLine("No SLAs tasks to check.");
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
      
        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_016_VerifyActivitySection()
        {
            try
            {
                temp = Base.GData("Activity_16");
                flag = prb.VerifyActivity(temp);
               
                if (!flag)
                {
                    error = "Invalid activity note 16. Expected:(" + temp + ")";
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
        public void Step_017_019_AddAWorkNotes()
        {
            try
            {
                temp = Base.GData("ProworkNotes") + " Step 17";
                textarea = prb.Textarea_Worknotes_Update;
                flag = textarea.Existed;
                if (flag)
                {
                    flag = textarea.SetText(temp);
                    if (flag)
                    {
                        flag = prb.Save();
                        if (flag)
                        {
                            prb.WaitLoading();
                        }
                        else
                            error = "Cannot save problem";
                    }
                    else
                        error = "Cannot set Work Notes";
                }
                else
                {
                    flag = false;
                    error = "Not found Work Notes element.";
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
        public void Step_018_AttachAFile()
        {
            try
            {
                string problemAttachmentFile = "problemAttachment.txt";

                flag = prb.AttachmentFile(problemAttachmentFile);
                if (!flag)
                {
                    error = "Error when attach file (" + problemAttachmentFile + ")";
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

        [Test]
        public void Step_020_AssignTicket()
        {
            try
            {
                lookup = prb.Lookup_AssignedTo;
                flag = lookup.Existed;
                if(flag)
                {
                    temp = Base.GData("ProAssignee1");
                    flag = lookup.Select(temp);
                    prb.WaitLoading();
                    if(!flag)
                    {error = "Cannot populate assigned to.";}
                }  
                else
                    error = "Cannot find element Assigned To.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_021_SaveProblem()
        {
            try
            {
                flag = prb.Save();
                if (flag)
                {prb.WaitLoading();}
                else
                {
                    error = "Cannot save problem.";
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
        public void Step_022_VerifyActivitySection()
        {
            try
            {
                temp = Base.GData("Activity_22");
                flag = prb.VerifyActivity(temp);
                if (!flag)
                {
                    error = "Not found activity notes or activity note is invalid.";
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
        public void Step_023_AddAWorkNotes()
        {
            try
            {
                temp = Base.GData("ProworkNotes") + " Step 23";
                textarea = prb.Textarea_Worknotes_Update;
                flag = textarea.Existed;
                if (flag)
                {
                    flag = textarea.SetText(temp);
                    if (!flag)
                    {
                        error = "Cannot set Work Notes";
                    }
                }
                else
                {error = "Not found Work Notes element.";}
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_024_SaveProblem()
        {
            try
            {
                flag = prb.Save();
                if (flag)
                {
                    prb.WaitLoading();
                }
                else
                {error = "Cannot save problem.";}
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_025_ReassignTicket()
        {
            try
            {
                lookup = prb.Lookup_AssignedTo;
                flag = lookup.Existed;
                if (flag)
                {
                    temp = Base.GData("ProAssignee2");
                    flag = lookup.Select(temp);
                    prb.WaitLoading();
                    if (!flag)
                    { error = "Cannot populate assigned to."; }
                }
                else
                    error = "Cannot find element Assigned To.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_026_UpdateProblemDescription()
        {
            try
            {
                temp = Base.GData("ProDescription") + " Update Step 26";
                textarea = prb.Textarea_Description;
                flag = textarea.Existed;
                if (flag)
                {
                    flag = textarea.SetText(temp);
                    if (!flag)
                    { error = "Cannot update problem description."; }
                }
                else
                { error = "Cannot found problem description."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_027_SaveProblem()
        {
            try
            {
                flag = prb.Save();
                if (flag)
                {
                    prb.WaitLoading();
                }
                else
                {error = "Cannot save problem.";}
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_028_VerifyActivitySection()
        {
            try
            {
                temp = Base.GData("Activity_28");
                flag = prb.VerifyActivity(temp);
                if (!flag)
                {
                    error = "Not found activity notes or activity note is invalid.";
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
        public void Step_029_ImpersonateProblemAssignee()
        {
            try
            {
                temp = Base.GData("UserFullName");
                flag = home.ImpersonateUser(Base.GData("ProAssignee2"), true, temp, false);
                if (!flag)
                {
                    error = "Cannot impersonate Problem Assignee 2";
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
        public void Step_030_SystemSetting()
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

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_031_01_SearchAOpen_Problem()
        {
            try
            {
                temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && ProblemId == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem Id.");
                    addPara.ShowDialog();
                    ProblemId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //---------------------------------------------------------------
                temp = "Number=" + ProblemId;
                flag = home.LeftMenuItemSelect("Problem", "Open");
                if (flag)
                {
                    flag = prblist.SearchAndOpen("Number", ProblemId, temp, "Number");
                    if(!flag)error = "Error when search and open problem (Id:" + ProblemId + ")";
                    else
                    {
                        prb.WaitLoading();
                    }
                }
                else { error = "Cannot open Problem list"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_031_02_ValidateDeleteButton()
        {
            try
            {
                button = prb.Button_Delete;
                flag = button.Existed;

                if (flag)
                {
                    error = "Delete button should not be visible.";
                    flag = false;
                    flagExit = false;
                }
                else
                {
                    System.Console.WriteLine("Delete button does not exist in form");
                    flag = true;
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
        public void Step_032_SelectAffectedCIsTab()
        {
            try
            {
                tab = prb.GTab("Affected CIs");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = prb.GTab("Affected CIs", true);
                    i++;
                }
                flag = tab.Header.Click();
                if (!flag)
                { error = "Cannot select Affected CIs tab."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_033_SelectEdit()
        {
            try
            {
                tab = prb.GTab("Affected CIs");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = prb.GTab("Affected CIs", true);
                    i++;
                }
                flag = tab.ClickEdit();
                if (flag)
                {
                    member.WaitLoading();
                    ele = member.Ele_Filter();
                    flag = ele.Existed;
                    int count = 0;
                    while(!flag && count < 5)
                    {
                        Thread.Sleep(1000);
                        ele = member.Ele_Filter();
                        count++;
                        System.Console.WriteLine("");
                    }
                    flag = ele.Existed;
                    if(!flag)
                    { error = "Wailoading failed"; }
                }
                else
                {error = "Cannot find Edit button.";}
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_034_SearchAndSelect2ndCI()
        {
            try
            {
                temp = Base.GData("ProCI02");
                flag = member.AddMembers("Affected CIs", temp);
                if(!flag)
                {
                    flagExit = false;
                    error = "Cannot add 2nd CI.";
                }
                else {prb.WaitLoading();}
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

       
        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_035_CheckCIList()
        {
            try
            {
                temp = "Configuration Item=" + Base.GData("ProCI01");
                flag = prb.RelatedTableVerifyRow("Affected CIs",temp);
                if (flag)
                {
                        string condition = "Configuration Item=" + Base.GData("ProCI02");
                        flag = prb.RelatedTableVerifyRow("Affected CIs",condition);
                        if (!flag)
                        {
                            error = "Not found CI02 in Affected CIs table.";
                        }
                }
                else
                {
                   error = "Not found CI01 in Affect CIs table.";
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
        public void Step_036_SelectAffectedCIsTab()
        {
            try
            {
                tab = prb.GTab("Affected CIs");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = prb.GTab("Affected CIs", true);
                    i++;
                }
                flag = tab.Header.Click();
                if (!flag)
                { error = "Cannot select Affected CIs tab."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_037_SelectEdit()
        {
            try
            {
                tab = prb.GTab("Affected CIs");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = prb.GTab("Affected CIs", true);
                    i++;
                }
                flag = tab.ClickEdit();
                if (flag)
                {
                    member.WaitLoading();
                }
                else
                { error = "Cannot find Edit button."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_038_Remove2ndCI()
        {
            try
            {
                temp = Base.GData("ProCI02");
                flag = member.RemoveMembers("Affected CIs", temp);
                if(!flag)
                {
                    flagExit = false;
                    error = "Cannot remove 2nd CI.";
                }
                else {prb.WaitLoading();}
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_039_CheckCIList()
        {
            try
            {
                temp = "Configuration Item=" + Base.GData("ProCI01");
                flag = prb.RelatedTableVerifyRow("Affected CIs",temp);
                if (flag)
                {
                     string condition = "Configuration Item=" + Base.GData("ProCI02");
                     flag = prb.RelatedTableVerifyRow("Affected CIs",condition);
                     if (flag)
                     {
                         flag = false;
                         error = "Found CI02 in Affected CIs table.";
                     }
                     else 
                     {
                         flag = true;
                         System.Console.WriteLine("-----OK----- CI02 has been removed!");
                     }

                }
                else
                {
                   error = "Not found CI01 in Affect CIs table.";
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
        public void Step_040_Select_ImpactedServices_Tab()
        {
            try
            {
                tab = prb.GTab("Impacted Services");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = prb.GTab("Impacted Services", true);
                    i++;
                }
                flag = tab.Header.Click();
                if (!flag)
                { error = "Cannot select Impacted Services tab."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_041_Click_Edit()
        {
            try
            {
                if (tab != null) 
                {
                    flag = tab.ClickEdit();
                    if (flag)
                    {
                        member.WaitLoading();
                    }
                    else
                    { error = "Cannot find Edit button."; }
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
        public void Step_042_048_SearchAndSelect_ImpactedServices()
        {
            try
            {
                temp = Base.GData("ProBS01") + ";" + Base.GData("ProBS02");
                flag = member.AddMembers("Impacted Services", temp);
                if (!flag)
                {
                    flagExit = false;
                    error = "Cannot add Impacted Services.";
                }
                else { prb.WaitLoading(); }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_049_Select_ImpactedServices_Tab()
        {
            try
            {
                tab = prb.GTab("Impacted Services");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = prb.GTab("Impacted Services", true);
                    i++;
                }
                flag = tab.Header.Click();
                if (!flag)
                { error = "Cannot select Impacted Services tab."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_050_Select_Edit()
        {
            try
            {
                flag = tab.ClickEdit();
                if (flag)
                {
                    member.WaitLoading();
                }
                else
                { error = "Cannot find Edit button."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_051_Remove2nd_ImpactedService()
        {
            try
            {
                temp = Base.GData("ProBS02");
                flag = member.RemoveMembers("Impacted Services", temp);
                if (!flag)
                {
                    flagExit = false;
                    error = "Cannot remove 2nd Impacted Service.";
                }
                else { prb.WaitLoading(); }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_052_Check_ImpactedServices_List()
        {
            try
            {
                temp = "Business Service=" + Base.GData("ProBS01");
                flag = prb.RelatedTableVerifyRow("Impacted Services", temp);
                if (flag)
                {
                    string condition = "Business Service=" + Base.GData("ProBS02");
                    flag = prb.RelatedTableVerifyRow("Impacted Services", condition);
                    if (flag)
                    {
                        flag = false;
                        error = "Found CI02 in Impacted Services table.";
                    }
                    else
                    {
                        flag = true;
                        flag = prb.Reload();
                        System.Console.WriteLine("-----OK----- BS02 has been removed!");
                    }

                }
                else
                {
                    error = "Not found BS01 in Impacted Services table.";
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
        public void Step_053_054_AddThe1stProblemTask()
        {
            try
            {
                tab = prb.GTab("Problem Tasks");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = prb.GTab("Problem Tasks", true);
                    i++;
                }
                if (tab != null)
                {
                   flag = tab.Header.Click();
                   if (flag)
                   {
                       flag = tab.ClickNew();
                       if (flag)
                       {
                        prb.WaitLoading();
                       }
                       else {error = "Cannot click New button";}
                   }
                   else {error = "Cannot select Problem Tasks tab";}
                }
                else {error = "Cannot get Problem Tasks tab";}
              
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_055_PopulateAssignmentGroup()
        {
            try
            {
                temp = Base.GData("ProTask_AssignmentGroup");
                lookup = prb.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag)
                    { error = "Cannot populate Assignment Group"; }
                }
                else
                {error = "Cannot get lookup Assignment Group";}
                
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_056_PopulateShortDescription()
        {
            try
            {
                temp = Base.GData("ProTask_1_ShortDes");
                textbox = prb.Textbox_ShortDescription;
                flag = textbox.Existed;
                if (flag)
                {
                    flag = textbox.SetText(temp);
                    if (!flag)
                    { error = "Cannot input problem task Short Description"; }
                }
                else
                {error = "Cannot get textbox short description.";}
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }



        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_057_PopulateDueDate()
        {
            try
            {
                temp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                datetime = prb.Datetime_DueDate;
                flag = datetime.Existed;
                if (flag)
                {
                    flag = datetime.SetText(temp, true);
                    if (!flag)
                    { error = "Cannot populate Due date"; }
                }
                else { error = "Cannot get datetime field Due date"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_058_SaveProblemTask()
        {
            try
            {
                flag = prb.Save();
                if (flag)
                {
                    textbox = prb.Textbox_Number;
                    flag = textbox.Existed;
                    if (flag)
                    {
                        ProTask01 = textbox.Text;
                    }
                    else { error = "Cannot get Problem Task 01 ID"; }
                }
                else
                {error = "Cannot save problem task.";}
            }          
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_059_PopulateTaskWorkNotes()
        {
            try
            {
                temp = Base.GData("ProTask_1_WorkNotes") + " - 01";
                textarea = prb.Textarea_TaskWorkNotes;
                flag = textarea.Existed;
                if (flag)
                {
                    flag = textarea.SetText(temp);
                    if (!flag)
                        error = "Cannot input problem task work notes.";  
                }
                else { error = "Cannot get textarea work notes."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_060_1_SaveProblemTask_And_Open_Problem()
        {
            try
            {
                flag = prb.Save();
                if (!flag)
                {error = "Cannot save Problem Task";}
                else
                {
                    prb.WaitLoading();

                    temp = Base.GData("Debug");
                    if (temp == "yes" && ProblemId == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input problem Id.");
                        addPara.ShowDialog();
                        ProblemId = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    flag = globalSearch.GlobalSearchItem(ProblemId, true);
                    prb.WaitLoading();
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
        public void Step_060_2_CheckProblemTaskList()
        {
            try
            {
                temp = Base.GData("Debug");
                if (temp == "yes" && ProTask01 == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem task Id 1.");
                    addPara.ShowDialog();
                    ProTask01 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //------------------------------------------------------------------------------------------
                tab = prb.GTab("Problem Tasks");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = prb.GTab("Problem Tasks", true);
                    i++;
                }
                flag = tab.Header.Click();
                if (flag)
                {
                    temp = "Number=" + ProTask01 + "|Priority=3 - Medium";
                    temp = temp + "|State=Open" + "|Short description=" + Base.GData("ProTask_1_ShortDes");
                    temp = temp + "|Assignment group=" + Base.GData("ProTask_AssignmentGroup");
                    flag = tab.RelatedTableVerifyRow(temp);
                    if(!flag)
                    {
                        error = "Not found problem task.";
                    }
                }
                else
                {error = "Cannot open problem tasks tab.";}
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------
        [Test]
        public void Step_061_062_AddThe2ndProblemTask()
        {
            try
            {
                tab = prb.GTab("Problem Tasks");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = prb.GTab("Problem Tasks", true);
                    i++;
                }
                if (tab != null)
                {
                    flag = tab.Header.Click();
                    if (flag)
                    {
                        flag = tab.ClickNew();
                        if (flag)
                        {
                            prb.WaitLoading();
                        }
                        else { error = "Cannot click New button"; }
                    }
                    else { error = "Cannot select Problem Tasks tab"; }
                }
                else { error = "Cannot get Problem Tasks tab"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_063_PopulateAssignmentGroup()
        {
            try
            {
                temp = Base.GData("ProTask_AssignmentGroup");
                lookup = prb.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag)
                    { error = "Cannot populate Assignment Group"; }
                }
                else
                { error = "Cannot get lookup Assignment Group"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_064_PopulateShortDescription()
        {
            try
            {
                temp = Base.GData("ProTask_2_ShortDes");
                textbox = prb.Textbox_ShortDescription;
                flag = textbox.Existed;
                if (flag)
                {
                    flag = textbox.SetText(temp);
                    if (!flag)
                    { error = "Cannot input problem task Short Description"; }
                }
                else
                { error = "Cannot get textbox short description."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_065_PopulateDueDate()
        {
            try
            {
                temp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                datetime = prb.Datetime_DueDate;
                flag = datetime.Existed;
                if (flag)
                {
                    flag = datetime.SetText(temp, true);
                    if (!flag)
                    { error = "Cannot populate Due date"; }
                }
                else { error = "Cannot get datetime field Due date"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_066_SaveProblemTask()
        {
            try
            {

                flag = prb.Save();
                if (flag)
                {
                    textbox = prb.Textbox_Number;
                    flag = textbox.Existed;
                    if (flag)
                    {
                        ProTask02 = textbox.Text;
                    }
                    else { error = "Cannot get Problem Task 02 ID"; }
                }
                else
                { error = "Cannot save problem task."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_067_PopulateAssignedTo()
        {
            try
            {
                temp = Base.GData("TaskResolver1");
                lookup = prb.Lookup_AssignedTo;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag)
                    { error = "Cannot input Assigned To."; }
                }
                else { error = "Cannot get lookup Assigned To"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_068_SaveProblemTask()
        {
            try
            {
                flag = prb.Save();
                if (!flag)
                { error = "Cannot save problem task."; }
                else prb.WaitLoading();
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_069_PopulateTaskWorkNotes()
        {
            try
            {
                temp = Base.GData("ProTask_2_WorkNotes") + " - 01";
                textarea = prb.Textarea_TaskWorkNotes;
                flag = textarea.Existed;
                if (flag)
                {
                    flag = textarea.SetText(temp);
                    if (!flag)
                        error = "Cannot input problem task work notes.";
                }
                else { error = "Cannot get textarea work notes."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_070_1_SaveProblemTask_And_Open_Problem()
        {
            try
            {
                flag = prb.Save();
                if (flag)
                {
                    prb.WaitLoading();

                    temp = Base.GData("Debug");
                    if (temp == "yes" && ProblemId == string.Empty)
                    {
                        Auto.AddParameter addPara = new Auto.AddParameter("Please input problem Id.");
                        addPara.ShowDialog();
                        ProblemId = addPara.value;
                        addPara.Close();
                        addPara = null;
                    }

                    flag = globalSearch.GlobalSearchItem(ProblemId, true);
                    prb.WaitLoading();
                }
                if (!flag) { error = "Cannot update problem Task."; }
         
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_070_2_CheckProblemTaskList()
        {
            try
            {
                temp = Base.GData("Debug");
                if (temp == "yes" && ProTask01 == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem task Id 1.");
                    addPara.ShowDialog();
                    ProTask01 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }

                if (temp == "yes" && ProTask02 == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem task Id 2.");
                    addPara.ShowDialog();
                    ProTask02 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //------------------------------------------------------------------------------------------
                tab = prb.GTab("Problem Tasks");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = prb.GTab("Problem Tasks", true);
                    i++;
                }
                flag = tab.Header.Click();
                if (flag)
                {
                    bool flagCheck = false;
                    temp = "Number=" + ProTask01 + "|Priority=3 - Medium";
                    temp = temp + "|State=Open" + "|Short description=" + Base.GData("ProTask_1_ShortDes");
                    temp = temp + "|Assignment group=" + Base.GData("ProTask_AssignmentGroup");
                    flag = tab.RelatedTableVerifyRow(temp);
                    if (flag)
                    {
                        flagCheck = true;
                        System.Console.WriteLine("-----OK----- Found Problem Task 01 "+ ProTask01);
                    }
                    else{error = "Not found problem task 01."; }
                    if (flagCheck)
                    {
                        temp = temp.Replace(ProTask01, ProTask02);
                        temp = temp.Replace(Base.GData("ProTask_1_ShortDes"), Base.GData("ProTask_2_ShortDes"));
                        temp += "|Assigned to=" + Base.GData("TaskResolver1");
                        flag = tab.RelatedTableVerifyRow(temp);
                        if (!flag)
                        { error += "Not found problem task 02."; }
                    }
                }
                else
                { error = "Cannot open problem tasks tab."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_071_ImpersonateUser_TaskResolver1()
        {
            try
            {
                temp = Base.GData("UserFullName");
                flag = home.ImpersonateUser(Base.GData("TaskResolver1"), true, temp, false);
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_072_SystemSetting()
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

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_073_1_OpenMyGroupsWork()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Service Desk", "My Groups Work");
                if (flag)
                {
                    tsklist.WaitLoading();
                    ele = tsklist.Label_Title;
                    flag = ele.Existed;
                    if (!flag || ele.Text != "Tasks")
                    {
                        flag = false;
                        error = "Invalid tasks title or cannot open task form.";
                    }
                }
                else{error = "Cannot select My Groups Work.";}
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_073_2_SearchForTask1()
        {
            try
            {
                temp = Base.GData("Debug");
                if (temp == "yes" && ProTask01 == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem task Id 1.");
                    addPara.ShowDialog();
                    ProTask01 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //---------------------------------------------------
                temp = "Number=" + ProTask01 + "|Assignment group=" + Base.GData("ProTask_AssignmentGroup");
                flag = tsklist.SearchAndVerify("Number", ProTask01, temp);
                if (!flag)
                {error = "Cannot find problem task 1.";}   
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_073_3_SearchForTask2_NotFound()
        {
            try
            {
                temp = Base.GData("Debug");
                if (temp == "yes" && ProTask02 == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem task Id 2.");
                    addPara.ShowDialog();
                    ProTask02 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //---------------------------------------------------
                temp = "Number=" + ProTask02 + "|Assignment group=" + Base.GData("ProTask_AssignmentGroup");
                flag = tsklist.SearchAndVerify("Number", ProTask02, temp, true);
                if (!flag)
                {
                    flagExit = false;
                    error = "Found problem task 2. Expected not found."; 
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
        public void Step_074_1_OpenMyWork()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Service Desk", "My Work");
                if (flag == true)
                {
                   tsklist.WaitLoading();
                    ele = tsklist.Label_Title;
                    flag = ele.Existed;
                    if (!flag || ele.Text != "Tasks")
                    {
                        flag = false;
                        error = "Invalid tasks title or cannot open task form.";
                    }
                }
                else{error = "Cannot select My Groups Work.";}
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_074_2_SearchForTask1_NotFound()
        {
            try
            {
                  temp = Base.GData("Debug");
                if (temp == "yes" && ProTask01 == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem task Id 1.");
                    addPara.ShowDialog();
                    ProTask01 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //---------------------------------------------------
                temp = "Number=" + ProTask01 + "|Assignment group=" + Base.GData("ProTask_AssignmentGroup");
                flag = tsklist.SearchAndVerify("Number", ProTask01, temp, true);
                if (!flag)
                {
                    flagExit = false;
                    error = "Found problem task 1. Expected NOT Found.";
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
        public void Step_075_SearchAOpenTask2()
        {
            try
            {
                temp = Base.GData("Debug");
                if (temp == "yes" && ProTask02 == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem task Id 2.");
                    addPara.ShowDialog();
                    ProTask02 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //---------------------------------------------------
                temp = "Number=" + ProTask02 + "|Assignment group=" + Base.GData("ProTask_AssignmentGroup");
                flag = tsklist.SearchAndOpen("Number", ProTask02, temp, "Number");
                if (!flag)
                { error = "Cannot find problem task 2."; } 
                else
                { prb.WaitLoading(); }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_076_PopulateTaskCI()
        {
            try
            {
                temp = Base.GData("TaskCI");
                lookup = prb.Lookup_ConfigurationItem;
                flag = lookup.Existed;
                if(flag)
                {
                    flag = lookup.Select(temp);
                    if(!flag)
                    { error = "Cannot select Configuration Item for task."; }
                }
                else
                { error = "Cannot get lookup Configuration Item."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_077_AddTaskWorkNotes()
        {
            try
            {
                temp = Base.GData("ProTask_2_WorkNotes") + " - 02";
                textarea = prb.Textarea_TaskWorkNotes;
                flag = textarea.Existed;
                if (flag)
                {
                    flag = textarea.SetText(temp);
                    if (!flag)
                        error = "Cannot input problem task work notes.";
                }
                else { error = "Cannot get textarea work notes."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_078_ReassignTask02()
        {
            try
            {
                temp = Base.GData("TaskResolver2");
                lookup = prb.Lookup_AssignedTo;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag)
                    {error = "Cannot reassign task to Resolver 2."; }
                }
                else{error = "Cannot get lookup Assigned to";}
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_079_SaveProblemTask()
        {
            try
            {
                flag = prb.Save();
                if (flag)
                { prb.WaitLoading(); }
                else
                { error = "Cannot save Problem Task"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_080_VerifyActivitySection()
        {
            try
            {
                temp = Base.GData("TaskActivity_80_1");
                if (prb.VerifyActivity(temp))
                {
                    temp = Base.GData("TaskActivity_80_2");
                    if (!prb.VerifyActivity(temp))
                    {
                        flag = false;
                        error = "Not found or activity notes for task changes 80_02 is invalid.";
                    }
                }
                else
                {
                    flag = false;
                    error = "Not found or activity notes for work notes 80_01 is invalid.";
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
        public void Step_081_ImpersonateUser_TaskResolver2()
        {
            try
            {
                temp = Base.GData("UserFullName");
                flag = home.ImpersonateUser(Base.GData("TaskResolver2"),true,temp, false);
                if (!flag)
                { error = "Cannot impersonate Task Resolver 2"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_082_SystemSetting()
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

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_083_1_OpenMyWork()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Service Desk", "My Work");
                if (flag == true)
                {
                    tsklist.WaitLoading();
                    ele = tsklist.Label_Title;
                    flag = ele.Existed;
                    if (!flag || ele.Text != "Tasks")
                    {
                        flag = false;
                        error = "Invalid tasks title or cannot open task form.";
                    }
                }
                else { error = "Cannot select My Groups Work."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }



        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_083_2_084_SearchAOpenTask2()
        {
            try
            {
                temp = Base.GData("Debug");
                if (temp == "yes" && ProTask02 == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem task Id 2.");
                    addPara.ShowDialog();
                    ProTask02 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //---------------------------------------------------
                temp = "Number=" + ProTask02 + "|Assignment group=" + Base.GData("ProTask_AssignmentGroup");
                flag = tsklist.SearchAndOpen("Number", ProTask02, temp, "Number");
                if (!flag)
                { error = "Cannot find problem task 2."; }
                else
                { prb.WaitLoading(); }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_085_UpdateTask2State()
        {
            try
            {
                combobox = prb.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    temp = "Work in Progress";
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                    {error = "Cannot select state <" + temp + ">"; }
                }
                else
                {error = "Cannot get combobox state.";}
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_086_SaveProblemTask()
        {
            try
            {
                flag = prb.Save();
                if (!flag)
                { error = "Cannot save problem task."; }
                else
                { prb.WaitLoading(); }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_087_CloseTask02()
        {
            try
            {
                combobox = prb.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    temp = "Closed Complete";
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                    {error = "Cannot select state <" + temp + ">"; }
                }
                else
                {error = "Cannot get combobox state.";}
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_088_AddTaskWorkNotes()
        {
            try
            {
                temp = Base.GData("ProTask_2_WorkNotes") + " - 03";
                textarea = prb.Textarea_TaskWorkNotes;
                flag = textarea.Existed;
                if (flag)
                {
                    flag = textarea.SetText(temp);
                    if (!flag)
                        error = "Cannot input problem task work notes.";
                }
                else { error = "Cannot get textarea work notes."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_089_SaveProblemTask()
        {
            try
            {
                flag = prb.Update();
                if (flag)
                {tsklist.WaitLoading();}
                else
                {
                    error = "Cannot update Problem Task.";
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
        public void Step_090_1_OpenMyGroupsWork()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Service Desk", "My Groups Work");
                if (flag)
                {
                    tsklist.WaitLoading();
                    ele = tsklist.Label_Title;
                    flag = ele.Existed;
                    if (!flag || ele.Text != "Tasks")
                    {
                        flag = false;
                        error = "Invalid tasks title or cannot open task form.";
                    }
                }
                else { error = "Cannot select My Groups Work."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_090_2_091_SearchForTask1()
        {
            try
            {
                temp = Base.GData("Debug");
                if (temp == "yes" && ProTask01 == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem task Id 1.");
                    addPara.ShowDialog();
                    ProTask01 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //---------------------------------------------------
                temp = "Number=" + ProTask01 + "|Assignment group=" + Base.GData("ProTask_AssignmentGroup");
                flag = tsklist.SearchAndOpen("Number", ProTask01, temp, "Number");
                if (!flag)
                { error = "Cannot find problem task 1."; }
                else
                { prb.WaitLoading(); }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_092_CloseSkippedTask01()
        {
            try
            {
                combobox = prb.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    temp = "Closed Skipped";
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                    { error = "Cannot select state <" + temp + ">"; }
                }
                else
                { error = "Cannot get combobox state."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_093_AddTaskWorkNotes()
        {
            try
            {
                temp = Base.GData("ProTask_1_WorkNotes") + " - 02";
                textarea = prb.Textarea_TaskWorkNotes;
                flag = textarea.Existed;
                if (flag)
                {
                    flag = textarea.SetText(temp);
                    if (!flag)
                        error = "Cannot input problem task work notes.";
                }
                else { error = "Cannot get textarea work notes."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_094_SaveProblemTask()
        {
            try
            {
                flag = prb.Update();
                if (flag)
                { tsklist.WaitLoading(); }
                else
                {
                    error = "Cannot update Problem Task.";
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
        public void Step_095_ImpersonateProblemAssignee()
        {
            try
            {
                temp = Base.GData("UserFullName");
                flag = home.ImpersonateUser(Base.GData("ProAssignee2"), true, temp, false);
                if (!flag)
                { error = "Cannot impersonate Problem Assignee 2"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_096_SystemSetting()
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
        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_097_SearchProblem()
        {
            try
            {
               temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && ProblemId == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem Id.");
                    addPara.ShowDialog();
                    ProblemId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //---------------------------------------------------------------
                temp = "Number=" + ProblemId;
                flag = home.LeftMenuItemSelect("Problem", "Open");
                if (flag)
                {
                    flag = prblist.SearchAndOpen("Number", ProblemId, temp, "Number");
                    if(!flag)error = "Error when search and open problem (Id:" + ProblemId + ")";
                    else
                    {
                        prb.WaitLoading();
                    }
                }
                else { error = "Cannot open Problem list"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_098_CheckMeetRCA()
        {
            try
            {
                checkbox = prb.Checkbox_RCADelivered;
                flag = checkbox.Existed;
                if (flag)
                {
                    flag = checkbox.Checked;
                    if (!flag)
                    {
                        flag = checkbox.Click(true);
                        if (!flag)
                        { error = "Cannot check RCA Delivered checkbox"; }
                    }
                }
                else { error = "Cannot find RCA Delivered checkbox."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_099_SaveProblem()
        {
            try
            {
                flag = prb.Save();
                if (!flag)
                    error = "Cannot save problem.";
                else
                    prb.WaitLoading();
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //--------------------------------------------------------------------------------------------------

        [Test]
        public void Step_100_ChangeProblemState()
        {
            try
            {
                combobox = prb.Combobox_State;
                flag = combobox.Existed;
                if(flag)
                { 
                    temp = "Closed/Resolved";
                    flag = combobox.SelectItem(temp);
                    if(!flag)
                    { error = "Cannot change problem state."; }
                }
                else { error = "Not found combobox State"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_101_UpdateCauseCode_CloseNote_AndClosureCode()
        {
            try
            {
                combobox = prb.Combobox_CloseCode;
                flag = combobox.Existed;
                if(flag)
                {
                    temp = Base.GData("ProCauseCode");
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        textarea = prb.Textarea_Closenotes;
                        flag = textarea.Existed;
                        if(flag)
                        {
                            string close_note = Base.GData("ProCloseNote");
                            flag = textarea.SetText(close_note);

                            if(flag)
                            {
                                combobox = prb.Combobox_ClosureCode;
                                flag = combobox.Existed;
                                if(flag)
                                {
                                    temp = Base.GData("ProClosureCode");
                                    flag = combobox.SelectItem(temp);
                                    if(!flag)
                                    {
                                        error = "Cannot select Problem Closure Code";
                                    }
                                }
                                else
                                {
                                    error = "Not found Combobox Closure Code";
                                }

                            }

                            else
                            { error = "Cannot input Problem Close Notes"; }
                        }
                        else { error = "Not found Textarea Close Notes"; }
                    }
                    else { error = "Cannot select Problem Cause Code"; }
                }
                else { error = "Not found Combobox Cause Code"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_102_SaveProblem()
        {
            try
            {
                flag = prb.Save();
                if (!flag)
                    error = "Cannot save problem.";
                else
                    prb.WaitLoading();
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_103_VerifySLATasks()
        {
            try
            {
                temp = Base.GData("SLAs");
                if (temp != string.Empty && temp.ToLower() != "no")// add more condition to check without SLA
                {
                    string[] tempSLA = null;
                    if (temp.Contains(";"))
                    {
                        tempSLA = temp.Split(';');
                    }
                    else tempSLA = new string[]{temp};
                    tab = prb.GTab("Task SLAs");
                    //---------------------------------------
                    int j = 0;
                    while (tab == null && j < 2)
                    {
                        Thread.Sleep(2000);
                        tab = prb.GTab("Task SLAs", true);
                        j++;
                    }
                    flag = tab.Header.Click();
                    if (flag)
                    {
                        string tempStage = string.Empty;
                        for (int i = 0; i < tempSLA.Length; i++)
                        {
                            tempStage = Base.GData("StageSLA" + (i + 1).ToString() + "_2");
                            flag = tab.RelatedTableVerifyRow("SLA=" + tempSLA[i] + "|Stage=" + tempStage);
                            if (!flag)
                            {
                                flagExit = false;
                                error = string.Format("Cannot find SLA = {0} - Stage = {1}", tempSLA[i], tempStage);
                            }
                        }
                    }
                    else
                    {error = "Cannot select Task SLAs tab.";}
                }
                else
                    System.Console.WriteLine("No SLAs tasks to check.");
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_104_ImpersonateUser_TestUser()
        {
            try
            {
                temp = Base.GData("UserFullName");
                flag = home.ImpersonateUser(Base.GData("TestUser"),true,temp,false);
                if(!flag)
                { error = "Cannot impersonate Test User"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }


        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_105_01_OpenEmailLog()
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

        [Test]
        public void Step_105_02_Verify_Email_Problem_Assigned_SentTo_AssginmentGroup()
        {
            try
            {
                temp = Base.GData("Debug");
                if (temp == "yes" && ProblemId == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem Id.");
                    addPara.ShowDialog();
                    ProblemId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }

                //----------------------------------------------------------------------------------------------
                string group = Base.GData("ProAssignmentGroup");
                string email = Base.GData("ProAssignmentGroupEmail");
                if (email.ToLower() == "no" || email.ToLower() == "empty")
                {
                    email = Base.GData("ProAssignee2Email");
                }
                temp = "Subject;contains;" + ProblemId + "|and|Subject;contains;has been assigned to group " + group +"|and|Recipients;contains;" + email;
                flag = emailList.EmailFilter(temp);
                if (flag)
                {
                    emailList.WaitLoading();
                    string conditions = "Subject=@@" + ProblemId;
                    flag = emailList.Verify(conditions);
                    if (!flag) error = "Not found email sent to Assignment group (assigned)";
                }
                else { error = "Error when filter."; }
               
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_105_03_OpenEmailLog()
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

        [Test]
        public void Step_105_04_Verify_Email_Problem_Notification_SentTo_AssignmentGroup()
        {
            try
            {
                temp = Base.GData("Debug");
                if (temp == "yes" && ProblemId == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem Id.");
                    addPara.ShowDialog();
                    ProblemId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }

                //----------------------------------------------------------------------------------------------
                string email = Base.GData("ProAssignmentGroupEmail");

                if (email.ToLower() == "no" || email.ToLower() == "empty")
                {
                    email = Base.GData("ProAssignee2Email");
                }
                temp = "Subject;contains;" + ProblemId + "|and|Subject;contains;notification|and|Recipients;contains;" + email;
                flag = emailList.EmailFilter(temp);
                if (flag)
                {
                    emailList.WaitLoading();
                    string conditions = "Subject=@@" + ProblemId;
                    flag = emailList.Verify(conditions);
                    if (!flag) error = "Not found email sent to Assignment group (notification)";
                }
                else { error = "Error when filter."; }

               
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_105_05_OpenEmailLog()
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

        [Test]
        public void Step_105_06_Verify_Email_Problem_Notification_SentTo_Assignee1()
        {
            try
            {
                temp = Base.GData("Debug");
                if (temp == "yes" && ProblemId == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem Id.");
                    addPara.ShowDialog();
                    ProblemId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }

                //----------------------------------------------------------------------------------------------

                string email = Base.GData("ProAssignee1Email");
                temp = "Subject;contains;" + ProblemId + "|and|Subject;contains;notification|and|Recipients;contains;" + email;
                flag = emailList.EmailFilter(temp);
                if (flag)
                {
                    emailList.WaitLoading();
                    string conditions = "Subject=@@" + ProblemId;
                    flag = emailList.Verify(conditions);
                    if (!flag) error = "Not found email sent to assignee (notification)";
                }
                else { error = "Error when filter."; }

                
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_105_07_OpenEmailLog()
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

        [Test]
        public void Step_105_08_Verify_Email_Problem_Assigned_SentTo_Assginee2()
        {
            try
            {
                temp = Base.GData("Debug");
                if (temp == "yes" && ProblemId == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem Id.");
                    addPara.ShowDialog();
                    ProblemId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }

                //----------------------------------------------------------------------------------------------

                string email = Base.GData("ProAssignee2Email");
                temp = "Subject;contains;" + ProblemId + "|and|Subject;contains;has been assigned to you|and|Recipients;contains;" + email;
                flag = emailList.EmailFilter(temp);
                if (flag)
                {
                    emailList.WaitLoading();
                    string conditions = "Subject=@@" + ProblemId;
                    flag = emailList.Verify(conditions);
                    if (!flag) error = "Not found email sent to Assignment group (assigned)";
                }
                else { error = "Error when filter."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_106_01_OpenEmailLog()
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

        [Test]
        public void Step_106_02_Verify_Email_ProblemTask_Assigned_SentTo_AssginmentGroup()
        {
            try
            {
                temp = Base.GData("Debug");
                if (temp == "yes" && ProTask01 == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem task1 Id.");
                    addPara.ShowDialog();
                    ProTask01 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }

                //----------------------------------------------------------------------------------------------
                string group = Base.GData("ProTask_AssignmentGroup");
                string email = Base.GData("ProTaskAssignmentGroupEmail");

                if (email.ToLower() == "no" || email.ToLower() == "empty")
                {
                    email = Base.GData("ProTaskResolver1Email");
                }
                temp = "Subject;contains;" + ProTask01 + "|and|Subject;contains;has been assigned to group " + group + "|and|Recipients;contains;" + email;
                flag = emailList.EmailFilter(temp);
                if (flag)
                {
                    emailList.WaitLoading();
                    string conditions = "Subject=@@" + ProTask01;
                    flag = emailList.Verify(conditions);
                    if (!flag) error = "Not found email sent to Assignment group (assigned)";
                }
                else { error = "Error when filter."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }



        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_106_03_OpenEmailLog()
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

        [Test]
        public void Step_106_04_Verify_Email_ProblemTask_Notification_SentTo_AssginmentGroup()
        {
            try
            {
                temp = Base.GData("Debug");
                if (temp == "yes" && ProTask01 == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem task1 Id.");
                    addPara.ShowDialog();
                    ProTask01 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }

                //----------------------------------------------------------------------------------------------
                string group = Base.GData("ProTask_AssignmentGroup");
                string email = Base.GData("ProTaskAssignmentGroupEmail");

                if (email.ToLower() == "no" || email.ToLower() == "empty")
                {
                    email = Base.GData("ProTaskResolver1Email");
                }
                temp = "Subject;contains;" + ProTask01 + "|and|Subject;contains;notification|and|Recipients;contains;" + email;
                flag = emailList.EmailFilter(temp);
                if (flag)
                {
                    emailList.WaitLoading();
                    string conditions = "Subject=@@" + ProTask01;
                    flag = emailList.Verify(conditions);
                    if (!flag) error = "Not found email sent to Assignment group (notification)";
                }
                else { error = "Error when filter."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_107_01_OpenEmailLog()
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

        [Test]
        public void Step_107_02_Verify_Email_ProblemTask2_Assigned_SentTo_AssginmentGroup()
        {
            try
            {
                temp = Base.GData("Debug");
                if (temp == "yes" && ProTask02 == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem task2 Id.");
                    addPara.ShowDialog();
                    ProTask02 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }

                //----------------------------------------------------------------------------------------------
                string group = Base.GData("ProTask_AssignmentGroup");
                string email = Base.GData("ProTaskAssignmentGroupEmail");

                if (email.ToLower() == "no" || email.ToLower() == "empty")
                {
                    email = Base.GData("ProTaskResolver1Email");
                }
                temp = "Subject;contains;" + ProTask02 + "|and|Subject;contains;has been assigned to group " + group + "|and|Recipients;contains;" + email;
                flag = emailList.EmailFilter(temp);
                if (flag)
                {
                    emailList.WaitLoading();
                    string conditions = "Subject=@@" + ProTask02;
                    flag = emailList.Verify(conditions);
                    if (!flag) error = "Not found email sent to Assignment group (assigned)";
                }
                else { error = "Error when filter."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_107_03_OpenEmailLog()
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

        [Test]
        public void Step_107_04_Verify_Email_ProblemTask2_Assigned_SentTo_Resolver1()
        {
            try
            {
                temp = Base.GData("Debug");
                if (temp == "yes" && ProTask02 == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem task2 Id.");
                    addPara.ShowDialog();
                    ProTask02 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }

                //----------------------------------------------------------------------------------------------

                string email = Base.GData("ProTaskResolver1Email");
                temp = "Subject;contains;" + ProTask02 + "|and|Subject;contains;has been assigned to you|and|Recipients;contains;" + email;
                flag = emailList.EmailFilter(temp);
                if (flag)
                {
                    emailList.WaitLoading();
                    string conditions = "Subject=@@" + ProTask02;
                    flag = emailList.Verify(conditions);
                    if (!flag) error = "Not found email sent to Assignment group (assigned)";
                }
                else { error = "Error when filter."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_107_05_OpenEmailLog()
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

        [Test]
        public void Step_107_06_Verify_Email_ProblemTask2_Notification_SentTo_Resolver1()
        {
            try
            {
                temp = Base.GData("Debug");
                if (temp == "yes" && ProTask02 == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem task2 Id.");
                    addPara.ShowDialog();
                    ProTask02 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }

                //----------------------------------------------------------------------------------------------

                string email = Base.GData("ProTaskResolver1Email");
                temp = "Subject;contains;" + ProTask02 + "|and|Subject;contains;notification|and|Recipients;contains;" + email;
                flag = emailList.EmailFilter(temp);
                if (flag)
                {
                    emailList.WaitLoading();
                    string conditions = "Subject=@@" + ProTask02;
                    flag = emailList.Verify(conditions);
                    if (!flag) error = "Not found email (notification) sent to Resolver1";
                }
                else { error = "Error when filter."; }
                    
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_107_07_OpenEmailLog()
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

        [Test]
        public void Step_107_08_Verify_Email_ProblemTask2_Assigned_SentTo_Resolver2()
        {
            try
            {
                temp = Base.GData("Debug");
                if (temp == "yes" && ProTask02 == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem task2 Id.");
                    addPara.ShowDialog();
                    ProTask02 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }

                //----------------------------------------------------------------------------------------------

                string email = Base.GData("ProTaskResolver2Email");

                temp = "Subject;contains;" + ProTask02 + "|and|Subject;contains;has been assigned to you|and|Recipients;contains;" + email;
                flag = emailList.EmailFilter(temp);
                if (flag)
                {
                    emailList.WaitLoading();
                    string conditions = "Subject=@@" + ProTask02;
                    flag = emailList.Verify(conditions);
                    if (!flag) error = "Not found email sent to Assignment group (assigned)";
                }
                else { error = "Error when filter."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_107_09_OpenEmailLog()
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

        [Test]
        public void Step_107_10_Verify_Email_ProblemTask2_Notification_SentTo_Resolver2()
        {
            try
            {
                temp = Base.GData("Debug");
                if (temp == "yes" && ProTask02 == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem task2 Id.");
                    addPara.ShowDialog();
                    ProTask02 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }

                //----------------------------------------------------------------------------------------------

                string email = Base.GData("ProTaskResolver2Email");
                temp = "Subject;contains;" + ProTask02 + "|and|Subject;contains;notification|and|Recipients;contains;" + email;
                flag = emailList.EmailFilter(temp);
                if (flag)
                {
                    emailList.WaitLoading();
                    string conditions = "Subject=@@" + ProTask02;
                    flag = emailList.Verify(conditions);
                    if (!flag) error = "Not found email (notification) sent to Resolver1";
                }
                else { error = "Error when filter."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_108_01_OpenEmailLog()
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

        [Test]
        public void Step_108_02_Verify_Email_Task1_Close_SentTo_ProbAssignee2()
        {
            try
            {
                temp = Base.GData("Debug");
                if (temp == "yes" && ProTask01 == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem task Id 1.");
                    addPara.ShowDialog();
                    ProTask01 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }

                //----------------------------------------------------------------------------------------------

                string email = Base.GData("ProAssignee2Email");


                temp = "Subject;contains;" + ProTask01 + "|and|Subject;contains;has been closed|and|Recipients;contains;" + email;
                flag = emailList.EmailFilter(temp);
                if (flag)
                {
                    emailList.WaitLoading();
                    string conditions = "Subject=@@" + ProTask01;
                    flag = emailList.Verify(conditions);
                    if (!flag) error =  "Not found email (task 1 closed) sent to Problem Assignee 2";
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
        public void Step_108_03_OpenEmailLog()
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

        [Test]
        public void Step_108_04_Verify_Email_Task2_Close_SentTo_ProbAssignee2()
        {
            try
            {
                temp = Base.GData("Debug");
                if (temp == "yes" && ProTask02 == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input problem task Id 2.");
                    addPara.ShowDialog();
                    ProTask02 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }

                //----------------------------------------------------------------------------------------------

                string email = Base.GData("ProAssignee2Email");


                temp = "Subject;contains;" + ProTask02 + "|and|Subject;contains;has been closed|and|Recipients;contains;" + email;
                flag = emailList.EmailFilter(temp);
                if (flag)
                {
                    emailList.WaitLoading();
                    string conditions = "Subject=@@" + ProTask02;
                    flag = emailList.Verify(conditions);
                    if (!flag) error = "Not found email (task 2 closed) sent to Problem Assignee 2";
                }

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------

        [Test]
        public void Step_109_Logout()
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
