using Newtonsoft.Json;

namespace PrintNode.Net
{
    public class PrintNodePrintJobOptions
    {
        /// <summary>
        /// The name of one of the paper trays or output bins reported by the printer capability property "bins".
        /// 
        /// See <a href="https://www.printnode.com/docs/api/curl/#printer-capabilities">Printer capabilities</a>.
        /// </summary>
        public string Bin { get; set; }

        /// <summary>
        /// Enables print copy collation when printing multiple copies. If this option is not specified the printer default is used.
        /// </summary>
        public bool Collate { get; set; }

        /// <summary>
        /// Positive integer. Default 1. The number of copies to be printed. Maximum value as reported by the printer capability 
        /// property "copies".
        /// 
        /// See <a href="https://www.printnode.com/docs/api/curl/#printer-capabilities">Printer capabilities</a>.
        /// </summary>
        public int Copies { get; set; }

        /// <summary>
        /// The dpi setting to use for this PrintJob. Allowed values are those reported by the printer capability property "dpis".
        /// 
        /// See <a href="https://www.printnode.com/docs/api/curl/#printer-capabilities">Printer capabilities</a>.
        /// </summary>
        public string Dpi { get; set; }

        /// <summary>
        /// One of long-edge or short-edge for 2 sides printing along the long-edge (portrait) or the short edge (landscape) respectively. 
        /// If this option is not specified the the printer default is used.
        /// </summary>
        public string Duplex { get; set; }

        /// <summary>
        /// OSX only. Defaults true. If you wish to disable this option pass false here.
        /// </summary>
        [JsonProperty("fit_to_page")]
        public bool FitToPage { get; set; }

        /// <summary>
        /// The named media to use for this PrintJob. Allowed values are one of the values reported by the printer capability property "medias". 
        /// We've found some printers on OSX ignore this setting unless a "bin" (paper tray) option is also set.
        /// 
        /// See <a href="https://www.printnode.com/docs/api/curl/#printer-capabilities">Printer capabilities</a>.
        /// </summary>
        public string Media { get; set; }

        /// <summary>
        /// OSX only. Allows support for printing muliple pages per physical sheet of paper. Default 1. Allowed values are those reported 
        /// by the printer capability property "nup".
        /// 
        /// See <a href="https://www.printnode.com/docs/api/curl/#printer-capabilities">Printer capabilities</a>.
        /// </summary>
        public int Nup { get; set; }

        /// <summary>
        /// A set of pages to print from a PDF. The format is described here. A few quick exampleE.g. 1,3 prints pages 1 and 3. -5 prints 
        /// pages 1 through 5 inclusive. - prints all pages. Different components can be combined with a comma. 1,3- prints all pages except page 2.
        /// 
        /// See <a href="https://www.printnode.com/docs/api/curl/#parameters">Parameters</a>.
        /// </summary>
        public string Pages { get; set; }

        /// <summary>
        /// A named paper size to use. Allowed values are the keys in the object returned by the printer capability property "papers".
        /// 
        /// See <a href="https://www.printnode.com/docs/api/curl/#printer-capabilities">Printer capabilities</a>.
        /// </summary>
        public string Paper { get; set; }

        /// <summary>
        /// One of 90, 180 or 270. Supports rotating all pages while printing. We've found support on OSX to be patchy. Support depends on both printer and driver.
        /// </summary>
        public int Rotate { get; set; }
    }
}
