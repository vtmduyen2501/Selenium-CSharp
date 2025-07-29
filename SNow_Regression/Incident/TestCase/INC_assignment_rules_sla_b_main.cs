using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
namespace Incident
{
    [TestFixture("01")]
    [TestFixture("02")]
    [TestFixture("03")]
    [TestFixture("04")]
    [TestFixture("05")]
    [TestFixture("06")]
    [TestFixture("07")]
    [TestFixture("08")]
    [TestFixture("09")]
    [TestFixture("10")]
    [TestFixture("11")]
    [TestFixture("12")]
    [TestFixture("13")]
    [TestFixture("14")]
    [TestFixture("15")]
    [TestFixture("16")]
    [TestFixture("17")]
    [TestFixture("18")]
    [TestFixture("19")]
    [TestFixture("20")]
    [TestFixture("21")]
    [TestFixture("22")]
    [TestFixture("23")]
    [TestFixture("24")]
    [TestFixture("25")]
    [TestFixture("26")]
    [TestFixture("27")]
    [TestFixture("28")]
    [TestFixture("29")]
    [TestFixture("30")]
    [TestFixture("31")]
    [TestFixture("32")]
    [TestFixture("33")]
    [TestFixture("34")]
    [TestFixture("35")]
    [TestFixture("36")]
    [TestFixture("37")]
    [TestFixture("38")]
    [TestFixture("39")]
    [TestFixture("40")]
    [TestFixture("41")]
    [TestFixture("42")]
    [TestFixture("43")]
    [TestFixture("44")]
    [TestFixture("45")]
    [TestFixture("46")]
    [TestFixture("47")]
    [TestFixture("48")]
    [TestFixture("49")]
    [TestFixture("50")]
    public class INC_assignment_rules_sla_b_main
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
        string sRow = string.Empty;
        public INC_assignment_rules_sla_b_main(string _Row)
        {
            if (_Row != string.Empty)
                this.sRow = (int.Parse(_Row)).ToString();
        }
        
