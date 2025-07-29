using NUnit.Framework;
using System;
using System.Reflection;
using System.Threading;

namespace Incident
{
    [TestFixture]
    public class INC_relate_problem_14
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
        Auto.otab tab;
        Auto.otextarea textarea;
        Auto.otable table;
        //------------------------------------------------------------------
        Auto.Login login = null;
        Auto.Home home = null;
        Auto.Incident inc = null;
        Auto.Problem prb = null;
        Auto.KnowledgeSearch knls = null;
        Auto.IncidentList inclist = null;
        Auto.ProblemList prolist = null;
        Auto.EmailList emailList = null;
        //------------------------------------------------------------------
        string incidentId;
        string problemId;
        string prbState;

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
                knls = new Auto.KnowledgeSearch(Base);
                inclist = new Auto.IncidentList(Base, "Incident list");
                prolist = new Auto.ProblemList(Base, "Problem list");
                emailList = new Auto.EmailList(Base, "Email list");
                //------------------------------------------------------------------
                incidentId = string.Empty;
                problemId = string.Empty;
                prbState = string.Empty;
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        [Test]
        public void Pre_001_OpenSystem()
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
        public void Pre_002_Login()
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
        public void Pre_003_ImpersonateUser_SDA1()
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
        public void Pre_004_SystemSetting()
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
        public void Pre_005_OpenNewProblem()
        {
            //Try to create problem.
            try
            {
                flag = home.LeftMenuItemSelect("Problem", "Create New");
                if (flag)
                    prb.WaitLoading();
                else
                    error = "Error when create new Problem.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Pre_006_Populate_Company()
        {
            try
            {
                string temp = Base.GData("Company");
                lookup = prb.Lookup_Company;
                flag = lookup.Existed;
                if (flag)
                {
                    lookup.SetText(temp,true);
                    //flag = lookup.VerifyCurrentText(temp, true);
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
        public void Pre_007_PopulateProblemCategory()
        {
            try
            {
                string temp = Base.GData("PrbCat");
                combobox = prb.Combobox_Category;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        prb.WaitLoading();
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
        public void Pre_008_PopulateProblemSubCategory()
        {
            try
            {
                string temp = Base.GData("PrbSubCat");
                combobox = prb.Combobox_SubCategory;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = prb.VerifyHaveItemInComboboxList("subcategory", temp);
                    if (flag)
                    {
                        flag = combobox.SelectItem(temp);
                        if (flag)
                        {
                            prb.WaitLoading();
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
        public void Pre_009_01_PopulateImpact()
        {
            try
            {
                string temp = Base.GData("PrbImpact");
                combobox = prb.Combobox_Impact;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag) { error = "Cannot populate problem impact value."; }
                }
                else { error = "Cannot get combobox problem impact."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Pre_009_02_PopulateStatement()
        {
            try
            {
                string temp = Base.GData("ProblemStatement");
                textbox = prb.Textbox_ProblemStatement;
                flag = textbox.Existed;
                if (flag)
                {
                    flag = textbox.SetText(temp);
                    if (!flag) { error = "Cannot populate problem statement value."; }
                }
                else { error = "Cannot get textbox problem statement."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-------------------------------------------------------------------------------------------------
        [Test]
        public void Pre_009_03_Populate_More_Fields_If_Need()
        {
            try
            {
                string temp = Base.GData("Populate_More_Fields");
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
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Pre_010_SaveProblem()
        {
            problemId = prb.Textbox_Number.Text;
            prbState = prb.Combobox_State.Text;
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
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_001_OpenNewIncident()
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
        public void Step_002_01_PopulateCallerName()
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
        public void Step_002_02_Verify_Company()
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
        public void Step_002_03_Verify_CallerEmail()
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
        public void Step_003_PopulateBusinessService()
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
        public void Step_004_01_PopulateCategory()
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
        public void Step_004_02_PopulateSubCategory()
        {
            try
            {
                inc.WaitLoading();
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
        public void Step_005_PopulateShortDescription()
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
        public void Step_006_SaveIncident()
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
        public void Step_007_PopulateAssignmentGroup_SDG()
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
        public void Step_008_ChangePriority()
        {
            try
            {
                string temp = "3 - Medium";
                combobox = inc.Combobox_Impact;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        inc.WaitLoading();
                        combobox = inc.Combobox_Urgency;
                        flag = combobox.Existed;
                        if (flag)
                        {
                            flag = combobox.SelectItem(temp);
                            if (flag)
                            {
                                inc.WaitLoading();
                            }
                            else { error = "Cannot populate urgency value"; }
                        }
                        else { error = "Cannot get combobox urgency"; }
                    }
                    else { error = "Cannot populate impact value."; }
                }
                else { error = "Cannot get combobox impact."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_009_UpdateIncident()
        {
            try
            {
                flag = inc.Update();
                if (!flag) { error = "Error when update incident."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_010_ImpersonateUser_RV()
        {
            try
            {
                string temp = Base.GData("Resolver_UserID");
                if (temp.ToLower() != "no")
                    temp = Base.GData("Resolver") + ";" + temp;
                string rootuser = Base.GData("UserFullName");
                flag = home.ImpersonateUser(temp, true, rootuser);
                if (!flag) error = "Error when impersonate resolver user (" + Base.GData("Resolver") + ")";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_011_SystemSetting()
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
        public void Step_012_SearchAndOpenIncident()
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
        public void Step_013_AssignToSelf()
        {
            try
            {
                lookup = inc.Lookup_AssignedTo;
                flag = lookup.Existed;
                if (flag)
                {
                    string temp = Base.GData("Resolver");
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
        public void Step_014_Open_RelatedRecordsTab()
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
        public void Step_015_PopulateProblem()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (problemId == null || problemId == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input Problem Id.");
                    addPara.ShowDialog();
                    problemId = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                lookup = inc.Lookup_Problem;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(problemId);
                    if (!flag) { error = "Cannot populate problem value."; }
                }
                else
                    error = "Cannot get lookup problem.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_016_UpdateIncident()
        {
            try
            {
                flag = inc.Update();
                if (!flag) { error = "Error when update incident."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_017_018_SearchAndOpenProblem()
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

                flag = home.LeftMenuItemSelect("Problem", "Open");
                if (flag)
                {
                    prolist.WaitLoading();
                    temp = prolist.Label_Title.Text;
                    flag = temp.Equals("Problems");
                    if (flag)
                    {
                        flag = prolist.SearchAndOpen("Number", problemId, "Number=" + problemId, "Number");
                        if (!flag) error = "Error when search and open problems (id:" + problemId + ")";
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
        public void Step_019_Open_RelatedIncidentsTabAndOpenIncident()
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
                //table = prb.GRelatedTable("Incidents");
                string conditions = "Number=" + incidentId;
                //flag = prb.RelatedTableSearchAndOpenRecord("Incidents", "Number", incidentId, conditions, "Number");
                flag = prb.RelatedTableOpenRecord("Incidents", conditions, "Number");
                if (flag)
                {
                    inc.WaitLoading();
                }
                else { error = "Cannot get related table Incidents."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }

        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_020_ResolveIncident()
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
        public void Step_021_Open_ClosureTab()
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
        public void Step_022_PopulateCloseCode()
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
        public void Step_023_PopulateCloseNotes()
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
        public void Step_024_UpdateIncident()
        {
            try
            {
                flag = inc.Update();
                if (!flag) { error = "Error when update incident."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_025_ValidateIncident()
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

                table = prb.GRelatedTable("Incidents");
                flag = table.Existed;
                if (flag)
                {
                    temp = "Number=" + incidentId + "|State=Resolved";
                    //flag = table.FindRow(temp);
                    flag = inc.RelatedTableVerifyRow("Incidents", temp);
                    if (flag)
                    {
                        Console.WriteLine(" Confirmed that the incident have been resolved successfully...");
                    }
                    else
                    {
                        flagExit = false;
                        error = "Not found Incidents: " + temp;
                    }
                }
                else { error = "Cannot get table Incidents."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_026_ValidateProblem()
        {
            try
            {
                combobox = prb.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.VerifyCurrentText("Resolved");
                    if (flag)
                    {
                        flag = false;
                        error = "The Problem should not be resolved";
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
        //-----------------------------------------------------------------------------------------------------------------------------------
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
        //-----------------------------------------------------------------------------------------------------------------------------------
        #endregion End - Scenario of test case (NEED TO UPDATE)
    }
}
