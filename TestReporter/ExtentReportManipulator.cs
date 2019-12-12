using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace TestReporter
{
    /// <summary>
    /// @Author - Debasish Pradhan
    /// </summary>
    /// <seealso cref="TestReporter.ITestReportManipulator" />
    public class ExtentReportManipulator:ITestReportManipulator
    {

        private Dictionary<string, string> keyValuePair = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtentReportManipulator"/> class.
        /// </summary>
        /// <param name="keyValuePair">The key value pair.</param>
        public ExtentReportManipulator(Dictionary<string, string> keyValuePair)
        {
            this.keyValuePair = keyValuePair;
        }

        /*Dictionary should contain
         * SourceFile
         * Text1ToBeReplaced
         * Text1ToReplace
         * Text2ToBeReplaced
         * Text2ToReplace 
         * Text3ToBeReplaced
         * Text3ToReplace 
         
         */
        /// <summary>
        /// Manipulates this instance.
        /// </summary>
        public void Manipulate()
        {
            
            //get original file
            string fileName = this.keyValuePair["SourceFile"];
            
            //read all file contents
            string text = File.ReadAllText(fileName);            
            
            //replace text - 1st text
            string Text1ToBeReplaced = this.keyValuePair["Text1ToBeReplaced"];
            string Text1ToReplace = this.keyValuePair["Text1ToReplace"];
            text = text.Replace(Text1ToBeReplaced, Text1ToReplace);

            //replace text - 2nd text
            string Text2ToBeReplaced = this.keyValuePair["Text2ToBeReplaced"];
            string Text2ToReplace = this.keyValuePair["Text2ToReplace"];
            text = text.Replace(Text2ToBeReplaced, Text2ToReplace);

            //replace text - 3rd text
            string Text3ToBeReplaced = this.keyValuePair["Text3ToBeReplaced"];
            string Text3ToReplace = this.keyValuePair["Text3ToReplace"];
            text = text.Replace(Text3ToBeReplaced, Text3ToReplace);

            //write back to same file  
            File.WriteAllText(fileName, text);
        }
    }
}
