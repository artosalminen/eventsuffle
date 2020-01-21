using Eventsuffle.Core.Exceptions;
using Eventsuffle.Web.Converters;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Eventsuffle.UnitTests.Converters
{
    public class IsoDateConverters
    {
        [Theory]
        [InlineData("{ \"date\": \"2020-01-01\" }", 2020, 01, 01)]
        [InlineData("{ \"date\": \"2020-02-29\" }", 2020, 02, 29)]
        [InlineData("{ \"date\": \"2020-03-29\" }", 2020, 03, 29)]
        [InlineData("{ \"date\": \"2020-03-30\" }", 2020, 03, 30)]
        [InlineData("{ \"date\": \"2020-10-25\" }", 2020, 10, 25)]
        [InlineData("{ \"date\": \"2020-10-26\" }", 2020, 10, 26)]
        public void ConvertsFromJsonToDate(string jsonString, int expectedYear, int expectedMonth, int expectedDay)
        {
            // Arrange
            IsoDateConverter converter = new IsoDateConverter();
            Utf8JsonReader utf8JsonReader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonString), false, new JsonReaderState(new JsonReaderOptions()));

            // Advance the reader to the date string.
            while(utf8JsonReader.Read())
            {
                if(utf8JsonReader.TokenType == JsonTokenType.String)
                {
                    break;
                }
            }

            // Act
            DateTime result = converter.Read(ref utf8JsonReader, typeof(DateTime), new JsonSerializerOptions());

            // Assert
            DateTime expected = new DateTime(expectedYear, expectedMonth, expectedDay);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("\"2020-01-01\"", 2020, 01, 01)]
        [InlineData("\"2020-02-29\"", 2020, 02, 29)]
        [InlineData("\"2020-03-29\"", 2020, 03, 29)]
        [InlineData("\"2020-03-30\"", 2020, 03, 30)]
        [InlineData("\"2020-10-25\"", 2020, 10, 25)]
        [InlineData("\"2020-10-26\"", 2020, 10, 26)]
        public void ConvertsFromDateToJson(string expected, int year, int month, int day)
        {
            // Arrange
            IsoDateConverter converter = new IsoDateConverter();
            string result;

            // Act
            using (var stream = new MemoryStream())
            {
                Utf8JsonWriter utf8JsonWriter = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
                converter.Write(utf8JsonWriter, new DateTime(year, month, day), new JsonSerializerOptions());
                utf8JsonWriter.Flush();
                result = Encoding.UTF8.GetString(stream.ToArray());
            }

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ThrowsInvalidDateException()
        {
            Assert.Throws<InvalidISODateException>(() =>
            {
                IsoDateConverter converter = new IsoDateConverter();
                Utf8JsonReader utf8JsonReader = new Utf8JsonReader(Encoding.UTF8.GetBytes("{ \"date\": \"2021-02-29\" }"), false, new JsonReaderState(new JsonReaderOptions()));

                while (utf8JsonReader.Read())
                {
                    if (utf8JsonReader.TokenType == JsonTokenType.String)
                    {
                        break;
                    }
                }

                DateTime result = converter.Read(ref utf8JsonReader, typeof(DateTime), new JsonSerializerOptions());
            });
        }
    }
}
