using NUnit.Framework;
using System;
using System.Reflection;
using System.Threading;

namespace Incident
{
    class INC_create_prob_using_rr_15
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
            System.Console.WriteLine("Finished - Problem Id: " + problemId);

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
        
        //------------------------------------------------------------------
        Auto.Login login = null;
        Auto.Home home = null;
        Auto.Incident inc = null;
        Auto.Problem prb = null;
        Auto.IncidentList inclist = null;
        Auto.ProblemList prblist = null;
        //------------------------------------------------------------------
        string incidentId;
        string problemId;
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
                prb = new Auto.Problem(Base, "Problem");
                prblist = new Auto.ProblemList(Base, "Problem List");
                inclist = new Auto.IncidentList(Base, "Incident list");
                //------------------------------------------------------------------
                incidentId = string.Empty;
                problemId = string.Empty;
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
        public void Step_007_01_PopulateCallerName()
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
                    Console.WriteLine("-*-[Store]: Incident Id:(" + incidentId + ")");
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
                        else { Thread.Sleep(3000); }
                    }
                    else
                    {
                        error = "Cannot get Caller textbox.";
                    }
                }
                else { error = "Cannot get Number textbox."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_007_02_Verify_Company_Value()
        {
            try
            {
                lookup = inc.Lookup_Company;
                flag = lookup.Existed;
                if (flag)
                {
                    string temp = Base.GData("Company");
                    flag = lookup.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid Company value. Expected: [" + temp + "]. Runtime: [" + lookup.Text + "]";
                    }
                }
                else { error = "Cannot get Company lookup."; }
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
                        error = "Cannot populate Business Service value.";
                    }
                }
                else
                {
                    error = "Cannot get Business Service lookup .";
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
                    if(!flag)
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
        public void Step_011_SaveIncident()
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
        public void Step_012_01_PopulatedAssignmentGroup()
        {
            try
            {
                string temp = Base.GData("ServiceDeskGroup");
                lookup = inc.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag)
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
        public void Step_012_02_SaveIncident()
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
        public void Step_013_01_ValidateImpact()
        {
            try
            {
                combobox = inc.Combobox_Impact;
                string temp = Base.GData("Impact");
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.VerifyCurrentText(temp, true);
                    if (!flag)
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
                    flag = combobox.VerifyCurrentText(temp, true);
                    if (!flag)
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
                    flag = combobox.VerifyCurrentText(temp, true);
                    if (!flag)
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
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_014_ImpersonateUser_Resolver()
        {
            try
            {                
                string temp = Base.GData("Resolver");
                string loginUser = Base.GData("UserFullName");
                flag = home.ImpersonateUser(temp, true, loginUser);
                if (!flag)
                {
                    error = "Cannot impersonate user [" + temp + "].";
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
        public void Step_015_SystemSetting()
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
                        if(!flag)
                        {
                            error = "Error when search and open incident (id:" + incidentId + ")";
                        }
                    }
                    else
                    {
                        flagExit = false;
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
                    error = "Cannot get Assigned To lookup";
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
        public void Step_019_CreateProblem()
        {
            try
            {
                flag = inc.CreateProblem();
                if (!flag)
                {
                    error = "Cannot create problem";
                }
                else { prb.WaitLoading(); }
            } 
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_020_00_Get_ProblemNumber()
        {
            try
            {
                textbox = prb.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    problemId = textbox.Text;
                }
                else { error = "Cannot get Number textbox."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_020_01_Verify_Company_Value()
        {
            try
            {
                lookup = prb.Lookup_Company;
                flag = lookup.Existed;
                if (flag)
                {
                    string temp = Base.GData("Company");
                    flag = lookup.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid Company value. Expected: [" + temp + "]. Runtime: [" + lookup.Text + "]";
                    }
                }
                else { error = "Cannot get Company lookup."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_020_02_Verify_Priority_Value()
        {
            try
            {
                combobox = prb  .Combobox_Priority;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = Base.GData("Priority");
                    flag = combobox.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid Priority value. Expected: [" + temp + "]. Runtime: [" + combobox.Text + "]";
                    }
                }
                else { error = "Cannot get Priority lookup."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_020_03_Verify_ProblemStatement_Value()
        {
            try
            {
                textbox = prb.Textbox_ProblemStatement;
                flag = textbox.Existed;
                if (flag)
                {
                    string temp = Base.GData("ShortDescription");
                    flag = textbox.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid Problem Statement value. Expected: [" + temp + "]. Runtime: [" + textbox.Text + "]";
                    }
                }
                else { error = "Cannot get Problem Statement textbox."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_020_04_Verify_AssignmentGroup_Value()
        {
            try
            {
                lookup = prb.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    string temp = "";
                    if (!lookup.Text.Equals(temp))
                    {
                        flag = false;
                        flagExit = false;
                        error = "Invalid Assignment group value. Expected: [" + temp + "]. Runtime: [" + lookup.Text + "]";
                    }
                }
                else { error = "Cannot get Assignment group lookup."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_020_05_Verify_State_Value()
        {
            try
            {
                combobox = prb.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "Open";
                    flag = combobox.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid Statep value. Expected: [" + temp + "]. Runtime: [" + lookup.Text + "]";
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
        ////-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_021_01_Populate_Impact()
        {
            try
            {
                string temp = Base.GData("PrbImpact");
                combobox = prb.Combobox_Impact;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                        error = "Error when select item (" + temp + ")";
                }
                else error = "Cannot get combobox impact.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_021_02_UpdateProblem()
        {
            try
            {
                flag = prb.Update();
                if (!flag)
                {
                    error = "Error when update Problem.";
                }
                else { inc.WaitLoading(); }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ///-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_022_SaveIncident()
        {
            try
            {
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
        ///-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_023_024_SearchAndOpenProblem()
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
                //-------------------------------------------------------------------------------------
                flag = home.LeftMenuItemSelect("Problem", "Open");
                if (flag)
                {
                    prblist.WaitLoading();
                    temp = prblist.Label_Title.Text;
                    flag = temp.Equals("Problems");
                    if (flag)
                    {
                        flag = prblist.SearchAndOpen("Number", problemId, "Number=" + problemId, "Number");
                        if (!flag) error = "Error when search and open problems (id:" + problemId + ")";
                        else prb.WaitLoading();
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
        ///-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_025_00_Verify_Priority()
        {          
            //Priority of Problem is a 3
            try
            {
                combobox = prb.Combobox_Priority;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = Base.GData("Priority");
                    flag = combobox.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid Priority value. Expcted: [" + temp + "]. Runtime: [" + combobox.Text + "]";
                    }
                }
                else
                {
                    error = "Cannot get Priority combobox";
                }
            }
            catch (Exception wex)
            {
                flag = false;
                error = wex.Message;
            }
                
        }
        ///-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_025_01_Verify_ProblemStatement()
        {
            try
            {
                string temp = Base.GData("ShortDescription");
                textbox = prb.Textbox_ProblemStatement;
                flag = textbox.Existed;
                if (flag)
                {
                    flag = textbox.VerifyCurrentText(temp, true);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid Problem Statement value. Expected: [" + temp + "]. Runtime: [" + textbox.Text + "]";
                    }
                }
                else
                {
                    error = "Cannot get Problem Statement textbox.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ///-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_025_02_Verify_AssignmentGroup()
        {
            try
            {                
                lookup = prb.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    string temp = lookup.Text;
                    if (!temp.Equals(""))
                    {
                        flag = false;
                        flagExit = false;
                        error = "Invalid Assignment Group value. Expected: [BLANK]. Runtime: [" + temp +"].";
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
        ///-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_026_Verify_Incident_RelatedRecord()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId == null || incidentId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input Incident Id.");
                    addPara.ShowDialog();
                    incidentId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-------------------------------------------------------------------------------------
                string condition = "Number=" + incidentId;
                flag = prb.RelatedTableVerifyRow("Incidents", condition);
                if (!flag)
                {
                    flagExit = false;
                    error = "Cannot verify row with condition: " + condition;
                }
                            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ///-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_027_Logout()
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
