using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

#nullable enable

namespace YugiohCardDatabase
{

    /// <summary>
    /// カード情報をまとめて表す．
    /// </summary>
    public class Card : IEquatable<Card>, IComparable<Card>
    {
        private readonly string name;
        private readonly string pronunciation;
        private readonly string description;
        private readonly CardKind[] kinds;
        private readonly MonsterRace? race;
        private readonly MonsterAttribute? attribute;
        private readonly MonsterLevel? level;
        private readonly MonsterRank? rank;
        private readonly PendulumScale? pendulumScale;
        private readonly MonsterLink? link;
        private readonly MonsterAttack? attack;
        private readonly MonsterDefence? defence;

        /// <summary>
        /// 各カードに一意に与えられたカード名を取得する．
        /// </summary>
        [JsonIgnore]
        public string IdentityShortName => this.name;
        /// <summary>
        /// カード名の発音を取得する．
        /// </summary>
        [JsonIgnore]
        public string Pronunciation => this.pronunciation;
        /// <summary>
        /// カードに記載された効果テキストを取得する．
        /// </summary>
        [JsonIgnore]
        public string Description => this.description;
        [JsonIgnore]
        public IEnumerable<CardKind> Kinds => this.kinds;
        [JsonIgnore]
        public Option<MonsterRace> Race => Option.FromNullableClass(this.race);
        [JsonIgnore]
        public Option<MonsterAttribute> Attribute => Option.FromNullableClass(this.attribute);
        [JsonIgnore]
        public Option<MonsterLevel> Level => Option.FromNullableStruct(this.level);
        [JsonIgnore]
        public Option<MonsterRank> Rank => Option.FromNullableStruct(this.rank);
        [JsonIgnore]
        public Option<PendulumScale> PendulumScale => Option.FromNullableStruct(this.pendulumScale);
        [JsonIgnore]
        public Option<MonsterLink> Link => Option.FromNullableStruct(this.link);
        [JsonIgnore]
        public Option<MonsterAttack> Attack => Option.FromNullableClass(this.attack);
        [JsonIgnore]
        public Option<MonsterDefence> Defence => Option.FromNullableClass(this.defence);

        public Card(string name,
            string pronunciation,
            string description,
            IEnumerable<CardKind> cardKinds,
            MonsterRace? race,
            MonsterAttribute? attribute,
            MonsterLevel? level,
            MonsterRank? rank,
            PendulumScale? pendulumScale,
            MonsterLink? link,
            MonsterAttack? attack,
            MonsterDefence? defence)
        {
            this.name = name;
            this.pronunciation = pronunciation;
            this.description = description;
            this.kinds = cardKinds.ToArray();
            this.race = race;
            this.attribute = attribute;
            this.level = level;
            this.rank = rank;
            this.pendulumScale = pendulumScale;
            this.link = link;
            this.attack = attack;
            this.defence = defence;
        }

        /// <summary>
        /// カードの効果テキスト以外の情報 (カード名，種別，モンスターのレベルなど)を，視覚的に見やすいフォーマットで返す．
        /// </summary>
        /// <returns></returns>
        public string ConstructFormattedInfoWithoutDescription()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.name);
            builder.Append($" ({this.pronunciation}");
            foreach (var kind in this.Kinds)
            {
                builder.Append(" / " + kind);
            }
            this.Attribute.MayAct(attribute => builder.Append(" / " + attribute));
            this.Race.MayAct(race => builder.Append(" / " + race));
            this.Level.MayAct(level => builder.Append(" / " + level));
            this.Rank.MayAct(rank => builder.Append(" / " + rank));
            this.PendulumScale.MayAct(scale => builder.Append(" / " + scale));
            this.Link.MayAct(link => builder.Append(" / " + link));
            this.Attack.MayAct(attack => builder.Append(" / " + attack));
            this.Defence.MayAct(defence => builder.Append(" / " + defence));

            return builder.ToString();
        }

        public bool Equals(Card other) => this.IdentityShortName.Equals(other.IdentityShortName);

        public int CompareTo(Card other)
        {
            // まずはカード種別に順序付けを試みる．
            var maxKind = this.Kinds.Max();
            var otherMaxKind = other.Kinds.Max();
            var kindComparison = maxKind.CompareTo(otherMaxKind);
            if (kindComparison != 0) return kindComparison;
            // カード種別が同一なら，カード名で順序付けする．
            return this.name.CompareTo(other.name);
        }

        public override bool Equals(object obj) => obj is Card c && this.Equals(c);

        public override int GetHashCode() => this.IdentityShortName.GetHashCode();

        public override string ToString() => this.IdentityShortName;

        public string ConvertToJson()
        {
            // nullなフィールドは記録の必要がない情報なので，Jsonには残さない．
            // 例えば，魔法カードなら`attack`フィールドはnullになるが，魔法カードの情報を保存する場合には`attack`フィールドの情報は必要ない．
            var serializerOptions = new JsonSerializerOptions()
            {
                IgnoreNullValues = true,
            };
            return JsonSerializer.Serialize(this, serializerOptions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <exception cref="JsonException"></exception>
        /// <returns></returns>
        public static Card ConstructFromJson(string json) => JsonSerializer.Deserialize<Card>(json);
    }
}
