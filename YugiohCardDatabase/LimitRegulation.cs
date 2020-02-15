using System.Text.Json;

#nullable enable

namespace YugiohCardDatabase
{
    /// <summary>
    /// リミットレギュレーション (各カードのデッキ投入枚数の規定)を表す．
    /// </summary>
    public readonly struct LimitRegulation
    {
        /// <summary>
        /// 禁止カード．デッキに1枚も投入できない．
        /// </summary>
        public static readonly LimitRegulation Prohibited = new LimitRegulation(0);
        /// <summary>
        /// 制限カード．デッキに1枚まで投入できる．
        /// </summary>
        public static readonly LimitRegulation Limited = new LimitRegulation(1);
        /// <summary>
        /// 準制限カード．デッキに2枚まで投入できる．
        /// </summary>
        public static readonly LimitRegulation Semilimited = new LimitRegulation(2);
        /// <summary>
        /// 無制限カード．デッキに3枚まで投入できる．
        /// </summary>
        public static readonly LimitRegulation Unlimited = new LimitRegulation(3);

        /// <summary>
        /// デッキに投入可能な最大枚数．
        /// </summary>
        public readonly int MaxAdoptableCount;

        private LimitRegulation(int maxAdoptableCount)
        {
            this.MaxAdoptableCount = maxAdoptableCount;
        }

        public string ConvertToJson() => JsonSerializer.Serialize(this);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <exception cref="JsonException"></exception>
        /// <returns></returns>
        public static LimitRegulation ConstructFromJson(string json) => JsonSerializer.Deserialize<LimitRegulation>(json);
    }
}
