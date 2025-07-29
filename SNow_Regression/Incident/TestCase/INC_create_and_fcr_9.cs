using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Reflection;
using System.Threading;
namespace Incident
{
    [TestFixture]
    public class INC_create_and_fcr_9
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
        Auto.KnowledgeSearch knls = null;
        Auto.IncidentList inclist = null;
        Auto.EmailList emailList = null;
        Auto.Email email = null;
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
                email = new Auto.Email(Base);
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
        public void Step_006_01_PopulateCallerName()
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
        public void Step_006_02_Verify_Company()
        {
            try
            {
                string temp = Base.GData("Company");
                lookup = inc.Lookup_Company;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.VerifyCurrentText(temp, true);
                    if (!flag) { error = "Invalid company value or the value is not auto populate."; flagExit = false; }
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
        public void Step_006_03_Verify_CallerEmail()
        {
            try
            {
                string temp = Base.GData("CallerEmail");
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
        public void Step_008_02_PopulateSubCategory()
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
        public void Step_009_PopulateContactType()
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
        public void Step_011_1_AttachAFile()
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
        public void Step_011_2_Verify_AttachmentFile()
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
        public void Step_011_3_SaveIncident()
        {
            try
            {
                flag = inc.Save();
                if (!flag) { error = "Error when save incident."; }
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
        public void Step_012_AddAdditionalComment()
        {
            try
            {
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
        public void Step_013_ResolvingTicket()
        {
            try
            {
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "Resolved";
                    flag = combobox.SelectItem(temp);
                    if (!combobox.CurrentValue.ToLower().Equals(temp.ToLower()))
                    {
                        flag = false;
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
        public void Step_014_PopulateCloseCodeAndCloseNotes()
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
                    combobox = inc.Combobox_CloseCode;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        string temp = Base.GData("CloseCode");
                        flag = combobox.SelectItem(temp);
                        if (flag)
                        {
                            textarea = inc.Textarea_Closenotes;
                            flag = textarea.Existed;
                            if (flag)
                            {
                                temp = Base.GData("CloseNote");
                                flag = textarea.SetText(temp);
                                if (!flag) error = "Cannot populate close notes.";
                            }
                            else error = "Cannot get textarea close notes.";
                        }
                        else error = "Cannot populate close code value.";
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
        public void Step_016_PopulateAssignedTo_SDA1()
        {
            try
            {
                string temp = Base.GData("SDA1");
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
        public void Step_017_SaveIncident()
        {
            try
            {
                flag = inc.Save();
                if (!flag) { error = "Error when save incident."; }
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
        public void Step_018_019_020_SearchAndOpenIncident()
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
                flag = home.LeftMenuItemSelect("Incident", "Resolved");
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
        public void Step_021_PopulateImpact()
        {
            try
            {
                string temp = Base.GData("Impact");
                combobox = inc.Combobox_Impact;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag) { error = "Cannot populate impact value."; }
                }
                else
                    error = "Cannot get combobox impact.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_022_PopulateUrgency()
        {
            try
            {
                string temp = Base.GData("Urgency");
                combobox = inc.Combobox_Urgency;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag) { error = "Cannot populate urgency value."; }
                }
                else
                    error = "Cannot get combobox ugency.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_023_VerifyIncidentState()
        {
            try
            {
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.VerifyCurrentText("Resolved");
                    if (!flag) error = "Invalid state value.";
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
        public void Step_024_CheckActivityNote()
        {
            try
            {
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
                    string temp = Base.GData("Activity_24");
                    flag = inc.VerifyActivity(temp);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid activity";
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
        public void Step_025_SaveIncident()
        {
            try
            {
                combobox = inc.Combobox_Priority;
                flag = combobox.Existed;
                if (flag)
                {
                    if (combobox.Text == "1 - Critical" || combobox.Text == "2 - High")
                    {
                        flag = inc.SaveNoVerify();
                        if (flag)
                        {
                            IAlert alert = Base.Driver.SwitchTo().Alert();
                            alert.Accept();
                            inc.WaitLoading();
                        }
                        else
                        {
                            error = "Cannot save incident";
                        }
                    }
                    else 
                    {
                        flag = inc.Save();
                        if (!flag) { error = "Error when save incident."; }
                        else inc.WaitLoading();
                    }
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
        public void Step_026_ImpersonateUser_TU()
        {
            try
            {
                string temp = Base.GData("TestUser");
                string rootuser = Base.GData("UserFullName");
                flag = home.ImpersonateUser(temp, true, rootuser);
                if (!flag) error = "Error when impersonate user sda2.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_027_SystemSetting()
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
        public void Step_028_01_OpenEmailLog()
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
        public void Step_028_02_Filter_Email_For_Incident_Opened_For_Me()
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
        public void Step_028_03_Validate_Email_For_Incident_Opened_For_Me()
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
                if (!flag) { error = "Not found email sent to caller (opened)."; flagExit = false; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_029_01_OpenEmailLog()
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
        public void Step_029_02_Filter_Email_For_Incident_Resolved()
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
        public void Step_029_03_Validate_And_Open_Email_For_Incident_Resolved()
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
                flag = emailList.Open(conditions, "Created");
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
        public void Step_029_04_Verify_Email_Subject()
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
                string expected = "@@incident " + incidentId + " has been resolved";
                flag = email.VerifySubject(expected);
                if (!flag) error = "Invalid subject value.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_029_05_Verify_Email_Recipient_Caller()
        {
            try
            {
                string recipient = Base.GData("CallerEmail");
                flag = email.VerifyRecipient(recipient);
                if (!flag) error = "Invalid recipient value.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_029_06_ClickOn_HtmlReviewBody()
        {
            try
            {
                flag = email.ClickOnPreviewHtmlBody();
                if (!flag) error = "Error when click on priview html body.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_029_07_Verify_Email_Body()
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
                string expected = "Your incident " + incidentId + " has been resolved and will automatically close in 7 days. If you feel the issue is not resolved, please click the following link to reopen your incident: " + incidentId;
                flag = email.VerifyEmailBody(expected);
                if (!flag) error = "Invalid value in body.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_029_08_Close_EmailBody()
        {
            try
            {
                flag = email.ClickOnCloseButton();
                if (!flag) error = "Error when close bemail body.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_030_Logout()
        {
            try
            {
                flag = home.Logout();
                if (!flag)
                {
                    error = "Error when logout system.";
                }else
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
