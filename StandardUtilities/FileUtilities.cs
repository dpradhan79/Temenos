using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Diagnostics;
using log4net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
namespace StandardUtilities
{
    /// <summary>
    /// @Author - Debasish Pradhan
    /// </summary>
    public class FileUtilities
    {
        private static readonly ILog LOG = LogManager.GetLogger(typeof(FileUtilities).Name);
        /// <summary>
        /// Creates a relative path from one file or folder to another.
        /// </summary>
        /// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
        /// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
        /// <returns>The relative path from the start directory to the end path or <c>toPath</c> if the paths are not related.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static String MakeRelativePath(String fromPath, String toPath)
        {
            if (String.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (String.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }

        /// <summary>
        /// Gets the CSV data handle strings.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="rowNumber">The row number.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetCSVData(string fileName, string columnName, int rowNumber)
        {
            string columnData = null;
            System.IO.StreamReader bufferedReader = new StreamReader(fileName);
            string headersWithComma = bufferedReader.ReadLine();
            List<string> listHeaders = new List<string>();
            string[] arrayHeaders = headersWithComma.Split(',');
            //store all headers in List as getting the index back wont require re-calculation
            //index can be retrieved back by using List.IndexOf(Object)
            foreach (string columnHeader in arrayHeaders)
            {
                listHeaders.Add(columnHeader.ToLower());
            }

            //read test data on the basis of given rownumber
            for (int currentRowCounter = 1; currentRowCounter < rowNumber; currentRowCounter++)
            {
                //read the testrecord one by one but ignore rows before the given rowNumber
                bufferedReader.ReadLine();
            }

            //next testrecord is specified record
            string testRecord = bufferedReader.ReadLine();
            int lastIndex = 0;
            int nextIndex = -1;
            int columnIndex = -1;
            columnIndex = listHeaders.IndexOf(columnName.ToLower());
            if (columnIndex < 0)
            {
                throw new Exception(String.Format("Column - {0}, Not Found In CSV File - {1}", columnName, fileName));
            }

            //Here Logic for parsing the test data separated by comma

            for (int columnCounter = 0; columnCounter < columnIndex; columnCounter++)
            {
                nextIndex = GetNextIndexHandleStrings(testRecord, lastIndex);
                if (testRecord.ElementAt(nextIndex).Equals('\"') && testRecord.ElementAt(nextIndex + 1).Equals(','))
                {
                    lastIndex = nextIndex + 2; //point to start of next columnData
                }
                else if (testRecord.ElementAt(nextIndex).Equals('\"') && lastIndex == (testRecord.Length - 1))
                {
                    lastIndex = -1;
                }
                else if (testRecord.ElementAt(nextIndex).Equals('\"') == false && (lastIndex < testRecord.Length - 1))
                {
                    lastIndex = nextIndex + 1;
                }
            }
            //here the next data should belong to given column
            //read the column value
            nextIndex = GetNextIndexHandleStrings(testRecord, lastIndex);
            if (testRecord.ElementAt(lastIndex).Equals('\"'))
            {
                lastIndex++;
            }
            columnData = testRecord.Substring(lastIndex, nextIndex - lastIndex);
            //Console.WriteLine(String.Format("File - {0}, Column - {1}, Row - {2}, ColumnData - {3}", fileName, columnName, rowNumber, columnData));
            return columnData.Trim();

        }

        /// <summary>
        /// Gets the next index handle strings.
        /// </summary>
        /// <param name="testRecord">The test record.</param>
        /// <param name="lastIndex">The last index.</param>
        /// <returns></returns>
        private static int GetNextIndexHandleStrings(string testRecord, int lastIndex)
        {
            int nextIndex = -1;
            //get character at lastIndex
            string startingCharacter = testRecord.Substring(lastIndex, 1);
            if (startingCharacter.Equals("\""))
            {
                nextIndex = testRecord.IndexOf("\"", lastIndex + 1);
            }
            else
            {
                nextIndex = testRecord.IndexOf(",", lastIndex + 1);
                //if no comma is found after lastIndex then it is last field, nextIndex should be index of last character of testrecord
                if (nextIndex == -1)
                {
                    nextIndex = testRecord.Length;
                }
            }
            return nextIndex;
        }


        /**
        Sample Use :
         [TestMethod]
            public void TestFileDownLoad()
            {
                string fileLocationUrl = "http://www.prlog.org";
                string fileName = "11822075-cigniti-200x100.png";
                string destinationPath = @"C:\Users\in01518\Pictures\testFile.jpg";
                FileDownloader.DownLoadFile(fileLocationUrl, fileName, destinationPath);
            }
        **/
        /// <summary>
        /// Downloads file from remoteUri.
        /// </summary>
        /// <param name="remoteUri">The remote URI.</param>
        /// <param name="file">The file.</param>
        /// <param name="destinationFilePath">The destination file path.</param>
        /// <param name="authenticationUserName">Name of the authentication user. UserName required , for Windows based authentication. Will Not work for forms based authentication</param>
        /// <param name="authenticationPassword">The authentication password. Password required, for Windows based authentication. Will Not work for forms based authentication</param>
        /// <exception cref="System.Net.WebException"></exception>
        /// <exception cref="System.Exception">
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        /// <exception cref="System.IO.DirectoryNotFoundException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static void DownLoadFile(string remoteUri, string file, string destinationFilePath, string authenticationUserName = null, string authenticationPassword = null)
        {

            WebClient webClient = new WebClient();

            /*if url does not end with forward slash, then add forward slash to end of it*/
            if (remoteUri.EndsWith("/") == false)
            {
                remoteUri = remoteUri + "/";
            }

            string webResource = remoteUri + file;
            Console.WriteLine("File - {0} Will Be Downloaded From WebResource - {1}", file, webResource);
            try
            {
                webClient.Credentials = new NetworkCredential(authenticationUserName, authenticationPassword);
                webClient.DownloadFile(webResource, file);
                Console.WriteLine("File - {0} Was Successfully Downloaded From WebResource - {1} To Location - {2}", file, webResource, Directory.GetCurrentDirectory());
            }
            //catch Download specific error
            catch (WebException ex)
            {
                throw new WebException(String.Format("File - {0} Could Not be Downloaded From RemoteUri - {1}, Reason - {2} ", file, remoteUri, ex.Message));
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("File - {0} Could Not be Downloaded From RemoteUri - {1}, Reason - {2} ", file, remoteUri, ex.Message));
            }
            //verify file availability in Application Startup Path
            //Copy file from Application Execution Path to Specified path
            try
            {
                File.Copy(String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), file), destinationFilePath, true);
                Console.WriteLine(String.Format("File - {0}, Copied To Location - {1}", file, destinationFilePath));
                //after copy delete from execution folder
                try
                {
                    File.Delete(String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), file));
                    Console.WriteLine("File - {0}, Deleted From Location - {1}", file, Directory.GetCurrentDirectory());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("File - {0}, Could Not Be Deleted From - {1} : Detailed Message - {2}", file, Directory.GetCurrentDirectory(), ex.Message);
                }

            }
            //catch File Not Found Issue
            catch (FileNotFoundException ex)
            {
                throw new Exception(String.Format("File - {0}, Not Found At Location - {1}, To Be Coped To - {2} : Detailed Message - {3}", file, Directory.GetCurrentDirectory(), destinationFilePath, ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException(String.Format("Logged In User Is Not Autohorized To Perform Copy Operation : Detailed Message - {0}", ex.Message));
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new DirectoryNotFoundException(String.Format("Directory Not Found Exception Encountered, Detailed Message - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Exception Encountered : Detailed Message - {0}", ex.Message));
            }

        }

        public static int ExecuteBatchFile(string fileName, string directory)
        {
            int exitCode;
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo("cmd.exe", "/c " + "\"" + directory + @"\" + fileName + "\"");
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;
            //processInfo.WorkingDirectory = "\"" + directory + "\"";
            // processInfo.Arguments = ;
            process = Process.Start(processInfo);
            process.WaitForExit();

            // *** Read the streams ***
            // Warning: This approach can lead to deadlocks, see Edit #2
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            exitCode = process.ExitCode;

            Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            Console.WriteLine(String.Format("File Name - {0}, Exit Code - {1}", fileName, exitCode));
            process.Close();

            return exitCode;
        }

        public static string readPropertyFile(string fileName, string key)
        {
            string keyValue = null;

            System.IO.StreamReader bufferedReader = null;
            Properties prop = null;
            LOG.Debug("read method invoked");
            LOG.Info("fileName = " + fileName);
            LOG.Info("key = " + key);
            try
            {
                LOG.Debug("Inside try");
                bufferedReader = new System.IO.StreamReader(fileName);

                //bufferedReader = new System.IO.StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + Path.DirectorySeparatorChar + fileName);
                LOG.Debug("File : " + fileName + " Was Opened For Reading");

                LOG.Debug("File.separatorChar = " + System.IO.Path.DirectorySeparatorChar);
                /* Instantiate Properties */
                prop = new Properties(fileName);
                LOG.Debug("Properties Instance Was Created");

                LOG.Debug("properties from file : " + fileName + " Were Read");
                keyValue = prop.get(key);
            }
            catch (System.IO.FileNotFoundException)
            {
            }
            catch (System.IO.IOException)
            {
            }
            catch (System.NullReferenceException)
            {
            }
            finally
            {
                LOG.Debug("Inside finally");
                try
                {
                    bufferedReader.Close();
                    LOG.Debug("BufferedReader(bufferedReader) Instance Closed");
                }
                catch (System.IO.IOException)
                {
                }
                catch (System.NullReferenceException)
                {
                }
            }
            LOG.Debug("keyValue being returned = " + keyValue);
            LOG.Debug("Exiting read method");
            return keyValue;
        }

        public static void TakeScreenShot(string screenShotPath)
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }

                bitmap.Save(screenShotPath, ImageFormat.Jpeg);

            }
        }
    }
}
