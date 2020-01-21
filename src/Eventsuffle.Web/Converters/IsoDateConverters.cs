using Eventsuffle.Core.Exceptions;
using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Eventsuffle.Web.Converters
{
    public class IsoDateConverter: JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateString = reader.GetString();
            try
            {
                return DateTime.Parse(dateString, CultureInfo.InvariantCulture);
            } catch
            {
                throw new InvalidISODateException(reader.GetString());
            }
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.Date.ToString(Constants.Formats.DateFormat, CultureInfo.InvariantCulture));
    }
}
