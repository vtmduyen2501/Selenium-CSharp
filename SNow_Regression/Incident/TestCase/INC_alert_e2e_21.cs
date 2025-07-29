using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Reflection;
using System.Threading;
namespace Incident
{
    [TestFixture]
    public class INC_alert_e2e_21
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

            System.Console.WriteLine("Finished - Incident Id 1: " + incidentId1);
            System.Console.WriteLine("Finished - Incident Id 2: " + incidentId2);
            System.Console.WriteLine("Finished - Incident Id 3: " + incidentId3);
            System.Console.WriteLine("Finished - Incident Alert 1: " + incidentAlert1);
            System.Console.WriteLine("Finished - Incident Alert 2: " + incidentAlert2);
            System.Console.WriteLine("Finished - Incident Alert 3: " + incidentAlert3);
            System.Console.WriteLine("Finished - Incident Alert 4: " + incidentAlert4);
            System.Console.WriteLine("Finished - Change Id: " + changeId);
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
        
        Auto.oelement ele;
        Auto.odatetime datetime;
        //------------------------------------------------------------------
        Auto.Login login = null;
        Auto.Home home = null;
        Auto.Incident inc = null;
        Auto.Change chg = null;
        Auto.Problem prb = null;
        Auto.KnowledgeSearch knls = null;
        Auto.IncidentList inclist = null;
        Auto.EmailList emailList = null;
        Auto.Member member = null;
        //------------------------------------------------------------------
        string incidentId1, incidentId2, incidentId3;
        string incidentAlert1, incidentAlert2, incidentAlert3, incidentAlert4;
        string changeId, problemId;

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
                prb = new Auto.Problem(Base, "Problem");
                knls = new Auto.KnowledgeSearch(Base);
                inclist = new Auto.IncidentList(Base, "Incident list");
                emailList = new Auto.EmailList(Base, "Email list");
                member = new Auto.Member(Base);
                //------------------------------------------------------------------
                incidentId1 = string.Empty;
                incidentId2 = string.Empty;
                incidentId3 = string.Empty;
                incidentAlert1 = string.Empty;
                incidentAlert2 = string.Empty;
                incidentAlert3 = string.Empty;
                incidentAlert4 = string.Empty;
                changeId = string.Empty;
                problemId = string.Empty;

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
        public void Step_005_OpenNewIncident_P2()
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
                    incidentId1 = textbox.Text;
                    Console.WriteLine("-*-[Store]: Incident Id 1:(" + incidentId1 + ")");
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
        public void Step_007_01_PopulateCategory()
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
        public void Step_007_02_PopulateSubCategory()
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
        public void Step_008_ChangePriority()
        {
            try
            {
                string temp = "2 - High";
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
                        else { error = "Cannot get combobox urgency";}
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
                string temp = Base.GData("ShortDescription_P2");
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
        public void Step_011_AddACI()
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
        public void Step_012_SaveIncidentAndVerifyAlert()
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
                        string temp = "This Incident Record is now a Major Incident!Please contact your Service Desk to make sure it gets the attention it needs.";
                        string runtime = alert.Text.Replace("\r\n", "");
                        if (!runtime.Equals(temp))
                        {
                            flag = false;
                            flagExit = false;
                            error = "Invalid alert message. Runtime:(" + runtime + "). Expexted:(" + temp + ")";
                        }
                        alert.Accept();
                        inc.WaitLoading();
                    }
                    catch
                    {
                        flag = false;
                        flagExit = false;
                        error = "Alert is not visible.";
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
        public void Step_013_PopulateAssignmentGroup_RG()
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
        public void Step_014_PopulateResolver()
        {
            try
            {
                string temp = Base.GData("Resolver");
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
        public void Step_015_SaveIncident()
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

        /// <summary>
        /// Create a New P3 Incident		
        /// </summary>
        [Test]
        public void Step_016_01_OpenNewIncident_P3()
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
        public void Step_016_02_01_PopulateCallerName()
        {
            try
            {
                textbox = inc.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    textbox.Click();
                    //-- Store incident id
                    incidentId2 = textbox.Text;
                    Console.WriteLine("-*-[Store]: Incident Id 2:(" + incidentId2 + ")");
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
        public void Step_016_02_02_Verify_Company()
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
        public void Step_016_02_03_Verify_CallerEmail()
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
        public void Step_016_03_PopulateCategory()
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
        public void Step_016_04_PopulateSubCategory()
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
        public void Step_016_05_ChangePriority()
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
        public void Step_016_06_PopulateContactType()
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
        public void Step_016_07_PopulateShortDescription()
        {
            try
            {
                string temp = Base.GData("ShortDescription_P3");
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
        public void Step_016_08_AddACI()
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
        public void Step_016_09_SaveIncident()
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
        public void Step_016_10_PopulateAssignmentGroup_RG()
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
        public void Step_016_11_PopulateResolver()
        {
            try
            {
                string temp = Base.GData("Resolver");
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
        public void Step_016_12_SaveIncident()
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
        public void Step_017_018_SearchAndOpenIncident_INC1()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId1 == null || incidentId1 == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id 1.");
                    addPara.ShowDialog();
                    incidentId1 = addPara.value;
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
                        flag = inclist.SearchAndOpen("Number", incidentId1, "Number=" + incidentId1, "Number");
                        if (!flag) error = "Error when search and open incident (id:" + incidentId1 + ")";
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
        public void Step_019_01_ClickCreateIncidentAlertLink()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId1 == null || incidentId1 == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id 1.");
                    addPara.ShowDialog();
                    incidentId1 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                Thread.Sleep(3000);
                ele = inc.GRelatedLink("Create Incident Alert");
                flag = ele.Existed;
                if (flag)
                {
                    ele.Click();
                    inc.WaitLoading();
                    //-------------------------------------------------------------------------
                }
                else { error = "The Create Incident Alert link does not appeard"; }

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_019_02_VerifyDataFields()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId1 == null || incidentId1 == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id 1.");
                    addPara.ShowDialog();
                    incidentId1 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                    textbox = inc.Textbox_Number;
                    flag = textbox.Existed;
                    if (flag)
                    {
                        //Get incident alert number
                        textbox.Click();
                        incidentAlert1 = textbox.Text;

                        //Verify data of fields
                        lookup = inc.Lookup_Alert_SourceIncident;
                        flag = lookup.Existed;
                        if (flag)
                        {
                            if (lookup.Text == incidentId1)
                            {
                                combobox = inc.Combobox_State;
                                flag = combobox.Existed;
                                if (flag)
                                {
                                    if (combobox.VerifyCurrentText("New"))
                                    {
                                        combobox = inc.Combobox_Priority;
                                        flag = combobox.Existed;
                                        if (flag)
                                        {
                                            if (combobox.VerifyCurrentText("2 - High"))
                                            {
                                                combobox = inc.Combobox_Alert_TEMNeeded_Required;
                                                flag = combobox.Existed;
                                                if (flag)
                                                {
                                                    if (combobox.VerifyCurrentText("No"))
                                                    {
                                                        lookup = inc.Lookup_AssignmentGroup;
                                                        flag = lookup.Existed;
                                                        temp = Base.GData("IncAlert_AssignmentGroup");
                                                        if (flag)
                                                        {
                                                            if (lookup.Text == temp)
                                                            {
                                                                lookup = inc.Lookup_AssignedTo;
                                                                flag = lookup.Existed;
                                                                if (flag)
                                                                {
                                                                    if (lookup.Text != "")
                                                                    {
                                                                        error = "The value of assign to is not correct";
                                                                    }
                                                                }
                                                                else { error = "Cannot get assign to control"; }
                                                            }
                                                            else { error = "The value of assignment group is not correct"; }
                                                        }
                                                        else { error = "Cannot get assignment group control"; }
                                                    }
                                                    else { error = "The value of tem needed is not correct"; }
                                                }
                                                else { error = "Cannot get TEM needed control"; }
                                            }
                                            else { error = "The value of priority is not correct"; }
                                        }
                                        else { error = "Cannot get priority control"; }
                                    }
                                    else { error = "The value of state is not correct."; }
                                }
                                else { error = "Cannot get state control"; }
                            }
                            else { error = "The value of source incident is not correct"; }
                        }
                        else { error = "Cannot get source incident control"; }
                    }
                    else { error = "Cannot get  incident alert number control"; }

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------


        [Test]
        public void Step_020_PopulateTEMneeded()
        {
            try
            {
                combobox = inc.Combobox_Alert_TEMNeeded_Required;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem("Yes");
                    if (flag)
                    {
                        inc.WaitLoading();
                        combobox = inc.Combobox_Alert_TEMNeeded_Joined;
                        flag = combobox.Existed;
                        if (flag)
                        {
                            flag = combobox.VerifyCurrentText("No");
                            if (!flag)
                            {
                                error = "The value of tem need joined value is not correct";
                            }
                        }
                        else { error = "Cannot get combobox tem need joined control  "; }
                    }
                    else { error = "Cannot select tem needed value"; }
                }
                else { error = "Cannot get combobox tem need required control"; }

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_021_PopulateDescription_EventType()
        {
            try
            {
                tab = inc.GTab("Details");
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = inc.GTab("Details", true);
                    i++;
                }
                flag = tab.Header.Click();
                if (flag)
                {
                    textarea = inc.Textarea_Alert_Description;
                    flag = textarea.Existed;
                    if (flag)
                    {
                        string temp = Base.GData("IncAlert_Description_2");
                        flag = textarea.SetText(temp);
                        if (flag)
                        {
                            combobox = inc.Combobox_Alert_EventType;
                            flag = combobox.Existed;
                            if (flag)
                            {
                                temp = Base.GData("IncAlert_EventType_2");
                                flag = combobox.SelectItem(temp);
                                if (!flag)
                                {
                                    error = "Cannot populate event type";
                                }
                            }
                            else { error = "Cannot get combobox event type"; }
                        }
                        else { error = "Cannot populuate incident alert description 1"; }
                    }
                    else { error = "Cannot get description control"; }
                }
                else { error = "Cannot click tab (Details)"; }
                
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_022_PopulateBusinessSerivceImpact()
        {
            try
            {
                string temp = "Yes";
                combobox = inc.Combobox_Alert_BusinessServiceImpact;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        inc.WaitLoading();
                        textbox = inc.Textbox_Alert_BusinessServiceImpacted;
                        flag = textbox.Existed;
                        if (flag)
                        {
                            temp = Base.GData("IncAlert_BS_Impact_2"); 
                            flag = textbox.SetText(temp);
                            if (!flag)
                            {
                                error = "Cannot populate BS/Impacted value"; 
                            }
                        }
                        else { error = "Cannot get BS/Impacted control"; }
                    }
                    else { error = "Cannot populate BS/Impact value."; }
                }
                else
                {
                    error = "Cannot get BS/Impact control.";
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
        public void Step_023_VerifyShortDesciption()
        {
            try
            {
                string temp = Base.GData("ShortDescription_P2");
                textbox = inc.Textbox_ShortDescription;
                flag = textbox.Existed;
                if (flag)
                {
                    if (textbox.Text != temp)
                    {
                        flagExit = false;
                        error = "The value of short description is not correct";
                    }
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
        public void Step_024_PopulateAssignee()
        {
            try
            {
                string temp = Base.GData("IncAlert_Assignee_2");
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
        public void Step_025_SaveIncident()
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
        public void Step_026_27_SearchAndOpenIncident_INC2()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId2 == null || incidentId2 == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id 2.");
                    addPara.ShowDialog();
                    incidentId2 = addPara.value;
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
                        flag = inclist.SearchAndOpen("Number", incidentId2, "Number=" + incidentId2, "Number");
                        if (!flag) error = "Error when search and open incident (id:" + incidentId2 + ")";
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
        public void Step_028_Open_Incident_Communication_Plans_tab()
        {
            try
            {
                tab = inc.GTab("Incident Communication Plans");
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = inc.GTab("Incident Communication Plans", true);
                    i++;
                }
                flag = tab.Header.Click();
                if (flag)
                {
                    flag = tab.ClickNew();
                    if (flag)
                    {
                        inc.WaitLoading();                        
                    }
                    else { error = "Cannot click new button "; }
                }
                else { error = "Cannot click on tab (Incident Communication Plans)"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_029_VerifyDataFields()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId2 == null || incidentId2 == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id 2.");
                    addPara.ShowDialog();
                    incidentId2 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                textbox = inc.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    //Get incident alert number
                    textbox.Click();
                    incidentAlert2 = textbox.Text;

                    //Verify data of fields
                    lookup = inc.Lookup_Alert_SourceIncident;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        if (lookup.Text == incidentId2)
                        {
                            combobox = inc.Combobox_State;
                            flag = combobox.Existed;
                            if (flag)
                            {
                                if (combobox.VerifyCurrentText("New"))
                                {
                                    combobox = inc.Combobox_Priority;
                                    flag = combobox.Existed;
                                    if (flag)
                                    {
                                        if (combobox.VerifyCurrentText("3 - Medium"))
                                        {
                                            combobox = inc.Combobox_Alert_TEMNeeded_Required;
                                            flag = combobox.Existed;
                                            if (flag)
                                            {
                                                if (combobox.VerifyCurrentText("No"))
                                                {
                                                    lookup = inc.Lookup_AssignmentGroup;
                                                    flag = lookup.Existed;
                                                    temp = Base.GData("IncAlert_AssignmentGroup");
                                                    if (flag)
                                                    {
                                                        if (lookup.Text == temp)
                                                        {
                                                            lookup = inc.Lookup_AssignedTo;
                                                            flag = lookup.Existed;
                                                            if (flag)
                                                            {
                                                                if (lookup.Text != "")
                                                                {
                                                                    error = "The value of assign to is not correct";
                                                                }
                                                            }
                                                            else { error = "Cannot get assign to control"; }
                                                        }
                                                        else { error = "The value of assignment group is not correct"; }
                                                    }
                                                    else { error = "Cannot get assignment group control"; }
                                                }
                                                else { error = "The value of tem needed is not correct"; }
                                            }
                                            else { error = "Cannot get TEM needed control"; }
                                        }
                                        else { error = "The value of priority is not correct"; }
                                    }
                                    else { error = "Cannot get priority control"; }
                                }
                                else { error = "The value of state is not correct."; }
                            }
                            else { error = "Cannot get state control"; }
                        }
                        else { error = "The value of source incident is not correct"; }
                    }
                    else { error = "Cannot get source incident control"; }
                }
                else { error = "Cannot get  incident alert number control"; }

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_030_PopulateTEMneeded()
        {
            try
            {
                combobox = inc.Combobox_Alert_TEMNeeded_Required;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem("Yes");
                    if (flag)
                    {
                        inc.WaitLoading();
                        combobox = inc.Combobox_Alert_TEMNeeded_Joined;
                        flag = combobox.Existed;
                        if (flag)
                        {
                            flag = combobox.VerifyCurrentText("No");
                            if (!flag)
                            {
                                error = "The value of tem need joined value is not correct";
                            }
                        }
                        else { error = "Cannot get combobox tem need joined control  "; }
                    }
                    else { error = "Cannot select tem needed value"; }
                }
                else { error = "Cannot get combobox tem need required control"; }

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_031_PopulateDescription_EventType()
        {
            try
            {
                tab = inc.GTab("Details");
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = inc.GTab("Details", true);
                    i++;
                }
                flag = tab.Header.Click();
                if (flag)
                {
                    textarea = inc.Textarea_Alert_Description;
                    flag = textarea.Existed;
                    if (flag)
                    {
                        string temp = Base.GData("IncAlert_Description_3");
                        flag = textarea.SetText(temp);
                        if (flag)
                        {
                            combobox = inc.Combobox_Alert_EventType;
                            flag = combobox.Existed;
                            if (flag)
                            {
                                temp = Base.GData("IncAlert_EventType_3");
                                flag = combobox.SelectItem(temp);
                                if (!flag)
                                {
                                    error = "Cannot populate event type";
                                }
                            }
                            else { error = "Cannot get combobox event type"; }
                        }
                        else { error = "Cannot populuate incident alert description 1"; }
                    }
                    else { error = "Cannot get description control"; }
                }
                else { error = "Cannot click tab (Details)"; }

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_032_PopulateBusinessSerivceImpact()
        {
            try
            {
                string temp = "Yes";
                combobox = inc.Combobox_Alert_BusinessServiceImpact;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        inc.WaitLoading();
                        textbox = inc.Textbox_Alert_BusinessServiceImpacted;
                        flag = textbox.Existed;
                        if (flag)
                        {
                            temp = Base.GData("IncAlert_BS_Impact_3");
                            flag = textbox.SetText(temp);
                            if (!flag)
                            {
                                error = "Cannot populate BS/Impacted value";
                            }
                        }
                        else { error = "Cannot get BS/Impacted control"; }
                    }
                    else { error = "Cannot populate BS/Impact value."; }
                }
                else { error = "Cannot get BS/Impact control."; }                
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_033_VerifyShortDesciption()
        {
            try
            {
                string temp = Base.GData("ShortDescription_P3");
                textbox = inc.Textbox_ShortDescription;
                flag = textbox.Existed;
                if (flag)
                {
                    if (textbox.Text != temp)
                    {
                        flagExit = false;
                        error = "The value of short description is not correct";
                    }
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
        public void Step_034_PopulateAssignee()
        {
            try
            {
                string temp = Base.GData("IncAlert_Assignee_3");
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
        public void Step_035_SaveIncident()
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
        public void Step_036_OpenNewIncidentAlert_P1()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Incident Alert Management", "Create New");
                if (flag)
                    inc.WaitLoading();
                else
                    error = "Error when create new incident alert.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_037_VerifyDataFieldsAndPopulatePriority()
        {
            try
            {
                textbox = inc.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    //Get incident alert number
                    textbox.Click();
                    incidentAlert3 = textbox.Text;

                    //Verify data of fields
                    lookup = inc.Lookup_Alert_SourceIncident;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        if (lookup.Text == "")
                        {
                            combobox = inc.Combobox_State;
                            flag = combobox.Existed;
                            if (flag)
                            {
                                if (combobox.VerifyCurrentText("New"))
                                {
                                    combobox = inc.Combobox_Alert_BusinessServiceImpact;
                                    flag = combobox.Existed;
                                    if (flag)
                                    {
                                        if (combobox.VerifyCurrentText("No"))
                                        {
                                            combobox = inc.Combobox_Alert_TEMNeeded_Required;
                                            flag = combobox.Existed;
                                            if (flag)
                                            {
                                                if (combobox.VerifyCurrentText("No"))
                                                {
                                                    lookup = inc.Lookup_AssignmentGroup;
                                                    flag = lookup.Existed;
                                                    if (flag)
                                                    {
                                                        if (lookup.Text == "")
                                                        {
                                                            lookup = inc.Lookup_AssignedTo;
                                                            flag = lookup.Existed;
                                                            if (flag)
                                                            {
                                                                if (lookup.Text == "")
                                                                {
                                                                    combobox = inc.Combobox_Priority;
                                                                    flag = combobox.Existed;
                                                                    if (flag)
                                                                    {
                                                                        string val = combobox.CurrentValue;
                                                                        if (val == null || val.Trim() == "")
                                                                        {
                                                                            flag = true;
                                                                        }
                                                                        else
                                                                        {
                                                                            flag = false;
                                                                            error = "The value of priority value is not correct.";
                                                                        }
                                                                    }
                                                                    else { error = "Cannot get priority control"; }
                                                                }
                                                                else { error = "The value of assign to is not correct"; }
                                                            }
                                                            else { error = "Cannot get assign to control"; }
                                                        }
                                                        else { error = "The value of assignment group is not correct"; }
                                                    }
                                                    else { error = "Cannot get assignment group control"; }
                                                }
                                                else { error = "The value of tem needed is not correct"; }
                                            }
                                            else { error = "Cannot get TEM needed control"; }
                                        }
                                        else { error = "The value of priority is not correct"; }
                                    }
                                    else { error = "Cannot get priority control"; }
                                }
                                else { error = "The value of state is not correct."; }
                            }
                            else { error = "Cannot get state control"; }
                        }
                        else { error = "The value of source incident is not correct"; }
                    }
                    else { error = "Cannot get source incident control"; }
                }
                else { error = "Cannot get  incident alert number control"; }

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_038_PopulateTEMneeded()
        {
            try
            {
                combobox = inc.Combobox_Alert_TEMNeeded_Required;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem("Yes");
                    if (flag)
                    {
                        inc.WaitLoading();
                        combobox = inc.Combobox_Alert_TEMNeeded_Joined;
                        flag = combobox.Existed;
                        if (flag)
                        {
                            flag = combobox.VerifyCurrentText("No");
                            if (!flag)
                            {
                                error = "The value of tem need joined value is not correct";
                            }
                        }
                        else { error = "Cannot get combobox tem need joined control  "; }
                    }
                    else { error = "Cannot select tem needed value"; }
                }
                else { error = "Cannot get combobox tem need required control"; }

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_039_PopulateShort_Description_EventType()
        {
            try
            {
                tab = inc.GTab("Details");
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = inc.GTab("Details", true);
                    i++;
                }
                flag = tab.Header.Click();
                if (flag)
                {
                    textarea = inc.Textarea_Alert_Description;
                    flag = textarea.Existed;
                    if (flag)
                    {
                        string temp = Base.GData("IncAlert_Description_1");
                        flag = textarea.SetText(temp);
                        if (flag)
                        {
                            combobox = inc.Combobox_Alert_EventType;
                            flag = combobox.Existed;
                            if (flag)
                            {
                                temp = Base.GData("IncAlert_EventType_1");
                                flag = combobox.SelectItem(temp);
                                if (flag)
                                {
                                    textbox = inc.Textbox_ShortDescription;
                                    flag = textbox.Existed;
                                    if (flag)
                                    {
                                         temp = Base.GData("ShortDescription_P1");
                                         flag = textbox.SetText(temp);
                                         if (flag) 
                                         {
                                             combobox = inc.Combobox_Priority;
                                             flag = combobox.Existed;
                                             if (flag)
                                             {
                                                 temp = "1 - Critical";
                                                 flag = combobox.SelectItem(temp);
                                                 if (!flag)
                                                 {
                                                     error = "Cannot populate priority value";
                                                 }
                                             }
                                             else { error = "Cannot get combobox priority control."; }
                                         }
                                         else { error = "Cannot populate short description value"; }
                                    }
                                    else { error = "Cannot get short description control"; }
                                }
                                else { error = "Cannot populate event type"; }
                            }
                            else { error = "Cannot get combobox event type"; }
                        }
                        else { error = "Cannot populuate incident alert description 1"; }
                    }
                    else { error = "Cannot get description control"; }
                }
                else { error = "Cannot click tab (Details)"; }

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_040_PopulateBusinessSerivceImpact()
        {
            try
            {
                string temp = "Yes";
                combobox = inc.Combobox_Alert_BusinessServiceImpact;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        inc.WaitLoading();
                        textbox = inc.Textbox_Alert_BusinessServiceImpacted;
                        flag = textbox.Existed;
                        if (flag)
                        {
                            temp = Base.GData("IncAlert_BS_Impact_1");
                            flag = textbox.SetText(temp);
                            if (!flag)
                            {
                                error = "Cannot populate BS/Impacted value";
                            }
                        }
                        else { error = "Cannot get BS/Impacted control"; }
                    }
                    else { error = "Cannot populate BS/Impact value."; }
                }
                else { error = "Cannot get BS/Impact control."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_041_PopulateAssignee()
        {
            try
            {
                string temp = Base.GData("IncAlert_Assignee_1");
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
        public void Step_042_01_SaveIncident()
        {
            try
            {
                flag = inc.Save();
                inc.WaitLoading();
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
        public void Step_042_02_VerifyAssignmentGroup()
        {
            try
            {
                lookup = inc.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    string temp = Base.GData("IncAlert_AssignmentGroup");
                    if (lookup.Text != temp)
                    {
                       error = "The value of assignment group is not correct."; 
                    }                  
                }
                else { error = "Cannot get lookup assignment group control"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        
        [Test]
        public void Step_043_OpenNewIncidentAlert_P2()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Incident Alert Management", "Create New");
                if (flag)
                    inc.WaitLoading();
                else
                    error = "Error when create new incident alert.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_044_VerifyDataFieldsAndPopulatePriority()
        {
            try
            {
                textbox = inc.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    //Get incident alert number
                    textbox.Click();
                    incidentAlert4 = textbox.Text;

                    //Verify data of fields
                    lookup = inc.Lookup_Alert_SourceIncident;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        if (lookup.Text == "")
                        {
                            combobox = inc.Combobox_State;
                            flag = combobox.Existed;
                            if (flag)
                            {
                                if (combobox.VerifyCurrentText("New"))
                                {
                                    combobox = inc.Combobox_Alert_BusinessServiceImpact;
                                    flag = combobox.Existed;
                                    if (flag)
                                    {
                                        if (combobox.VerifyCurrentText("No"))
                                        {
                                            combobox = inc.Combobox_Alert_TEMNeeded_Required;
                                            flag = combobox.Existed;
                                            if (flag)
                                            {
                                                if (combobox.VerifyCurrentText("No"))
                                                {
                                                    lookup = inc.Lookup_AssignmentGroup;
                                                    flag = lookup.Existed;
                                                    if (flag)
                                                    {
                                                        if (lookup.Text == "")
                                                        {
                                                            lookup = inc.Lookup_AssignedTo;
                                                            flag = lookup.Existed;
                                                            if (flag)
                                                            {
                                                                if (lookup.Text == "")
                                                                {
                                                                    combobox = inc.Combobox_Priority;
                                                                    flag = combobox.Existed;
                                                                    if (flag)
                                                                    {
                                                                        string val = combobox.CurrentValue;
                                                                        if (val == null || val.Trim() == "")
                                                                        {
                                                                            flag = true;
                                                                        }
                                                                        else
                                                                        {
                                                                            flag = false;
                                                                            error = "The value of priority value is not correct.";
                                                                        }
                                                                    }
                                                                    else { error = "Cannot get priority control"; }
                                                                }
                                                                else { error = "The value of assign to is not correct"; }
                                                            }
                                                            else { error = "Cannot get assign to control"; }
                                                        }
                                                        else { error = "The value of assignment group is not correct"; }
                                                    }
                                                    else { error = "Cannot get assignment group control"; }
                                                }
                                                else { error = "The value of tem needed is not correct"; }
                                            }
                                            else { error = "Cannot get TEM needed control"; }
                                        }
                                        else { error = "The value of priority is not correct"; }
                                    }
                                    else { error = "Cannot get priority control"; }
                                }
                                else { error = "The value of state is not correct."; }
                            }
                            else { error = "Cannot get state control"; }
                        }
                        else { error = "The value of source incident is not correct"; }
                    }
                    else { error = "Cannot get source incident control"; }
                }
                else { error = "Cannot get  incident alert number control"; }

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_045_PopulateTEMneeded()
        {
            try
            {
                combobox = inc.Combobox_Alert_TEMNeeded_Required;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem("Yes");
                    if (flag)
                    {
                        inc.WaitLoading();
                        combobox = inc.Combobox_Alert_TEMNeeded_Joined;
                        flag = combobox.Existed;
                        if (flag)
                        {
                            flag = combobox.VerifyCurrentText("No");
                            if (!flag)
                            {
                                error = "The value of tem need joined value is not correct";
                            }
                        }
                        else { error = "Cannot get combobox tem need joined control  "; }
                    }
                    else { error = "Cannot select tem needed value"; }
                }
                else { error = "Cannot get combobox tem need required control"; }

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_046_PopulateShort_Description_EventType()
        {
            try
            {
                tab = inc.GTab("Details");
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = inc.GTab("Details", true);
                    i++;
                }
                flag = tab.Header.Click();
                if (flag)
                {
                    textarea = inc.Textarea_Alert_Description;
                    flag = textarea.Existed;
                    if (flag)
                    {
                        string temp = Base.GData("IncAlert_Description_2");
                        flag = textarea.SetText(temp);
                        if (flag)
                        {
                            combobox = inc.Combobox_Alert_EventType;
                            flag = combobox.Existed;
                            if (flag)
                            {
                                temp = Base.GData("IncAlert_EventType_2");
                                flag = combobox.SelectItem(temp);
                                if (flag)
                                {
                                    textbox = inc.Textbox_ShortDescription;
                                    flag = textbox.Existed;
                                    if (flag)
                                    {
                                        temp = Base.GData("ShortDescription_P2");
                                        flag = textbox.SetText(temp);
                                        if (flag)
                                        {
                                            combobox = inc.Combobox_Priority;
                                            flag = combobox.Existed;
                                            if (flag)
                                            {
                                                temp = "2 - High";
                                                flag = combobox.SelectItem(temp);
                                                if (!flag)
                                                {
                                                    error = "Cannot select priority value";
                                                }
                                            }
                                            else { error = "Cannot get combobox priority control."; }
                                        }
                                        else { error = "Cannot populate short description value"; }
                                    }
                                    else { error = "Cannot get short description control"; }
                                }
                                else { error = "Cannot populate event type"; }
                            }
                            else { error = "Cannot get combobox event type"; }
                        }
                        else { error = "Cannot populuate incident alert description 1"; }
                    }
                    else { error = "Cannot get description control"; }
                }
                else { error = "Cannot click tab (Details)"; }

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_047_PopulateBusinessSerivceImpact()
        {
            try
            {
                string temp = "Yes";
                combobox = inc.Combobox_Alert_BusinessServiceImpact;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        inc.WaitLoading();
                        textbox = inc.Textbox_Alert_BusinessServiceImpacted;
                        flag = textbox.Existed;
                        if (flag)
                        {
                            temp = Base.GData("IncAlert_BS_Impact_2");
                            flag = textbox.SetText(temp);
                            if (!flag)
                            {
                                error = "Cannot populate BS/Impacted value";
                            }
                        }
                        else { error = "Cannot get BS/Impacted control"; }
                    }
                    else { error = "Cannot populate BS/Impact value."; }
                }
                else { error = "Cannot get BS/Impact control."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_048_PopulateAssignee()
        {
            try
            {
                string temp = Base.GData("IncAlert_Assignee_2");
                lookup = inc.Lookup_AssignedTo;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag) { error = "Cannot populate assigned to value."; }
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
        public void Step_049_01_SaveIncident()
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
        public void Step_049_02_VerifyAssignmentGroup()
        {
            try
            {
                lookup = inc.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    string temp = Base.GData("IncAlert_AssignmentGroup");
                    if (lookup.Text != temp)
                    {
                        error = "The value of assignment group is not correct.";
                    }
                }
                else { error = "Cannot get lookup assignment group control"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------


        [Test]
        public void Step_050_051_SearchAndOpenIncident_INC2()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId2 == null || incidentId2 == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id 2.");
                    addPara.ShowDialog();
                    incidentId2 = addPara.value;
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
                        flag = inclist.SearchAndOpen("Number", incidentId2, "Number=" + incidentId2, "Number");
                        if (!flag) error = "Error when search and open incident (id:" + incidentId2 + ")";
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
        public void Step_052_01_OpenIncidentAlert()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentAlert2 == null || incidentAlert2 == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident alert Id 2.");
                    addPara.ShowDialog();
                    incidentAlert2 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                string conditions = "Number=" + incidentAlert2;
                flag = inc.RelatedTableOpenRecord("Incident Alerts", conditions, "Number");
                if (!flag)
                {
                    flagExit = false;
                    error = "Not found Incident Alert response: " + conditions;
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
        public void Step_052_02_ValidateStateItem()
        {
            try
            {
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    string temp = "Cancelled";
                    flag = combobox.VerifyItemList(temp);

                    //Cancelled option is not available in the State choice list
                    if (flag)
                    {
                        flag = false;
                        error = "The value " + temp + " is in the list.";
                    }
                    else { flag = true; }
                }
                else { error = "Cannot get combobox state control"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        
        
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_053_01_OpenNewPrivateBrowser()
        {
            try
            {
                
                textbox = inc.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    string temp = Base.GData("Type").ToLower();
                    if(temp == "ff")
                    {
                        textbox.MyEle.SendKeys(Keys.LeftControl + Keys.Shift + "P");
                        flag = Base.SwitchToPage(1);
                    }
                    else if(temp == "chr")
                    {
                        System.Windows.Forms.SendKeys.SendWait("^+{N}");
                        flag = Base.SwitchToPage(1);
                    }
                    if (!flag)
                        error = "Cannot switch to page (1).";
                    
                }
                else { error = "Cannot get attached knowledge control."; }
                
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_053_02_Login()
        {
            try
            {
                //Open the URL
                Base.Driver.Navigate().GoToUrl(Base.GData("Url"));
                login.WaitLoading();
                //--------------------------------------------------
                //Log IN
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
        public void Step_053_03_ImpersonateUser_MIMUser()
        {
            try
            {
                string temp = Base.GData("MIM_User");                
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
        public void Step_054_SystemSetting()
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
        public void Step_055_056_SearchAndOpenIncidentAlert_IA2()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentAlert2 == null || incidentAlert2 == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident alert Id 2.");
                    addPara.ShowDialog();
                    incidentAlert2 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                flag = home.LeftMenuItemSelect("Incident Alert Management", "My Alerts");
                if (flag)
                {
                    inclist.WaitLoading();
                    temp = inclist.Label_Title.Text;
                    flag = temp.Equals("Incident Alerts");
                    if (flag)
                    {
                        flag = inclist.SearchAndOpen("Number", incidentAlert2, "Number=" + incidentAlert2, "Number");
                        if (!flag) error = "Error when search and open incident alert (id:" + incidentAlert2 + ")";
                        else inc.WaitLoading();
                    }
                    else { error = "Invalid title of page. Runtime:(" + temp + "). Expected:(Incidents Alert)"; }
                }
                else error = "Error when select open incident alert.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_057_ValidateDefaultContact()
        {
            try
            {
                //User Contacts Tab
                string temp = Base.GData("IncAlert_UserContact_1");
                string conditions = "Responsibility=" + temp;
                flag = inc.RelatedTableVerifyRow("User Contacts", conditions);
                if (flag)
                {
                    temp = Base.GData("IncAlert_UserContact_2");
                    conditions = "Responsibility=" + temp;
                    flag = inc.RelatedTableVerifyRow("User Contacts", conditions);
                    if (flag)
                    {
                        temp = Base.GData("IncAlert_UserContact_3");
                        conditions = "Responsibility=" + temp;
                        flag = inc.RelatedTableVerifyRow("User Contacts", conditions);
                        if (!flag)
                        {
                            error = "Not found User Contacts with condition: " + conditions;
                        }       
                    }
                    else { error = "Not found User Contacts with condition: " + conditions; }
                }
                else { error = "Not found User Contacts with condition: " + conditions; }

                //Group Contacts
                temp = Base.GData("IncAlert_GroupContact_1");
                conditions = "Responsibility=" + temp;
                flag = inc.RelatedTableVerifyRow("Group Contacts", conditions);
                if (flag)
                {
                    temp = Base.GData("IncAlert_GroupContact_2");
                    conditions = "Responsibility=" + temp;
                    flag = inc.RelatedTableVerifyRow("Group Contacts", conditions);
                    if (!flag)
                    {
                        { error = "Not found User Contacts with condition: " + conditions; }
                    }                    
                }
                else { error = "Not found User Contacts with condition: " + conditions; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_058_PopulateState()
        {
            try
            {
                string temp = "Cancelled";
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
        public void Step_059_PopulateCancellationCodeCombox()
        {
            try
            {
                combobox = inc.Combobox_Alert_CancellationCode;
                flag = combobox.Existed;
                if (flag)
                {
                    string item = Base.GData("IncAlert_2_CancellationCode");
                    flag = combobox.SelectItem(item);
                    if (!flag)
                    {
                        error = "Cannot populate cancellation code value.";
                    }
                }
                else { error = "Cannot get cancellation code control"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        
        [Test]
        public void Step_060_SaveIncidentAlert()
        {
            try
            {
                flag = Base.SwitchToPage(1);
                if (flag)               
                {
                    
                    flag = inc.Save();
                    if (!flag) { error = "Error when save incident alert."; }
                }
                else { error = "Cannot switch to previous page"; }
                
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_061_01_SearchAndOpenIncidentAlert_IA3()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentAlert3 == null || incidentAlert3 == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident alert Id 3.");
                    addPara.ShowDialog();
                    incidentAlert3 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                flag = home.LeftMenuItemSelect("Incident Alert Management", "My Alerts");
                if (flag)
                {
                    inclist.WaitLoading();
                    temp = inclist.Label_Title.Text;
                    flag = temp.Equals("Incident Alerts");
                    if (flag)
                    {
                        flag = inclist.SearchAndOpen("Number", incidentAlert3, "Number=" + incidentAlert3, "Number");
                        if (!flag) error = "Error when search and open incident alert (id:" + incidentAlert3 + ")";
                        else inc.WaitLoading();
                    }
                    else { error = "Invalid title of page. Runtime:(" + temp + "). Expected:(Incidents Alert)"; }
                }
                else error = "Error when select open incident alert.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_061_02_PopulateState()
        {
            try
            {
                string temp = "Cancelled";
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
        public void Step_061_03_PopulateCancellationCodeCombox()
        {
            try
            {
                combobox = inc.Combobox_Alert_CancellationCode;
                flag = combobox.Existed;
                if (flag)
                {
                    string item = Base.GData("IncAlert_3_CancellationCode");
                    flag = combobox.SelectItem(item);
                    if (!flag)
                    {
                        error = "Cannot populate cancellation code value.";
                    }
                }
                else { error = "Cannot get cancellation code control"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_061_04_SaveIncidentAlert()
        {
            try
            {
                flag = Base.SwitchToPage(1);
                if (flag)
                {                    
                    flag = inc.Save();
                    if (!flag) { error = "Error when save incident alert."; }
                }
                else { error = "Cannot switch to previous page"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_061_05_SearchAndOpenIncidentAlert_IA4()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentAlert4 == null || incidentAlert4 == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident alert Id 4.");
                    addPara.ShowDialog();
                    incidentAlert4 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                flag = home.LeftMenuItemSelect("Incident Alert Management", "My Alerts");
                if (flag)
                {
                    inclist.WaitLoading();
                    temp = inclist.Label_Title.Text;
                    flag = temp.Equals("Incident Alerts");
                    if (flag)
                    {
                        flag = inclist.SearchAndOpen("Number", incidentAlert4, "Number=" + incidentAlert4, "Number");
                        if (!flag) error = "Error when search and open incident alert (id:" + incidentAlert4 + ")";
                        else inc.WaitLoading();
                    }
                    else { error = "Invalid title of page. Runtime:(" + temp + "). Expected:(Incidents Alert)"; }
                }
                else error = "Error when select open incident alert.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_061_06_PopulateState()
        {
            try
            {
                string temp = "Cancelled";
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
        public void Step_061_07_PopulateCancellationCodeCombox()
        {
            try
            {
                combobox = inc.Combobox_Alert_CancellationCode;
                flag = combobox.Existed;
                if (flag)
                {
                    string item = Base.GData("IncAlert_4_CancellationCode");
                    flag = combobox.SelectItem(item);
                    if (!flag)
                    {
                        error = "Cannot populate cancellation code value.";
                    }
                }
                else { error = "Cannot get cancellation code control"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        
        [Test]
        public void Step_061_08_SaveIncidentAlert()
        {
            try
            {
                flag = Base.SwitchToPage(1);
                if (flag)
                {                    
                    flag = inc.Save();
                    if (!flag) { error = "Error when save incident alert."; }
                }
                else { error = "Cannot switch to previous page"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_062_063_SearchAndOpenIncidentAlert_IA1()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentAlert1 == null || incidentAlert1 == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident alert Id 1.");
                    addPara.ShowDialog();
                    incidentAlert1 = addPara.value;
                    addPara.Close();
                    addPara = null;
                }
                //-----------------------------------------------------------------------
                flag = home.LeftMenuItemSelect("Incident Alert Management", "My Alerts");
                if (flag)
                {
                    inclist.WaitLoading();
                    temp = inclist.Label_Title.Text;
                    flag = temp.Equals("Incident Alerts");
                    if (flag)
                    {
                        flag = inclist.SearchAndOpen("Number", incidentAlert1, "Number=" + incidentAlert1, "Number");
                        if (!flag) error = "Error when search and open incident alert (id:" + incidentAlert1 + ")";
                        else inc.WaitLoading();
                    }
                    else { error = "Invalid title of page. Runtime:(" + temp + "). Expected:(Incidents Alert)"; }
                }
                else error = "Error when select open incident alert.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_064_01_PopulateActionTaken()
        {
            try
            {
                tab = inc.GTab("Activity");
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = inc.GTab("Activity", true);
                    i++;
                }
                flag = tab.Header.Click(true);
                if (flag)
                {
                    string temp = Base.GData("IncAlert_1_ActionsTaken");
                    textarea = inc.Textarea_Alert_ActionsTaken;
                    flag = textarea.Existed;
                    if (flag)
                    {
                        flag = textarea.SetText(temp);
                        if (!flag)
                        {
                            error = "Cannot populate actions taken value";
                        }
                    }
                    else { error = "Cannot get actions taken control"; }
                }
                else { error = "Cannot select tab (Activity)."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_064_02_SaveIncidentAlert()
        {
            try
            {
                flag = Base.SwitchToPage(1);
                if (flag)
                {                    
                    flag = inc.Save();
                    inc.WaitLoading();
                    if (!flag) { error = "Error when save incident alert."; }
                }
                else { error = "Cannot switch to previous page"; }
                
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_064_03_VerifyWorkInProgressState()
        {
            try
            {
                string temp = "Work in Progress";
                combobox = inc.Combobox_State;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.VerifyCurrentText(temp);
                    if (!flag)
                    {
                        flagExit = false;
                        error = "Invalid state value. Expected: " + "(" + temp + ")";
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
        public void Step_065_01_SwitchToPage_SDA1()
        {
            try
            {
                flag = Base.SwitchToPage(0);
                if (!flag)
                {
                    error = "Cannot switch to previous page";                    
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
        public void Step_065_02_ImpersonateUser_Resolver()
        {
            try
            {
                string temp = Base.GData("Resolver");
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
        public void Step_066_SystemSetting()
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
        public void Step_067_068_SearchAndOpenIncident_INC1()
        {
            try
            {
                //-- Input information
                string temp = Base.GData("Debug").ToLower();
                if (temp == "yes" && (incidentId1 == null || incidentId1 == string.Empty))
                {
                    Auto.AddParameter addPara = new Auto.AddParameter("Please input incident Id 1.");
                    addPara.ShowDialog();
                    incidentId1 = addPara.value;
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
                        flag = inclist.SearchAndOpen("Number", incidentId1, "Number=" + incidentId1, "Number");
                        if (!flag) error = "Error when search and open incident (id:" + incidentId1 + ")";
                        else inc.WaitLoading();
                    }
                    else { error = "Invalid title of page. Runtime:(" + temp + "). Expected:(Incidents)"; }
                }
                else { error = "Error when select open incident."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_069_PopulateState()
        {
            try
            {
                string temp = "Resolved";
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
        public void Step_070_PopulateCloseCodeAndCloseNotes()
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
        public void Step_071_SaveIncident()
        {
            try
            {
                flag = Base.SwitchToPage(0);
                if (flag)
                {                    
                    flag = inc.Save();
                    if (!flag) { error = "Error when save incident."; }
                }
                else { error = "Cannot switch to previous page"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_072_01_SwitchToPage_MIMUser()
        {
            try
            {
                flag = Base.SwitchToPage(1);
                if (!flag)
                {
                    error = "Cannot switch to previous page";
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
        public void Step_072_02_PopulateState()
        {
            try
            {
                string temp = "Resolved";
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
        public void Step_072_03_SaveIncidentAlert()
        {
            try
            {
                flag = Base.SwitchToPage(1);
                if (flag)
                {                    
                    flag = inc.Save();
                    if (!flag) { error = "Error when save incident alert."; }
                }
                else { error = "Cannot switch to previous page"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_073_ValidateResolutionCode()
        {
            try
            {
                tab = inc.GTab("Post Incident Review");
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = inc.GTab("Post Incident Review", true);
                    i++;
                }
                flag = tab.Header.Click(true);
                if (flag)
                {
                    string temp = Base.GData("IncAlert_1_ResolutionCode");
                    combobox = inc.Combobox_Alert_ResolutionCode;
                    flag = combobox.Existed;
                    if (flag)
                    {
                        flag = combobox.VerifyCurrentText(temp);
                        if (!flag)
                        { error = "The value of resolution code is not correct."; }
                    }
                    else { error = "Cannot get combobox resolution code control."; }
                }
                else { error = "Cannot select tab (Post Incident Review)"; }
                
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_074_01_PopulateNote_Summary_LessonsLearned()
        {
            try
            {
                tab = inc.GTab("Post Incident Review");
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = inc.GTab("Post Incident Review", true);
                    i++;
                }
                flag = tab.Header.Click();
                if (flag)
                {
                    string temp = Base.GData("IncAlert_1_PostIncReview_ResolutionNotes");
                    textarea = inc.Textarea_Alert_ResolutionNotes;
                    flag = textarea.Existed;
                    if (flag)
                    {
                        flag = textarea.SetText(temp);
                        if (flag)
                        {
                            temp = Base.GData("IncAlert_1_PostIncReview_Summary");
                            textarea = inc.Textarea_Alert_Summary;
                            flag = textarea.Existed;
                            if (flag)
                            {
                                flag = textarea.SetText(temp);
                                if (flag)
                                {
                                    temp = Base.GData("IncAlert_1_PostIncReview_LessonsLearned");
                                    textarea = inc.Textarea_Alert_LessonsLearned;
                                    flag = textarea.Existed;
                                    if (flag)
                                    {
                                        flag = textarea.SetText(temp);
                                        if (!flag)
                                        {
                                            error = "Cannot populate the value of lessons learned.";
                                        }
                                    }
                                    else { error = "Cannot get lessons learned control."; }
                                }
                                else { error = "Cannot populate the value of summary."; }
                            }
                            else { error = "Cannot get summary control."; }
                        }
                        else { error = "Cannot populate the value of resolution notes."; }
                    }
                    else { error = "Cannot get resolution notes control."; }
                }
                else { error = "Cannot select tab (Post Incident Review)"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_074_02_SaveIncidentAlert()
        {
            try
            {
                flag = Base.SwitchToPage(1);
                if (flag)
                {                    
                    flag = inc.Save();
                    if (!flag) { error = "Error when save incident alert."; }
                }
                else { error = "Cannot switch to previous page"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------


        [Test]
        public void Step_075_01_PopulateState()
        {
            try
            {
                string temp = "Closed";
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
        public void Step_075_02_SaveIncidentAlert()
        {
            try
            {
                flag = Base.SwitchToPage(1);
                if (flag)
                {                    
                    flag = inc.Save();
                    if (!flag) { error = "Error when save incident alert."; }
                }
                else { error = "Cannot switch to previous page"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_076_ValidateClosedAndClosedBy()
        {
            try
            {
                tab = inc.GTab("Details");
                int i = 0;
                while (tab == null && i < 5)
                {
                    Thread.Sleep(2000);
                    tab = inc.GTab("Details", true);
                    i++;
                }
                flag = tab.Header.Click(true);
                if (flag)
                {                    
                    lookup = inc.Lookup_Alert_ClosedBy;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        string temp = Base.GData("MIM_User");
                        if (lookup.Text != temp)
                        {                            
                            error = "The value of Closed By field is not correct.";
                        }
                    }
                    else { error = "Cannot get Closed By control"; }                   
                }
                else { error = "Cannot select tab (Details)"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_077_01_SwitchToPage_SDA()
        {
            try
            {
                flag = Base.SwitchToPage(0);
                if (!flag)
                {
                    error = "Cannot switch to previous page";
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
        public void Step_077_02_ImpersonateUser_SDA1()
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
        public void Step_078_SystemSetting()
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
        public void Step_079_OpenNewIncident_INC3()
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
        public void Step_080_01_PopulateCaller()
        {
            try
            {
                textbox = inc.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    textbox.Click();
                    //-- Store incident id
                    incidentId3 = textbox.Text;                   
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
        public void Step_080_02_PopulateCategory()
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
                else { error = "Cannot get combobox category."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_080_03_PopulateSubCategory()
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
                else { error = "Cannot get combobox sub category."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_080_04_PopulateShortDescription()
        {
            try
            {
                string temp = Base.GData("Inc_ShortDescription_3");
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
        public void Step_081_PopulateAssignmentGroup_MIM_Expect_Invalid()
        {
            try
            {
                string temp = Base.GData("IncAlert_AssignmentGroup");
                lookup = inc.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    //MIM Assessment group is not visible for selecting as a assignment group 
                    flag = lookup.SetText(temp);
                    if (flag) 
                    {
                        if (lookup.MyEle.GetAttribute("title").ToLower() == "invalid reference")
                        {
                            flag = true;
                        }
                        else { error = "The " + temp + " can be selected." + " Expected result: the " + temp + " is not visible"; }                            
                    }
                    else { error = "Cannot set text."; }                   
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
        public void Step_082_SaveIncident()
        {
            try
            {
                flag = Base.SwitchToPage(0);
                if (flag)
                {
                    flag = inc.SaveNoVerify();
                    if (!flag) { error = "Error when save incident."; }                    
                }
                else { error = "Cannot switch to previous page"; }
                
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
                
        //-----------------------------------------------------------------------------------------------------------------------------------
        
        /// <summary>
        /// Create CHANGE ticket
        /// </summary>
        [Test]
        public void Step_083_01_OpenNewChange()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Change", "Create New");
                if (flag)
                {
                    chg.WaitLoading();
                    string temp = Base.GData("ChangeType");
                    flag = chg.Select_ChangeType(temp);
                    if (!flag) error = "Error when select change type.";
                    else chg.WaitLoading();
                }
                else
                    error = "Error when create new change.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_083_02_PopulateCompany()
        {
            try
            {
                //Get change ID number
                textbox = chg.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    changeId = textbox.Text;
                }
                else { error = "Cannot get text number control."; }
                
                //--------------------------------------------------------------

                string temp = Base.GData("Company");
                lookup = chg.Lookup_Company;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag) { error = "Cannot populate company value."; }
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
        public void Step_083_03_PopulateCategory()
        {
            try
            {
                string temp = Base.GData("Chg_Category");
                combobox = chg.Combobox_Category;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        chg.WaitLoading();
                    }
                    else { error = "Cannot populate category value."; }
                }
                else { error = "Cannot get combobox category."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        
        [Test]
        public void Step_083_04_PopulateShortDescription()
        {
            try
            {
                string temp = Base.GData("Chg_ShortDescription");
                textbox = chg.Textbox_ShortDescription;
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
        public void Step_083_05_PopulateJustification()
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
                flag = tab.Header.Click(true);
                if (flag)
                {
                    string temp = Base.GData("Justification");
                    textarea = chg.Textarea_Justification;
                    flag = textarea.Existed;
                    if (flag)
                    {
                        flag = textarea.SetText(temp);
                        if (!flag) { error = "Cannot populate justification value."; }
                    }
                    else { error = "Cannot get textarea justification."; }
                }
                else error = "Cannot select tab (Planning).";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_083_06_PopulatePlannedDate()
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
                flag = tab.Header.Click(true);
                if (flag)
                {
                    string startDate = DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss");
                    string endDate = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
                    datetime = chg.Datetime_PlannedStartDate;
                    flag = datetime.Existed;
                    if (flag)
                    {
                        flag = datetime.SetText(startDate, true);
                        if (flag)
                        {
                            datetime = chg.Datetime_PlannedEndDate;
                            flag = datetime.Existed;
                            if (flag)
                            {
                                flag = datetime.SetText(endDate, true);
                                if (!flag) error = "Cannot populate planned end date.";
                            }
                            else error = "Cannot get datetime planned end date.";
                        }
                        else error = "Cannot populate planned start date.";
                    }
                    else error = "Cannot get datetime planned start date.";
                }
                else error = "Cannot select tab (Planning).";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        

        [Test]
        public void Step_083_07_PopulateAssignmentGroup_MIM()
        {
            try
            {
                string temp = Base.GData("ChgAlert_AssignmentGroup");
                lookup = chg.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    //MIM Assessment group is not visible for selecting as a assignment group 
                    flag = lookup.SetText(temp);
                    if (flag)
                    {
                        if (lookup.MyEle.GetAttribute("title").ToLower() == "invalid reference")
                        {
                            flag = true;
                        }
                        else { error = "The " + temp + " can be selected." + " Expected result: the " + temp + " is not visible"; }
                    }
                    else { error = "Cannot set text."; }                
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
        public void Step_083_08_SaveChange()
        {
            try
            {
                flag = Base.SwitchToPage(0);
                if (flag)
                {
                    flag = chg.SaveNoVerify();
                    if (!flag) { error = "Error when save change."; }
                }
                else { error = "Cannot switch to previous page"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Create PROBLEM ticket
        /// </summary>
        [Test]
        public void Step_083_09_OpenNewproblem()
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
        public void Step_083_10_PopulateCompany()
        {
            try
            {
                //Get problem ID number
                textbox = prb.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    problemId = textbox.Text;
                }
                else { error = "Cannot get text number control."; }

                //--------------------------------------------------------------

                string temp = Base.GData("Company");
                lookup = prb.Lookup_Company;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.Select(temp);
                    if (!flag) { error = "Cannot populate company value."; }
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
        public void Step_083_11_PopulateShortDescription()
        {
            try
            {
                string temp = Base.GData("Prb_ShortDescription");
                textbox = prb.Textbox_ShortDescription;
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
        public void Step_083_12_PopulateAssignmentGroup_MIM()
        {
            try
            {
                string temp = Base.GData("PrbAlert_AssignmentGroup");
                lookup = prb.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    //MIM Assessment group is not visible for selecting as a assignment group 
                    flag = lookup.SetText(temp);
                    if (flag)
                    {
                        if (lookup.MyEle.GetAttribute("title").ToLower() == "invalid reference")
                        {
                            flag = true;
                        }
                        else { error = "The " + temp + " can be selected." + " Expected result: the " + temp + " is not visible"; }
                    }
                    else { error = "Cannot set text."; }                
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
        public void Step_083_13_PopulateImpact()
        {
            try
            {
                string temp = Base.GData("Prb_Impact");
                combobox = prb.Combobox_Impact;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (!flag) { error = "Cannot populate impact value."; }
                }
                else { error = "Cannot get combobox impact."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_083_14_Populate_More_Fields_If_Need()
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
        public void Step_083_15_SaveProblem()
        {
            try
            {
                flag = Base.SwitchToPage(0);
                if (flag)
                {
                    flag = prb.SaveNoVerify();
                    if (!flag) { error = "Error when save problem."; }
                    else prb.WaitLoading();
                }
                else { error = "Cannot switch to previous page"; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void Step_84_Logout()
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

        #endregion End - Scenario of test case (NEED TO UPDATE)
    }
}
