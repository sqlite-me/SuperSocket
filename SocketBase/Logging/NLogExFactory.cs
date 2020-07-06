using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace SuperSocket.SocketBase.Logging
{
    /// <summary>
    /// Log4NetLogFactory
    /// </summary>
    public class NLogExFactory : LogFactoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NLogExFactory"/> class.
        /// </summary>
        public NLogExFactory()
            : this("NLog.config")
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NLogExFactory"/> class.
        /// </summary>
        /// <param name="logConfig">The log4net config.</param>
        public NLogExFactory(string logConfig)
            : base(logConfig)
        {
            if (!IsSharedConfig)
            {
                NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(ConfigFile);
            }
            else
            {
                //Disable Performance logger
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                var docElement = xmlDoc.DocumentElement;
                var perfLogNode = docElement.SelectSingleNode("logger[@name='Performance']");
                if (perfLogNode != null)
                    docElement.RemoveChild(perfLogNode);
                NLog.LogManager.Configuration = NLog.Config.XmlLoggingConfiguration.CreateFromXmlString(xmlDoc.OuterXml);
            }
        }

        /// <summary>
        /// Gets the log by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public override ILog GetLog(string name)
        {
            return new NLogEx(NLog.LogManager.GetLogger(name));
        }
    }
}
