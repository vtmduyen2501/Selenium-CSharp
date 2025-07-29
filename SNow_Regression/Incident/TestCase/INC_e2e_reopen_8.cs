using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Reflection;
using System.Threading;
namespace Incident
{
    [TestFixture]
    public class INC_e2e_reopen_8
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
        Auto.oelement ele;
        Auto.odatetime datetime;
        //------------------------------------------------------------------
        Auto.Login login = null;
        Auto.Home home = null;
        Auto.Incident inc = null;
        Auto.KnowledgeSearch knls = null;
        Auto.IncidentList inclist = null;
        Auto.EmailList emailList = null;
        Auto.Member member = null;
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
        public void Step_007_OpenNewIncident()
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
        public void Step_008_01_PopulateCallerName()
        {
            try
            {
                textbox = inc.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    textbox.Click();
                    Thread.Sleep(1000);
                    //-- Store incident id
                    incidentId = textbox.Text;
                    Console.WriteLine("-*-[Store]: Incident Id:(" + incidentId + ")");
                    string temp = Base.GData("Caller");
                    lookup = inc.Lookup_Caller;
                    lookup.Click();
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
        public void Step_008_02_Verify_Company()
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
        public void Step_008_03_Verify_CallerEmail()
        {
            try
            {
                string temp = Base.GData("Caller_Email");
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
        public void Step_009_PopulateBusinessService()
        {
            try
            {
                string temp = Base.GData("BusinessService1");
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
        public void Step_010_01_PopulateCategory()
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
        public void Step_010_02_PopulateSubCategory()
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
        public void Step_015_AddWorkNote()
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
                    textarea = inc.Textarea_Worknotes_Create;
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
        public void Step_016_AttachAFile()
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
        public void Step_017_SaveIncident()
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
        public void Step_018_PopulateAssignmentGroup_RG()
        {
            try
            {
                string temp = Base.GData("ResolverGroup");
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
        public void Step_019_SaveIncident()
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
        public void Step_020_SearchAndOpenIncident()
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
        public void Step_021_Verify_AttachmentFile()
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
        public void Step_023_ImpersonateUser_RV_1()
        {
            try
            {
                string temp = Base.GData("Resolver1");
                string rootuser = Base.GData("UserFullName");
                flag = home.ImpersonateUser(temp, true, rootuser);
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
        public void Step_024_SystemSetting()
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
        public void Step_025_SearchAndChangeStateInlist()
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
                        string conditions = "Number=" + incidentId;
                        flag = inclist.SearchAndEditCellValue("Number", incidentId, conditions, "State", "Awaiting Vendor");
                        if (flag)
                        {
                            IAlert alert = Base.Driver.SwitchTo().Alert();
                            if (alert != null)
                            {
                                temp = "You can only select Awaiting Vendor state on the Incident form";
                                if (!alert.Text.Equals(temp)) 
                                {
                                    flag = false;
                                    error = "Invalid alert message. Runtime:(" + alert.Text + "). Expexted:(" + temp + ")";
                                }
                                alert.Accept();
                            }
                            else error = "Alert is not visible.";
                        }
                        else error = "Cannot change state value in list.";
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
        public void Step_026_OpenIncident()
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
        public void Step_027_01_PopulateState()
        {
            try
            {
                string temp = "Awaiting Vendor";
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        inc.WaitLoading();
                    }
                    else { error = "Cannot populate state value."; }
                }
                else
                {
                    error = "Cannot get combobox state.";
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
        public void Step_027_02_PopulateVendor()
        {
            try
            {
                string temp = Base.GData("Vendor");
                lookup = inc.Lookup_Vendor();
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag) { error = "Cannot populate vendor value."; }
                }
                else
                    error = "Cannot get lookup vendor field.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_027_03_SaveIncident()
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
        public void Step_028_SearchAndVerifyIncident()
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
                        string vendor = Base.GData("Vendor");
                        string conditions = "Number=" + incidentId + "|State=Awaiting Vendor|Vendor=" + vendor;
                        flag = inclist.SearchAndVerify("Number", incidentId, conditions);
                        if (!flag) error = "Not found incident with condition (" + conditions + ")";
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
        public void Step_029_ChangeStateAndVendorInList()
        {
            try
            {
                //Change state to Awaiting Evidence and change back to Awaiting Vendor
                string conditions = "Number=" + incidentId;
                flag = inclist.SearchAndEditCellValue("Number", incidentId, conditions, "State", "Awaiting Evidence");
                inclist.WaitLoading();
                if (flag)
                {
                    flag = inclist.SearchAndEditCellValue("Number", incidentId, conditions, "State", "Awaiting Vendor");
                    if (flag)
                    {
                        IAlert alert = Base.Driver.SwitchTo().Alert();
                        if (alert != null)
                        {
                            string temp = "You can only select Awaiting Vendor state on the Incident form";
                            if (!alert.Text.Equals(temp))
                            {
                                flag = false;
                                error = "Invalid alert message. Runtime:(" + alert.Text + "). Expexted:(" + temp + ")";
                            }
                            else
                            {
                                alert.Accept();
                                //Verify the Vendor field is readonly or not
                                flag = inclist.SearchAndVerifyCellReadOnly("Number", incidentId, conditions, "Vendor");
                                if (!flag)
                                {
                                    flagExit = false;
                                    error = "Vendor field is not read-only field";
                                }
                            }                            
                        }
                        else error = "Alert is not visible.";
                    }
                    else error = "Cannot change state Awaiting Vendor value in list.";
                }
                else error = "Cannot change state Awaiting Evidence value in list.";
            }      
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_030_OpenIncident()
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
        public void Step_031_01_VerifyVendorField_NotVisible()
        {
            try
            {                
                lookup = inc.Lookup_Vendor(true);
                flag = lookup.Existed;
                if (flag)
                {
                    flag = false;
                    error = "Vendor field is on the form. This field is only visible when state is awaiting vendor."; ;
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
        ////-----------------------------------------------------------------------------------------------------------------------------------
        //[Test]
        //public void Step_031_02_VeifyFollowUpDate_Visible()
        //{
        //    try
        //    {
        //        datetime = inc.Datetime_FollowUpDate;
        //        flag = datetime.Existed;
        //        if (!flag)
        //        {
        //            flag = false;
        //            flagExit = false;
        //            error = "-*- ERROR: Follow up date is not visible on form. Should be displayed as incident's state is Awaiting Evidence.";
        //        }
        //        else
        //            System.Console.WriteLine("-*- Passed: Follow up date is visible on form. Incident's state is Awaiting Evidence");
        //    }
        //    catch (Exception ex)
        //    {
        //        flag = false;
        //        error = ex.Message;
        //    }
        //}
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_032_Populate_FollowUpdate_Invalid_Value()
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
        public void Step_033_Save_Expected_CannotSave()
        {
            try
            {
                flag = inc.SaveNoVerify();
                if (flag)
                {
                    flag = inc.VerifyErrorMessage("The Follow up date must be greater than the Current date");
                    if (!flag)
                        error = "Not found error message.";
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
        public void Step_034_01_PopulateState()
        {
            try
            {
                string temp = "Awaiting Vendor";
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        inc.WaitLoading();
                    }
                    else { error = "Cannot populate state value."; }
                }
                else
                {
                    error = "Cannot get combobox state.";
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
        public void Step_034_02_ClearDataVendor()
        {
            try
            {
                string temp = string.Empty;
                lookup = inc.Lookup_Vendor();
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.SetText(temp);
                    if (flag)
                    {
                        lookup.MyEle.Clear();
                        Thread.Sleep(5000);
                        textbox = inc.Textbox_Number;
                        flag = textbox.Existed;
                        if (flag)
                        {
                            textbox.MyEle.Click();
                        }
                        else
                        {
                            error = "Cannot click incident number field";
                        }
                    }
                    else { error = "Cannot clear vendor value."; }
                }
                else
                    error = "Cannot get lookup vendor field.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_034_03_SaveIncidentAndVerifyAlert()
        {
            try
            {
                //Save Incident and Verify alert message
                flag = inc.SaveNoVerify();

                if (flag)
                {
                    try
                    {
                        IAlert alert = Base.Driver.SwitchTo().Alert();
                        string temp = "The following mandatory fields are not filled in: Vendor";
                        if (!alert.Text.Equals(temp))
                        {
                            flag = false;
                            error = "Invalid alert message. Runtime:(" + alert.Text + "). Expexted:(" + temp + ")";
                        }
                        alert.Accept();
                    }
                    catch
                    {
                        flagExit = false;
                        error = "Alert is not visible.";
                    }
                }

                inc.WaitLoading();
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_034_04_PopulateVendor()
        {
            try
            {
                string temp = Base.GData("Vendor");
                lookup = inc.Lookup_Vendor();
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag) { error = "Cannot populate vendor value."; }
                }
                else
                    error = "Cannot get lookup vendor field.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_034_05_SaveIncident()
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
        public void Step_036_037_SearchAndAssignToMe()
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
                        flag = inclist.SearchAndSelectContextMenu("Number", incidentId, "Number=" + incidentId, "Number", "Assign to me");
                        if (!flag) error = "Error when search and select assign to me.(id:" + incidentId + ")";
                        else
                            inclist.WaitLoading();
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
        public void Step_038_OpenIncident()
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
        public void Step_039_40_VerifyGroupAndAssignee()
        {
            try
            {
                lookup = inc.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    string temp = Base.GData("ResolverGroup");
                    if (lookup.Text == temp)
                    {
                        lookup = inc.Lookup_AssignedTo;
                        flag = lookup.Existed;
                        if (flag)
                        {
                            temp = Base.GData("Resolver1");
                            if (lookup.Text != temp)
                            {
                                flag = false;
                                error = "Resolver value does not match";
                            }
                        }
                        else
                        {
                            error = "Cannot get Assgin to field";
                        }
                    }
                    else
                    {
                        error = "Resolver group does not match";
                    }
                }
                else
                {
                    error = "Cannot get Assginment group field";
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
        public void Step_041_ImpersonateUser_RV_A()
        {
            try
            {
                string temp = Base.GData("Resolver_A");
                string rootuser = Base.GData("UserFullName");
                flag = home.ImpersonateUser(temp, true, rootuser);
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
        public void Step_042_SystemSetting()
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
        public void Step_043_044_SearchAndAssignToMe()
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
                        flag = inclist.SearchAndSelectContextMenu("Number", incidentId, "Number=" + incidentId, "Number", "Assign to me");
                        if (!flag) error = "Error when search and select assign to me.(id:" + incidentId + ")";
                        else
                            inclist.WaitLoading();
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
        public void Step_045_VerifyErrorMessage()
        {
            try
            {
                string temp = Base.GData("ErrorMessage");
                flag = inclist.VerifyErrorMessage(temp);
                if (!flag)
                {
                    flagExit = false;
                    error = "The error message is not expected";
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
        public void Step_046_01_ImpersonateUser_RV_1()
        {
            try
            {
                string temp = Base.GData("Resolver1");
                string rootuser = Base.GData("UserFullName");
                flag = home.ImpersonateUser(temp, true, rootuser);
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
        public void Step_046_02_SystemSetting()
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
        public void Step_046_03_SearchAndOpenIncident()
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
        public void Step_047_01_PopulateResolver2()
        {
            try
            {
                string temp = Base.GData("Resolver2");
                lookup = inc.Lookup_AssignedTo;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag) { error = "Cannot populate assignment to value."; }
                }
                else { error = "Cannot get lookup assignment to field."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_047_02_SaveIncident()
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
        public void Step_048_CheckActivityNote()
        {
            try
            {
                string temp = Base.GData("Activity_48");
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
        public void Step_050_AddWorkNote()
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
        public void Step_051_SaveIncident()
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
        public void Step_052_CheckActivityNote()
        {
            try
            {
                string temp = Base.GData("Activity_52");
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
        public void Step_053_ImpersonateUser_RV_2()
        {
            try
            {
                string temp = Base.GData("Resolver2");
                string rootuser = Base.GData("UserFullName");
                flag = home.ImpersonateUser(temp, true, rootuser);
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
        public void Step_054_SystemSetting()
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
        public void Step_055_056_SearchAndOpenIncident()
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
        public void Step_057_AttachAFile()
        {
            try
            {
                string attachmentFile = "incidentAttachment_1.txt";
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
        public void Step_058_Verify_AttachmentFile()
        {
            try
            {
                string attachmentFile = "incidentAttachment_1.txt";
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
        public void Step_059_ClickOnSearchKnowledge()
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
        public void Step_060_SearchAndSelectArticle()
        {
            try
            {
                string temp = Base.GData("KnowledgeArticle");
                textbox = knls.Textbox_Search();
                flag = textbox.Existed;
                if (flag)
                {
                    flag = textbox.SetText(temp);
                    if (flag)
                    {
                        button = knls.Button_Search;
                        flag = button.Existed;
                        if (flag)
                        {
                            flag = button.Click();
                            if (flag)
                            {
                                knls.WaitLoading();
                                flag = knls.FindKnowledge(temp);
                                if (!flag) { error = "Not found knowledge artical."; }
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
        public void Step_061_ClickOnAttachToIncident()
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
        public void Step_062_SaveIncident()
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
        public void Step_063_VerifyKnowledgeArticle()
        {
            try
            {
                ele = inc.AttachedKnowledge(null);
                flag = ele.Existed;
                if (flag)
                {
                    string temp = Base.GData("KnowledgeArticle");
                    if (!ele.MyEle.Text.Contains(temp))
                    {
                        flag = false;
                        error = "The knowledge article is not correct as expect result";
                    }

                }
                else { error = "Cannot get attach knowledge field"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_064_01_PopulateStateAwaitingEvidence()
        {
            try
            {
                string temp = "Awaiting Evidence";
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        inc.WaitLoading();

                    }
                    else { error = "Cannot populate state value."; }
                }
                else
                {
                    error = "Cannot get combobox state.";
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
        public void Step_064_02_AddAdditionalComment()
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
        public void Step_065_SaveIncident()
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
        ////-----------------------------------------------------------------------------------------------------------------------------------
        //[Test]
        //public void Step_066_Verify_Found_FollowUpdate()
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
        public void Step_067_01_PopulateStateActive()
        {
            try
            {
                string temp = "Active";
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        inc.WaitLoading();

                    }
                    else { error = "Cannot populate state value."; }
                }
                else
                {
                    error = "Cannot get combobox state.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        //[Test]
        //public void Step_067_02_VeifyFollowUpDate_NotVisible()
        //{
        //    try
        //    {
        //        datetime = inc.Datetime_FollowUpDate;
        //        if (datetime.Existed)
        //        {
        //            flag = false;
        //            flagExit = false;
        //            error = "-*- ERROR: Follow up date is visible on form.";
        //        }
        //        else
        //            System.Console.WriteLine("-*- Passed: Follow up date is not visible on form. Incident's state is Active.");
        //    }
        //    catch (Exception ex)
        //    {
        //        flag = false;
        //        error = ex.Message;
        //    }
        //}
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_067_03_AddAdditionalComment2()
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
                            string temp = Base.GData("AdditionalComment2");
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
        public void Step_068_SaveIncident()
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
        public void Step_069_CheckActivityNote()
        {
            try
            {
                string temp = Base.GData("Activity_69");
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
        public void Step_070_OpenImpactedServicesTab_1()
        {
            try
            {
                tab = inc.GTab("Impacted Services");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = inc.GTab("Impacted Services", true);
                    i++;
                }
                flag = tab.Header.Click(true);
                if (!flag)
                {
                    error = "Cannot click on tab (Impacted Services)";
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
        public void Step_071_AddTwoImpactService()
        {
            try
            {                
                //Loc Truong workaround due to issue cannot at 2 CIs at the same time//
                //Add CI frequently//
                flag = tab.ClickEdit();
                if (flag)
                {
                    member.WaitLoading();
                    Thread.Sleep(3000);
                    string temp = Base.GData("BusinessService1");
                    flag = member.AddMembers("impacted services", temp);
                    if (flag) inc.WaitLoading();
                    else System.Console.WriteLine("-*-[ERROR]: Error when add impacted services.");

                    if (flag)
                    {
                        tab = inc.GTab("Impacted Services");
                        //---------------------------------------
                        int i = 0;
                        while (tab == null && i < 5)
                        {
                            Thread.Sleep(2000);
                            tab = inc.GTab("Impacted Services", true);
                            i++;
                        }
                        flag = tab.Header.Click(true);
                        if (!flag)
                        {
                            error = "Cannot click on tab (Impacted Services)";
                        }
                        else
                        {
                            flag = tab.ClickEdit();
                            if (flag)
                            {
                                member.WaitLoading();
                                Thread.Sleep(3000);
                                temp = Base.GData("BusinessService2");
                                flag = member.AddMembers("impacted services", temp);
                                if (flag) inc.WaitLoading();
                                else System.Console.WriteLine("-*-[ERROR]: Error when add impacted services.");
                            }
                            else System.Console.WriteLine("-*-[ERROR]: Error when click on button edit.");
                        }
                    }
                }
                else System.Console.WriteLine("-*-[ERROR]: Error when click on button edit.");
                
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_072_OpenImpactedServicesTab_2()
        {
            try
            {
                tab = inc.GTab("Impacted Services");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = inc.GTab("Impacted Services", true);
                    i++;
                }
                flag = tab.Header.Click(true);
                if (!flag)
                {
                    error = "Cannot click on tab (Impacted Services)";
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
        public void Step_073_Delete2ndImpactService()
        {
            try
            {
                flag = tab.ClickEdit();
                if (flag)
                {
                    member.WaitLoading();
                    Thread.Sleep(3000);
                    string temp = Base.GData("BusinessService2");
                    flag = member.RemoveMembers("impacted services", temp);
                    if (flag) inc.WaitLoading();
                    else System.Console.WriteLine("-*-[ERROR]: Error when delete impacted services.");
                }
                else System.Console.WriteLine("-*-[ERROR]: Error when click on button edit.");
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_074_OpenImpactedServicesTab_3()
        {
            try
            {
                tab = inc.GTab("Impacted Services");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = inc.GTab("Impacted Services", true);
                    i++;
                }
                flag = tab.Header.Click(true);
                if (!flag)
                {
                    error = "Cannot click on tab (Impacted Services)";
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
        public void Step_075_Verify1sImpactService()
        {
            try
            {
                string condition = "Business Service=" + Base.GData("BusinessService1");
                flag = inc.RelatedTableVerifyRow("Impacted Services", condition);
                if (!flag)
                {
                    flagExit = false;
                    error = "Not found Impacted Service response: " + condition;
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
        public void Step_076_ReAssignedIncident_RV_3()
        {
            try
            {
                lookup = inc.Lookup_AssignedTo;
                flag = lookup.Existed;
                if (flag)
                {
                    string temp = Base.GData("Resolver3");
                    flag = lookup.Select(temp);
                    if (!flag) error = "Cannot populate assigned to value.";
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
        public void Step_077_SaveIncident()
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
        public void Step_078_ImpersonateUser_RV_3()
        {
            try
            {
                string temp = Base.GData("Resolver3");
                string rootuser = Base.GData("UserFullName");
                flag = home.ImpersonateUser(temp, true, rootuser);
                if (!flag) error = "Error when impersonate resolver user (" + temp + ")";
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
        public void Step_079_SystemSetting()
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
        public void Step_080_081_SearchAndOpenIncident()
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
        public void Step_082_CheckActivityNote()
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
                    string temp = Base.GData("Activity_82");
                    flag = inc.VerifyActivity(temp);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid activity";
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
        public void Step_086_ResolvingTicket()
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
        public void Step_087_PopulateCloseCodeAndCloseNotes()
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
        public void Step_088_01_SaveIncident()
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
        public void Step_088_02_Verify_ProcessFlow_Resolved()
        {
            try
            {
                string temp = "Resolved";
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
        public void Step_089_ReopeningTicket()
        {
            try
            {
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "Active";
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
        public void Step_090_01_SaveIncident()
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
        public void Step_090_02_Verify_ProcessFlow_Active()
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
        public void Step_091_01_ResolvingTicket()
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
        public void Step_091_02_Verify_CloseCodeCloseNote_NotKeepOldValue()
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
                        string temp = "-- None --";
                        flag = combobox.VerifyCurrentText(temp);
                        if (flag)
                        {
                            textarea = inc.Textarea_Closenotes;
                            flag = textarea.Existed;
                            if (flag)
                            {
                                if (!textarea.Text.Equals(""))
                                {
                                    flag = false;
                                    flagExit = false;
                                    error = "Invalid expected value. Expected: [" + "]. Runtime: [" + textarea.Text + "]";
                                }
                            }
                            else error = "Cannot get textarea close notes.";
                        }
                        else { error = "Cannot verify or invalid value. Expected: [" + temp + "]. Runtime: [" + combobox.Text + "]"; flagExit = false; }
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
        public void Step_092_Populate_CloseCode_CloseNote()
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
        public void Step_093_01_SaveIncident()
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
        public void Step_093_02_Verify_ProcessFlow_Resolved()
        {
            try
            {
                string temp = "Resolved";
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
        public void Step_094_ValidateCloseState()
        {
            try
            {
                string temp = "Closed";
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    bool flagTemp;
                    flagTemp = combobox.HaveItemInlist(temp);
                    if (!flagTemp)
                    {
                        // OK
                        flag = true;
                    }
                    else
                    {
                        // WRONG
                        flag = false;
                        error = "The state [" + temp + "] is in the list";
                    }
                }
                else
                {
                    error = "Cannot get combobox state.";
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
        public void Step_095_ImpersonateUser_TestUser()
        {
            try
            {
                string temp = Base.GData("TestUser");
                string rootuser = Base.GData("UserFullName");
                flag = home.ImpersonateUser(temp, true, rootuser);
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
        public void Step_096_SystemSetting()
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
        public void Step_097_1_OpenEmailLog()
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
        public void Step_097_2_Filter_EmailSentToCaller_Opened()
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
                string email = Base.GData("Caller_Email");

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
        public void Step_097_3_Validate_EmailSentToCaller_Opened()
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
                if (!flag) error = "Not found email sent to caller (opened).";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_098_1_OpenEmailLog()
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
        public void Step_098_2_Filter_EmailSentGroup_Assigned()
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
                string group = Base.GData("ResolverGroup");
                string email = Base.GData("ResolverGroup_Email");

                if (email.ToLower() == "no" || email.ToLower() == "empty")
                {
                    email = Base.GData("ResolverGroup_Email_Member");
                }

                temp = "Subject;contains;" + incidentId + "|and|Subject;contains;has been assigned to group " + group + "|and|" + "Recipients;contains;" + email;
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
        public void Step_098_3_Validate_EmailSentGroup_Assigned()
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
                if (!flag) error = "Not found email sent to group (assigned)";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_099_1_OpenEmailLog()
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
        public void Step_099_2_Filter_EmailSentToCaller_CommentAdd()
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

                string email = Base.GData("Caller_Email");

                temp = "Subject;contains;" + incidentId + "|and|Subject;contains;comments added|and|" + "Recipients;contains;" + email;
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
        public void Step_099_3_Validate_EmailSentToCaller_CommentAdd()
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
                if (!flag) error = "Not found email sent to caller with additional comments";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_100_1_OpenEmailLog()
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
        public void Step_100_2_Filter_EmailSentToResolver2_Assigned()
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

                string email = Base.GData("Resolver2_Email");

                if (email.ToLower() == "no" || email.ToLower() == "empty")
                {
                    email = Base.GData("ResolverGroup_Email_Member");
                }

                temp = "Subject;contains;" + incidentId + "|and|Subject;contains;has been assigned to you|and|" + "Recipients;contains;" + email;
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
        public void Step_100_3_Validate_EmailSentToResolver2_Assigned()
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
                if (!flag) error = "Not found email sent to resolver 2 (assigned)";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_101_1_OpenEmailLog()
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
        public void Step_101_2_Filter_EmailSentToResolver3_Assigned()
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

                string email = Base.GData("Resolver3_Email");

                if (email.ToLower() == "no" || email.ToLower() == "empty")
                {
                    email = Base.GData("ResolverGroup_Email_Member");
                }

                temp = "Subject;contains;" + incidentId + "|and|Subject;contains;has been assigned to you|and|" + "Recipients;contains;" + email;
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
        public void Step_101_3_Validate_EmailSentToResolver3_Assigned()
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
                if (!flag) error = "Not found email sent to resolver 3 (assigned)";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------               
        [Test]
        public void Step_102_1_OpenEmailLog()
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
        public void Step_102_2_Filter_EmailSentToCaller_Resolved()
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

                string email = Base.GData("Caller_Email");

                temp = "Subject;contains;" + incidentId + "|and|Subject;contains;has been resolved|and|" + "Recipients;contains;" + email;
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
        public void Step_102_3_Validate_EmailSentToCaller_Resolved()
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
                if (!flag) error = "Not found email sent to caller (resolved)";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_103_Logout()
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
        //-----------------------------------------------------------------------------------------------------------------------------------
        #endregion End - Scenario of test case (NEED TO UPDATE)
    }
}
