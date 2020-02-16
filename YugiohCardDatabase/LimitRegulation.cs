using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

#nullable enable

namespace YugiohCardDatabase
{
    /// <summary>
    /// リミットレギュレーション (各カードのデッキ投入枚数の規定)を表す．
    /// </summary>
    [DataContract]
    public readonly struct LimitRegulation : IEquatable<LimitRegulation>
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
        [DataMember]
        public readonly int MaxAdoptableCount;

        internal LimitRegulation(int maxAdoptableCount)
        {
            this.MaxAdoptableCount = maxAdoptableCount;
        }

        public bool Equals(LimitRegulation other) => this.MaxAdoptableCount.Equals(other.MaxAdoptableCount);

        public override string ToString()
        {
            return this.MaxAdoptableCount switch
            {
                0 => "禁止",
                1 => "制限",
                2 => "準制限",
                _ => "",
            };
        }
    }

    [DataContract]
    public class LimitRegulationDatabase
    {
        [DataMember]
        private readonly Dictionary<int, List<string>> regulations = new Dictionary<int, List<string>>();

        [IgnoreDataMember]
        private Dictionary<string, LimitRegulation>? regulationsForSearch;

        public void AddLimitRegulation(string cardName, LimitRegulation limitRegulation)
        {
            if (this.regulationsForSearch == null) this.regulationsForSearch = new Dictionary<string, LimitRegulation>();
            if (!this.regulations.ContainsKey(limitRegulation.MaxAdoptableCount))
            {
                this.regulations.Add(limitRegulation.MaxAdoptableCount, new List<string>());
            }
            this.regulations[limitRegulation.MaxAdoptableCount].Add(cardName);
            this.regulationsForSearch.Add(cardName, limitRegulation);
        }

        public LimitRegulation GetLimitRegulationOf(string cardName)
        {
            if (this.regulationsForSearch == null) this.regulationsForSearch = new Dictionary<string, LimitRegulation>();
            if (!this.regulationsForSearch.Any())
            {
                foreach (var item in this.regulations)
                {
                    var regulation = item.Key;
                    foreach (var name in item.Value)
                    {
                        this.regulationsForSearch.Add(name, new LimitRegulation(regulation));
                    }
                }
            }
            if (this.regulationsForSearch.ContainsKey(cardName))
            {
                return this.regulationsForSearch[cardName];
            }
            else
            {
                return LimitRegulation.Unlimited;
            }
        }
    }
}
