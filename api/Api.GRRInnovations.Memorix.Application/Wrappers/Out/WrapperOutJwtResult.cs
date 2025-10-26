using Api.GRRInnovations.Memorix.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Application.Wrappers.Out
{
    public class WrapperOutJwtResult : WrapperBase<JwtResultModel, WrapperOutJwtResult>
    {
        public WrapperOutJwtResult() : base(null) { }

        public WrapperOutJwtResult(JwtResultModel data) : base(data) { }

        [JsonPropertyName("access_token")]
        public string AccessToken
        {
            get => Data.AccessToken;
            set => Data.AccessToken = value;
        }

        [JsonPropertyName("expire")]
        public double Expire
        {
            get => Data.Expire;
            set => Data.Expire = value;
        }

        [JsonPropertyName("type")]
        public string Type
        {
            get => Data.Type;
            set => Data.Type = value;
        }

        public new static async Task<WrapperOutJwtResult> From(JwtResultModel model)
        {
            var wrapper = new WrapperOutJwtResult();
            await wrapper.Populate(model).ConfigureAwait(false);

            return wrapper;
        }
    }
}
