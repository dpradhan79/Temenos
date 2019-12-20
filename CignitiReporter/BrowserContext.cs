using System.Collections.Generic;
using log4net;

namespace CignitiReporter
{
    /// <summary>
    /// @Author - Debasish Pradhan
    /// </summary>
    public class BrowserContext
    {
        private static readonly ILog LOG = LogManager.GetLogger(typeof (BrowserContext).Name);
        private string browserName = null;
        private string version = null;
        private string platform = null;

        private static IDictionary<BrowserContext, BrowserContext> mapBrowserContext =
            new Dictionary<BrowserContext, BrowserContext>();

        private static IList<BrowserContext> listBrowserContext = new List<BrowserContext>();
        private static readonly object _locker = new object();

        /// <summary>
        /// No Argument Constructor
        /// </summary>
        private BrowserContext()
        {
        }

        /// <summary>
        /// Consutructor With Arguments </summary>
        /// <param name="browserName"> </param>
        /// <param name="version"> </param>
        /// <param name="platform"> </param>
        private BrowserContext(string browserName, string version, string platform)
        {
            this.browserName = browserName;
            this.version = version;
            this.platform = platform;
            LOG.Debug("BrowserContext Object Initialized With browserName ( " + this.browserName +
                      " ) , browserVersion  ( " + this.BrowserVersion + " , browserPlatform ( " + this.BrowserPlatform +
                      " )");
        }

        /// 
        /// <param name="browserName"> </param>
        public virtual string BrowserName
        {
            set
            {
                this.browserName = value;
                LOG.Debug("BrowserContext Object Set With browserName ( " + this.browserName + " ) ");
            }
            get
            {
                LOG.Debug("Being returned : " + this.browserName);
                return this.browserName;
            }
        }

        /// 
        /// <param name="version"> </param>
        public virtual string Version
        {
            set
            {
                this.version = value;
                LOG.Debug("BrowserContext Object Set With browserVersion ( " + this.version + " ) ");
            }
        }

        /// 
        /// <param name="platform"> </param>
        public virtual string Platform
        {
            set
            {
                this.platform = value;
                LOG.Debug("BrowserContext Object Set With browserVersion ( " + this.version + " ) ");
            }
        }


        /// 
        /// <returns> version </returns>
        public virtual string BrowserVersion
        {
            get
            {
                LOG.Debug("Being returned : " + this.version);
                return this.version;
            }
        }

        /// 
        /// <returns> platform </returns>
        public virtual string BrowserPlatform
        {
            get
            {
                LOG.Debug("Being returned : " + this.platform);
                return this.platform;
            }
        }

        public override int GetHashCode()
        {
            int length = 0;

            /*
		 * get length of browserName
		 */
            length = this.BrowserName == null ? 0 : this.BrowserName.Length;
            LOG.Debug("length of BrowserName = " + length);
            /// <summary>
            /// add length of browser version to existing length
            /// </summary>
            length = length + (this.BrowserVersion == null ? 0 : this.BrowserName.Length);
            LOG.Debug("length of BrowserName + version = " + length);

            /// <summary>
            /// add length of platform to existing length
            /// </summary>
            length = length + (this.BrowserPlatform == null ? 0 : this.BrowserPlatform.Length);

            LOG.Debug("length of BrowserName + version + platform = " + length);
            return length;
        }

        public override bool Equals(object o)
        {
            bool isEquals = false;

            if (o is BrowserContext)
            {
                /// <summary>
                /// get browserName from Object o
                /// </summary>
                string objObrowserName = ((BrowserContext) o).BrowserName;

                /// <summary>
                /// get version from Object o
                /// </summary>
                string objOBrowserVer = ((BrowserContext) o).BrowserVersion;

                string objOPlatform = ((BrowserContext) o).BrowserPlatform;

                if (this.browserName.Equals(objObrowserName) && this.version.Equals(objOBrowserVer) &&
                    this.platform.Equals(objOPlatform))
                {
                    isEquals = true;
                }
            }
            return isEquals;
        }

        public override string ToString()
        {
            string browserInfo = "<BrowserContext><Name> " + this.browserName + "</Name><Version> " + this.version +
                                 "</Version><Platform>" + this.platform + "</Platform></BrowserContext>";
            LOG.Info(browserInfo);
            return browserInfo;
        }

        /// <summary>
        /// synchronized static method which gets unique BrowserContext with respect to given
        /// browserName,version and platform
        /// </summary>
        public static BrowserContext getBrowserContext(string browserName, string version, string platform)
        {
            lock (_locker)
            {
                BrowserContext browserContext = new BrowserContext(browserName, version, platform);

                //if (BrowserContext.mapBrowserContext[browserContext] == null)
                if (BrowserContext.mapBrowserContext.ContainsKey(browserContext) == false)
                {
                    string logInfo =
                        "New BrowserContext Instance Was Created And Placed In Map : BrowserContext.mapBrowserContext";
                    LOG.Info(logInfo);
                    BrowserContext.mapBrowserContext.Add(browserContext, browserContext);
                    BrowserContext.listBrowserContext.Add(browserContext);
                }
                browserContext = BrowserContext.mapBrowserContext[browserContext];
                string logInfo1 = browserContext.ToString();
                LOG.Info("BrowserContext Info Being returned: " + logInfo1);
                return browserContext;
            }
        }

        public static IList<BrowserContext> BrowserContextList
        {
            get { return BrowserContext.listBrowserContext; }
        }
    }
}