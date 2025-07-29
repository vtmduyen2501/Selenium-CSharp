using NUnit.Framework;
using System;
using System.Reflection;
using System.Threading;

namespace Incident
{
    class INC_create_prob_using_hdr_16
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
                    Console.WriteLine("-*-[Store]: Incident Id:(" + incidentId + ")");
                    string temp = Base.GData("Caller");
                    lookup = inc.Lookup_Caller;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.Select(temp);
                        if(!flag)
                        {
                            error = "Cannot populate caller value";
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
                    flag = combobox.VerifyCurrentText(temp, true);
                    if (flag)
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
        public void Step_019_01_CreateProblem()
        {
            try
            {
                flag = inc.CreateProblem();
                if (!flag)
                {
                    error = "Cannot create problem";
                }
                else
                {
                    prb.WaitLoading();
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
        public void Step_019_02_GetProblemNumber()
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
        public void Step_020_ValidateProblemInfomation()
        {
            // Problem Company = Incident Company
            try
            {
                string temp = Base.GData("Company");
                lookup = prb.Lookup_Company;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.VerifyCurrentText(temp, true);
                    if (!flag)                 
                    {
                        error = "Problem company is different with incident company.";
                    }
                }
                else
                {
                    error = "Cannot find the lookup company.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }

            // Problem Priority = Incident Priority
            try
            {
                string temp = Base.GData("Priority");
                combobox = prb.Combobox_Priority;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.VerifyCurrentText(temp, true);
                    if (!flag)
                    {
                        error = "Priority is different.";
                    }
                }
                else { error = "Not found combobox Priority"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
            
            // Problem statement = Incident Short description
            try
            {
                string temp = Base.GData("ShortDescription");
                textbox = prb.Textbox_ProblemStatement;
                flag = textbox.Existed;
                if (flag)
                {
                    flag = textbox.VerifyCurrentText(temp, true);
                    if(!flag)
                    {
                        error = "Problem statement different with incident short description.";
                    }
                }
                else
                {
                    error = "Not found textbox Problem Statement.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }

            // Assignment group = < blank >
            try
            {
                lookup = prb.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    if (lookup.Text.Equals(""))
                    {
                        System.Console.WriteLine("Assignment group is empty.");
                    }
                    else
                    {
                        error = "Assignment group is not empty.";
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

            // State = Open
            try
            {
                combobox = prb.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.VerifyCurrentText("Open", true);
                    if (!flag)
                    {
                        error = "State is not Open";
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
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test] 
        public void Step_021_ValidateIncidentRelatedToProblem()
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
                flag = prb.RelatedTableVerifyRow("Incidents", "Number=" + incidentId, false);
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
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_022_PopulateAssignmentGroupProblem()
        {
            try
            {
                string temp = Base.GData("ResolverGroup");
                lookup = prb.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag)
                    {
                        error = "Cannot select the Assignment group.";
                    }
                }
                else
                {
                    error = "Cannot lookup the Assignment group.";
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
        public void Step_023_PopulateImpact()
        {
            try
            {
                string temp = Base.GData("PrbImpact");
                combobox = inc.Combobox_Impact;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag)
                    {
                        error = "Cannot select item: [" + temp + "].";
                    }
                }
                else { error = "Cannot get Impact combobox."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_024_Update()
        {
            try
            {
                flag = prb.Update();
                if (!flag) error = "Error when update Problem.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        ////-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_025_SaveTheIncident()
        {
            try
            {
                inc.WaitLoading();
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
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_026_Logout()
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
