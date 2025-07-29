using NUnit.Framework;
using System;
using System.Reflection;
using System.Threading;

namespace Incident
{
    public class INC_Flag_for_Knowledge_13
    {
        #region Define default variables for test case (No need to update)
        //***********************************************************************************************************************************
        public bool flagC;
        public bool flag, flagExit, flagW;
        string caseName, error, temp;

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
            System.Console.WriteLine("Finished - Submission Id: " + submissionId);
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
        Auto.otextarea textarea;
        Auto.olookup lookup;
        Auto.ocombobox combobox;
        Auto.otab tab;
        Auto.ocheckbox checkbox;

        //------------------------------------------------------------------
        Auto.Login login = null;
        Auto.Home home = null;
        Auto.Incident inc = null;
        Auto.IncidentList inclist = null;
        Auto.KnowledgeList kmlist = null;
        Auto.Submission submission = null;     
           
        //------------------------------------------------------------------

        string incidentId;       
        string submissionId;

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
                submission = new Auto.Submission(Base, "Submission");
                kmlist = new Auto.KnowledgeList(Base, "Knowledge list");
                //------------------------------------------------------------------
                incidentId = string.Empty;
                submissionId = string.Empty;
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

        //-------------------------------------------------------------------------------------------------------------------------------------

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

        //-------------------------------------------------------------------------------------------------------------------------------------

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

        //-------------------------------------------------------------------------------------------------------------------------------------

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

        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_005_OpenIncident()
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
        public void Step_006_PopulateCallerName()
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
        public void Step_007_PopulateBussinessService()
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
        public void Step_008_01_PopulateCategory()
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
        public void Step_008_02_PopulateSubCategory()
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
        public void Step_009_PopulateShortDescription()
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
        public void Step_010_SubmitIncident()
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
        public void Step_011_PopulatedAssignmentGroup()
        {
            try
            {
                string temp = Base.GData("ResolverGroup");
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
        public void Step_012_ImpersonateUser_Resolver()
        {
            try
            {                
                string temp = Base.GData("Resolver");
                string loginUser = Base.GData("UserFullName");
                flag = home.ImpersonateUser(temp, true, loginUser);
                if (!flag)
                {
                    error = "Cannot impersonate user:  [" + temp + "].";
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
        public void Step_013_SystemSetting()
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
        public void Step_014_SearchAndOpen_Incident()
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
                        if (!flag)
                        {
                            error = "Error when search and open incident (id:" + incidentId + ")";
                        }
                        else inc.WaitLoading();
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
        public void Step_015_00_AssignedIncident()
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
        [Test]
        public void Step_015_01_AddWorkNote()
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
                //---------------------------------------
                flag = tab.Header.Click(true);
                if (flag)
                {
                    string temp = Base.GData("WorkNote");
                    textarea = inc.Textarea_Worknotes;
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
        public void Step_015_02_AttachAFile()
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
        public void Step_015_03_Verify_AttachmentFile()
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
        public void Step_016_00_ResolveIncident()
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
        public void Step_016_01_Open_ClosureTab()
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
                { error = "Cannot open Closure Information tab"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_016_02_PopulateCloseCode()
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
        public void Step_016_03_PopulateCloseNotes()
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

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_016_04_ClickOnKnowledgeCheckbox()
        {
            try
            {
                //------------------------------------------------
                tab = inc.GTab("Closure Information");
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = inc.GTab("Closure Information", true);
                    i++;
                }
                //------------------------------------------------
                flag = tab.Header.Click(true);
                if (flag)
                {
                    checkbox = inc.Checkbox_Knowledge;
                    flag = checkbox.Existed;
                    if (flag)
                    {
                        //flag = checkbox.Click(true);
                        //if (!flag && !checkbox.Checked)
                        //{
                        //    error = "Cannot select Knowledge checkbox.";
                        //}
                        if (!checkbox.Checked)
                        {
                            flag = checkbox.Click();
                            if (!flag)
                            {
                                error = "Cannot check Knowledge checkbox.";
                            }
                        }
                        else { error = "Knowledge checkbox is already checked. Expected: NOT checked."; }
                    }
                    else { error = "Cannot get Knowledge checkbox."; }
                }
                else { error = "Cannot click on (Closure Information) tab."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_017_Save_Incident()
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
        public void Step_018_VerifySubmissionCreated()
        {
            try
            {
                temp = "Knowledge Submission created";
                flag = inc.VerifyMessageInfo(temp);
                if (!flag)
                {
                    flagExit = false;
                    error = "Unexpected message value. Expected: [" + temp + "]";
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
        public void Step_019_ImpersonateUser_ServiceDesk()
        {
            try
            {
                temp = Base.GData("UserFullName");
                flag = home.ImpersonateUser(Base.GData("SDA1"), true, temp);
                if (!flag)
                { error = "Cannot impersonate Service Desk"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_020_OpenSubmissions()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Open Submissions", "Open Submissions");
                if (flag)
                { kmlist.WaitLoading(); }
                else
                {
                    error = "Cannot open submissions list.";
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
        public void Step_021_SearchAndOpen_Submission()
        {
            try
            {
                temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && incidentId == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //---------------------------------------------------------------------------
                temp = "Parent=" + incidentId;
                flag = inclist.SearchAndOpen("Parent", incidentId, temp, "Number");
                if (flag)
                {
                    submission.WaitLoading();
                }
                else { error = "Cannot search and open Knowledge Submission"; }

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_022_ValidateSubmission()
        {
            try
            {
                //-- Input information
                temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input Incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                submissionId = submission.Textbox_Number.Text;
                System.Console.WriteLine("KB Submission Id: " + submissionId);
                if (submissionId != string.Empty)
                {
                    temp = Base.GData("ResolverGroup");
                    if (submission.Lookup_AssignmentGroup.Text == temp)
                    {
                        temp = Base.GData("CloseNote");
                        if (submission.Textarea_Text.Text.Contains(temp))
                        {
                            combobox = submission.Combobox_Status;
                            if (combobox.CurrentValue == "Submitted")
                            {
                                if (submission.Lookup_Parent.Text != incidentId)
                                {
                                    flag = false;
                                    flagExit = false;
                                    error = "Invalid parent value.";
                                }
                            }
                            else
                            {
                                flag = false;
                                flagExit = false;
                                error = "Invalid status value.";
                            }
                        }
                        else
                        {
                            flag = false;
                            flagExit = false;
                            error = "Invalid KB close note value.";
                        }
                    }
                    else
                    {
                        flag = false;
                        flagExit = false;
                        error = "Invalid assignment group value.";
                    }
                }
                else
                {
                    flag = false;
                    flagExit = false;
                    error = "Invalid KB Submission number value.";
                }

                lookup = submission.Lookup_Parent;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.VerifyCurrentText(incidentId);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid Parent value. Expected: [" + incidentId + "]. Runtime: [" + lookup.Text + "].";
                    }
                }
                else { error = "Cannot get Parent lookup."; }
            }

            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_023_End_Logout()
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
