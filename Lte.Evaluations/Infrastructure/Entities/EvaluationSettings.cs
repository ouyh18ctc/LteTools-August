namespace Lte.Evaluations.Infrastructure.Entities
{
    public static class EvaluationSettings
    {
        private static double trafficLoad = 0.1;

        public static double TrafficLoad
        {
            get { return trafficLoad; }
            set { trafficLoad = value; }
        }

        private static double distanceInMeter = 50;

        public static double DistanceInMeter
        {
            get { return distanceInMeter; }
            set { distanceInMeter = value; }
        }

        private static double degreeSpan = 0.03;

        public static double DegreeSpan
        {
            get { return degreeSpan; }
            set { degreeSpan = value; }
        }
    }
}
