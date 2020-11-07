using System;

namespace Tests.Extensions.Http
{
    public class Request
    {
        public int IntValue { get; set; }

        public string StringValue { get; set; }

        public DateTimeOffset DateTimeOffsetValue { get; set; }
    }

    public class Response : IEquatable<Response>
    {
        public int IntValue { get; set; }

        public string StringValue { get; set; }

        public DateTimeOffset DateTimeOffsetValue { get; set; }

        bool IEquatable<Response>.Equals(Response other)
        {
            if (other == null)
                return false;

            if (IntValue != other.IntValue)
                return false;

            if (StringValue != other.StringValue)
                return false;

            if (DateTimeOffsetValue != other.DateTimeOffsetValue)
                return false;

            return true;
        }
    }
}
