using static OA.Domain.Entities.WeatherInfo;

namespace OA.Domain.Entities
{
    public class WeatherForecast : Weather
    {
        /// <summary>
        ///     Time of data receiving in unixtime GMT.
        /// </summary>
        public int DateUnixFormat { get; set; }

        /// <summary>
        ///     Cloudiness in %
        /// </summary>
        public Double Clouds { get; set; }
    }
}
