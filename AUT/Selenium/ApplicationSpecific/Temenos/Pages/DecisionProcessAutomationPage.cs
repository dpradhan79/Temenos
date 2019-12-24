using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Setup;
using AUT.Selenium.CommonReUsablePages;
using OpenQA.Selenium;
using Engine.UIHandlers.Selenium;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using Engine.Factories;

namespace AUT.Selenium.ApplicationSpecific.Pages
{
    public class DecisionProcessAutomationPage : AbstractTemplatePage
    {

        #region UI Object Repository
        private By reviewIndicatorsTableRow = By.XPath("//table[@id='ReviewIndicatorLoanGrid-data-table']//tbody/tr/td[1]");


        #endregion

        #region Public Methods

        /// <summary>
        /// Verify review indicators
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// </summary>
        public void VerifyReviewIndicators(string name, string description)
        {
            try
            {
                int row = GetReviewIndicatorNamePosition(name);
                if (row > 0)
                {
                    this.TESTREPORT.LogSuccess("Review Indicators Name", String.Format("Name : <mark>{0}</mark> is present under Name column in Review Indicators grid", name));
                    By nameColumn = By.XPath("//table[@id='ReviewIndicatorLoanGrid-data-table']//tbody/tr[" + row + "]/td[2]/span");
                    driver.VerifyTextValue(nameColumn, description);
                }
                else
                {
                    this.TESTREPORT.LogFailure("Review Indicators Name", String.Format("Name : <mark>{0}</mark> is not present under Name column in Review Indicators grid", name));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// Verify review indicators
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// </summary>
        public void VerifyReviewIndicators(Dictionary<string, string> validationTestData,int index=0)
        {
            try
            {
                int row = GetReviewIndicatorNamePosition(validationTestData["ReviewIndicatorName" + index]);
                if (row > 0)
                {
                    this.TESTREPORT.LogSuccess("Review Indicators Name", String.Format("Name : <mark>{0}</mark> is present under Name column in Review Indicators grid", validationTestData["ReviewIndicatorName" + index]));
                    By nameColumn = By.XPath("//table[@id='ReviewIndicatorLoanGrid-data-table']//tbody/tr[" + row + "]/td[2]/span");
                    driver.VerifyTextValue(nameColumn, validationTestData["ReviewIndicatorDescription" + index]);
                }
                else
                {
                    this.TESTREPORT.LogFailure("Review Indicators Name", String.Format("Name : <mark>{0}</mark> is not present under Name column in Review Indicators grid", validationTestData["ReviewIndicatorName" + index]),this.SCREENSHOTFILE);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Get review indicator name position
        /// <param name=" name">The name .</param>
        /// </summary>
        private int GetReviewIndicatorNamePosition(String name)
        {
            try
            {
                int position = 0;
                var rows = driver.FindElements(reviewIndicatorsTableRow).Count;
                for (int i = 1; i <= rows; i++)
                {
                    By nameColumn = By.XPath("//table[@id='ReviewIndicatorLoanGrid-data-table']//tbody/tr[" + i + "]/td[1]");
                    if (driver.GetElementText(nameColumn).Equals(name))
                    {
                        position = i;
                        break;
                    }
                }
                return position;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion
    }
}