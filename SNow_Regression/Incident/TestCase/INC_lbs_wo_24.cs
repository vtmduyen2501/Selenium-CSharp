using NUnit.Framework;
using System;
using System.Reflection;
using System.Threading;
namespace Incident
{
    [TestFixture]
    public class INC_lbs_wo_24
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
            System.Console.WriteLine("Finished - Work Order Id: " + workorderId);
            System.Console.WriteLine("Finished - Work Order Task Id: " + wotaskId);
            System.Console.WriteLine("Current time: " + currentDateTime);

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
        Auto.olabel label;
        Auto.ocheckbox checkbox;
        
        //------------------------------------------------------------------
        Auto.Login login = null;
        Auto.Home home = null;
        Auto.Incident inc = null;
        Auto.KnowledgeSearch knls = null;
        Auto.IncidentList inclist = null;
        Auto.EmailList emailList = null;
        Auto.PHome phome = null;
        Auto.PCreateTicket pTicket = null;
        Auto.GlobalSearch globalSearch = null;
        Auto.WorkOrder wo = null;
        Auto.WorkOrderList wolist = null;
        Auto.WorkOrder_QuickComment woQuickCmt = null;
        Auto.WorkOrder_AssignedToSearch woAssigned = null;
        //------------------------------------------------------------------
        string incidentId;
        string currentDateTime;
        string workorderId;
        string wotaskId;
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
                phome = new Auto.PHome(Base);
                pTicket = new Auto.PCreateTicket(Base,"Create ticket");
                globalSearch = new Auto.GlobalSearch(Base);
                wo = new Auto.WorkOrder(Base, "Work Order");
                wolist = new Auto.WorkOrderList(Base, "Work Order list");
                woQuickCmt = new Auto.WorkOrder_QuickComment(Base);
                woAssigned = new Auto.WorkOrder_AssignedToSearch(Base);
                //------------------------------------------------------------------
                incidentId = string.Empty;
                currentDateTime = string.Empty;
                workorderId = string.Empty;
                wotaskId = string.Empty;
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
        public void Step_003_ImpersonateUser_Customer()
        {
            try
            {
                string temp = Base.GData("Customer_UserID");
                if (temp.ToLower() != "no")
                    temp = Base.GData("Customer") + ";" + temp;
                flag = home.ImpersonateUser(temp, false, null, true);
                if (!flag) { error = "Cannot impersonate user (" + temp + ")"; }
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
        public void Step_004_Validate_Landing_Page()
        {
            try
            {
                string temp = Base.GData("Customer");
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
        public void Step_005_01_ClickOn_Issue_Link()
        {
            try
            {
                ele = phome.GMenu("Issues");
                flag = ele.Existed;
                if (flag)
                {
                    flag = ele.Click();
                    if (!flag) error = "Error when click on menu issue";
                    else pTicket.MyWaitLoading();
                }
                else error = "Cannot get menu issue.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_005_02_Verify_UserName()
        {
            try
            {
                lookup = pTicket.Lookup_UserName;
                flag = lookup.Existed;
                if (flag)
                {
                    string temp = Base.GData("Customer_FullName");
                    flag = lookup.VerifyCurrentText(temp, true);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid user name value.";
                    }
                }
                else error = "Cannot get lookup user name.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_005_03_Verify_FirstName()
        {
            try
            {
                textbox = pTicket.Textbox_FirstName;
                flag = textbox.Existed;
                if (flag)
                {
                    string temp = Base.GData("Customer_FirstName");
                    flag = textbox.VerifyCurrentText(temp, true);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid first name value.";
                    }
                }
                else error = "Cannot get textbox first name.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_005_04_Verify_LastName()
        {
            try
            {
                textbox = pTicket.Textbox_LastName;
                flag = textbox.Existed;
                if (flag)
                {
                    string temp = Base.GData("Customer_LastName");
                    flag = textbox.VerifyCurrentText(temp, true);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid last name value.";
                    }
                }
                else error = "Cannot get textbox last name.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_005_05_Verify_Email()
        {
            try
            {
                textbox = pTicket.Textbox_Email;
                flag = textbox.Existed;
                if (flag)
                {
                    string temp = Base.GData("Customer_Email");
                    flag = textbox.VerifyCurrentText(temp, true);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid email value.";
                    }
                }
                else error = "Cannot get textbox email.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_005_06_Verify_UserID()
        {
            try
            {
                textbox = pTicket.Textbox_UserID;
                flag = textbox.Existed;
                if (flag)
                {
                    string temp = Base.GData("Customer_UserID");
                    flag = textbox.VerifyCurrentText(temp, true);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid user id value.";
                    }
                }
                else error = "Cannot get textbox user id.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_005_07_Verify_UserTelephoneNumber()
        {
            try
            {
                textbox = pTicket.Textbox_UserTelephoneNumber;
                flag = textbox.Existed;
                if (flag)
                {
                    string temp = Base.GData("Customer_Phone");
                    flag = textbox.VerifyCurrentText(temp, true);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid user telephone number value.";
                    }
                }
                else error = "Cannot get textbox user telephone number.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_005_08_Verify_UserLocation()
        {
            try
            {
                lookup = pTicket.Lookup_UserLocation;
                flag = lookup.Existed;
                if (flag)
                {
                    string temp = Base.GData("Customer_Location");
                    flag = lookup.VerifyCurrentText(temp, true);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid user location value.";
                    }
                }
                else error = "Cannot get lookup user location.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_005_09_Verify_TypeLevel_1()
        {
            try
            {
                combobox = pTicket.Combobox_TypeLevel1;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "-- None --";
                    flag = combobox.VerifyCurrentText(temp, true);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid type level 1 value.";
                    }
                }
                else error = "Cannot get combobox type level 1.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_005_10_Verify_TypeLevel_2()
        {
            try
            {
                combobox = pTicket.Combobox_TypeLevel2;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "-- None --";
                    flag = combobox.VerifyCurrentText(temp, true);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid type level 2 value.";
                    }
                }
                else error = "Cannot get combobox type level 2.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_006_Populate_BriefDescription()
        {
            try
            {
                currentDateTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
                string temp = Base.GData("Inc_ShortDescription") + currentDateTime;
                textbox = pTicket.Textbox_BriefDescription;
                flag = textbox.Existed;
                if (flag)
                {
                    flag = textbox.SetText(temp, true);
                    if (!flag) error = "Error when populate brief description.";
                }
                else error = "Cannot get textbox brief description.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_007_01_Populate_TypeLevel_1()
        {
            try
            {
                string temp = Base.GData("Type_Level_1");
                combobox = pTicket.Combobox_TypeLevel1;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag) error = "Error when populate type level 1";
                }
                else error = "Cannot get combobox type level 1.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_007_02_Populate_TypeLevel_2()
        {
            try
            {
                string temp = Base.GData("Type_Level_2");
                combobox = pTicket.Combobox_TypeLevel2;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag) error = "Error when populate type level 2";
                }
                else error = "Cannot get combobox type level 2.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_008_01_Populate_DetailedInformation()
        {
            try
            {
                string temp = "Auto - Detailed Information";
                textarea = pTicket.Textarea_DetailedInformation;
                flag = textarea.Existed;
                if (flag)
                {
                    flag = textarea.SetText(temp);
                    if (!flag) error = "Error when populate detailed information.";
                }
                else error = "Cannot get textarea detailed information.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_008_02_Verify_PriorityAssessment_Items()
        {
            try
            {
                combobox = pTicket.Combobox_PriorityAssessment;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = Base.GData("PriorityAssessment_Items");
                    flag = combobox.VerifyItemList(temp);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid Priority Assessment. Expected: [" + temp + "].";
                    }
                }
                else error = "Cannot get Priority Assessment combobox.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------


        [Test]
        public void Step_008_03_Populate_PriorityAssessment()
        {
            try
            {
                string temp = Base.GData("PriorityAssessment");
                combobox = pTicket.Combobox_PriorityAssessment;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag) error = "Error when populate priority assessment";
                }
                else error = "Cannot get combobox priority assessment.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_009_01_Submit_Incident()
        {
            try
            {
                button = pTicket.Button_Submit;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (!flag) error = "Error when click on button submit.";
                    else
                    {
                        inc.WaitLoading();
                        textbox = inc.Textbox_Number;
                        flag = textbox.Existed;
                        if (flag)
                        {
                            incidentId = textbox.Text;
                            Console.WriteLine("-*-[STORE]: Incident id (" + incidentId + ")");
                        }
                        else error = "Cannot get textbox number.";
                    }
                }
                else error = "Cannot get button submit.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_009_02_Verify_Caller()
        {
            try
            {
                lookup = inc.Lookup_Caller;
                flag = lookup.Existed;
                if (flag)
                {
                    string temp = Base.GData("Customer_FullName");
                    flag = lookup.VerifyCurrentText(temp, true);
                    if (!flag) { error = "Invalid caller value."; flagExit = false; }
                }
                else error = "Cannot get lookup caller.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_009_03_Verify_ShortDescription()
        {
            try
            {
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && currentDateTime == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input date time value.");
                    addPara.ShowDialog();
                    currentDateTime = addPara.value;
                    addPara.Close();
                    addPara = null;
                }

                //------------------------------------------------------------------------------------------
                textbox = inc.Textbox_ShortDescription;
                flag = textbox.Existed;
                if (flag)
                {
                    temp = Base.GData("Inc_ShortDescription") + currentDateTime;
                    flag = textbox.VerifyCurrentText(temp, true);
                    if (!flag) { error = "Invalid short description value."; flagExit = false; }
                }
                else error = "Cannot get textbox short description.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_009_04_Verify_State()
        {
            try
            {
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "New";
                    flag = combobox.VerifyCurrentText(temp, true);
                    if (!flag) { error = "Invalid state value."; flagExit = false; }
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
        public void Step_010_01_Navigate_To_Itil_Page()
        {
            try
            {
                string temp = Base.GData("Url");
                Base.ClearCache();
                Base.Driver.Navigate().GoToUrl(temp);
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
        public void Step_010_02_Login()
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
        public void Step_010_03_ImpersonateUser_SD_Agent()
        {
            try
            {
                string temp = Base.GData("ServiceDeskAgent");
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
        public void Step_010_04_SystemSetting()
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
        public void Step_011_GlobalSearch_Incident()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------

                flag = globalSearch.GlobalSearchItem(incidentId, true);
                if (flag)
                {
                    inc.WaitLoading();
                }
                else { error = "Error when set text into textbox search."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_012_01_Assign_To_LBS()
        {
            try
            {
                string temp = Base.GData("AssignmentGroup01");
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
        public void Step_012_02_Add_CI()
        {
            try
            {
                Thread.Sleep(2000);
                string temp = Base.GData("ConfigurationItem");
                lookup = inc.Lookup_ConfigurationItem;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag) { error = "Cannot populate configuration item value."; }
                }
                else { error = "Cannot get lookup configuration item."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_012_02_SaveIncident()
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
        public void Step_013_Verify_Created_WorkOrder()
        {
            try
            {
                
                string conditions = "Number=@@WO";
                flag = inc.RelatedTableVerifyRow("Work Orders", conditions);
                if (!flag) error = "Not found any Work Order.";
                else
                {
                    workorderId = inc.RelatedTableGetCellValue("Work Orders", conditions, "Number");
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
        public void Step_014_01_Assign_To_Another_Group()
        {
            try
            {
                string temp = Base.GData("AssignmentGroup02");
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
        public void Step_014_02_SaveIncident()
        {
            try
            {
                flag = inc.SaveNoVerify();
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
        public void Step_014_03_Verify_Error_Message()
        {
            try
            {
                flag = inc.VerifyErrorMessage("Work Notes are required to reassign this Incident;Invalid update");
                if (!flag)
                {
                    error = "Can not verify error message.";
                    flagExit = false;
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_015_01_Open_Incident_list()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Incident", "Open");
                if (flag)
                    inc.WaitLoading();
                else
                    error = "Error when open incident list.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_015_02_Add_Reassignment_Count_Column()
        {
            try
            {
                flag = inclist.AddColumnHeader("Reassignment count", 5);
                if (!flag) 
                {
                    error = "Error when adding Personalize column.";
                }
            }
        catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_016_Verify_Reassignment_Count()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                string condition = "Reassignment count=" + Base.GData("ReassignmentCount");
                
                flag = inclist.SearchAndVerify("Number", incidentId, condition);
                if (!flag)
                {
                    error = "Error when search and verify Incident Reassignment Count.";
                    flagExit = false;
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_017_Search_And_Open_Incident()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                string condition = "Number=" + incidentId;
                flag = inclist.SearchAndOpen("Number", incidentId, condition, "Number");
                if (!flag)
                {
                    error = "Error when search and verify Incident Reassignment Count.";
                }
                else inc.WaitLoading();
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_018_Add_Attachment()
        {
            try
            {
                flag = inc.AttachmentFile("incidentAttachment.txt");
                if (!flag)
                {
                    error = "Can not add attachment.";
                }
                
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
         [Test]
        public void Step_019_SaveIncident()
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

        //----------------------------------------------------------------------------------------------------------------------------------
         [Test]
         public void Step_020_ImpersonateUser_Logistic_Coordinator()
         {
             try
             {
                 string temp = Base.GData("LogisticCoordinator");
                 string loginUser = Base.GData("UserFullName");
                 flag = home.ImpersonateUser(temp, true, loginUser);
                 if (!flag) { error = "Cannot impersonate user (" + temp + ")"; }
             }
             catch (Exception ex)
             {
                 flag = false;
                 error = ex.Message;
             }
         }

        //----------------------------------------------------------------------------------------------------------------------------------
         [Test]
         public void Step_021_SystemSetting()
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

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_022_Global_Search_Work_Order()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (workorderId == null || workorderId  == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input work order Id");
                    addPara.ShowDialog();
                    workorderId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                button = globalSearch.Button_Search();
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (flag)
                    {
                        textbox = globalSearch.Textbox_Search();
                        flag = textbox.Existed;
                        if (flag)
                        {
                            flag = textbox.SetText(workorderId, true);
                            if (flag)
                            {
                                wo.WaitLoading();
                            }
                            else { error = "Error when set text into textbox search."; }
                        }
                        else { error = "Cannot get textbox search."; }
                    }
                    else { error = "Error when click on search button."; }
                }
                else { error = "Cannot get button search."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_023_01_Verify_Work_Order_Caller()
        {
            try
            {
                lookup = wo.Lookup_Wo_Caller;
                flag = lookup.Existed;
                if (flag)
                {
                    string temp = Base.GData("Customer_FullName");
                    flag = lookup.VerifyCurrentText(temp, true);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid Caller name value. Expected: [" + temp + "]. Runtime: [" + lookup.Text + "]";
                    }
                }
                else error = "Cannot get lookup Caller name.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_023_02_Verify_Work_Order_Location()
        {
            try
            {
                lookup = wo.Lookup_Location;
                flag = lookup.Existed;
                if (flag)
                {
                    string temp = Base.GData("Customer_Location");
                    flag = lookup.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid Caller Location value. Expected: [" + temp + "]. Runtime: [" + lookup.Text + "]";
                    }
                }
                else error = "Cannot get lookup Caller Location.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_023_03_Verify_Work_Order_ShortDescription()
        {
            try
            {
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && currentDateTime == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input date time value.");
                    addPara.ShowDialog();
                    currentDateTime = addPara.value;
                    addPara.Close();
                    addPara = null;
                }

                //------------------------------------------------------------------------------------------
                textbox = wo.Textbox_ShortDescription;
                flag = textbox.Existed;
                if (flag)
                {
                    temp = Base.GData("Inc_ShortDescription") + currentDateTime;
                    flag = textbox.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        error = "Invalid short description value. Expected: [" + temp + "]. Runtime: [" + textbox.Text + "]";
                        flagExit = false;
                    }
                }
                else error = "Cannot get textbox short description.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_024_Verify_Work_Order_Initiated_From()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                
                textbox = wo.Textbox_Initiatedfrom;
                flag = textbox.Existed;
                if (flag)
                {                    
                    flag = textbox.VerifyCurrentText(incidentId);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Wrong Incident number. Expected: [" + incidentId + "]. Runtime: [" + textbox.Text + "]";
                    }
                }
                else error = "Cannot get lookup Incident number.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_025_Verify_Work_Order_Attachment()
        {
            try
            {
                flag = wo.VerifyAttachmentFile("incidentAttachment.txt");
                if (!flag)
                {
                    error = "Attachment file is not correct.";
                    flagExit = false;
                }    
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_026_01_Verify_Category_ItemList()
        {
            try
            {                
                combobox = wo.Combobox_Category;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = Base.GData("VerifyCategory");
                    flag = combobox.VerifyItemList(temp);
                    if (!flag)
                    {
                        error = "Can not verify Item in the list.";
                        flagExit = false;
                    }
                }
                else { error = "Cannot get Category combobox."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_026_02_Populate_Category()
        {
            try
            {
                combobox = wo.Combobox_Category;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = Base.GData("Category");
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                    {
                        error = "Cannot select [" + temp + "] in Category";
                    }
                }
                else
                {
                    error = "Cannot get Category combobox.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_027_Add_Attachment()
        {
            try
            {
                flag = wo.AttachmentFile("wordAttachment.docx");
                if (!flag)
                {
                    error = "Cannot add work order attachment.";
                }            
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_028_Click_ReadyForQualification_Button()
        {
            try
            {
                button = wo.Button_ReadyForQualification;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (!flag)
                    {
                        error = "Cannot click on Ready for Qualification button.";
                    }
                    else { wo.WaitLoading(); Thread.Sleep(2000); }
                }
                else
                {
                    error = "Cannot get WO Qualification button.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_031_Verify_BlueAssignedTo_Button()
        {
            try
            {
                /*Get Work Order Task Id*/
                textbox = wo.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    wotaskId = textbox.Text;
                }
                else
                {

                    error = "Can not get Work Order Task number.";
                }
                
                //-----------------------------------------------------------
                
                ele = wo.GImage_AssignedToPickupFromGroup;
                flag = ele.Existed;
                if (!flag)
                {
                    error = "The blue Assigned To button shoud be existed.";
                    flagExit = false;                   
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_032_01_Verify_WOT_Parent()
        {
            try
            {
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && workorderId == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input Work Order Id");
                    addPara.ShowDialog();
                    workorderId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //------------------------------------------------------------------------------------------
                lookup = wo.Lookup_Parent;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.VerifyCurrentText(workorderId);
                    if (!flag) 
                    {
                        error = "Can not verify Parent value.";
                        flagExit = false;
                    }
                }
                else 
                {
                    error = "Cannot get Parent look up.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_032_02_Verify_WOT_ShortDescription()
        {
            try
            {
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && currentDateTime == string.Empty)
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input date time value");
                    addPara.ShowDialog();
                    currentDateTime = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //------------------------------------------------------------------------------------------

                textbox = wo.Textbox_ShortDescription;
                flag = textbox.Existed;
                if (flag)
                {
                    temp = Base.GData("Inc_ShortDescription");
                    string des = temp + currentDateTime;
                    flag = textbox.VerifyCurrentText(des);
                    if (!flag)
                    {
                        error = "Can not verify Work Order task Short Description.";
                        flagExit = false;
                    }
                }
                else
                {
                    error = "Can not get Short Description text box.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        
        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_032_03_Verify_WOT_Caller()
        {
            try
            {
                lookup = wo.Lookup_Wo_Caller;
                flag = lookup.Existed;
                if (flag)
                {
                    string temp = Base.GData("Customer_FullName");
                    flag = lookup.VerifyCurrentText(temp, true);
                    if (!flag)
                    {
                        error = "Invalid Caller name value.";
                    }
                }
                else error = "Cannot get lookup Caller name.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_032_04_Verify_WOT_Company()
        {
            try
            {
                lookup = wo.Lookup_Company;
                flag = lookup.Existed;
                if (flag)
                {
                    string temp = Base.GData("Company");
                    flag = lookup.VerifyCurrentText(temp, true);
                    if (!flag)
                    {
                        error = "Invalid Company name value.";
                    }
                }
                else error = "Cannot get lookup Company name.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_032_05_Verify_WOT_Dispatch_Group()
        {
            try
            {
                lookup = wo.Lookup_DispatchGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    string temp = Base.GData("DispatchGroup");
                    flag = lookup.VerifyCurrentText(temp, true);
                    if (!flag)
                    {
                        error = "Invalid Dispatch Group name value.";
                    }
                }
                else error = "Cannot get look up Dispatch Group name.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_033_01_Verify_Configuration_Item_Field()
        {
            try
            {
                lookup = wo.Lookup_ConfigurationItem;
                flag = lookup.Existed;
                if (!flag)
                {
                    error = "Cannot get look up Configuration Item.";
                    flagExit = false;
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_033_02_Verify_Configuration_Item()
        {
            try
            {                
                lookup = wo.Lookup_ConfigurationItem;
                flag = lookup.Existed;
                if (flag)
                {
                    string temp = Base.GData("ConfigurationItem");
                    flag = lookup.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid Configuration Item value.";
                    }
                }                    
                else 
                {
                    error = "Cannot get look up Configuration Item.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_034_01_Save_WOT()
        {
            try
            {
                flag = wo.Save();
                if (!flag) { error = "Error when save Work Order Task."; }
                else { wo.WaitLoading(); }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_034_02_Verify_Affected_Cis()
        {
            try
            {
               string temp = Base.GData("ConfigurationItem");
               string condition = "Configuration Item=" + temp; 
               flag = wo.RelatedTableVerifyRow("Affected CIs", condition);
               if (!flag)
               {
                   error = "Canot verify Affected CIs with conditon: " + condition ;
                   flagExit = false;
               }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_035_Verify_WOT_Attachments()
        {
            try
            {
                flag = wo.VerifyAttachmentFile("incidentAttachment.txt");
                if (flag)
                {
                    flag = wo.VerifyAttachmentFile("wordAttachment.docx");
                    if (!flag) 
                    {
                        error = "The Work Order attachment is not correct.";
                        flagExit = false;
                    }
                }
                else
                {
                    error = "The Incident attachment is not correct.";
                    flagExit = false;
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_036_01_Click_Qualified_Button()
        {
            try
            {
                button = wo.Button_Qualified;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (!flag) { error = "Cannot click Qualified button."; }
                    else wo.WaitLoading();
                }
                else 
                {
                    error = "Cannot get Qualified button.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_036_02_Verify_WOT_State()
        {
            try
            {
                combobox = wo.Combobox_State;
                flag = combobox.VerifyCurrentText("Pending Dispatch");
                if (!flag)
                {
                    error = "Cannot verify WOT State value. Expected: [Pending Dispatch]. Runtime: [" + combobox.Text + "].";
                    flagExit = false;
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_037_01_Verify_WOT_Assignment_Group()
        {
            try
            {
                string temp = Base.GData("WOTAssignmentGroup");
                lookup = wo.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.VerifyCurrentText(temp);
                    if (!flag)
                    {                    
                        error = "Cannot verify WOT Assignment group value. Expected: [" + temp + "]. Runtime: [" + lookup.Text + "]";
                        flagExit = false;
                    }
                }
                else
                {
                    error = "Cannot get Assignment Group lookup.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_037_02_Verify_Caller_Location()
        {
            try
            {
                string temp = Base.GData("Customer_Location");
                lookup = wo.Lookup_Location;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        error = "Cannot verify Caller Location value. Expected: [" + temp + "]. Runtime: [" + lookup.Text + "]";
                        flagExit = false;
                    }
                }
                else
                {
                    error = "Cannot get Assignment Group lookup.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_038_Verify_BlueAssignedTo_Button()
        {
            try
            {
                ele = wo.GImage_AssignedToPickupFromGroup;
                flag = ele.Existed;
                if (!flag)
                {
                    error = "The blue Assigned To button shoud be existed.";
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
        public void Step_039_01_Click_SkillButton()
        {
            try
            {
                button = wo.Button_Skill;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click(true);
                    if (!flag)
                    { error = "Cannot click Skill button."; }
                    else wo.WaitLoading();
                }
                else { error = "Cannot get Skill button."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_039_02_PopulateSkill()
        {
            try
            {
                lookup = wo.Lookup_Skills;
                flag = lookup.Existed;
                if (flag)
                {
                    string temp = Base.GData("Skill");
                    flag = lookup.SetText(temp);
                    if (!flag) { error = "Cannot populate Skill value,"; }
                }
                else { error = "Cannot get Skill lookup."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_039_03_Click_BlueAssignedTo_Button()
        {
            try
            {
                ele = wo.GImage_AssignedToPickupFromGroup;
                flag = ele.Existed;
                if (flag)
                {
                    flag = ele.Click();
                    if (flag)
                    {
                        wo.WaitLoading();
                    }
                    else { error = "Cannot click Assigned To Pick Up button."; }
                }
                else { error = "Cannot get Assigned To Pick Up button."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_039_04_SwitchToPage_1()
        {
            try
            {
                flag = Base.SwitchToPage(1);
                if (!flag) { error = "Cannot switch to page (1)."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_039_05_Verify_SelectAssignee_Window()
        {
            try
            {
                string expectedColumn = Base.GData("ColString");
                string realColumn = woAssigned.AllColumnString();
                if (realColumn.Trim().ToLower() != expectedColumn.Trim().ToLower())
                {
                    flag = false;
                    flagExit = false;
                    error = "The column header is not displayed as expected. Expected: " + expectedColumn;
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
        public void Step_039_06_Choose_WOTechnician()
        {
            try
            {
                string temp = Base.GData("WOTechnician");
                string conditions = "Name=" + temp;
                flag = woAssigned.TableOpenRecord(conditions, "Name");
                if (!flag)
                {
                    error = "Cannot choose WO Technician with condition: " + temp;
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
        public void Step_039_07_SwitchToPage_0()
        {
            try
            {
                flag = Base.SwitchToPage(0);
                if (!flag) { error = "Cannot switch to page (0)."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_040_01_Save_WOT()
        {
            try
            {
                flag = wo.Save();
                if (!flag) { error = "Error when save Work Order Task."; flagExit = false; }
                else { wo.WaitLoading(); }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_040_02_Verify_WOT_State()
        {
            try
            {
                combobox = wo.Combobox_State;
                flag = combobox.VerifyCurrentText("Assigned");
                if (!flag)
                {
                    error = "WOT State is not correct.";
                    flagExit = false;
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_040_03_Verify_WOT_Substate()
        {
            try
            {
                combobox = wo.Combobox_SubState;
                flag = combobox.VerifyCurrentText("-- None --");
                if (!flag)
                {
                    error = "WOT Substate is not correct.";
                    flagExit = false;
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_041_GlobalSearch_Incident()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------

                flag = globalSearch.GlobalSearchItem(incidentId, true);
                if (flag)
                {
                    inc.WaitLoading();
                }
                else { error = "Error when set text into textbox search."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_042_Verify_Incident_State_Active()
        {
            try
            {
                combobox = inc.Combobox_State;
                string temp = "Active";
                flag = combobox.VerifyCurrentText(temp);
                if (!flag)
                {
                    error = "Invalid State value. Expected: [" + temp + "]. Runtime: [" + combobox.Text + "]";
                    flagExit = false;
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_043_01_Global_Search_WOT()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (wotaskId == null || wotaskId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input work order task Id");
                    addPara.ShowDialog();
                    wotaskId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                button = globalSearch.Button_Search();
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (flag)
                    {
                        textbox = globalSearch.Textbox_Search();
                        flag = textbox.Existed;
                        if (flag)
                        {
                            flag = textbox.SetText(wotaskId, true);
                            if (flag)
                            {
                                inc.WaitLoading();
                            }
                            else { error = "Error when set text into textbox search."; }
                        }
                        else { error = "Cannot get textbox search."; }
                    }
                    else { error = "Error when click on search button."; }
                }
                else { error = "Cannot get button search."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_043_02_Populate_Substate()
        {
            try
            {
                combobox = wo.Combobox_SubState;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "Awaiting customer";
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid State value. Expected: [" + temp + "]. Runtime: [" + combobox.Text + "]";
                    }
                }
                else
                {
                    error = "Cannot get combobox Substate.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_043_03_Populate_Additional_Comment()
        {
            try
            {
                textarea = wo.Textarea_AdditionComments_Update;
                flag = textarea.Existed;
                if (flag)
                {
                    string temp = Base.GData("AdditionalComment");
                    flag = textarea.SetText(temp);
                    if (!flag)
                    {
                        error = "Can not populate WOT Additional Comment.";
                    }
                }
                else
                {
                    error = "Cannot get textarea Additional Comment textarea.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_043_04_Save_WOT()
        {
            try
            {
                flag = wo.Save();
                if (!flag) { error = "Error when save Work Order Task."; }
                else { wo.WaitLoading(); }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_043_05_Verify_WOT_State()
        {
            try
            {
                combobox = wo.Combobox_State;
                string temp = "Assigned";
                flag = combobox.VerifyCurrentText(temp);
                if (!flag)
                {
                    error = "Invalid WOT State value. Expected: [" + temp + "]. Runtime: [" + combobox.Text + "]";
                    flagExit = false;
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_043_06_Verify_WOT_Substate()
        {
            try
            {
                combobox = wo.Combobox_SubState;
                string temp = "Awaiting customer";
                flag = combobox.VerifyCurrentText(temp);
                if (!flag)
                {
                    error = "Invalid WOT Substate value. Expected: [" + temp + "]. Runtime: [" + combobox.Text + "]";
                    flagExit = false;
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_044_01_GlobalSearch_Incident()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------

                flag = globalSearch.GlobalSearchItem(incidentId, true);

                if (flag)
                {
                    inc.WaitLoading();
                }
                else { error = "Error when set text into textbox search."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_044_02_Verify_Incident_State_AwaitingCustomer()
        {
            try
            {
                combobox = inc.Combobox_State;
                string temp = "Awaiting Customer";
                flag = combobox.VerifyCurrentText(temp);
                if (!flag)
                {
                    error = "Invalid WOT State value. Expected: [" + temp + "]. Runtime: [" + combobox.Text + "]";
                    flagExit = false;
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_045_01_Global_Search_WOT()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (wotaskId == null || wotaskId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input work order task Id");
                    addPara.ShowDialog();
                    wotaskId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                button = globalSearch.Button_Search();
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (flag)
                    {
                        textbox = globalSearch.Textbox_Search();
                        flag = textbox.Existed;
                        if (flag)
                        {
                            flag = textbox.SetText(wotaskId, true);
                            if (flag)
                            {
                                inc.WaitLoading();
                            }
                            else { error = "Error when set text into textbox search."; }
                        }
                        else { error = "Cannot get textbox search."; }
                    }
                    else { error = "Error when click on search button."; }
                }
                else { error = "Cannot get button search."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_045_02_Populate_WOT_Substate()
        {
            try
            {
                combobox = wo.Combobox_SubState;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "-- None --";
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid WOT Substate value. Expected: [" + temp + "].Runtime: [" + combobox.Text + "]";
                    }
                }
                else
                {
                    error = "Can not get combobox Substate.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_045_03_Save_WOT()
        {
            try
            {
                flag = wo.Save();
                if (!flag) { error = "Error when save Work Order Task."; }
                else { wo.WaitLoading(); }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_045_04_Verify_WOT_State()
        {
            try
            {
                combobox = wo.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "Assigned";
                    flag = combobox.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        error = "Invalid WOT State value. Expected: [" + temp + "].Runtime: [" + combobox.Text + "]";
                        flagExit = false;
                    }
                }
                else
                {
                    error = "Cannot get State combobox.";
                }                
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_045_05_Verify_WOT_Substate()
        {
            try
            {
                combobox = wo.Combobox_SubState;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "-- None --";
                    flag = combobox.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        error = "Invalid WOT Substate value. Expected: [" + temp + "].Runtime: [" + combobox.Text + "]";
                        flagExit = false;
                    }
                }
                else { error = "Cannot get Substate combobox."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_046_01_ImpersonateUser_WO_Technician()
        {
            try
            {
                string temp = Base.GData("WOTechnician"); 
                string loginUser = Base.GData("UserFullName");
                flag = home.ImpersonateUser(temp, true, loginUser);
                if (!flag) { error = "Cannot impersonate user (" + temp + ")"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_046_02_SystemSetting()
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

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_047_Global_Search_WOT()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (wotaskId == null || wotaskId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input work order task Id");
                    addPara.ShowDialog();
                    wotaskId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                button = globalSearch.Button_Search();
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (flag)
                    {
                        textbox = globalSearch.Textbox_Search();
                        flag = textbox.Existed;
                        if (flag)
                        {
                            flag = textbox.SetText(wotaskId, true);
                            if (flag)
                            {
                                wo.WaitLoading();
                            }
                            else { error = "Error when set text into textbox search."; }
                        }
                        else { error = "Cannot get textbox search."; }
                    }
                    else { error = "Error when click on search button."; }
                }
                else { error = "Cannot get button search."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_048_01_Click_Accept_WOT()
        {
            try
            {
                button = wo.Button_Accept;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click(true);
                    if (!flag)
                    {
                        error = "Can not click on Accept button.";
                    }
                    else { wo.WaitLoading(); }
                }                
                else
                {
                    error = "Cannot get button Accept.";
                }    
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_048_02_Verify_WOT_Progress_Bar_Accepted()
        {
            try
            {
                Thread.Sleep(3000);
                string temp = "Accepted";
                flag = wo.CheckCurrentState(temp);
                if (!flag)
                {
                    error = "Invalid current state. Expected: [" + temp + "]";
                    flagExit = false;
                }
                else
                {
                    error = "Can not get Accepted State in Progress Bar ";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_048_03_Verify_WOT_State()
        {
            try
            {
                combobox = wo.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "Accepted";
                    flag = combobox.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        error = "Invalid WOT State value. Expected: [" + temp + "].Runtime: [" + combobox.Text + "]";
                        flagExit = false;
                    }
                }
                else { error = "Cannot get State combobox."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_048_04_Verify_WOT_Substate()
        {
            try
            {
                combobox = wo.Combobox_SubState;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "-- None --";
                    flag = combobox.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        error = "Invalid WOT Substate value. Expected: [" + temp + "].Runtime: [" + combobox.Text + "]";
                        flagExit = false;
                    }
                }
                else { error = "Cannot get Substate combobox."; }                
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_049_01_GlobalSearch_Incident()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------

                flag = globalSearch.GlobalSearchItem(incidentId, true);
                if (flag)
                {
                    inc.WaitLoading();
                }
                else { error = "Error when set text into textbox search."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_049_02_Verify_Responded()
        {
            try
            {
                checkbox = inc.Checkbox_Responded;
                flag = checkbox.Existed;
                if (flag)
                {
                    flag = checkbox.Checked;
                    if (!flag)
                    {
                        error = "Responded check box is not checked.";
                        flagExit = false;
                    }
                }
                else
                {
                    error = "Cannot get checkbox Responded.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_050_01_Global_Search_WOT()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (wotaskId == null || wotaskId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input work order task Id");
                    addPara.ShowDialog();
                    wotaskId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                button = globalSearch.Button_Search();
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (flag)
                    {
                        textbox = globalSearch.Textbox_Search();
                        flag = textbox.Existed;
                        if (flag)
                        {
                            flag = textbox.SetText(wotaskId, true);
                            if (flag)
                            {
                                wo.WaitLoading();
                            }
                            else { error = "Error when set text into textbox search."; }
                        }
                        else { error = "Cannot get textbox search."; }
                    }
                    else { error = "Error when click on search button."; }
                }
                else { error = "Cannot get button search."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_050_02_Add_Work_Notes_WOT()
        {
            try
            {
                textarea = wo.Textarea_Worknotes_Update;
                flag = textarea.Existed;
                if (flag)
                {
                    string temp = Base.GData("WOTWorkNotes");
                    flag = textarea.SetText(temp);
                    if (!flag)
                    {
                        error = "Can not set text for WOT Work Notes.";
                    }
                }
                else
                {
                    error = "Can not get textarea Work Notes.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_050_03_Save_WOT()
        {
            try
            {
                flag = wo.Save();
                if (!flag) { error = "Error when save Work Order Task."; }
                else { wo.WaitLoading(); }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_050_04_Verify_WOT_WorkNote()
        {
            try
            {
                string temp = Base.GData("VerifyWOTWorkNotes");
                flag = wo.VerifyActivity(temp);
                if (!flag)
                {
                    error = "WOT Work Note is not correct in Activity.";
                    flagExit = false;
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_051_01_Open_Quick_Comment()
        {
            try
            {
                Base.SwitchToPage(0);
                lookup = wo.Lookup_QuickComment;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Click();
                    if (!flag)
                    {
                        error = "Can not open Quick Comment.";
                    }                    
                }
                else
                {
                    error = "Cannot get look up Quick Comment.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_051_02_SwitchToPage_1()
        {
            try
            {
                Thread.Sleep(3000);
                flag = Base.SwitchToPage(1);
                if (!flag) { error = "Cannot switch to page (1)."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_051_03_Select_Comment()
        {
            try
            {
                string temp = Base.GData("QuickComment");
                string conditions = "Label=" + temp;
                flag = woQuickCmt.TableOpenRecord(conditions, "Label");
                if (!flag)
                {
                    error = "Cannot choose Quick Comment.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_051_04_SwitchToPage_0()
        {
            try
            {
                flag = Base.SwitchToPage(0);
                if (!flag) { error = "Cannot switch to page (0)."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_052_01_Click_Start_Travel_WOT()
        {
            try
            {
                button = wo.Button_StartTravel;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (!flag)
                    {
                        error = "Cannot click on Start Travel button.";
                    }
                    else { wo.WaitLoading(); }
                }
                else
                {
                    error = "Cannot get button Start Travel.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_052_03_Verify_WOT_State()
        {
            try
            {
                Thread.Sleep(3000);
                combobox = wo.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "Work In Progress";
                    flag = combobox.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        error = "Invalid WOT State value. Expected: [" + temp + "].Runtime: [" + combobox.Text + "]";
                        flagExit = false;
                    }
                }
                else { error = "Cannot get State combobox."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_052_04_Verify_WOT_Substate()
        {
            try
            {
                combobox = wo.Combobox_SubState;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "-- None --";
                    flag = combobox.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        error = "Invalid WOT Substate value. Expected: [" + temp + "].Runtime: [" + combobox.Text + "]";
                        flagExit = false;
                    }
                }
                else { error = "Cannot get Substate combobox."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_053_01_GlobalSearch_Incident()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------

                flag = globalSearch.GlobalSearchItem(incidentId, true);

                if (flag)
                {
                    inc.WaitLoading();
                }
                else { error = "Error when set text into textbox search."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_053_02_Verify_Incident_State_Active()
        {
            try
            {
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "Active";
                    flag = combobox.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        error = "Invalid WOT State value. Expected: [" + temp + "].Runtime: [" + combobox.Text + "]";
                        flagExit = false;
                    }
                }
                else { error = "Cannot get State combobox."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_054_01_Global_Search_WOT()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (wotaskId == null || wotaskId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input work order task Id");
                    addPara.ShowDialog();
                    wotaskId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                button = globalSearch.Button_Search();
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (flag)
                    {
                        textbox = globalSearch.Textbox_Search();
                        flag = textbox.Existed;
                        if (flag)
                        {
                            flag = textbox.SetText(wotaskId, true);
                            if (flag)
                            {
                                wo.WaitLoading();
                            }
                            else { error = "Error when set text into textbox search."; }
                        }
                        else { error = "Cannot get textbox search."; }
                    }
                    else { error = "Error when click on search button."; }
                }
                else { error = "Cannot get button search."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_054_02_Populate_WorkNote()
        {
            try
            {
                tab = wo.GTab("Notes");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = wo.GTab("Notes", true);
                    i++;
                }

                flag = tab.Header.Click(true);
                if (flag)
                {
                    textarea = wo.Textarea_Worknotes_Update;
                    flag = textarea.Existed;
                    if (flag)
                    {
                        string temp = Base.GData("WOTWorkNotes");
                        flag = textarea.SetText(temp);
                        if (!flag)
                        {
                            error = "Cannot set text for WOT Work Notes.";
                        }
                    }
                    else
                    {
                        error = "Cannot get textarea Work Notes.";
                    }
                }
                else
                { 
                    error = "Cannot select tab (Notes)."; 
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_054_03_Select_Closure_Information_tab()
        {
            try
            {
                tab = wo.GTab("Closure Information");
                //---------------------------------------
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = wo.GTab("Closure Information", true);
                    i++;
                }
                flag = tab.Header.Click(true);
                if (!flag)
                { error = "Cannot select tab (Closure Information)."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_054_04_Verify_And_Select_CIUpdateStatus()
        {
            try
            {
                combobox = wo.Combobox_CIUpdate_Status;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = Base.GData("CIUpdateStatus");
                    flag = combobox.VerifyItemList(temp);
                    if (flag)
                    {
                        flag = combobox.SelectItem("CI Updated");
                        if (!flag) 
                        {
                            error = "Can not select item in combobox CI Update Status.";
                        }
                    }
                    else { error = "Cannot verify CI Update Status item list."; }
                }
                else
                {
                    error = "Cannot get combobox CI Update Status combobox.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_054_05_Populate_Close_Notes()
        {
            try
            {
                textarea = wo.Textarea_Closenotes;
                flag = textarea.Existed;
                if (flag)
                {
                    string temp = Base.GData("CloseNotes");
                    flag = textarea.SetText(temp);
                    if (!flag)
                    {
                        error = "Cannot populate Close Notes value.";
                    }
                }
                else
                {
                    error = "Cannot get textare Close Notes.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_054_06_Click_Close_Complete()
        {
            try
            {
                button = wo.Button_CloseComplete;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click(true);                    
                    if (!flag)
                    {
                        error = "Can not click on Close Complete button.";
                    }
                    else { wo.WaitLoading(); }
                }
                else
                {
                    error = "Can not get button Close Complete.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_057_Global_Search_WOT()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (wotaskId == null || wotaskId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input work order task Id");
                    addPara.ShowDialog();
                    wotaskId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                button = globalSearch.Button_Search();
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (flag)
                    {
                        textbox = globalSearch.Textbox_Search();
                        flag = textbox.Existed;
                        if (flag)
                        {
                            flag = textbox.SetText(wotaskId, true);
                            if (flag)
                            {
                                wo.WaitLoading();
                            }
                            else { error = "Error when set text into textbox search."; }
                        }
                        else { error = "Cannot get textbox search."; }
                    }
                    else { error = "Error when click on search button."; }
                }
                else { error = "Cannot get button search."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_058_01_Verify_WOT_Progress_Bar_Complete()
        {
            try
            {
                Thread.Sleep(3000);
                string temp = "Complete";
                flag = wo.CheckCurrentState(temp);
                if (!flag)
                {
                    error = "Invalid current state. Expected: [" + temp + "]";
                    flagExit = false;
                }
                else
                {
                    error = "Can not get Complete State in Progress Bar ";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_058_02_Verify_WOT_State()
        {
            try
            {
                combobox = wo.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "Closed Complete";
                    flag = combobox.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        error = "Invalid State value. Expected: [" + temp + "]. Runtime: [" + combobox.Text + "]";
                        flagExit = false;
                    }
                }
                else { error = "Cannot get State combobox."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_059_Open_WO_Parent_Field()
        {
            try
            {
                button = wo.Button_WO_Parent;
                flag = button.Existed;
                if (flag)
                {
                    flag = button.Click();
                    if (!flag)
                    {
                        error = "Can not click on button WO Parent.";
                    }
                    else { wo.WaitLoading(); }
                }
                else
                {
                    error = "Can not get button WO_Parent";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_060_Verify_WO_State()
        {
            try
            {
                combobox = wo.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "Closed Complete";
                    flag = combobox.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        error = "Invalid State value. Expected: [" + temp + "]. Runtime: [" + combobox.Text + "]";
                        flagExit = false;
                    }
                }
                else
                {
                    error = "Can not get combobox WO State.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_061_01_GlobalSearch_Incident()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------

                flag = globalSearch.GlobalSearchItem(incidentId, true);

                if (flag)
                {
                    inc.WaitLoading();
                }
                else { error = "Error when set text into textbox search."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_061_02_Verify_Incident_State_Resolved()
        {
            try
            {
                combobox = wo.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "Resolved";
                    flag = combobox.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        error = "Invalid State value. Expected: [" + temp + "]. Runtime: [" + combobox.Text + "]";
                        flagExit = false;
                    }
                }
                else
                {
                    error = "Cannot get State combobox.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_062_Logout()
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