        [TestFixtureSetUp]
        public void Setup()
        {
            caseName = MethodBase.GetCurrentMethod().DeclaringType.Name;
            Base = new Auto.obase();
            Base.BeforeRunTestCase(caseName, ref Base, ref flagExit, ref flagW, ref flag, ref flagC, sRow);
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
        
        //------------------------------------------------------------------
        Auto.Login login = null;
        Auto.Home home = null;
        Auto.Incident inc = null;
        
        //------------------------------------------------------------------
        string incidentId, category, subcategory, bs, ci, priority, shortdescription, slas;

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
                
                //------------------------------------------------------------------
                incidentId = string.Empty;
                category = Base.GData("Category");
                subcategory = Base.GData("Sub_Category");
                bs = Base.GData("Business_Service");
                ci = Base.GData("Configuration_Item");
                priority = Base.GData("Priority");
                slas = Base.GData("SLAs");
                shortdescription = "Cat:" + category + "|Subcat:" + subcategory + "|Bs:" + bs + "|Ci:" + ci + "|Pri:" + priority;
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
        public void Step_002_PopulateCallerName()
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
        public void Step_003_Verify_Company()
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
        public void Step_004_PopulateBusinessService()
        {
            try
            {
                if (bs.ToLower() != "no" && bs != string.Empty) 
                {
                    lookup = inc.Lookup_BusinessService;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.Select(bs);
                        if (!flag) { error = "Cannot populate business service value."; }
                    }
                    else
                        error = "Cannot get lookup business service.";
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
        public void Step_005_PopulateCategory()
        {
            try
            {
                combobox = inc.Combobox_Category;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(category);
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
        public void Step_006_PopulateSubCategory()
        {
            try
            {
                combobox = inc.Combobox_SubCategory;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = inc.VerifyHaveItemInComboboxList("subcategory", subcategory);
                    if (flag)
                    {
                        flag = combobox.SelectItem(subcategory);
                        if (flag)
                        {
                            inc.WaitLoading();
                        }
                        else { error = "Cannot populate sub category value."; }
                    }
                    else error = "Not found item (" + subcategory + ") in sub category list.";
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
        public void Step_007_PopulateConfigurationItem()
        {
            try
            {
                if (ci.ToLower() != "no" && ci != string.Empty)
                {
                    lookup = inc.Lookup_ConfigurationItem;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.Select(ci);
                        if (!flag) { error = "Cannot populate configuration item value."; }
                    }
                    else
                        error = "Cannot get lookup configuration item.";
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
        public void Step_008_PopulateImpact()
        {
            try
            {
                string temp = Base.GData("Impact");
                combobox = inc.Combobox_Impact;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        inc.WaitLoading();
                    }
                    else { error = "Cannot populate impact value."; }
                }
                else
                {
                    error = "Cannot get combobox impact.";
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
        public void Step_009_PopulateUrgency()
        {
            try
            {
                string temp = Base.GData("Urgency");
                combobox = inc.Combobox_Urgency;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        inc.WaitLoading();
                    }
                    else { error = "Cannot populate urgency value."; }
                }
                else
                {
                    error = "Cannot get combobox urgency.";
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
        public void Step_010_Verify_Priority()
        {
            try
            {
                Thread.Sleep(2000);
                combobox = inc.Combobox_Priority;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.VerifyCurrentText(priority, true);
                    if (flag)
                    {
                        inc.WaitLoading();
                    }
                    else { error = "Invalid priority value. Expected:" + priority; }
                }
                else
                {
                    error = "Cannot get combobox priority.";
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
        public void Step_011_PopulateShortDescription()
        {
            try
            {
                textbox = inc.Textbox_ShortDescription;
                flag = textbox.Existed;
                if (flag)
                {
                    flag = textbox.SetText(shortdescription);
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
        public void Step_012_SaveIncident()
        {
            try
            {
                flag = inc.SaveNoVerify();
                if (flag)
                {
                    try
                    {
                        IAlert alert = Base.Driver.SwitchTo().Alert();
                        alert.Accept();
                    }
                    catch
                    {}
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
        public void Step_013_Verify_AssignmentGroup()
        {
            try
            {
                string temp = Base.GData("Group_Name");
                lookup = inc.Lookup_AssignmentGroup;
                flag = lookup.Existed;
                if (flag)
                {
                    flag = lookup.VerifyCurrentText(temp, true);
                    if (!flag) { error = "Invalid group value or the value is not auto populate."; flagExit = false; }
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
        public void Step_014_ValidateSLAs()
        {
            try
            {
                string result = string.Empty;
                if (slas.ToLower() != "no" && slas.ToLower() != string.Empty) 
                {
                    string condition = string.Empty;
                    if (slas.Contains(";"))
                    {
                        string[] arr = slas.Split(';');
                        bool flagF = true;
                        foreach (string sla in arr) 
                        {
                            condition = "SLA definition=" + sla;
                            flagF = inc.RelatedTableVerifyRow("Task SLAs", condition);
                            if (flagF)
                            {
                                result = result + "*** PASSED: Found SLA: [" + condition + "]" + "\n";
                            }
                            else 
                            {
                                result = result + "*** FAILED: Not found SLA: [" + condition + "]" + "\n";
                                if (flag)
                                    flag = false;
                            }
                        }
                    }
                    else 
                    {
                        condition = "SLA definition=" + slas;
                        flag = inc.RelatedTableVerifyRow("Task SLAs", condition);
                        if (!flag)
                        {
                            result = result + "*** FAILED: Not found SLA: [" + condition + "]" + "\n";
                        }
                        else
                        {
                            result = result + "*** PASSED: Found SLA: [" + condition + "]" + "\n";
                        }
                    }

                    Console.WriteLine(result);
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
        public void Step_015_StoreIncidentId()
        {
            try
            {
                string filePath = Base.tempFolderPath + @"Export\incidentId.txt";
                using (StreamWriter tw = new StreamWriter(filePath, true))
                {
                    tw.WriteLine(incidentId);
                    tw.Close();
                }
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
