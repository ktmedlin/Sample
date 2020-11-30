using DevExpress.Utils;
using DevExpress.XtraCharts;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace Reporting
{
    public class DevExpressChart : ChartControl
    {
        private const string FileExtension = ".png";

        public static readonly Font DefaultChartFont = new Font("Verdana", 10);
        public static readonly Palette PerformancePalette = new Palette("PerformancePalette")
        {
            DMAC.Colors.Performance.Low.Color,
            DMAC.Colors.Performance.Medium.Color,
            DMAC.Colors.Performance.High.Color
        };
        public static readonly RangeColorizer PerformaceColorizer;

        private readonly string fileName;

        static DevExpressChart()
        {
            PerformaceColorizer = new RangeColorizer()
            {
                Palette = PerformancePalette
            };
            PerformaceColorizer.RangeStops.AddRange(new double[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 });
        }
        public DevExpressChart()
        {
            fileName = string.Format("{0}{1}", HttpContext.Current.Session.SessionID, FileExtension);
            Legend.Visibility = DefaultBoolean.False;
            BorderOptions.Visibility = DefaultBoolean.False;
            Font = DefaultChartFont;
        }

        private string PhysicalFilePath
        {
            get { return Path.Combine(TempFolder.PhysicalPath, fileName); }
        }

        public void AssignDiagramSettings(XYDiagram diagram)
        {
            diagram.AxisX.Title.Visibility = diagram.AxisY.Title.Visibility = DefaultBoolean.True;
            diagram.AxisX.Title.Font = diagram.AxisY.Title.Font = DefaultChartFont;
            diagram.AxisX.Title.TextColor = diagram.AxisY.Title.TextColor = Color.Black;
            diagram.AxisX.Label.Font = diagram.AxisY.Label.Font = DefaultChartFont;

            diagram.AxisY.WholeRange.AutoSideMargins = false;
            diagram.AxisY.Interlaced = true;
            diagram.AxisY.InterlacedColor = Color.GhostWhite;
        }
        public string SaveImage()
        {
            Image image;
            using (var stream = new MemoryStream())
            {
                ExportToImage(stream, ImageFormat.Png);
                image = Image.FromStream(stream);
            }
            image.Save(PhysicalFilePath);
            return PhysicalFilePath;
        }
    }
}
