using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

#nullable enable

namespace YugiohCardDatabase
{
    /// <summary>
    /// モンスターカードの種族．
    /// </summary>
    public class MonsterRace : IEquatable<MonsterRace>
    {
        public readonly string Race;

        public MonsterRace(string race) => this.Race = race.TrimEnd('族');

        public bool Equals(MonsterRace other) => this.Race.Equals(other.Race);

        public override string ToString() => $"{this.Race}族";
    }

    /// <summary>
    /// モンスターカードの属性．
    /// </summary>
    public class MonsterAttribute : IEquatable<MonsterAttribute>
    {
        public readonly string Attribute;

        public MonsterAttribute(string attribute) => this.Attribute = attribute.Replace("属性", "").TrimEnd('属');

        public bool Equals(MonsterAttribute other) => this.Attribute.Equals(other.Attribute);

        public override string ToString() => $"{this.Attribute}属性";
    }

    /// <summary>
    /// モンスターのレベル．
    /// </summary>
    public struct MonsterLevel
    {
        public readonly byte Level;

        public MonsterLevel(byte level) => this.Level = level;

        public override string ToString() => $"★{this.Level}";
    }

    /// <summary>
    /// エクシーズモンスターのランク．
    /// </summary>
    public struct MonsterRank
    {
        public readonly byte Rank;

        public MonsterRank(byte rank) => this.Rank = rank;

        public override string ToString() => $"★{this.Rank}";
    }

    /// <summary>
    /// ペンデュラムモンスターのペンデュラムスケール．
    /// </summary>
    public struct PendulumScale
    {
        public readonly byte Red;
        public readonly byte Blue;

        public PendulumScale(byte red, byte blue)
        {
            this.Red = red;
            this.Blue = blue;
        }

        public override string ToString() => $"ペンデュラムスケール: 赤{this.Red}/青{this.Blue}";
    }

    /// <summary>
    /// リンクモンスターのリンク数およびリンクマーカー方向．
    /// </summary>
    public struct MonsterLink
    {
        public readonly bool Up;
        public readonly bool UpperRight;
        public readonly bool Right;
        public readonly bool LowerRight;
        public readonly bool Low;
        public readonly bool LowerLeft;
        public readonly bool Left;
        public readonly bool UpperLeft;

        [JsonIgnore]
        public int LinkCount
        {
            get
            {
                var count = 0;
                if (this.Up) count++;
                if (this.UpperRight) count++;
                if (this.Right) count++;
                if (this.LowerRight) count++;
                if (this.Low) count++;
                if (this.LowerLeft) count++;
                if (this.Left) count++;
                if (this.UpperLeft) count++;

                return count;
            }
        }

        public MonsterLink(bool up, bool upperRight, bool right, bool lowerRight, bool low, bool lowerLeft, bool left, bool upperLeft)
        {
            this.Up = up;
            this.UpperRight = upperRight;
            this.Right = right;
            this.LowerRight = lowerRight;
            this.Low = low;
            this.LowerLeft = lowerLeft;
            this.Left = left;
            this.UpperLeft = upperLeft;
        }

        public override string ToString()
        {
            List<string> markers = new List<string>();
            if (this.Up) markers.Add("上");
            if (this.UpperRight) markers.Add("右上");
            if (this.Right) markers.Add("右");
            if (this.LowerRight) markers.Add("右下");
            if (this.Low) markers.Add("下");
            if (this.LowerLeft) markers.Add("左下");
            if (this.Left) markers.Add("左");
            if (this.UpperLeft) markers.Add("左上");

            return $"リンク:{markers.Count}/マーカー:" + markers.Aggregate((acc, next) => acc + "/" + next);
        }
    }

    /// <summary>
    /// モンスターの攻撃力．
    /// </summary>
    public class MonsterAttack
    {
        /// <summary>
        /// 攻撃力が一意に定まっていない状態．カード表記の`?`に相当する．
        /// </summary>
        public static readonly MonsterAttack Variable = new MonsterAttack("?");

        private readonly string status;

        [JsonIgnore]
        public Option<int> Status => int.TryParse(this.status, out var s) ? Option.Some(s) : Option.None<int>();

        [JsonIgnore]
        public bool IsFixedStatus => int.TryParse(this.status, out var _);

        private MonsterAttack(string status) => this.status = status;

        public override string ToString() => this.status;

        public static MonsterAttack Fixed(int status) => new MonsterAttack(status.ToString());
    }

    /// <summary>
    /// モンスターの守備力．
    /// </summary>
    public class MonsterDefence
    {
        /// <summary>
        /// 守備力が一意に定まっていない状態．カード表記の`?`に相当する．
        /// </summary>
        public static readonly MonsterDefence Variable = new MonsterDefence("?");

        private readonly string status;

        [JsonIgnore]
        public Option<int> Status => int.TryParse(this.status, out var s) ? Option.Some(s) : Option.None<int>();

        [JsonIgnore]
        public bool IsFixedStatus => int.TryParse(this.status, out var _);

        private MonsterDefence(string status) => this.status = status;

        public override string ToString() => this.status;

        public static MonsterDefence Fixed(int status) => new MonsterDefence(status.ToString());
    }
}
