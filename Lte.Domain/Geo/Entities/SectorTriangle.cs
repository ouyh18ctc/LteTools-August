namespace Lte.Domain.Geo.Entities
{
    public class SectorTriangle
    {
        public double X1 { get; set; }

        public double X2 { get; set; }

        public double X3 { get; set; }

        public double Y1 { get; set; }

        public double Y2 { get; set; }

        public double Y3 { get; set; }

        public string SerializeString
        {
            get
            {
                return X1 + "," + Y1 + "," + X2 + "," + Y2 + "," + X3 + "," + Y3;
            }
        }

        public string Info { get; set; }

        public string ColorString { get; set; }

        public string CellName { get; set; }
    } 
}
