namespace Noxypedia.Model
{
    // DynamoDB 의존성 제거 — 히스토리 이력은 더 이상 사용하지 않음
    public class NoxypediaHistory
    {
        public string Key { get; set; } = string.Empty;
        public string Range { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; } = DateTime.MinValue;
        public string Auther { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
    }
}